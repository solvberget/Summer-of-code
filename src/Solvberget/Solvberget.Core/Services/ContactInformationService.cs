using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.Properties;

namespace Solvberget.Core.Services
{
    public class ContactInformationService : IContactInformationService
    {
        private readonly DtoDownloader _downloader;

        public ContactInformationService(DtoDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<ContactInfoDto>> GetContactInfo()
        {
            var result = await _downloader.DownloadList<ContactInfoDto>(Resources.ServiceUrl + Resources.ServiceUrl_Contact);
            return result.Results;
        }
    }
}