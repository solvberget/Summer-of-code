using System;
using System.Collections.Generic;
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
        private string GetUrl(string function, Dictionary<string, string> options)
        {
            var sb = new StringBuilder();
            sb.Append(Properties.Settings.Default.ServerUrl);
            sb.Append(string.Format("op={0}", function));
            foreach (var option in options)
            {
                sb.Append(string.Format("&{0}={1}", option.Key, option.Value));
            }
            return sb.ToString();
        }


        public List<Document> Search(string value)
        {
            var documents = new List<Document>();
            var function = "find";
            var options = new Dictionary<string, string> {{"base", "NOR01"}, {"request", value}};

            string xml = string.Empty;

            var request = WebRequest.Create(GetUrl(function, options));
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.Default);
                xml = readStream.ReadToEnd();
            }

            var doc = XDocument.Parse(xml);
            var setNumber = doc.Root.Elements("set_number").Select(x => x.Value).FirstOrDefault();
            var numberOfrecords = doc.Root.Elements("no_records").Select(x => x.Value).FirstOrDefault();
            
            if (setNumber != null)
            {
                //Todo: Implement a chech which returns wheter or not there are over 99 records (the interface does not acceppt a range higher than 99 items)
                string setEntry = string.Format("0000000001-{0}", numberOfrecords);
                function = "present";
                options = new Dictionary<string, string>
                              {{"base", "NOR01"}, {"set_number", setNumber}, {"set_entry", setEntry}};

                request = WebRequest.Create(GetUrl(function, options));
                response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    var readStream = new StreamReader(stream, Encoding.Default);
                    xml = readStream.ReadToEnd();
                }
                 doc = XDocument.Parse(xml);
                
                doc.Root.Elements("record").Select(x => x).ToList().ForEach(x => documents.Add(Document.GetDocumentFromFindDocXml(x.ToString())));
            }
            documents.RemoveAll(x => x.Title == null);
            return documents;
        }
    }
}
