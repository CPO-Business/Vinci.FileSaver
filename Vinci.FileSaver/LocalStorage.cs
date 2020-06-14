using System;
using System.Drawing;
using System.IO;

namespace Vinci.FileSaver
{
    public class LocalStorage
    {
        DirectoryInfo RootDirectory = null;

        internal LocalStorage()
        {

        }
        public bool SaveImage(Image img)
        {

        }

        public Image GetImage(string id)
        {

        }


        public bool SaveFile()
        {

        }

        public FileStream GetFile(string id)
        {
            GetDictionary
        }

        public void SetRootDirectory(DirectoryInfo dir)
        {
            RootDirectory = dir;
        }
        //LocalConfiguration _config;
        //public LocalConfiguration MyProperty { get => _config ?? (_config = new LocalConfiguration()); }
    }
}
