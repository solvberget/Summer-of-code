using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class UserInformationService : IUserService
    {
        
        private readonly IStringDownloader _downloader;

        public UserInformationService(IStringDownloader downloader)
        {
             _downloader = downloader;
        }

        public string GetUserId()
        {
            //return "N000708254";
            //return "123456";
            return "164916";
        }

        public async Task<UserInfoDto> GetUserInformation(string userId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_UserInfo, GetUserId()));
                return JsonConvert.DeserializeObject<UserInfoDto>(response);
            }
            catch (Exception e)
            {
                return new UserInfoDto
                {
                    Name = e.Message
                    //Name = "Feil ved lasting, kunne desverre ikke finne brukeren. Prøv igjen senere.",
                };
            }
        }
    }
}