using System;
using Autofac;

namespace Solvberget.Nancy.Extensions
{
    public static class ComponentContextExtensions
    {
        public static void Update(this IComponentContext container, Action<ContainerBuilder> builderAction)
        {
            var builder = new ContainerBuilder();

            builderAction(builder);

            builder.Update(container.ComponentRegistry);
        }
    }
}