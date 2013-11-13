using System;
using System.Linq;
using Nancy;
using Nancy.Security;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Users;
using Solvberget.Nancy.Authentication;

namespace Solvberget.Nancy.Modules
{
    public class ReservationsModule : NancyModule
    {
        public ReservationsModule(IReservationsRepository reservations, IRepository documents)
            : base("/reservations")
        {
            this.RequiresAuthentication();

            Get["/"] = _ => reservations.GetReservations(Context.GetUserInfo()).Select(MapToDto).ToArray();

            Delete["/{documentId}"] = args =>
            {
                var document = documents.GetDocument(args.documentId, true);
                reservations.RemoveReservation(document, Context.GetUserInfo());

                return new { Success = true };
            };

            Put["/{documentId}"] = args =>
            {
                var document = documents.GetDocument(args.documentId, true);
                reservations.AddReservation(document, Context.GetUserInfo());

                return new { Success = true };
            };
        }
        private ReservationDto MapToDto(Reservation reservation)
        {
            DateTime holdEnd;
            DateTime holdFrom;
            DateTime holdTo;

            DateTime.TryParse(reservation.HoldRequestEnd, out holdEnd);
            DateTime.TryParse(reservation.HoldRequestFrom, out holdFrom);
            DateTime.TryParse(reservation.HoldRequestTo, out holdTo);

            return new ReservationDto
            {
                DocumentNumber = reservation.DocumentNumber,
                DocumentTitle = reservation.DocumentTitle,
                HoldRequestEnd = holdEnd,
                HoldRequestFrom = holdFrom,
                HoldRequestTo = holdTo,
                ItemDocumentNumber = reservation.ItemDocumentNumber,
                ItemSeq = reservation.ItemSeq,
                PickupLocation = reservation.PickupLocation,
                Status = reservation.Status,
                CancellationSequence = reservation.CancellationSequence
            };
        }
    }
}