using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    interface IUserInformationService
    {
        UserInfo GetUserInformation(string userId);
    }

    class UserInformationService : IUserInformationService
    {
        public UserInfo GetUserInformation(string userId)
        {
            return new UserInfo()
            {
                Name = "Ellen Wiig Andresen"
            };
        }
    }
}
