using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vinci.FileSaver
{
    class PathResolver
    {
        public string GetPathId()
        {
            var now = DateTime.UtcNow;
        return now.ToString("yyyy-MM-dd-mm-ss-") $"{now.Year}-{now.Month}-{now.}"
            return string.Empty;
        }

        public DirectoryInfo GetDictionary(DirectoryInfo root, string Id)
        {
            var subPath = Id.Replace('-', '\\');
            return Directory.CreateDirectory(Path.Combine(root.FullName, subPath));
        }
    }
}
