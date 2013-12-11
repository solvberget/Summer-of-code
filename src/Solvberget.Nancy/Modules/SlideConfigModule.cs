using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using Nancy.LightningCache.Extensions;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Utils;

namespace Solvberget.Nancy.Modules
{
    public class SlideConfigModule : NancyModule
    {
        private readonly IEnvironmentPathProvider _pathProvider;

        public SlideConfigModule(IEnvironmentPathProvider pathProvider) : base("/slides")
        {
            _pathProvider = pathProvider;

            Get["/{id}"] = args =>
                {
                    var slideConfigs = GetSlideConfigurationsFromFile();
                    if (slideConfigs.ContainsKey(args.id))
                    {
                        return Response.AsJson(slideConfigs[(string)args.id]).AsCacheable(DateTime.Now.AddMinutes(20));
                    }

                    return slideConfigs.ContainsKey("default") ? Response.AsJson(slideConfigs["default"]).AsCacheable(DateTime.Now.AddMinutes(20)) : 404;
                };
        }

        private Dictionary<string, SlideConfigDto[]> GetSlideConfigurationsFromFile()
        {
            var file = _pathProvider.GetSlideConfigurationPath();

            if (!File.Exists(file)) return new Dictionary<string, SlideConfigDto[]>();
            var rawConfigJson = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<Dictionary<string, SlideConfigDto[]>>(rawConfigJson);
        }
    }
}