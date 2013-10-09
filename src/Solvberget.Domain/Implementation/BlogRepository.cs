using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class BlogRepository : IBlogRepository
    {
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
            blog.Entries = blog.ContentType.Equals("atom") ? BlogEntry.FillEntriesFromAtom(xml) : BlogEntry.FillEntriesFromRss(xml);

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

    }
}
