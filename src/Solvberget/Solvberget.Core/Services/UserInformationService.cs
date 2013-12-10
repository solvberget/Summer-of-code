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
        private readonly IStringDownloader _downloader;

        public UserInformationService(IStringDownloader downloader)
        {
            _downloader = downloader;
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

		public async Task<RequestReplyDto> AddUserFavorite(string documentNumber)
        {
            try
            {
				var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "PUT");

				return JsonConvert.DeserializeObject<RequestReplyDto>(response);
			}
            catch (Exception e)
            {
				return new RequestReplyDto{Reply = e.Message, Success = false};
            }
        }

		public async Task<RequestReplyDto> RemoveUserFavorite(string documentNumber)
        {
            try
            {
				var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Favorites + documentNumber, "DELETE");

				return JsonConvert.DeserializeObject<RequestReplyDto>(response);
			 }
            catch (Exception e)
			{
				return new RequestReplyDto{Reply = e.Message, Success = false};
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

        public async Task<RequestReplyDto> AddReservation(string documentNumber, string branch)
        {
            try
            {
                var result = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Reservations + documentNumber + "?branch=" + branch, "PUT");
                return JsonConvert.DeserializeObject<RequestReplyDto>(result);
            }
            catch (Exception e)
            {
                return new RequestReplyDto {Reply = e.Message, Success = false};
            }
        }

        public async Task<RequestReplyDto> RemoveReservation(string documentNumber, string branch)
        {
            try
            {
                var result = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Reservations + documentNumber + "?branch=" + branch, "DELETE");
                return JsonConvert.DeserializeObject<RequestReplyDto>(result);
            }
            catch (Exception e)
            {
                return new RequestReplyDto { Reply = e.Message, Success = false };
            }
        }

        public async Task<List<ReservationDto>> GetUerReservations()
        {
            try
            {

                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Reservations, "GET");

                return JsonConvert.DeserializeObject<List<ReservationDto>>(response);
            }
            catch (Exception)
            {
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
            try
            {
                var response = await _downloader.Download(Resources.ServiceUrl + Resources.ServiceUrl_Renew + documentNumber, "PUT");

                return JsonConvert.DeserializeObject<RequestReplyDto>(response);
            }
            catch (Exception)
            {
                return new RequestReplyDto
                {
                    Success = false,
                    Reply = "Feil: Klarte ikke å utvide lånetiden"
                };
            }
        }
    }
}
