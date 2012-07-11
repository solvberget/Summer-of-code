using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class LibraryList
    {
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public List<string> DocumentNumbers { get; private set; }
        public List<Document> Documents { get; set; } 

        private LibraryList()
        {
            DocumentNumbers = new List<string>();
            Documents = new List<Document>();
        }

        public static LibraryList GetLibraryListFromXml(string xmlFilePath)
        {
            var libraryList = new LibraryList();
            libraryList.FillProperties(xmlFilePath);
            return libraryList;
        }

        private void FillProperties(string xmlFilePath)
        {
            var xmlDoc = XElement.Load(xmlFilePath);
            if (xmlDoc.Attribute("name") == null)
                return;
            else
            {
                Name = xmlDoc.Attribute("name").Value;

                var priAsString = xmlDoc.Attribute("pri").Value;
                int priAsInt; 
                if(int.TryParse(priAsString, out priAsInt))
                {
                    //Set a lowest priority if priority range is invalid (below 0 or above 10) 
                    Priority = priAsInt < 1 || priAsInt > 10 ? 10 : priAsInt;
                }
                else
                {
                    //Set a lowest priority if null or wrong type
                    Priority = 10;
                }
                
                xmlDoc.Elements().Where(e => e.Name == "docnumber").ToList().ForEach(element => DocumentNumbers.Add(element.Value));

            }
        }

    }
}
