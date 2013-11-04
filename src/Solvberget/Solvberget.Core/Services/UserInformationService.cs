using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
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
            return "164916";
        }

        public async Task<UserInfoDto> GetUserInformation(string userId)
        {
            const string url = "https://dl.dropboxusercontent.com/u/19550193/User_info/164916.json";
            try
            {
                var response = await _downloader.Download(url);
                return JsonConvert.DeserializeObject<UserInfoDto>(response);
            }
            catch (Exception)
            {
                return new UserInfoDto
                {
                    Name = "Feil ved lasting, kunne desverre ikke finne brukeren. Prøv igjen senere.",
                };
            }
        }

        //public Task<List<Loan>> GetUserLoans(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<Reservation>> GetUserReservations(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<List<Notification>> GetUserNotifications(string userId)
        //{
        //    var url = "https://dl.dropboxusercontent.com/u/19550193/User_info/164916.json";

        //    try
        //    {
        //        var response = await _downloader.Download(url);
        //        return JsonConvert.DeserializeObject<List<Notification>>(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return new List<Notification>
        //        {
        //            new Notification
        //            {
        //                Title = "Ingen meldinger ble funnet",
        //                Content = e.Message
        //            }
                    
        //        };
        //    }
        //}

        //public Task<List<Fine>> GetUserFines(string userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<Document>> GetUserFavorites(string userId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}