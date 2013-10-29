using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    class AudioBookRepository
    {
        public new static AudioBook GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var audioBook = new AudioBook();
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();
                audioBook.ClassificationNumber = MarcUtils.GetVarfield(nodes, "090", "c");
                audioBook.Numbering = MarcUtils.GetVarfield(nodes, "245", "n");

                audioBook.PartTitle = MarcUtils.GetVarfield(nodes, "245", "p");
                audioBook.Edition = MarcUtils.GetVarfield(nodes, "250", "a");
                audioBook.TypeAndNumberOfDiscs = MarcUtils.GetVarfield(nodes, "300", "a");
                audioBook.ReferredPersons = GeneratePersonsFromXml(nodes, "600");
                audioBook.ReferredOrganizations = GenerateOrganizationsFromXml(nodes, "610");
                audioBook.Subject = MarcUtils.GetVarfield(nodes, "650", "a");
                audioBook.ReferencedPlaces = MarcUtils.GetVarfieldAsList(nodes, "651", "a");
                audioBook.Genre = MarcUtils.GetVarfieldAsList(nodes, "655", "a");
                audioBook.InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                audioBook.InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");
            }
            return audioBook;
        }

        public new static AudioBook GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var audioBook = new AudioBook();
            audioBook.FillPropertiesLight(xml);
            return audioBook;
        }

        protected override void FillProperties(string xml)
        {
            
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants();

                Isbn = GetVarfield(nodes, "020", "a");

                //Author, check BSMARC field 100 for author
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                Author = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "Author"
                };

                //If N/A, check BSMARC field 110 for author
                if (Author.Name == null)
                {
                    Author.Name = GetVarfield(nodes, "110", "a");

                    //Organization (110abq)
                    Organization = GenerateOrganizationsFromXml(nodes, "110").FirstOrDefault();

                }

                //If still N/A, check BSMARC field 130 for title when title is main scheme word
                if (Author.Name == null)
                    StandarizedTitle = GetVarfield(nodes, "130", "a");
                if (Author.Name != null)
                    Author.InvertName(Author.Name);
                MainResponsible = Author;
            }
        }

        protected override string GetCompressedString()
        {
            var returnValue = Author.Name;

            if (string.IsNullOrEmpty(returnValue))
            {
                return base.GetCompressedString();
            }

            if (PublishedYear != 0)
            {
                returnValue += " (" + PublishedYear + ")";
            }

            return returnValue;
        }
    }
}
