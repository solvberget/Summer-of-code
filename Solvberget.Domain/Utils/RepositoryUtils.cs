using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Solvberget.Domain.Utils
{
    public static class RepositoryUtils
    {
        public static XDocument GetXmlFromStream(string url)
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
