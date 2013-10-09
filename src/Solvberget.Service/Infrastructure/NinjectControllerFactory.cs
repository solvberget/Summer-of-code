using System;
using System.Web.Mvc;
using Ninject;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Implementation;

namespace Solvberget.Service.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _kernel;

        public NinjectControllerFactory()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IRootPathProvider>().To<RootPathProvider>();
            _kernel.Bind<IEnvironmentPathProvider>().To<EnvironmentPathProvider>();
            _kernel.Bind<IInformationRepository>().To<InformationRepositoryXml>();
            _kernel.Bind<IRepository>().To<AlephRepository>();
            _kernel.Bind<IBlogRepository>().To<BlogRepository>();
            _kernel.Bind<INewsRepository>().To<NewsRepository>();
            _kernel.Bind<IEventRepository>().To<LinticketRepository>();
            _kernel.Bind<IReviewRepository>().To<ReviewRepository>();
            _kernel.Bind<IRatingRepository>().To<RatingRepository>();
            _kernel.Bind<IImageRepository>().To<ImageRepository>();
            _kernel.Bind<IListRepositoryStatic>().To<LibraryListXmlRepository>();
            _kernel.Bind<IListRepository>().To<LibraryListDynamicRepository>();
            _kernel.Bind<ISuggestionDictionary>().To<LuceneRepository>();
        }
    }
}
