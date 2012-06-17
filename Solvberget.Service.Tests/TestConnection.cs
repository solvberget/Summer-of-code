using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace Solvberget.Service.Tests
{
    [TestFixture]
    public class TestConnection
    {

        private string GetUrl(string function, Dictionary<string, string> options)
        {
            var sb = new StringBuilder();
            sb.Append("http://aleph.stavanger.kommune.no/X?");
            sb.Append(string.Format("op={0}", function));
            foreach (var option in options)
            {
                sb.Append(string.Format("&{0}={1}", option.Key, option.Value));
            }
            return sb.ToString();
        }

        [Test]
        public void Test_Get_Sort_codes()
        {
            //var url = "http://aleph.stavanger.kommune.no/X?op=get-sort-codes&library=NOR01";

            var function = "get-sort-codes";

            var options = new Dictionary<string, string>();

            options.Add("library", "NOR01");

            var url = GetUrl(function, options);


            var request = WebRequest.Create(url);

            var response = request.GetResponse();

            var list = LoadSortingIntoObject(response);

            if(list.Count > 0)
            {
                var first = list.FirstOrDefault();
                var test = first.fieldcode1;
            }
            list.ForEach(x => PrintDynamicProperties(x));
            Assert.AreEqual(22, list.Count);

        }

        [Test]
        public void TEST_FIND()
        {
            var url = "http://aleph.stavanger.kommune.no/X?op=get-sort-codes&library=NOR01";


            var request = WebRequest.Create(url);

            var response = request.GetResponse();

            var list = LoadSortingIntoObject(response);

            if (list.Count > 0)
            {
                var first = list.FirstOrDefault();
                var test = first.fieldcode1;
            }
            list.ForEach(x => PrintDynamicProperties(x));
            Assert.AreEqual(22, list.Count);

        }


        private List<dynamic> LoadSortingIntoObject(WebResponse response) {
            var list = new List<dynamic>();
            var doc = XDocument.Load(response.GetResponseStream());
            var nodes = from node in doc.Root.Descendants() select node;

            

            foreach (var n in nodes)
            {
                dynamic sorting = new ExpandoObject();
                foreach (var child in n.Descendants())
                {
                    var p = sorting as IDictionary<String, object>;
                    var propertyName = child.Name.LocalName.Replace("-", "");
                    p[propertyName] = child.Value.Trim();
                }

                list.Add(sorting);
            }

            return list;
        }

        private void PrintDynamicProperties(dynamic item)
        {
            foreach (KeyValuePair<string, object> kvp in item) // enumerating over it exposes the Properties and Values as a KeyValuePair
                Console.WriteLine("{0} = {1}", kvp.Key, kvp.Value);

        }


    }
}
