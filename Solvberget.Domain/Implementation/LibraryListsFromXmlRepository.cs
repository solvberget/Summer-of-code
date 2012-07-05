using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class LibraryListsFromXmlRepository
    {
        private const string StdFolderPath = @"App_Data\librarylists\";
        private readonly string _folderPath;

        public LibraryListsFromXmlRepository(string folderPath = null)
        {
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<LibraryList> GetLists()
        {
            var lists = new ConcurrentBag<LibraryList>();

            Directory.EnumerateFiles(_folderPath, "*.xml").AsParallel().ToList().ForEach(file => lists.Add(LibraryList.GetLibraryListFromXml(file)));

            return lists.OrderBy(list => list.Priority).ToArray().ToList();

        }

        public List<LibraryList> GetLists(int limit)
        {
            return GetLists().Take(limit).ToList();
        }

    }
}
