using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class InformationRepositoryHtml : IInformationRepository
    {

        public List<OpeningHoursInformation> GetOpeningHoursInformation()
        {
            var webpage = new OpeningHoursWebPage();
            webpage.FillProperties();
            return webpage.OpeningHoursInformationList;
        }

        public List<ContactInformation> GetContactInformation()
        {
            var webpage = new ContactWebPage();
            webpage.FillProperties();
            return webpage.ContactInformationList;
        }
    }
}
