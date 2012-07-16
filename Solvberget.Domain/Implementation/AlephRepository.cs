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
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    public class AlephRepository : IRepository
    {

        private readonly StorageHelper _storageHelper;

        public AlephRepository(string pathToImageCache)
        {
            _storageHelper = new StorageHelper(pathToImageCache);
        }

        public List<Document> Search(string value)
        {
            dynamic result = new ExpandoObject();
            const Operation function = Operation.KeywordSearch;
            var options = new Dictionary<string, string> { { "request", value } };

            var url = GetUrl(function, options);

            var doc = RepositoryUtils.GetXmlFromStream(url);
               
            if (doc.Root != null)
            {
                result.SetNumber = doc.Root.Elements("set_number").Select(x => x.Value).FirstOrDefault();
                result.NumberOfRecords = doc.Root.Elements("no_records").Select(x =>  x.Value).FirstOrDefault();
            }

            return result.SetNumber != null ? GetSearchResults(result) : new List<Document>();
        }

        public Document GetDocument(string documentNumber, bool isLight)
        {
            const Operation function = Operation.FindDocument;
            var options = new Dictionary<string, string> { { "doc_number", documentNumber} };

            var url = GetUrl(function, options);

            var doc = RepositoryUtils.GetXmlFromStream(url);

            if (doc != null && doc.Root != null)
            {
                var xmlResult = doc.Root.Elements("record").Select(x => x).FirstOrDefault();
                if (xmlResult != null)
                {
                    var docToReturn = PopulateDocument(xmlResult, isLight);
                    //We add the number here because it is not in the result 
                    //when getting the document it self from Aleph
                    docToReturn.DocumentNumber = documentNumber;

                    docToReturn.ThumbnailUrl = _storageHelper.GetLocalImageFileCacheUrl(docToReturn.DocumentNumber, true);

                    if (!isLight)
                        docToReturn.ImageUrl = _storageHelper.GetLocalImageFileCacheUrl(docToReturn.DocumentNumber, false);

                    
                    return docToReturn;
                }
            }

            return null;
            
        }

        public List<Document> GetDocumentsLight(IEnumerable<string> docNumbers)
        {
            return (from docNumber in docNumbers let doc = GetDocument(docNumber, true) where doc != null select GetDocument(docNumber, true)).ToList();
        }


        public UserInfo GetUserInformation( string userId, string verification )
        {
            
            var user = new UserInfo();
            AuthenticateUser(ref user, userId, verification);

            return user;

        }

        private bool AuthenticateUser ( ref UserInfo user, string userId, string verification )
        {

            const Operation function = Operation.AuthenticateUser;
            var options = new Dictionary<string, string> { { "bor_id", userId }, {"verification", verification} };

            var url = GetUrl(function, options);
            var authenticationDoc = RepositoryUtils.GetXmlFromStream(url);

            if (authenticationDoc.Root != null)
            {

                var xElement = authenticationDoc.Root.DescendantsAndSelf("z303").FirstOrDefault();
                user.IsAuthorized = xElement != null;
 
            }

            return user.IsAuthorized;
        }


        private List<Document> GetSearchResults(dynamic result)
        {
            string setEntry = string.Format("0000000001-{0}", result.NumberOfRecords);
            const Operation function = Operation.PresentSetNumber;
            var options = new Dictionary<string, string> { { "set_number", result.SetNumber }, { "set_entry", setEntry } };

            var url = GetUrl(function, options);

            var doc = RepositoryUtils.GetXmlFromStream(url);

            var documents = new List<Document>();
            if (doc.Root != null)
            {
                var xmlResult = doc.Root.Elements("record").Select(x => x).ToList();
                //Populate list with light documents of correct type
                xmlResult.ForEach(x => documents.Add(PopulateDocument(x, true)));
            }
            documents.RemoveAll(x => x.Title == null);
            documents.ForEach(d => d.ThumbnailUrl = _storageHelper.GetLocalImageFileCacheUrl(d.DocumentNumber, true));
            
            return documents;
        }

        private static Document PopulateDocument(XElement record, bool populateLight)
        {
            var xmlDoc = XDocument.Parse(record.ToString());
            var nodes = xmlDoc.Root.Descendants("oai_marc");

            var docTypeString = Document.GetVarfield(nodes, "019", "b");

            if (docTypeString != null)
            {
                var className =  GetDocumentType(docTypeString.Split(','));

                var type = Type.GetType(className);

                var methodInfo = type.GetMethod(populateLight ? "GetObjectFromFindDocXmlBsMarcLight" : "GetObjectFromFindDocXmlBsMarc");

                return (Document)methodInfo.Invoke(type, BindingFlags.InvokeMethod | BindingFlags.Default, null, new object[] { record.ToString() }, CultureInfo.CurrentCulture);

            }
            else
            {
               return Document.GetObjectFromFindDocXmlBsMarcLight(record.ToString());
            }
            
        }

        private string GetUrl(Operation function, Dictionary<string, string> options)
        {
            var sb = new StringBuilder();
            sb.Append(Properties.Settings.Default.AlephServerUrl);
            sb.Append(GetOperationPrefix(function));
            foreach (var option in options)
            {
                sb.Append(string.Format("&{0}={1}", option.Key, option.Value));
            }
            return sb.ToString();
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
                case 4:
                    return "op=bor-auth&library=nor50";
                default:
                    return null;
            }   
        }

        private enum Operation { ItemData, PresentSetNumber, KeywordSearch, FindDocument, AuthenticateUser }

        private static string GetDocumentType(IEnumerable<string> documentTypeCodes)
        {
            foreach(string dtc in documentTypeCodes)
            {
                //Logic for determining DocumentType from combination of DocumentCodes
                //TODO: Generally improve and add logic for CD, Journal and Sheet music

                if (dtc.Equals("l"))
                {
                    return typeof(Book).FullName;
                }
                else if (dtc.StartsWith("e"))
                {
                    return typeof(Film).FullName;
                }
                else if (dtc.Equals("di"))
                {
                    return typeof(AudioBook).FullName;
                }
            }
            
            return typeof(Document).FullName;

        }   
    }

}
