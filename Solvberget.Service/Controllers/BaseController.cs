using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solvberget.Service.Controllers
{
    public class BaseController : Controller
    {

        protected JsonResult Json(object data)
        {
            var jsonresult = new ChildJsonResult();
            jsonresult.Data = data;
            jsonresult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonresult;
        }

    }
}
