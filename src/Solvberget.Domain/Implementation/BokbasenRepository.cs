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
                var xmlBook = new BokBasenBook();

                xmlBook.FillProperties(_xmluri + "&ISBN=" + isbn);

                return xmlBook;
            }
            if (Equals(doc.DocType, typeof(AudioBook).Name))
            {
                var book = doc as AudioBook;
                var isbn = book.Isbn;
                var xmlBook = new BokBasenBook();

                xmlBook.FillProperties(_xmluri + "&ISBN=" + isbn);
                return xmlBook;
            }

            return null;

        }


    }
}
