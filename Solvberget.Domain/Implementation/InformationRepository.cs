using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class InformationRepository : IInformationRepository
    {

        public List<Information> GetOpeningHoursInformation()
        {
            var webpage = new OpeningHoursWebPage();
            webpage.FillProperties();
            return webpage.OpeningHoursInformationList;
        }

        public List<Information> GetContactInformation()
        {
            var webpage = new ContactWebPage();
            webpage.FillProperties();
            return webpage.ContactInformationList;
        }
    }
}
