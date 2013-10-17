using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class BokelskereRepository
    {
        private readonly IRepository _documentRepository;
        private readonly string _serveruri = string.Empty;
        private readonly string _xmluri = string.Empty;


        public BokelskereRepository(IRepository documentRepository)
        {
            _documentRepository = documentRepository;
            _serveruri = Properties.Settings.Default.BokElskereServerUrl;
            _xmluri = _serveruri;
        }

        public BokElskereBook GetExternalBokelskereBook(string id)
        {
            var doc = _documentRepository.GetDocument(id, true);
            return Equals(doc.DocType, typeof(Book).Name) ? GetExternalBokelskereBook(doc as Book) : null;
        }

       

        public BokElskereBook GetExternalBokelskereBook(Document doc)
        {
            if (Equals(doc.DocType, typeof(Book).Name))
            {
                var book = doc as Book;
                if (book != null && book.Isbn != null)
                {
                    var isbn = book.Isbn;
                    isbn =  isbn.Replace("-", "");
                    

                    var xmlBook = FillProperties(_xmluri + "/" + isbn+"/?format=xml");

                    return xmlBook;
                }
            }
            return null;
        }

        public BokElskereBook FillProperties(string xml)
        {
            var book = new BokElskereBook();
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

            var xElement = xdoc.Element("response");
            if (xElement == null) return null;
            book.gjennomsnittelig_terningkast = GetXmlValue(xElement, GetPropertyName(() => book.gjennomsnittelig_terningkast));
            decimal terningkast;
            if (Decimal.TryParse(book.gjennomsnittelig_terningkast, NumberStyles.Any, CultureInfo.InvariantCulture, out terningkast))
            {
                var gjennomsnittelig_terningkast_decimal = Math.Round(terningkast, 1);
                book.gjennomsnittelig_terningkast = gjennomsnittelig_terningkast_decimal.ToString(CultureInfo.InvariantCulture);
            }

            return book;
        }

        private static string GetXmlValue(XElement node, string tag)
        {
            var xElement = node.DescendantsAndSelf(tag.ToLower()).FirstOrDefault();
            return xElement == null ? string.Empty : xElement.Value;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            return memberExpression != null ? memberExpression.Member.Name : string.Empty;
        }
    }
}
