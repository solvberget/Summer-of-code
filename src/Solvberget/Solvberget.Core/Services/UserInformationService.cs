using System;
using System.Collections.Generic;
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
        private readonly IUserAuthenticationDataService _userAuthenticationService;

        public UserInformationService(IStringDownloader downloader, IUserAuthenticationDataService userAuthenticationService)
        {
             _downloader = downloader;
            _userAuthenticationService = userAuthenticationService;
        }

        public async Task<UserInfoDto> GetUserInformation(string userId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_UserInfo);
                return JsonConvert.DeserializeObject<UserInfoDto>(response);
            }
            catch (Exception e)
            {
                return new UserInfoDto
                {
                    //Name = "Feil ved lasting, kunne desverre ikke finne brukeren. Prøv igjen senere.",
                    Name = e.Message
                };
            }
        }

        public async Task<List<FavoriteDto>> GetUserFavorites()
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Favorites);
                return JsonConvert.DeserializeObject<List<FavoriteDto>>(response);
            }
            catch (Exception)
            {
                return new List<FavoriteDto>
                {
                    new FavoriteDto
                    {
                        Document = new DocumentDto
                        {
                            Title = "Feil ved lasting, kunne desverre ikke finne listen. Prøv igjen senere.",
                        }
                    }
                };
            }
        }

        public async Task<string> AddUserFavorite(string documentNumber)
        {
            try
            {
                return await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "PUT");
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> RemoveUserFavorite(string documentNumber)
        {
            try
            {
                return await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "DELETE");
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<MessageDto> Login(string userId, string userPin)
        {
            try
            {
                var formData = new Dictionary<string, string>
                {
                    {"Username", userId},
                    {"Password", userPin}
                };



                var response = await _downloader.PostForm(Resources.ServiceUrl + Resources.ServiceUrl_Login, formData);

                return JsonConvert.DeserializeObject<MessageDto>(response);
            }
            catch (Exception e)
            {
                return new MessageDto
                {
                    Message = e.Message
                };
            }
        }

        public Task<string> AddReservation(string userId, string documentNumber)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveReservation(string documentNumber)
        {
            throw new NotImplementedException();
        }
    }
}