using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class EventController : BaseController
    {

        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public JsonResult GetEvents()
        {
            var result = _eventRepository.GetEvents();
            return Json(result);
        }

    }
}
