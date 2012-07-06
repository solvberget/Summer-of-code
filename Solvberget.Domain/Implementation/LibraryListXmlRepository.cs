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

        public LibraryListXmlRepository(string folderPath = null)
        {
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }


        public List<LibraryList> GetLists(int? limit = null)
        {
            var lists = new ConcurrentBag<LibraryList>();

            Directory.EnumerateFiles(_folderPath, "*.xml").AsParallel().ToList().ForEach(file => lists.Add(LibraryList.GetLibraryListFromXml(file)));

            return limit != null 
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList() 
                : lists.OrderBy(list => list.Priority).ToList();
        }

    }
}
