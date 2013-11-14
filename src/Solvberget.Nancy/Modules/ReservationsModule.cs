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
                var pin = Request.Headers.Authorization.Split(':')[1];
                var userName = Request.Headers.Authorization.Split(':')[0];

                return repository.GetUserInformation(userName, pin).Reservations.Select(r => DtoMaps.Map(r, repository)).ToArray();
            };

            Delete["/{documentId}"] = args =>
            {
                var pin = Request.Headers.Authorization.Split(':')[1];
                var userName = Request.Headers.Authorization.Split(':')[0];
                var userReservations = repository.GetUserInformation(userName, pin).Reservations;
                var resToRemove = userReservations.FirstOrDefault(r => r.DocumentNumber == args.documentId);

                if (resToRemove != null)
                    return repository.CancelReservation(resToRemove.ItemDocumentNumber, resToRemove.ItemSeq, resToRemove.CancellationSequence);
                
                return "Kunne ikke finne dokument i liste over reservasjoner";
            };

            Put["/{documentId}"] = args =>
            {
                //Hvordan få med andeling (=branch)?
                var response = repository.RequestReservation(args.documentId, Context.GetUserInfo().Id, "Hovedbibl.");

                return response;
            };
        }

        
    }
}