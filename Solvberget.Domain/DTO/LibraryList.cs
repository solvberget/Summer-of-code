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
        public bool IsRanked { get; private set; }
        public List<string> DocumentNumbers { get; private set; }
        public List<Document> Documents { get; private set; } 

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
                var name = xmlDoc.Attribute("name");
                if (name != null) Name = name.Value;

                var pri = xmlDoc.Attribute("pri");
                if (pri != null)
                {
                    var priAsString = pri.Value;
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
                }

                var isRanked = xmlDoc.Attribute("ranked");
                if (isRanked != null)
                {
                    IsRanked = isRanked.Value.Equals("true") ? true : false;
                }

                xmlDoc.Elements().Where(e => e.Name == "docnumber").ToList().ForEach(element => DocumentNumbers.Add(element.Value));

            }
        }

    }
}
