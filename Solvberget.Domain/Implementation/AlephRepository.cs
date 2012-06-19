using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class AlephRepository : IRepository
    {
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


        public List<Document> Search(string value)
        {
            dynamic result = new ExpandoObject();
            const Operation function =  Operation.KeywordSearch;
            var options = new Dictionary<string, string> { { "request", value } };

            string xml = string.Empty;

            var request = WebRequest.Create(GetUrl(function, options));
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    var readStream = new StreamReader(stream, Encoding.Default);
                    xml = readStream.ReadToEnd();
                }
            }
            var doc = XDocument.Parse(xml);
            if (doc.Root != null)
            {
                result.SetNumber = doc.Root.Elements("set_number").Select(x => x.Value).FirstOrDefault();
                result.NumberOfRecords = doc.Root.Elements("no_records").Select(x => x.Value).FirstOrDefault();
            }

            return result.SetNumber != null ? GetSearchResults(result) : new List<Document>();
        }

        private List<Document> GetSearchResults(dynamic result)
        {

            string setEntry = string.Format("0000000001-{0}", result.NumberOfRecords);
            const Operation function = Operation.PresentSetNumber;
            var options = new Dictionary<string, string> { { "set_number", result.SetNumber }, { "set_entry", setEntry } };

            var request = WebRequest.Create(GetUrl(function, options));
            var response = request.GetResponse();
            string xml = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.Default);
                xml = readStream.ReadToEnd();
            }
            var doc = XDocument.Parse(xml);

            var documents = new List<Document>();
            if (doc.Root != null)
            {
                var xmlResult = doc.Root.Elements("record").Select(x => x).ToList();
                //Todo: Implement checking of document type and create appropriate document
                xmlResult.ForEach(x => documents.Add(Document.GetDocumentFromFindDocXml(x.ToString())));
            }
            documents.RemoveAll(x => x.Title == null);
            return documents;
        }

        private static string GetOperationPrefix(Operation op)
        {
            switch ((int) op)
            {
                case 0:
                    return "op=item-data&base=NOR01";
                case 1:
                    return "op=present&base=NOR50";
                case 2:
                    return "op=find&base=NOR01";
                default:
                    return null;
            }
        }

        private enum Operation { ItemData = 0, PresentSetNumber, KeywordSearch }
    }
}
