using Nancy;
using Solvberget.Domain.Aleph;

namespace Solvberget.Nancy.Modules
{
    public class UserModule : NancyModule
    {
        public UserModule(IRepository repository) : base("/user")
        {
            Get["/{userId}/info"] = args => repository.GetUserInformation(args.userId, Request.Query.verify);
            
            Get["/{userId}/pin"] = args => repository.RequestPinCodeToSms(args.userId);
        }
    }
}
