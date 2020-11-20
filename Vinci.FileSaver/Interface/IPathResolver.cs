using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vinci.FileSaver.Interface
{
    public interface IPathResolver
    {
        string GetPathId();
        DirectoryInfo GetDictionary(DirectoryInfo root, string Id);
        string GetDictionary(string root, string Id);
    }
}
