using System.Collections.Generic;

using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IInformationRepository
    {
        List<OpeningHoursInformation> GetOpeningHoursInformation();
        List<ContactInformation> GetContactInformation();
    }
}