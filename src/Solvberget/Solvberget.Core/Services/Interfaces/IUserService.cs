using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IUserService
    {
        //string GetUserId();
        Task<UserInfoDto> GetUserInformation(string userId);
        Task<List<FavoriteDto>> GetUserFavorites();
        Task<string> AddUserFavorite(string documentNumber);
        Task<string> RemoveUserFavorite(string documentNumber);
        Task<MessageDto> Login(string userId, string userPin);
        Task<RequestReplyDto> AddReservation(string documentNumber, string branch);
        Task<RequestReplyDto> RemoveReservation(string documentNumber, string branch);
        Task<List<ReservationDto>> GetUerReservations();
        Task<List<string>> GetUserReserverdDocuments();
        Task<bool> IsFavorite(string documentNumber);
        Task<RequestReplyDto> ExpandLoan(string documentNumber);
    }
}
