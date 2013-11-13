using System.Collections.Generic;

namespace Solvberget.Domain.Users
{
    public interface IReservationsRepository
    {
        IEnumerable<Reservation> GetReservations(UserInfo user);
        void AddReservation(Reservation res, UserInfo user);
        void RemoveReservation(Reservation res, UserInfo user);
        bool IsReserved(Reservation res, UserInfo user);
    }
}
