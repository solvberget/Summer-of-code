using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    public class BokbasenRepository
    {
        private readonly IRepository _documentRepository;
        private readonly string _serveruri = string.Empty;
        private readonly string _serverSystem = string.Empty;
        private readonly string _xmluri = string.Empty;

        private readonly string[] _trueParams = {
                                                    "SMALL_PICTURE", "LARGE_PICTURE", "PUBLISHER_TEXT", "FSREVIEW",
                                                    "CONTENTS", "SOUND", "EXTRACT", "REVIEWS", "nocrypt"
                                                };

        public BokbasenRepository(IRepository documentRepository)
        {

            _documentRepository = documentRepository;

            _serveruri = Properties.Settings.Default.BokBasenServerUri;
            _serverSystem = Properties.Settings.Default.BokBasenSystem;

            _xmluri = _serveruri;

            foreach (var param in _trueParams)
                _xmluri += param + "=true&";

            _xmluri += "SYSTEM=" + _serverSystem;

        }

        public BokBasenBook GetExternalBokbasenBook(string id)
        {
            var doc = _documentRepository.GetDocument(id, true);
            
             if (Equals(doc.DocType, typeof(Book).Name))
             {
                return GetExternalBokbasenBook(doc as Book);
             }
             if (Equals(doc.DocType, typeof(AudioBook).Name))
             {
                 return GetExternalBokbasenBook(doc as AudioBook);
             }
            return null;
        }

       

        public BokBasenBook GetExternalBokbasenBook(Document doc)
        {

            if (Equals(doc.DocType, typeof(Book).Name))
            {
                var book = doc as Book;
                var isbn = book.Isbn;

                return GenerateBookFromXml(_xmluri + "&ISBN=" + isbn);
            }
            if (Equals(doc.DocType, typeof(AudioBook).Name))
            {
                var book = doc as AudioBook;
                var isbn = book.Isbn;

                return GenerateBookFromXml(_xmluri + "&ISBN=" + isbn);
            }

            return null;

        }

        public BokBasenBook GenerateBookFromXml(string xml)
        {
            var book = new BokBasenBook();
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(xml);
            }
            catch
            {
                return null;
            }

            if (xdoc.Root == null) return null;

            var xElement = xdoc.Element("BOOK_JACKETS");
            if (xElement == null) return null;
            var xElementRecord = xElement.Element("RECORD");
            if (xElementRecord == null) return null;

            var element = xElementRecord.FirstAttribute;
            if (element != null) book.Id = element.Value;

            book.Fsreview = GetXmlValue(xElementRecord, GetPropertyName(() => book.Fsreview));
            book.Publisher_text = GetXmlValue(xElementRecord, GetPropertyName(() => book.Publisher_text));
            book.Contents = GetXmlValue(xElementRecord, GetPropertyName(() => book.Contents));
            book.Thumb_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => book.Thumb_Cover_Picture));
            book.Small_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => book.Small_Cover_Picture));
            book.Large_Cover_Picture = GetXmlValue(xElementRecord, GetPropertyName(() => book.Large_Cover_Picture));
            book.Sound = GetXmlValue(xElementRecord, GetPropertyName(() => book.Sound));
            book.Extract = GetXmlValue(xElementRecord, GetPropertyName(() => book.Extract));
            book.Marc = GetXmlValue(xElementRecord, GetPropertyName(() => book.Marc));
            book.Reviews = GetXmlValue(xElementRecord, GetPropertyName(() => book.Reviews));

            return book;
        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag.ToUpper()).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : string.Empty;
        }
    }
}
