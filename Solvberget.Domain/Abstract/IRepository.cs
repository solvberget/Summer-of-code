using System.Collections.Generic;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRepository
    {
        List<Document> Search(string value);
        Document GetDocument(string documentNumber, bool isLight);
        UserInfo GetUserInformation(string userId, string verification);
        ReservationReply RequestReservation(string documentNumber, string userId, string branch);

        ReservationReply CancelReservation(string documentItemNumber, string documentItemSequence,
                                           string cancellationSequence);

    }  
    
}
