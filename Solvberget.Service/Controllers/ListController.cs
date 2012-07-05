using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Controllers
{
    public class ListController : Controller
    {

        private readonly LibraryListsFromXmlRepository _xmlRepository;

        public ListController(LibraryListsFromXmlRepository xmlRepository)
        {
            _xmlRepository = xmlRepository;
        }

        public JsonResult GetLists()
        {
            var result = _xmlRepository.GetLists();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLists(int limit)
        {
            var result = _xmlRepository.GetLists(limit);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
