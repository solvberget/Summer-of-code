using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRepository
    {
        List<Document> Search(string value);
        Document GetDocument(string documentNumber, bool isLight);
        UserInfo GetUserInformation(string userId, string verification);
        RequestReply RequestRenewalOfLoan(string documentNumber, string itemSecq, string barcode, string libraryUserId);
        RequestReply RequestReservation(string documentNumber, string userId, string branch);
        RequestReply CancelReservation(string documentItemNumber, string documentItemSequence, string cancellationSequence);
        RequestReply RequestPinCodeToSms(string userId);
    }
}