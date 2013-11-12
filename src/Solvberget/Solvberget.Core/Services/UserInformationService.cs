using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.File;
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
        private MvxFileStore _fileStore;

        public UserInformationService(IStringDownloader downloader, IUserAuthenticationDataService userAuthenticationService)
        {
             _downloader = downloader;
            _userAuthenticationService = userAuthenticationService;
        }

        //public string GetUserId()
        //{

        //    //var userId = "164916";

        //    //_fileStore.WriteFile("/Solvberget/userId", userId);

        //    var readId = "";

        //    //var read = _fileStore.TryReadTextFile("", out readId);
        //    //return "N000708254";
        //    //return "123456";
        //    //return "164916";
        //    return readId;
        //}

        public async Task<UserInfoDto> GetUserInformation(string userId)
        {
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_UserInfo, _userAuthenticationService.GetUserId()));
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

        public async Task<string> Login(string userId, string userPin)
        {
            try
            {
                var result =  await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Login, "POST");

                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}