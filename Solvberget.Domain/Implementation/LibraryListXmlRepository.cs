using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;
using Solvberget.Domain.Utils;

namespace Solvberget.Domain.Implementation
{
    public class LibraryListXmlRepository : IListRepositoryStatic
    {
        private const string StdFolderPath = @"App_Data\librarylists\";
        private readonly string _folderPath;
        private readonly IRepository _repository;
        private readonly IImageRepository _imageRepository;

        public LibraryListXmlRepository(IRepository repository, IImageRepository imageRepository, string folderPath = null)
        {
            _repository = repository;
            _imageRepository = imageRepository;
            _folderPath = string.IsNullOrEmpty(folderPath) ? StdFolderPath : folderPath;
        }

        public List<LibraryList> GetLists(int? limit = null)
        {
            var lists = new ConcurrentBag<LibraryList>();

            Directory.EnumerateFiles(_folderPath, "*.xml").AsParallel().ToList().ForEach(file => lists.Add(LibraryList.GetLibraryListFromXml(file)));

            lists.ToList().ForEach(liblist => { if (liblist != null) AddContentToList(liblist); });

            return limit != null 
                ? lists.OrderBy(list => list.Priority).Take((int)limit).ToList() 
                : lists.OrderBy(list => list.Priority).ToList();
        }

        public DateTime? GetTimestampForLatestChange()
        {
            var newestFile = FileUtils.GetNewestFile(new DirectoryInfo(_folderPath));
            return newestFile != null ? (DateTime?)newestFile.LastWriteTimeUtc : null;
        }

        private void AddContentToList(LibraryList libraryList)
        {
            foreach(var docnr in libraryList.DocumentNumbers)
            {
                Document document = (_repository.GetDocument(docnr, true));
                //We want to add the thumbnail url to the document in this case
                //Check if already cached
                if (string.IsNullOrEmpty(document.ThumbnailUrl))
                    document.ThumbnailUrl = _imageRepository.GetDocumentThumbnailImage(docnr, "60");
                libraryList.Documents.Add(document);
            }
        }

    }
}
