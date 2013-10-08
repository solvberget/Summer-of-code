using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class InformationController : BaseController
    {

        private readonly IInformationRepository _informationRepository;

        public InformationController(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }

        public JsonResult GetContactInformation()
        {
            var result = _informationRepository.GetContactInformation();
            return Json(result);
        }

        public JsonResult GetOpeningHoursInformation()
        {
            var result = _informationRepository.GetOpeningHoursInformation();
            return Json(result);
        }

    }
}
