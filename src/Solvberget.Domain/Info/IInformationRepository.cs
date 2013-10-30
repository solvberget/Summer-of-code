using System.Collections.Generic;

namespace Solvberget.Domain.Info
{
    public interface IInformationRepository
    {
        List<OpeningHoursInformation> GetOpeningHoursInformation();
        List<ContactInformation> GetContactInformation();
    }
}