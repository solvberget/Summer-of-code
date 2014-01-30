using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solvberget.Domain.Utils
{
    public static class RepositoryUtils
    {
        
        public static XDocument GetXmlFromStream(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;

            var response = request.GetResponse();
            
            var xml = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                using (var buffer = new BufferedStream(stream))
                {
                    using (var reader = new StreamReader(buffer, Encoding.UTF8))
                    {
                        xml = reader.ReadToEnd();
                    }
                }
            }

            //The Star-Wars-Beatles-PeerGynt-bug
            //Aleph XML-repsonses somtimes returns Unicode SOH instead of SP 
            //a few places in the document.
            //This causes the parsing to fail..
            const char soh ='\u0001';
            const char sp = '\u0020';
            var xmlEscaped = xml.Replace(soh, sp);

            return XDocument.Parse(xmlEscaped);
        }

        public static XDocument GetXmlFromStreamWithParam(string uri, string param)
        {
            var url = uri + param;
            url = url.Replace(" ", "+");

            return GetXmlFromStream(url);
        }

        public static string GetJsonFromStream(string url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            string json = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                var readStream = new StreamReader(stream, Encoding.UTF8);
                json = readStream.ReadToEnd();
            }

            return json;
        }

        public static string GetJsonFromStreamWithParam(string uri, string param)
        {
            var url = uri + param;
            url = url.Replace(" ", "+");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            
            var response = request.GetResponse();

            string json = string.Empty;
            using (var stream = response.GetResponseStream())
            {

                var readStream = new StreamReader(stream, Encoding.UTF8);
                json = readStream.ReadToEnd();

            }

            return json;
        }

    }
}

