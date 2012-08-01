using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Blog
    {

        public string Url { get; set; }
        public string Site { get; set; }
        public int Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<BlogEntry> Entries { get; set; }


        public static Blog FillBlog(string xml)
        {
            var blog = new Blog();
            var xElement = XElement.Parse(xml);
            blog.Url = GetXmlValue(xElement, "url");
            blog.Priority = int.Parse(GetXmlValue(xElement, "priority"));
            blog.Title = GetXmlValue(xElement, "title");
            blog.ContentType = GetXmlValue(xElement, "type");
            blog.Description = GetXmlValue(xElement, "description");

            blog.Site = GetBlogSite(blog.Url);

            var categories = new List<string>();
            var element = xElement.Element("categories");

            if (element != null)
                categories.AddRange(element.Elements().Select(category => GetXmlValue(category, "category")));

            blog.Categories = categories;
            return blog;
        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }
        private static string GetBlogSite (string url)
        {
            var uri = new Uri(url);
            return "http://"+uri.Host;
        }

    }
}
