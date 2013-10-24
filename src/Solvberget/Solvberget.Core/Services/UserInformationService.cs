using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.Services
{
    interface IUserInformationService
    {
        UserInfo GetUserInformation(string userId);
        List<Loan> GetUserLoans(string userId);
        List<Reservation> GetUserReservations(string userId);
        List<Notification> GetUserNotifications(string userId);
        List<Fine> GetUserFines(string userId);
        List<Document> GetUserFavorites(string userId);
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
                LoanDate = "01.11.2013",    
                DueDate = "01.01.2014"
            });

            return userLoans;
        }

        public List<Reservation> GetUserReservations(string userId)
        {
            var userReservations = new List<Reservation>();

            userReservations.Add(new Reservation
            {
                DocumentTitle = "Harry Potter og Fangen fra Azkaban",
                Status = "Ikke klar til henting"
            });

            userReservations.Add(new Reservation
            {
                DocumentTitle = "Harry Potter og Mysteriekammeret",
                Status = "Klar for henting hos Hovedbiblioteket"
            });

            return userReservations;
        }

        public List<Notification> GetUserNotifications(string userId)
        {
            var notifications = new List<Notification>();

            notifications.Add(new Notification
            {
                Title = "Hentemelding",
                Content = "Harry Potter og Mysteriekammeret er klar for henting hos Hovedbiblioteket"
            });

            return notifications;
        }

        public List<Fine> GetUserFines(string userId)
        {
            var fines = new List<Fine>();

            fines.Add(new Fine
            {
                Description = "Du har ikke levert boka di!",
                Sum = 50.0,
                
            });

            return fines;
        }

        public List<Document> GetUserFavorites(string userId)
        {
            var favorites = new List<Document>();

            favorites.Add(new Book
            {
                Title = "Harry Potter og De Vises Stein",
            });

            return favorites;
        }
    }
}