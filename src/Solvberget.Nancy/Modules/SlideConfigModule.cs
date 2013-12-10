using System;
using System.Collections.Generic;
using Nancy;
using Nancy.LightningCache.Extensions;
using Solvberget.Core.DTOs;

namespace Solvberget.Nancy.Modules
{
    public class SlideConfigModule : NancyModule
    {
        public SlideConfigModule() : base("/slides")
        {
            var slideConfigs = new Dictionary<string, SlideConfigDto[]>
                {
                    {
                        "PBZJGT", new[]
                            {
                                new SlideConfigDto { Template = "views/screen_news.html", Duration = 3000},
                                new SlideConfigDto { Template = "views/screen_events.html", Duration = 3000}
                            }
                    },
                    {
                        "default", new[]
                            {
                                new SlideConfigDto { Template = "views/screen_news.html", Duration = 6000},
                                new SlideConfigDto { Template = "views/screen_events.html", Duration = 6000}
                            }
                    }
                };

            // TODO: Implement persistence of data.
            Get["/{id}"] = args =>
                {
                    if (slideConfigs.ContainsKey(args.id))
                    {
                        return Response.AsJson(slideConfigs[(string)args.id]).AsCacheable(DateTime.Now.AddMinutes(20));
                    }

                    return slideConfigs.ContainsKey("default") ? Response.AsJson(slideConfigs["default"]).AsCacheable(DateTime.Now.AddMinutes(20)) : 404;
                };
        }
    }
}