using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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

            var openingHoursList = new ConcurrentBag<OpeningHoursInformation>();
            var xmlDoc = XDocument.Load(_filePathOpening);
            if (xmlDoc.Root != null)
            {
                var locations = xmlDoc.Root.Descendants("locations");
                locations.AsParallel().ToList().ForEach(location => openingHoursList.Add(OpeningHoursInformation.GenerateFromXml(location)));
            }
            
            return openingHoursList.ToList();

        }

        public List<ContactInformation> GetContactInformation()
        {
            var webpage = new ContactWebPage();
            webpage.FillProperties();
            return webpage.ContactInformationList;
        }

    }

}
