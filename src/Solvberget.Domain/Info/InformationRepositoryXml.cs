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

        public InformationRepositoryXml(IEnvironmentPathProvider environment)
        {
            var filePathOpening = environment.GetOpeningInfoAsXmlPath();
            var filePathContact = environment.GetContactInfoAsXmlPath();

            _filePathOpening = string.IsNullOrEmpty(filePathOpening) ? StdFilePathOpening : filePathOpening;
            _filePathContact = string.IsNullOrEmpty(filePathContact) ? StdFilePathContact : filePathContact;
        }

        public List<OpeningHoursInformation> GetOpeningHoursInformation()
        {

            var openingHoursList = new ConcurrentBag<OpeningHoursInformation>();
            var xmlDoc = XDocument.Load(_filePathOpening);
            if (xmlDoc.Root != null)
            {
                var branches = xmlDoc.Root.Descendants("branch");
                branches.AsParallel().ToList().ForEach(branch => openingHoursList.Add(OpeningHoursInformation.GenerateFromXml(branch)));
            }

            return openingHoursList.Reverse().ToList();

        }

        public List<ContactInformation> GetContactInformation()
        {

            var contactInformationList = new ConcurrentBag<ContactInformation>();
            var xmlDoc = XDocument.Load((_filePathContact));
            if (xmlDoc.Root != null)
            {
                var infoItems = xmlDoc.Root.Descendants("info");
                infoItems.AsParallel().ToList().ForEach(infoItem => contactInformationList.Add(ContactInformation.GenerateFromXml(infoItem)));
            }

            return contactInformationList.Reverse().ToList();

        }

    }

}
