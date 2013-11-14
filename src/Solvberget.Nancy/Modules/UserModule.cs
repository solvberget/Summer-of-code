using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.Security;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Users;
using Solvberget.Nancy.Authentication;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class UserModule : NancyModule
    {
        public UserModule(IRepository documents) : base("/user")
        {
            this.RequiresAuthentication();

            Get["/info"] = _ =>
            {
                UserInfo results = Context.GetUserInfo();

                var reservationsList = results.Reservations ?? new List<Reservation>();
                var finesList = results.Fines ?? new List<Fine>();
                var loansList = results.Loans ?? new List<Loan>();
                var notificationList = results.Notifications ?? new List<Notification>();
                var reservations = reservationsList.Select(r => DtoMaps.Map(r, documents));

                var fines = finesList.Select(f => new FineDto
                {
                    Date = f.Date,
                    Description = f.Description,
                    Document = DtoMaps.Map(documents.GetDocument(f.DocumentNumber, true)),
                    Status = f.Status,
                    Sum = f.Sum
                });

                var loans = loansList.Select(l => new LoanDto
                {
                    AdminisrtativeDocumentNumber = l.AdminisrtativeDocumentNumber,
                    Barcode = l.Barcode,
                    DocumentNumber = l.DocumentNumber,
                    DocumentTitle = l.DocumentTitle,
                    DueDate = ParseDateString(l.DueDate),
                    ItemSequence = l.ItemSequence,
                    ItemStatus = l.ItemStatus,
                    LoanDate = ParseDateString(l.LoanDate),
                    LoanHour = l.LoanHour,
                    Material = l.Material,
                    OriginalDueDate = ParseDateString(l.OriginalDueDate),
                    SubLibrary = l.SubLibrary
                });

                var notifications = notificationList.Select(n => new NotificationDto
                {
                     Content = n.Content,
                     DocumentNumber = n.DocumentNumber,
                     DocumentTitle = n.DocumentTitle,
                     Title = n.Title,
                     Type = n.Type
                });

                var userDto = new UserInfoDto
                {
                    BorrowerId = results.BorrowerId,
                    Balance = results.Balance,
                    CashLimit = results.CashLimit,
                    CellPhoneNumber = results.CellPhoneNumber,
                    CityAddress = results.CityAddress,
                    DateOfBirth = results.DateOfBirth,
                    Email = results.Email,
                    HomeLibrary = results.HomeLibrary,
                    HomePhoneNumber = results.HomePhoneNumber,
                    Id = results.Id,
                    IsAuthorized = results.IsAuthorized,
                    Name = results.Name,
                    PrefixAddress = results.PrefixAddress,
                    StreetAddress = results.StreetAddress,
                    Zip = results.Zip,

                    Reservations = reservations,
                    Fines = fines,
                    Loans = loans,
                    Notifications = notifications
                };

                return userDto;

            };
        }

        private static DateTime? ParseDateString(string dateString)
        {
            try
            {
                return !string.IsNullOrEmpty(dateString) ? DateTime.ParseExact(dateString, "d.M.yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
