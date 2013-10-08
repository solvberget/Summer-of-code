using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogRepository _repository;

        public BlogController(IBlogRepository repository)
        {
            _repository = repository;
        }

        public JsonResult GetBlogs()
        {
            var result = _repository.GetBlogs();
            return Json(result);
        }

        public JsonResult GetBlogWithEntries(int blogId)
        {
            var result = _repository.GetBlogWithEntries(blogId);
            return Json(result);
        }
    }

}
