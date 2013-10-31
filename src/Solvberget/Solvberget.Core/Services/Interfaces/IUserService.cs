using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs.Deprecated.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IUserService
    {
        string GetUserId();
        Task<UserInfo> GetUserInformation(string userId);
        Task<List<Loan>> GetUserLoans(string userId);
        Task<List<Reservation>> GetUserReservations(string userId);
        Task<List<Notification>> GetUserNotifications(string userId);
        Task<List<Fine>> GetUserFines(string userId);
        Task<List<Document>> GetUserFavorites(string userId);
    }
}
