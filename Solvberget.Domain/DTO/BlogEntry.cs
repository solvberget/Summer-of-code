using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class BlogEntry
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedDateFormatted
        {
            get { return PublishedDate.Year == 1 ? null : PublishedDate.ToString("dd.MM.yyyy"); }
        }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateFormatted
        {
            get { return UpdatedDate.Year == 1 ? null : UpdatedDate.ToString("dd.MM.yyyy"); }
        }
        public string AuthorName { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }

        public static List<BlogEntry> FillEntriesFromAtom(string xml)
        {
            var entries = new List<BlogEntry>();
            XDocument docTemp = XDocument.Parse(xml);
            docTemp.Declaration = new XDeclaration("1.0", "utf-8", null);
            var doc = new XDocument(docTemp);
            XNamespace xmlns = "http://www.w3.org/2005/Atom";
            XNamespace thumbNamespace = "http://search.yahoo.com/mrss/";


            var rawEntries = doc.Root.Descendants(xmlns + "entry");

            foreach (var rawEntry in rawEntries)
            {
                var entry = new BlogEntry();

                entry.Title = GetXmlValueFromNamespace(rawEntry, xmlns + "title");
                entry.PublishedDate = DateTime.Parse(GetXmlValueFromNamespace(rawEntry, xmlns + "published"),
                                                     new CultureInfo("nb-NO", false));
                entry.UpdatedDate = DateTime.Parse(GetXmlValueFromNamespace(rawEntry, xmlns + "updated"),
                                                   new CultureInfo("nb-NO", false));
                entry.Content = GetXmlValueFromNamespace(rawEntry, xmlns + "content");
                entry.AuthorName = GetXmlValueFromNamespace(rawEntry.Descendants(xmlns + "author").FirstOrDefault(), xmlns + "name");

                // thumbnail
                var xAttribute = rawEntry.Elements(thumbNamespace + "thumbnail").Attributes("url").FirstOrDefault();
                if (xAttribute != null)
                    entry.ThumbnailUrl = xAttribute.Value;

                // url
                var link = rawEntry.Elements(xmlns + "link").Where(x => x.FirstAttribute.Value.Equals("alternate"));
                var firstOrDefault = link.Attributes("href").FirstOrDefault();

                if (firstOrDefault != null)
                    entry.Url = firstOrDefault.Value;

                entries.Add(entry);
            }

            return entries;

        }
        public static List<BlogEntry> FillEntriesFromRss(string xml)
        {
            var entries = new List<BlogEntry>();

            var doc = XDocument.Parse(xml);
            if (doc.Root == null)
                return entries;

            var rawEntries = doc.Root.Descendants("item");
            XNamespace contentNamespace = "http://purl.org/rss/1.0/modules/content/";
            XNamespace media = "http://search.yahoo.com/mrss/";
            XNamespace author = "http://purl.org/dc/elements/1.1/";
            foreach (var rawEntry in rawEntries)
            {
                var entry = new BlogEntry
                                {
                                    Title = GetXmlValue(rawEntry, "title"),
                                    Url = GetXmlValue(rawEntry, "link"),
                                    Content =
                                        HttpUtility.HtmlDecode(GetXmlValueFromNamespace(rawEntry,
                                                                                        contentNamespace + "encoded")),
                                    PublishedDate = DateTime.Parse(GetXmlValue(rawEntry, "pubDate")),
                                    Description = GetXmlValue(rawEntry, "description"),
                                    
                                };
                entry.AuthorName = GetXmlValueFromNamespace(rawEntry, author.GetName("creator"));
                //thumbnail
                var imageElements = rawEntry.Elements(media.GetName("content"));
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
