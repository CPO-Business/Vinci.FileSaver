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
            return now.ToString("yyyy-MM-dd.HHmmssfff");
        }

        public DirectoryInfo GetDictionary(DirectoryInfo root, string Id)
        {
            var subPath = Id.Split('.')[0].Replace('-', '\\');
            return Directory.CreateDirectory(Path.Combine(root.FullName, subPath));
        }
    }
}
