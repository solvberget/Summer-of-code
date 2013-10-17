using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class LibraryListDynamicRepository : IListRepository
    {
        private const int LimitNumberOfElementsPerList = 25;
        private const string StdFolderPath = @"App_Data\librarylists\dynamic\";
        private readonly string _xmlFolderPath;
        private readonly IRepository _documentRepository;
        private readonly IImageRepository _imageRepository;

        public LibraryListDynamicRepository(IRepository documentRepository, IImageRepository imageRepository, IEnvironmentPathProvider environment)
        {
            this._documentRepository = documentRepository;
            this._imageRepository = imageRepository;

            var xmlFilePath = environment.GetXmlFilePath();
            _xmlFolderPath = string.IsNullOrEmpty(xmlFilePath) ? StdFolderPath : xmlFilePath;
        }

        public List<LibraryList> GetLists(int? limit = null)
        {
            
            var lists = Directory.EnumerateFiles(_xmlFolderPath, "*.xml").AsParallel().ToList().Select(GenerateList).Where(liblist => liblist != null).ToList();

            return limit != null
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList()
                : lists.OrderBy(list => list.Priority).ToList();

        }

        private LibraryList GenerateList(string file)
        {
            var xmlDoc = XDocument.Load(file);

            if (xmlDoc.Root != null)
            {
                var dynListXml = xmlDoc.Root;
                var dynamicList = LibraryListXmlRepository.GetLibraryListFromXml(dynListXml);
                var givenTimespanAttr = dynListXml.Attribute("timespan");
                var givenTimespanString = givenTimespanAttr == null ? "month" : givenTimespanAttr.Value;
                var timespan = GetTimespan(givenTimespanString);
                var request = System.Web.HttpUtility.UrlEncode(GenerateRequest(dynListXml, timespan));
                var docsForList = _documentRepository.Search(request);

                if (docsForList.Count <= 0) return null;
                dynamicList.Documents = docsForList.Take(LimitNumberOfElementsPerList).ToList();
                dynamicList.Documents.ForEach(x => dynamicList.DocumentNumbers.Add(x.DocumentNumber, true));

                //Add thumbs to each document
                //foreach (var document in docsForList.Where(document => string.IsNullOrEmpty(document.ThumbnailUrl)))
                //{
                //    document.ThumbnailUrl = _imageRepository.GetDocumentThumbnailImage(document.DocumentNumber, "60");
                //}

                return dynamicList;

            }

            return null;

        }

        private static string GenerateRequest(XContainer dynListXml, Timespan timespan)
        {
            
            var sb = new StringBuilder();
            sb.Append("wda=" + GenerateTimespanString(timespan)+ " and ");

            using (var enumerator = dynListXml.Elements().GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    bool isLast;
                    do
                    {
                        var current = enumerator.Current;
                        isLast = !enumerator.MoveNext();
                        sb.Append(current.Name + "=" + current.Value);
                        if (!isLast)
                        {
                            sb.Append(" and ");
                        }
                    } while (!isLast);
                }
            }
            
            return sb.ToString();
        
        }

        private static string GenerateTimespanString(Timespan timespan)
        {

            DateTime from;
            var to = DateTime.Today;
            
            switch(timespan)
            {
                case Timespan.Week:
                    from = to.AddDays(-7);
                    break;
                case Timespan.Month:
                    from = to.AddDays(-(DateTime.DaysInMonth(to.Year, to.Month)));
                    break;
                case Timespan.Year:
                    from = to.AddDays(-(DateTime.IsLeapYear(to.Year) ? 366 : 365));
                    break;
                default:
                    from = to.AddDays(-(DateTime.DaysInMonth(to.Year, to.Month)));
                    break;
            }
            
            return from.ToString("yyMMdd") + "->" + to.ToString("yyMMdd");

        }

        private static Timespan GetTimespan(string timespan)
        {
            if (timespan.Equals("week"))
                return Timespan.Week;
            if (timespan.Equals("year"))
                return Timespan.Year;
            return Timespan.Month;
        }

        private enum Timespan{ Week, Month, Year }

    }
}
