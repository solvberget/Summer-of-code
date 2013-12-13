using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class UserInformationService : IUserService
    {
        private readonly DtoDownloader _downloader;
        private readonly IStringDownloader _rawHttp;

        public UserInformationService(DtoDownloader downloader, IStringDownloader stringDownloader)
        {
            _downloader = downloader;
            _rawHttp = stringDownloader;
        }

		public async Task<UserInfoDto> GetUserInformation(bool ignoreLoginRedirect = false)
        {
			return await _downloader.Download<UserInfoDto>(Resources.ServiceUrl + Resources.ServiceUrl_UserInfo, ignoreError: ignoreLoginRedirect);
        }

        public async Task<List<FavoriteDto>> GetUserFavorites()
        {
            var result = await _downloader.DownloadList<FavoriteDto>(Resources.ServiceUrl + Resources.ServiceUrl_Favorites);
            if (result.Success) return result.Results;

            return new List<FavoriteDto>{new FavoriteDto
            {
                Document = new DocumentDto
                {
                    Title = "Feil ved lasting, kunne desverre ikke finne listen. Prøv igjen senere.",
                }
            }};
        }

		public async Task<RequestReplyDto> AddUserFavorite(string documentNumber)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "PUT");
        }

		public async Task<RequestReplyDto> RemoveUserFavorite(string documentNumber)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "DELETE");
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

                var response = await _rawHttp.PostForm(Resources.ServiceUrl + Resources.ServiceUrl_Login, formData);
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

        public async Task<RequestReplyDto> AddReservation(string documentNumber, string branch)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + Resources.ServiceUrl_Reservations + documentNumber + "?branch=" + branch, "PUT");
        }

        public async Task<RequestReplyDto> RemoveReservation(string documentNumber, string branch)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + Resources.ServiceUrl_Reservations + documentNumber + "?branch=" + branch, "DELETE");
        }

        public async Task<List<ReservationDto>> GetUerReservations()
        {
            var result = await _downloader.DownloadList<ReservationDto>(Resources.ServiceUrl + Resources.ServiceUrl_Reservations);

            if (result.Success) return result.Results;

            return new List<ReservationDto>
                {
                    new ReservationDto
                    {
                        Document = new DocumentDto
                        {
                            Title = "Ingen reservasjoner",
                            Id = ""
                        }
                    }
                };
        }

        public async Task<List<string>> GetUserReserverdDocuments()
        {
            var reservations = await GetUerReservations();

            var docs = reservations.Select(r => r.Document.Id).ToList();

            return docs;
        }

        public async Task<bool> IsFavorite(string documentNumber)
        {
            var favs = await GetUserFavorites();
            var docIds = favs.Select(fav => fav.Document.Id).ToList();
            
            return docIds.Contains(documentNumber);
        }

        public async Task<RequestReplyDto> ExpandLoan(string documentNumber)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + Resources.ServiceUrl_Renew + documentNumber, "PUT");
        }

        public async Task<RequestReplyDto> RequestPinCode(string userId)
        {
            return await _downloader.Download<RequestReplyDto>(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_RequestPin, userId));
        }
    }
}
