using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class InformationRepositoryXml : IInformationRepository
    {

        private const string StdFilePathOpening = @"App_Data\info\opening.xml";
        private readonly string _filePathOpening;

        private const string StdFilePathContact = @"App_Data\info\contact.xml";
        private readonly string _filePathContact;
        
        public InformationRepositoryXml(string filePathOpening = null, string filePathContact = null)
        {
            _filePathOpening = string.IsNullOrEmpty(filePathOpening) ? StdFilePathOpening : filePathOpening;
            _filePathContact = string.IsNullOrEmpty(filePathContact) ? StdFilePathContact : filePathContact;
        }

        public List<OpeningHoursInformation> GetOpeningHoursInformation()
        {

            return null;
        }

        public List<ContactInformation> GetContactInformation()
        {
            var webpage = new ContactWebPage();
            webpage.FillProperties();
            return webpage.ContactInformationList;
        }

    }

}
