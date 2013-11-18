using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class ContactInformationService : IContactInformationService
    {
        private readonly IStringDownloader _downloader;

        public ContactInformationService(IStringDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task<IList<ContactInfoDto>> GetContactInfo()
        {
            try
            {
                var json = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Contact);
                return JsonConvert.DeserializeObject<List<ContactInfoDto>>(json);
            }
            catch (Exception)
            {
                return new List<ContactInfoDto>
                {
                    new ContactInfoDto
                    {
                        Title = "Klarte desverre ikke å hente kontaktinformasjon"
                    }
                };
            }
        }
    }
}