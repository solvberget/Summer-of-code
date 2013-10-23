using Solvberget.Domain.DTO;
using Solvberget.Domain;

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
                Name = "Ellen Wiig Andresen",
                DateOfBirth = "01.01.01",
                Email = "l@n.no",
                CellPhoneNumber = "81549300",
                StreetAddress = "Veigata 9",
                CityAddress = "1234 Byen"
            };
        }
    }
}
