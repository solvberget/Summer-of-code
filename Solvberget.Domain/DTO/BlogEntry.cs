using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class BlogEntry
    {

        // ReSharper disable InconsistentNaming
        private const int DESCRIPTION_LENGTH = 300;
        // ReSharper restore InconsistentNaming

        private static readonly XNamespace XmlnsAtom = "http://www.w3.org/2005/Atom";
        private static readonly XNamespace XmlnsThumb = "http://search.yahoo.com/mrss/";
        private static readonly XNamespace ContentNamespace = "http://purl.org/rss/1.0/modules/content/";
        private static readonly XNamespace Media = "http://search.yahoo.com/mrss/";
        private static readonly XNamespace Author = "http://purl.org/dc/elements/1.1/";

        public string Title { get; private set; }
        public string Content { get; private set; }
        private DateTime PublishedDate { get; set; }
        public string PublishedDateFormatted
        {
            get { return PublishedDate.Year == 1 ? null : PublishedDate.ToString("dd.MM.yyyy"); }
        }

        private DateTime UpdatedDate { get; set; }
        public string UpdatedDateFormatted
        {
            get { return UpdatedDate.Year == 1 ? null : UpdatedDate.ToString("dd.MM.yyyy"); }
        }
        public string AuthorName { get; private set; }
        public string Url { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string Description { get; private set; }

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
