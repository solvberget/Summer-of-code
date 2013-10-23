using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    interface IUserInformationService
    {
        UserInfo GetUserInformation(string userId);
        List<Loan> GetUserLoans(string userId);
        List<Reservation> GetUserReservations(string userId);
    }

    internal class UserInformationService : IUserInformationService
    {
        public UserInfo GetUserInformation(string userId)
        {
            return new UserInfo
            {
                Name = "Ellen Wiig Andresen",
                DateOfBirth = "01.01.0001",
                Email = "l@n.no",
                CellPhoneNumber = "81549300",
                StreetAddress = "Veigata 9",
                CityAddress = "1234 Byen",
                Id = "STV000209626",
                HomeLibrary = "Hovedbibl."
            };
        }

        public List<Loan> GetUserLoans(string userId)
        {
            var userLoans = new List<Loan>();

            userLoans.Add(new Loan
            {
                DocumentTitle = "Ringenes Herre - Atter en konge",
                LoanDate = "01.11.2013"    
            });

            return userLoans;
        }

        public List<Reservation> GetUserReservations(string userId)
        {
            var userReservations = new List<Reservation>();

            userReservations.Add(new Reservation
            {
                DocumentTitle = "Harry Potter og De Vises Sten",
                Status = "Ikke klar til henting"
            });

            userReservations.Add(new Reservation
            {
                DocumentTitle = "Harry Potter og De Vises Sten",
                Status = "Kler for henting hos Hovedbiblioteket"
            });

            return userReservations;
        } 
    }
}