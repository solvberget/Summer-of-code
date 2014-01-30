using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IUserService
    {
        //string GetUserId();
		Task<UserInfoDto> GetUserInformation(bool ignoreLoginRedirect = false);
        Task<List<FavoriteDto>> GetUserFavorites();
		Task<RequestReplyDto> AddUserFavorite(string documentNumber);
		Task<RequestReplyDto> RemoveUserFavorite(string documentNumber);
        Task<MessageDto> Login(string userId, string userPin);
        Task<RequestReplyDto> AddReservation(string documentNumber, string branch);
        Task<RequestReplyDto> RemoveReservation(string documentNumber, string branch);
        Task<List<ReservationDto>> GetUerReservations();
        Task<List<string>> GetUserReserverdDocuments();
        Task<bool> IsFavorite(string documentNumber);
        Task<RequestReplyDto> ExpandLoan(string documentNumber);
        Task<RequestReplyDto> RequestPinCode(string userId);
    }
}
