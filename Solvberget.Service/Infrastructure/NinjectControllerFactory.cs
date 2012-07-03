using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Solvberget.Domain;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IRepository>().To<AlephRepository>();
            ninjectKernel.Bind<IEventRepository>().To<LinticketRepository>();
            ninjectKernel.Bind<ISpellingDictionary>()
                .To<LuceneRepository>()
                .WithConstructorArgument("pathToDictionary", EnvironmentHelper.GetDictionaryPath())
                .WithConstructorArgument("pathToDictionaryDirectory", EnvironmentHelper.GetDictionaryIndexPath())
                .WithConstructorArgument("pathToStopWordsDict", EnvironmentHelper.GetStopwordsPath())
                .WithConstructorArgument("pathToSuggestionListDict", EnvironmentHelper.GetSuggestionListPath())
                .WithConstructorArgument("pathToTestDict", EnvironmentHelper.GetTestDictPath());
                
        }
    }
}