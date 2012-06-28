using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Book : Document
    {

        public string Isbn { get; set; }
        public string LanguagesInBook { get; set; }
        public string ClassificationNr { get; set; }
        public Person Author { get; set; }
        public Organization Organization { get; set; }
        public string StandarizedTitle { get; set; }
        public string StdOrOrgTitle { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string NumberOfPages { get; set; }
        public string Content { get; set; }
        public IEnumerable<Person> ReferredPersons { get; set; }
        public IEnumerable<Organization> ReferredOrganizations { get; set; } 
        public IEnumerable<string> ReferencedPlaces { get; set; } 
        public IEnumerable<string> Subject { get; set; }
        public IEnumerable<string> Genre { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<Organization> InvolvedOrganizations { get; set; }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                
                Isbn = GetVarfield(nodes, "020", "a");
                ClassificationNr = GetVarfield(nodes, "090", "c");

                //Author, check BSMARC field 100 for author
                Author  = new Person
                              {
                                    Name = GetVarfield(nodes, "100", "a"),
                                    LivingYears = GetVarfield(nodes, "100", "d"),
                                    Nationality = GetVarfield(nodes, "100", "j"),
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

                StdOrOrgTitle = GetVarfield(nodes, "240", "a");
                Numbering = GetVarfield(nodes, "245", "n");
                PartTitle = GetVarfield(nodes, "245", "p");
                Edition = GetVarfield(nodes, "250", "a");
                NumberOfPages = GetVarfield(nodes, "300", "a");
                Content = GetVarfield(nodes, "505", "a");
                ReferredPersons = GeneratePersonsFromXml(nodes, "600");
                ReferredOrganizations = GenerateOrganizationsFromXml(nodes, "610");
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");
                Subject = GetVarfieldAsList(nodes, "650", "a");
                Genre = GetVarfieldAsList(nodes, "655", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");

            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {

                var nodes = xmlDoc.Root.Descendants("oai_marc");

                //Author, check BSMARC field 100 for author
                Author = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = GetVarfield(nodes, "100", "j"),
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

            }
        }

        public new static Book GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var book = new Book();

            book.FillProperties(xml);

            return book;
        }

        public new static Book GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var book = new Book();

            book.FillPropertiesLight(xml);

            return book;
        }

    }

}