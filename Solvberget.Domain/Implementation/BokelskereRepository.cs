using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

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
            
             if (Equals(doc.DocType, typeof(Book).Name))
             {
                return GetExternalBokelskereBook(doc as Book);
             }
             
            return null;
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
                    var xmlBook = new BokElskereBook();

                    xmlBook.FillProperties(_xmluri + "/" + isbn+"/?format=xml");

                    return xmlBook;
                }
            }

            return null;

        }


    }
}
