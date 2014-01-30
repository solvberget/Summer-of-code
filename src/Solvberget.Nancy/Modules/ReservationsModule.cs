using System;
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
    public class ReservationsModule : NancyModule
    {
        public ReservationsModule(IRepository repository)
            : base("/reservations")
        {
            this.RequiresAuthentication();

            Get["/"] = _ =>
            {
                var user = Context.GetAlephUserIdentity();

                var reservations = repository.GetUserInformation(user.UserName, user.Password).Reservations;
                if (reservations == null)
                {
                    return new ReservationDto[0];
                }
                return reservations.Select(r => DtoMaps.Map(r, repository)).ToArray();
            };

            Delete["/{documentId}"] = args =>
            {
                var user = Context.GetAlephUserIdentity();

                var userReservations = repository.GetUserInformation(user.UserName, user.Password).Reservations;
                var resToRemove = userReservations.FirstOrDefault(r => r.DocumentNumber == args.documentId);

                if (resToRemove != null)
                {
                    var response = repository.CancelReservation(resToRemove.ItemDocumentNumber, resToRemove.ItemSeq, resToRemove.CancellationSequence);
                    if (response.Success) Context.RequireUserInfoRefresh();

                    return response;
                }

                return "Kunne ikke finne dokument i liste over reservasjoner";
            };

            Put["/{documentId}"] = args =>
            {
                string branch = Request.Form.branch.HasValue ? Request.Form.branch : Request.Query.branch;

                var response = repository.RequestReservation(args.documentId, Context.GetUserInfo().Id, branch);
                if (response.Success) Context.RequireUserInfoRefresh();

                return response;
            };
        }
    }
}