using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Solvberget.Domain.Implementation;
using Solvberget.Service.Infrastructure;

namespace Solvberget.Service
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Document", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SetupLuceneDictionary();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            
            
        }

        private static void SetupLuceneDictionary()
        {
            
            var repository = new LuceneRepository(EnvironmentHelper.GetDictionaryPath(), EnvironmentHelper.GetDictionaryIndexPath(), EnvironmentHelper.GetStopwordsPath());
            repository.BuildDictionary();
        }
    }
}