using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class LibraryListXmlRepository : IListRepository
    {
        private const string StdFolderPath = @"App_Data\librarylists\";
        private readonly string _folderPath;
        private readonly IRepository _repository;

        public LibraryListXmlRepository(IRepository repository, string folderPath = null)
        {
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
            _repository = repository;
        }


        public List<LibraryList> GetLists(int? limit = null)
        {
            var lists = new ConcurrentBag<LibraryList>();

            Directory.EnumerateFiles(_folderPath, "*.xml").AsParallel().ToList().ForEach(file => lists.Add(LibraryList.GetLibraryListFromXml(file)));

            foreach(var l in lists)
            {
                AddContentToList(l);
            }

            return limit != null 
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList() 
                : lists.OrderBy(list => list.Priority).ToList();
        }

        private void AddContentToList(LibraryList libraryList)
        {
            foreach(var docnr in libraryList.DocumentNumbers)
            {
                libraryList.Documents.Add(_repository.GetDocument(docnr, true));       
            }
        }



    }
}
