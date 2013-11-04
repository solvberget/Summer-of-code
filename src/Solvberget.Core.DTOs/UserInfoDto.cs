using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public bool IsAuthorized { get; set; }
        public string BorrowerId { get; set; }
        public string Name { get; set; }
        public string PrefixAddress { get; set; }
        public string StreetAddress { get; set; }
        public string CityAddress { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string CashLimit { get; set; }
        public string HomeLibrary { get; set; }
        public string Balance { get; set; }
        public IEnumerable<FineDto> Fines { get; set; }
        public IEnumerable<FineDto> ActiveFines { get; set; }
        public IEnumerable<LoanDto> Loans { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
        public IEnumerable<NotificationDto> Notifications { get; set; }
    }
}
