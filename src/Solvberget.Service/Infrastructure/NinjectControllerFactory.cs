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
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IInformationRepository>().To<InformationRepositoryXml>()
                .WithConstructorArgument("filePathOpening", EnvironmentHelper.GetOpeningInfoAsXmlPath())
                .WithConstructorArgument("filePathContact", EnvironmentHelper.GetContactInfoAsXmlPath());
            ninjectKernel.Bind<IRepository>().To<AlephRepository>()
                .WithConstructorArgument("pathToImageCache", EnvironmentHelper.GetImageCachePath())
                .WithConstructorArgument("pathToRulesFolder", EnvironmentHelper.GetRulesPath());
            ninjectKernel.Bind<IBlogRepository>().To<BlogRepository>()
                .WithConstructorArgument("folderPath", EnvironmentHelper.GetBlogFeedPath());
            ninjectKernel.Bind<INewsRepository>().To<NewsRepository>();
            ninjectKernel.Bind<IEventRepository>().To<LinticketRepository>();
            ninjectKernel.Bind<IReviewRepository>().To<ReviewRepository>();
            ninjectKernel.Bind<IRatingRepository>().To<RatingRepository>();
            ninjectKernel.Bind<IImageRepository>().To<ImageRepository>()
                 .WithConstructorArgument("pathToImageCache", EnvironmentHelper.GetImageCachePath());
            ninjectKernel.Bind<IListRepositoryStatic>().To<LibraryListXmlRepository>()
                .WithConstructorArgument("folderPath", EnvironmentHelper.GetXmlListPath());
            ninjectKernel.Bind<IListRepository>().To<LibraryListDynamicRepository>()
                .WithConstructorArgument("xmlFilePath", EnvironmentHelper.GetXmlFilePath());
            ninjectKernel.Bind<ISuggestionDictionary>()
                .To<LuceneRepository>()
                .WithConstructorArgument("indexPath", EnvironmentHelper.GetDictionaryIndexPath())
                .WithConstructorArgument("suggestionPath", EnvironmentHelper.GetSuggestionListPath());

        }
    }
}
