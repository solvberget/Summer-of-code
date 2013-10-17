using System.Collections.Generic;
using System.Linq;
﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class BlogRepository : IBlogRepository
    {
        // ReSharper disable InconsistentNaming
        private const int DESCRIPTION_LENGTH = 300;
        // ReSharper restore InconsistentNaming

        private static readonly XNamespace XmlnsAtom = "http://www.w3.org/2005/Atom";
        private static readonly XNamespace XmlnsThumb = "http://search.yahoo.com/mrss/";
        private static readonly XNamespace ContentNamespace = "http://purl.org/rss/1.0/modules/content/";
        private static readonly XNamespace Media = "http://search.yahoo.com/mrss/";
        private static readonly XNamespace Author = "http://purl.org/dc/elements/1.1/";

        private const string StdFolderPath = @"App_Data\blogs\";
        private const string BlogFeedsFile = @"feeds.xml";
        private readonly string _folderPath;

        public BlogRepository(IEnvironmentPathProvider environment)
        {
            var folderPath = environment.GetBlogFeedPath();
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<Blog> GetBlogs()
        {
            return GetBlogsFromFile(_folderPath + BlogFeedsFile);
        }

        public Blog GetBlogWithEntries(int blogId)
        {
            var blogs = GetBlogsFromFile(_folderPath + BlogFeedsFile);
            var blog = blogs.ElementAt(blogId);
            string xml;
            try
            {
                xml = XDocument.Load(blog.Url, LoadOptions.PreserveWhitespace).ToString();
            }
            catch (System.Exception)
            {
                // TODO: Log exception, mer spesifikk exception (blog not exist)
                System.Console.WriteLine("Fatal feil: Kunne ikke hente blogg (bloggen finnes ikke?)");
                return blog;
            }
            blog.Entries = blog.ContentType.Equals("atom") ? FillEntriesFromAtom(xml) : FillEntriesFromRss(xml);

            return blog;
        }

        private static List<Blog> GetBlogsFromFile(string blogFeedsXmlFile)
        {


            var blogs = new List<Blog>();
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load(blogFeedsXmlFile);
            }
            catch
            {
                return blogs;
            }

            var root = xdoc.Root;
            if (root == null) return blogs;

            var feeds = root.Elements();
            blogs.AddRange(feeds.Select(xElement => Blog.FillBlog(xElement.ToString())));
            blogs = blogs.OrderBy(blog => blog.Priority).ToList();
            return blogs;

        }

        public static List<BlogEntry> FillEntriesFromAtom(string xml)
        {
            var entries = new List<BlogEntry>();
            var docTemp = XDocument.Parse(xml);
            docTemp.Declaration = new XDeclaration("1.0", "utf-8", null);
            var doc = new XDocument(docTemp);

            if (doc.Root == null)
                return entries;

            var rawEntries = doc.Root.Descendants(XmlnsAtom + "entry");

            foreach (var rawEntry in rawEntries)
            {
                var entry = new BlogEntry
                {
                    Title = GetXmlValueFromNamespace(rawEntry, XmlnsAtom + "title"),
                    PublishedDate =
                        DateTime.Parse(GetXmlValueFromNamespace(rawEntry, XmlnsAtom + "published"),
                                       new CultureInfo("nb-NO", false)),
                    UpdatedDate = DateTime.Parse(GetXmlValueFromNamespace(rawEntry, XmlnsAtom + "updated"),
                                                 new CultureInfo("nb-NO", false)),
                    Content = GetXmlValueFromNamespace(rawEntry, XmlnsAtom + "content"),
                    AuthorName =
                        GetXmlValueFromNamespace(
                            rawEntry.Descendants(XmlnsAtom.GetName("author")).FirstOrDefault(), XmlnsAtom.GetName("name"))
                };

                // thumbnail
                var xAttribute = rawEntry.Elements(XmlnsThumb.GetName("thumbnail")).Attributes("url").FirstOrDefault();
                if (xAttribute != null)
                    entry.ThumbnailUrl = xAttribute.Value;

                // url
                var link = rawEntry.Elements(XmlnsAtom.GetName("link")).Where(x => x.FirstAttribute.Value.Equals("alternate"));
                var firstOrDefaultHref = link.Attributes("href").FirstOrDefault();

                if (firstOrDefaultHref != null)
                    entry.Url = firstOrDefaultHref.Value;

                // description
                var desc = Regex.Replace(entry.Content, "<(.|\n)*?>", "");

                entry.Description = Regex.Replace(desc, "\\n", "");
                var descLength = DESCRIPTION_LENGTH < entry.Description.Length
                                     ? DESCRIPTION_LENGTH
                                     : entry.Description.Length;

                entry.Description = entry.Description.Substring(0, descLength) + " [...]";

                entries.Add(entry);
            }
            entries = entries.OrderByDescending(x => x.PublishedDate).ToList();
            return entries;

        }
        public static List<BlogEntry> FillEntriesFromRss(string xml)
        {
            var entries = new List<BlogEntry>();

            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
                return entries;

            var rawEntries = doc.Root.Descendants("item");
            foreach (var rawEntry in rawEntries)
            {
                var entry = new BlogEntry
                {
                    Title = GetXmlValue(rawEntry, "title"),
                    Url = GetXmlValue(rawEntry, "link"),
                    Content =
                        HttpUtility.HtmlDecode(GetXmlValueFromNamespace(rawEntry,
                                                                        ContentNamespace.GetName(
                                                                            "encoded"))),
                    PublishedDate = DateTime.Parse(GetXmlValue(rawEntry, "pubDate")),
                    Description = GetXmlValue(rawEntry, "description"),
                    AuthorName = GetXmlValueFromNamespace(rawEntry, Author.GetName("creator")),

                };
                //thumbnail
                var imageElements = rawEntry.Elements(Media.GetName("content"));
                var imageContent = imageElements.Attributes("url").FirstOrDefault(x => !string.IsNullOrEmpty(x.Value));
                if (imageContent != null)
                    entry.ThumbnailUrl = imageContent.Value;

                entries.Add(entry);
            }
            return entries;
        }

        private static string GetXmlValueFromNamespace(XElement node, XName tag)
        {
            var xElement = node.DescendantsAndSelf(tag).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }


        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }
    }
}
