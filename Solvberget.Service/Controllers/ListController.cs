using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Controllers
{
    public class ListController : BaseController
    {

        private readonly IListRepositoryStatic _staticRepository;
        private readonly IListRepository _dynamicRepository;

        public ListController(IListRepositoryStatic staticRepository, IListRepository dynamicRepository)
        {
            _staticRepository = staticRepository;
            _dynamicRepository = dynamicRepository;
        }

        public JsonResult GetListsStatic(int? limit)
        {
            var resultStatic = _staticRepository.GetLists(limit);
            var latestChange = _staticRepository.GetTimestampForLatestChange();
            var timestamp = latestChange != null ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
            var response = new { Timestamp = timestamp, Lists = resultStatic };
            return Json(response);
        }

        public JsonResult GetListsStaticLastModified()
        {
            var timestamp = _staticRepository.GetTimestampForLatestChange();
            var response = timestamp != null ? timestamp.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
            return Json(response);
        }

        public JsonResult GetListsStaticAndDynamic(int? limit)
        {
            var resultStatic = _staticRepository.GetLists(limit);
            var resultDynamic = _dynamicRepository.GetLists(limit);
            var totalResult = resultStatic.Union(resultDynamic).OrderBy(x => x.Priority);
            var latestChange = _staticRepository.GetTimestampForLatestChange();
            var timestamp = latestChange != null ? latestChange.Value.Ticks.ToString(CultureInfo.InvariantCulture) : "0";
            var response = new { TimestampForStatic = timestamp, Lists =  totalResult };
            return Json(response);
        }

    }
}