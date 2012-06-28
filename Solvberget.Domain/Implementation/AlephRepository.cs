using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class AlephRepository : IRepository
    {
       

        public List<Document> Search(string value)
        {
            dynamic result = new ExpandoObject();
            const Operation function = Operation.KeywordSearch;
            var options = new Dictionary<string, string> { { "request", value } };

            var url = GetUrl(function, options);

            var doc = GetXmlFromStream(url);
               
            if (doc.Root != null)
            {
                result.SetNumber = doc.Root.Elements("set_number").Select(x => x.Value).FirstOrDefault();
                result.NumberOfRecords = doc.Root.Elements("no_records").Select(x =>  x.Value).FirstOrDefault();
            }

            return result.SetNumber != null ? GetSearchResults(result) : new List<Document>();
        }

        public Document FindDocument(string documentNumber)
        {
            const Operation function = Operation.FindDocument;
            var options = new Dictionary<string, string> { { "doc_number", documentNumber} };

            var url = GetUrl(function, options);

            var doc = GetXmlFromStream(url);

            if (doc.Root != null)
            {
                var xmlResult = doc.Root.Elements("record").Select(x => x).FirstOrDefault();
                if (xmlResult != null)
                {
                    return PopulateDocument(xmlResult, false);
                }
            }

            return null;
            
        }

        private List<Document> GetSearchResults(dynamic result)
        {
            string setEntry = string.Format("0000000001-{0}", result.NumberOfRecords);
            const Operation function = Operation.PresentSetNumber;
            var options = new Dictionary<string, string> { { "set_number", result.SetNumber }, { "set_entry", setEntry } };

            var url = GetUrl(function, options);

            var doc = GetXmlFromStream(url);

            var documents = new List<Document>();
            if (doc.Root != null)
            {
                var xmlResult = doc.Root.Elements("record").Select(x => x).ToList();
                //Populate list with light documents of correct type
                xmlResult.ForEach(x => documents.Add(PopulateDocument(x, true)));
            }
            documents.RemoveAll(x => x.Title == null);
            return documents;
        }

        private static Document PopulateDocument(XElement record, Boolean populateLight)
        {
            var xmlDoc = XDocument.Parse(record.ToString());
            var nodes = xmlDoc.Root.Descendants("oai_marc");

            var docTypeString = Document.GetVarfield(nodes, "019", "b");

            if (docTypeString != null)
            {
                var className =  GetDocumentType(docTypeString.Split(';'));

                var type = Type.GetType("Solvberget.Domain.DTO." + className);

                var methodInfo = type.GetMethod(populateLight ? "GetObjectFromFindDocXmlBSMarcLight" : "GetObjectFromFindDocXmlBSMarc");

                return (Document)methodInfo.Invoke(type, BindingFlags.InvokeMethod | BindingFlags.Default, null, new object[] { record.ToString() }, CultureInfo.CurrentCulture);

            }
            else
            {
               return Document.GetObjectFromFindDocXmlBSMarcLight(record.ToString());
            }
            
        }

        private string GetUrl(Operation function, Dictionary<string, string> options)
        {
            var sb = new StringBuilder();
            sb.Append(Properties.Settings.Default.ServerUrl);
            sb.Append(GetOperationPrefix(function));
            foreach (var option in options)
            {
                sb.Append(string.Format("&{0}={1}", option.Key, option.Value));
            }
            return sb.ToString();
        }

        private static XDocument GetXmlFromStream(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            string xml = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.UTF8);
                xml = readStream.ReadToEnd();
            }

            return XDocument.Parse(xml);
        }

        private static string GetOperationPrefix(Operation op)
        {
            switch ((int)op)
            {
                case 0:
                    return "op=item-data&base=NOR01";
                case 1:
                    return "op=present&base=NOR50";
                case 2:
                    return "op=find&base=NOR01";
                case 3:
                    return "op=find-doc&base=NOR01";
                default:
                    return null;
            }   
        }

        private enum Operation { ItemData, PresentSetNumber, KeywordSearch, FindDocument }

        private static string GetDocumentType(IEnumerable<string> documentTypeCodes)
        {
            foreach(string dtc in documentTypeCodes)
            {
                //Logic for determining DocumentType from combination of DocumentCodes
                //TODO: Generally improve and add logic for CD, Journal and Sheet music

                if (dtc.Equals("l"))
                {
                    return "Book";
                }
                else if (dtc.StartsWith("e"))
                {
                    return "Film";
                }
                else if (dtc.Equals("di"))
                {
                    return "AudioBook";
                }
            }
            
            return "Document";

        }   
    }

}
