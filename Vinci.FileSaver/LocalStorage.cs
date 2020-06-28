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

        private ImageFormat GetImgFormat(Image img, string extension)
        {
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
            return format;
        }

        /// <summary>
        /// 保存图片，如果要自定义保存文件名称
        /// </summary>
        /// <param name="img"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath">用户自定义的文件名称和路径，将没有系统默认的文件夹</param>
        /// <returns>id</returns>
        public string SaveImage(Image img, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                if (!dir.Exists) { dir.Create(); }
                var id = pathResolver.GetPathId();
                dir = pathResolver.GetDictionary(dir, id);
                var format = GetImgFormat(img, extension);
                img.Save(Path.Combine(dir.FullName, $"{id}.{extension}"), format);
                return $"id:{id}";
            }
            else
            {
                var path = Path.Combine(dir.FullName, $"{filePath}.{extension}");
                dir = new FileInfo(path).Directory;
                if (!dir.Exists) { dir.Create(); }
                var format = GetImgFormat(img, extension);
                img.Save(path, format);
                return $"path:{filePath}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath">id or Path: id 的格式为："id:***",path 的格式为:"path:***",若无前缀标识则为id</param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        public Image GetImage(string idOrPath, DirectoryInfo rootDir = null)
        {
            if (idOrPath.StartsWith("path:")) return GetImageC(idOrPath, rootDir);
            var id = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
            var dir = RootDirectory;
            if (rootDir != null)
                dir = rootDir;
            dir = pathResolver.GetDictionary(dir, id);
            var files = dir.GetFiles($"{id}.*");
            if (files.Length > 0)
            {
                Image im;
                using (var fs = new FileStream(files[0].FullName, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    im = Image.FromStream(ms);
                }
                return im;
            }
            return null;
        }

        /// <summary>
        /// 获取图片 通过用户自定义的存储路径
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="filePath">用户自定义的文件名称和路径，将没有系统默认的文件夹</param>
        /// <returns></returns>
        private Image GetImageC(string filePath, DirectoryInfo rootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
                dir = rootDir;
            filePath = filePath.Substring(5);
            var files = dir.GetFiles($"{filePath}.*");
            if (files.Length > 0)
            {
                Image im;
                using (var fs = new FileStream(files[0].FullName, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    im = Image.FromStream(ms);
                }
                return im;
            }
            return null;
        }

        public bool UpdateImage(string idOrPath, Image img, string extension, DirectoryInfo rootDir = null)
        {
            if (idOrPath.StartsWith("path:")) return UpdateImageC(idOrPath, img, extension, rootDir);
            var id = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            dir = pathResolver.GetDictionary(dir, id);
            var files = dir.GetFiles($"{id}.*");
            var oldPath = "";
            if (files.Length > 0)
            {
                files[0].MoveTo(oldPath = (files[0].FullName + ".old"));
                //files[0].Delete();
            }
            var format = GetImgFormat(img, extension);
            img.Save(Path.Combine(dir.FullName, $"{id}.{extension}"), format);
            File.Delete(oldPath);
            return true;
        }

        private bool UpdateImageC(string filePath, Image img, string extension, DirectoryInfo rootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            filePath = filePath.Substring(5);
            var files = dir.GetFiles($"{filePath}.*");
            var oldPath = "";
            if (files.Length > 0)
            {
                files[0].MoveTo(oldPath = (files[0].FullName + ".old"));
                var format = GetImgFormat(img, extension);
                img.Save(Path.Combine(dir.FullName, $"{filePath}.{extension}"), format);
                File.Delete(oldPath);
                return true;
            }
            return false;
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
