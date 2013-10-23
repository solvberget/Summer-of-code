using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Services.Stubs;
using Solvberget.Core.ViewModels;

namespace Solvberget.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // Bootstrapping up some stubs while developing. Just remove these lines to start using proper implementations
            Mvx.LazyConstructAndRegisterSingleton<ISearchService, SearchServiceTemporaryStub>();
            Mvx.LazyConstructAndRegisterSingleton<IDocumentService, DocumentServiceTemporaryStub>();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
