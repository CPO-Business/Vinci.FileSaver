using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Vinci.FileSaver
{
    public class LocalStorage
    {
        DirectoryInfo RootDirectory = null;
        PathResolver pathResolver = new PathResolver();
        internal LocalStorage()
        {
            RootDirectory = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "LStorage\\"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <returns>id</returns>
        public string SaveImage(Image img, string extension, DirectoryInfo rootDir = null)
        {
            if (rootDir != null)
            {
                RootDirectory = rootDir;
            }
            var id = pathResolver.GetPathId();
            var dir = pathResolver.GetDictionary(RootDirectory, id);
            var format = img.RawFormat;
            switch (extension)
            {
                case "icon":
                case "ico":
                    format = ImageFormat.Icon;
                    break;
                default:
                    break;
            }
            img.Save(Path.Combine(dir.FullName, $"{id}.{extension}"), format);
            return id;
        }

        public Image GetImage(string id, DirectoryInfo rootDir = null)
        {
            if (rootDir != null)
                RootDirectory = rootDir;
            var dir = pathResolver.GetDictionary(RootDirectory, id);
            var files = dir.GetFiles($"{id}.*");
            if (files.Length > 0)
            {
                
                var origin = Image.FromFile(files[0].FullName);
                var f = origin.RawFormat;
                return (Image)origin.Clone();
            }
            return null;
        }


        private string SaveFile(Stream img, string extension, DirectoryInfo rootDir = null)
        {
            if (rootDir != null)
                RootDirectory = rootDir;
            var id = pathResolver.GetPathId();
            var dir = pathResolver.GetDictionary(RootDirectory, id);
            img.Seek(0, SeekOrigin.Begin);
            using (var file = File.Open(Path.Combine(dir.FullName, $"{id}.{extension}"), FileMode.OpenOrCreate, FileAccess.Write))
            {
                img.CopyTo(file);
            }
            return id;
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
