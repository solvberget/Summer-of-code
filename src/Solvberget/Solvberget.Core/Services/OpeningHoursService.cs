﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class OpeningHoursService : IOpeningHoursService
    {
        private readonly DtoDownloader _downloader;

        public OpeningHoursService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<OpeningHoursDto>> GetOpeningHours()
        {
            var result = await _downloader.DownloadList<OpeningHoursDto>(Resources.ServiceUrl + Resources.ServiceUrl_OpeningHours);
            return result.Results;
        }
    }
}
