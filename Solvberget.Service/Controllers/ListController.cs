﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Controllers
{
    public class ListController : Controller
    {

        private readonly IListRepositoryStatic _xmlRepository;

        public ListController(IListRepositoryStatic xmlRepository)
        {
            _xmlRepository = xmlRepository;
        }

        public JsonResult GetLists(int? limit)
        {
            var result = _xmlRepository.GetLists(limit);
            var latestChange = _xmlRepository.GetTimestampForLatestChange();
            var timestamp = latestChange != null ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
            var response = new { Timestamp = timestamp, Lists = result };
            return this.Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}