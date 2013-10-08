using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class NewsController : BaseController
    {

        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public JsonResult GetNewsItems(int? limit)
        {
            var result = _newsRepository.GetNewsItems(limit);
            return Json(result);
        }

    }
}
