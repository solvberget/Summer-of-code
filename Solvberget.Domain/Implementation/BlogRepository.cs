using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public BlogRepository(string folderPath = null)
        {
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<Blog> GetBlogs()
        {
            // TODO: Open xml file with blogs, parse it
            // TODO: Fo
            var blogs = GetBlogsFromFile(_folderPath + BlogFeedsFile);

            return blogs;
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

            return blogs;

        }

    }
}
