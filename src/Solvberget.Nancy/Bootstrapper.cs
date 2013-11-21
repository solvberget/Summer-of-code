using System.Collections.Generic;
using Autofac;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.LightningCache.Extensions;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
using Nancy.Routing;
using Nancy.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Favorites;
using Solvberget.Domain.Lists;
using Solvberget.Domain.Utils;
using Solvberget.Nancy.Authentication;
using Solvberget.Nancy.Extensions;

namespace Solvberget.Nancy
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        public static ILifetimeScope Container { get; private set; }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(OverrideDefaultConfiguration); }
        }
        
        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            /*enable lightningcache, vary by url params id,query,take and skip*/
            this.EnableLightningCache(container.Resolve<IRouteResolver>(), ApplicationPipelines, new[] { "id", "query", "take", "skip" });

            var statelessAuthConfiguration = new StatelessAuthenticationConfiguration(ctx => container.Resolve<NancyContextAuthenticator>().Authenticate(ctx));

            StatelessAuthentication.Enable(pipelines, statelessAuthConfiguration);

            pipelines.OnError.AddItemToEndOfPipeline((z, a) =>
            {
                while (null != a.InnerException) a = a.InnerException;

                return new TextResponse(a.Message) {StatusCode = HttpStatusCode.InternalServerError};
            });

        }
        
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            Container = container;

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            

            container.Update(builder =>
            {
                builder.Register(c => JsonSerializer.Create(jsonSettings))
                    .SingleInstance();

                builder.RegisterAssemblyTypes(typeof(AlephRepository).Assembly)
                    .AsImplementedInterfaces();

                builder.RegisterType<EnvironmentPathProvider>()
                    .As<IEnvironmentPathProvider>()
                    .SingleInstance();


                builder.RegisterType<FavoritesRepository>().As<IFavoritesRepository>().SingleInstance(); // todo: singleton while its impl only keeps favorite state in-memory.

                builder.RegisterType<AlephAuthenticationProvider>().As<IAuthenticationProvider>();
                builder.RegisterType<NancyContextAuthenticator>().AsSelf().SingleInstance();

                builder.RegisterType<LibraryListDynamicRepository>().AsSelf();
                builder.RegisterType<LibraryListXmlRepository>().AsSelf();
            });
        }

        private static void OverrideDefaultConfiguration(NancyInternalConfiguration config)
        {
            config.ResponseProcessors = new[] { typeof(JsonProcessor) };
        }
    }
}