using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Solvberget.Domain;
using Solvberget.Domain.Abstract;

namespace Solvberget.Service.Controllers
{
    public class DocumentController : Controller
    {
        private IRepository _repository;
        private ISpellingDictionary _spellingRepository;

        public DocumentController(IRepository repository, ISpellingDictionary spellingRepository)
        {
            _repository = repository;
            _spellingRepository = spellingRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search(string id)
        {
            var result = _repository.Search(id);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SpellingDictionaryLookup(string value)
        {
            var result = _spellingRepository.Lookup(value);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }

}
