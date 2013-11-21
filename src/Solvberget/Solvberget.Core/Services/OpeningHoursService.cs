﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class OpeningHoursService : IOpeningHoursService
    {
        private readonly IStringDownloader _downloader;

        public OpeningHoursService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<OpeningHoursDto>> GetOpeningHours()
        {
            try
            {
                var openingHoursJson =
                    await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_OpeningHours);
                return JsonConvert.DeserializeObject<List<OpeningHoursDto>>(openingHoursJson);
            }
            catch
            {
                return new List<OpeningHoursDto>
                {
                    new OpeningHoursDto
                    {
                        Title = "Ingen åpningstider",
                        Hours = new OpeningHourInfoDto[0]
                    }
                };
            }
        }
    }
}
