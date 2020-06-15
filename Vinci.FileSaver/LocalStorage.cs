using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Vinci.FileSaver
{
    public class LocalStorage
    {
        DirectoryInfo RootDirectory =   null;
        PathResolver pathResolver = new PathResolver();
        internal LocalStorage()
        {
            RootDirectory =Directory.CreateDirectory(Path.GetDirectoryName( this.GetType().Assembly.Location));
        }
        public bool SaveImage(Stream img, string extension,DirectoryInfo rootDir=null)
        {
          if(rootDir!=null)
            RootDirectory = rootDir;
            return SaveFile(img,extension);
        }

        public Image GetImage(string id, DirectoryInfo rootDir = null)
        {
          if(rootDir!=null)
            RootDirectory = rootDir;
            var dir = pathResolver.GetDictionary(RootDirectory, id);
           var files= dir.GetFiles($"{id}.*");
            if (files.Length > 0) {

               return (Image) Image.FromFile(files[0].FullName).Clone();
            }
            return null;
        }


        public bool SaveFile(Stream img, string extension, DirectoryInfo rootDir = null)
        {
            if (rootDir != null)
                RootDirectory = rootDir;
            var id = pathResolver.GetPathId();
            var dir = pathResolver.GetDictionary(RootDirectory, id);
            using (var file = File.Open(Path.Combine(dir.FullName, $"{id}.{extension}"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                img.CopyTo(file);
            }
            return true;
        }

        //public FileStream GetFile(string id)
        //{
        //    var dir = pathResolver.GetDictionary(RootDirectory, id);
        //    var files = dir.GetFiles($"{id}.*");
        //    if (files.Length > 0)
        //    {

        //        return files[0].OpenRead().c;
        //    }
        //    return null;
        //}

        //public void SetRootDirectory(DirectoryInfo dir)
        //{
        //    RootDirectory = dir;
        //}

        //LocalConfiguration _config;
        //public LocalConfiguration MyProperty { get => _config ?? (_config = new LocalConfiguration()); }
    }
}
