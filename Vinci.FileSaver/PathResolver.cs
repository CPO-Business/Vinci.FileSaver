using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vinci.FileSaver.Interface;

namespace Vinci.FileSaver
{
    class PathResolver : IPathResolver
    {
        public string GetPathId()
        {
            var now = DateTime.UtcNow;
            return now.ToString("yyyy-MM-dd.HHmmssfff");
        }

        public DirectoryInfo GetDictionary(DirectoryInfo root, string Id)
        {
            var subPath = Id.Split('.')[0].Replace('-', '\\');
            return new DirectoryInfo(Path.Combine(root.FullName, subPath));
        }

        public string GetDictionary(string root, string Id)
        {
            var subPath = Id.Split('.')[0].Replace('-', '\\');
            return Path.Combine(root, subPath);
        }
    }
}
