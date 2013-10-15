using Autofac;

using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Responses.Negotiation;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Solvberget.Domain.Abstract;
using Solvberget.Domain.Implementation;

namespace Solvberget.Nancy
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(OverrideDefaultConfiguration); }
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
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
            });
        }

        private static void OverrideDefaultConfiguration(NancyInternalConfiguration config)
        {
            config.ResponseProcessors = new[] { typeof(JsonProcessor) };
        }
    }
}