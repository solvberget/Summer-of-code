using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class UserInformationService : IUserService
    {
        
        private IStringDownloader _downloader;

        public UserInformationService(IStringDownloader downloader)
        {
             _downloader = downloader;
        }

        public string GetUserId()
        {
            return "164916";
        }

        public async Task<UserInfo> GetUserInformation(string userId)
        {
            var url = "https://dl.dropboxusercontent.com/u/19550193/User_info/164916.json";
            try
            {
                var response = await _downloader.Download(url);
                return JsonConvert.DeserializeObject<UserInfo>(response);
            }
            catch (Exception)
            {
                return new UserInfo
                {
                    Name = "Feil ved lasting, kunne desverre ikke finne brukeren. Prøv igjen senere.",
                };
            }
        }

        public Task<List<Loan>> GetUserLoans(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reservation>> GetUserReservations(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetUserNotifications(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Fine>> GetUserFines(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Document>> GetUserFavorites(string userId)
        {
            throw new NotImplementedException();
        }
    }
}