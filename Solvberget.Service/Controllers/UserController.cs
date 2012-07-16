using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class UserController : Controller
    {

        private readonly IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        public JsonResult GetUserInformation(string userId, string verification)
        {
            var result = _repository.GetUserInformation(userId, verification);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
