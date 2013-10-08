using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.Utils
{
    public static class FileUtils
    {
        
        public static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(GetNewestFile))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
        }

    }
}
