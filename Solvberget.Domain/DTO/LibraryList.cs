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
        public Dictionary<string, bool> DocumentNumbers { get; set; }
        //public List<string> DocumentNumbers { get; set; }
        public List<Document> Documents { get; set; } 

        private LibraryList()
        {
            DocumentNumbers = new Dictionary<string, bool>();
            Documents = new List<Document>();
        }

        public static LibraryList GetLibraryListFromXmlFile(string xmlFilePath)
        {
            var xmlDoc = XElement.Load(xmlFilePath);
            var libraryList = new LibraryList();
            libraryList.FillProperties(xmlDoc);
            return libraryList;
        }

        public static LibraryList GetLibraryListFromXml(XElement xml)
        {
            var libraryList = new LibraryList();
            libraryList.FillProperties(xml);
            return libraryList;
        }

        private void FillProperties(XElement xml)
        {
            
            if (xml.Attribute("name") == null)
                return;
            else
            {
                var name = xml.Attribute("name");
                if (name != null) Name = name.Value;

                var pri = xml.Attribute("pri");
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

                var isRanked = xml.Attribute("ranked");
                if (isRanked != null)
                {
                    IsRanked = isRanked.Value.Equals("true") ? true : false;
                }

                xml.Elements().Where(e => e.Name == "docnumber").ToList().ForEach(element => DocumentNumbers.Add(element.Value, false));

            }
        }

    }
}
