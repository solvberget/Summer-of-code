using System.Linq;
using Nancy;
using Nancy.Security;
using Solvberget.Domain.Aleph;
using Solvberget.Nancy.Authentication;

namespace Solvberget.Nancy.Modules
{
    public class RenewalsModule : NancyModule
    {
        public RenewalsModule(IRepository repository) : base("/documents/renew")
        {
            this.RequiresAuthentication();

            Put["/{documentId}"] = args =>
            {
                var loan = Context.GetUserInfo().Loans.FirstOrDefault(l => l.DocumentNumber == args.documentId);

                if(null == loan) return Response.AsJson(new { message = "Du har ikke lånt denne boken."}, HttpStatusCode.BadRequest);

                var response = repository.RequestRenewalOfLoan(loan.DocumentNumber, loan.ItemSequence, loan.Barcode, Context.GetUserInfo().Id);

                if (response.Success) Context.RequireUserInfoRefresh();
                
                return response;
            };
        }
    }
}