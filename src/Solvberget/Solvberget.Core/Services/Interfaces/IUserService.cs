using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.DTOs.Deprecated.DTO;

namespace Solvberget.Core.Services.Interfaces
{
    public interface IUserService
    {
        string GetUserId();
        Task<UserInfoDto> GetUserInformation(string userId);
        Task<List<FavoriteDto>> GetUserFavorites();
        Task<string> AddUserFavorite(string documentNumber);
        Task<string> RemoveUserFavorite(string documentNumber);
    }
}
