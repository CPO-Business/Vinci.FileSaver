using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Vinci.FileSaver.Interface;

namespace Vinci.FileSaver
{
    class LocalStorage : IStorage
    {
        DirectoryInfo RootDirectory = null;
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
                var id = Storage.PathHelper.GetPathId();
                dir = Storage.PathHelper.GetDictionary(dir, id);
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
            Image im = null;
            using (var fs = GetFile(idOrPath, rootDir))
            {
                if (fs == null) return im;
                fs.Seek(0, SeekOrigin.Begin);
                var ms = new MemoryStream();
                fs.CopyTo(ms);
                im = Image.FromStream(ms);
            }
            return im;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileToSave">将要保存的文件流</param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string SaveFile(Stream fileToSave, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {
            return SaveFile(s => fileToSave.CopyTo(s), extension, rootDir, filePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileSaveAct">Action<FileStream> 将创建文件流传递给方法，用户需要在方法中自行写入文件内容，注意不要释放文件流，系统会自动完成所有动作 </param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string SaveFile(Action<FileStream> fileSaveAct, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {
            if (fileSaveAct == null) throw new ArgumentNullException("fileSaveAct");
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                if (!dir.Exists) { dir.Create(); }
                var id = Storage.PathHelper.GetPathId();
                dir = Storage.PathHelper.GetDictionary(dir, id);
                using (var fs = File.Open(Path.Combine(dir.FullName, $"{id}.{extension}"), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileSaveAct(fs);
                }
                return $"id:{id}";
            }
            else
            {
                var path = Path.Combine(dir.FullName, $"{filePath}.{extension}");
                dir = new FileInfo(path).Directory;
                if (!dir.Exists) { dir.Create(); }
                using (var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileSaveAct(fs);
                }
                return $"path:{filePath}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath"></param>
        /// <param name="rootDir"></param>
        /// <returns>FileStream 需要用户自己释放</returns>
        public FileStream GetFile(string idOrPath, DirectoryInfo rootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
                dir = rootDir;
            string realName;
            FileInfo[] files;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                dir = Storage.PathHelper.GetDictionary(dir, realName);
            }
            files = dir.GetFiles($"{realName}.*");

            if (files.Length > 0)
            {
                var fs = new FileStream(files[0].FullName, FileMode.Open, FileAccess.ReadWrite);
                return fs;
            }
            throw new FileNotFoundException(Path.Combine(dir.FullName, $"{realName}.*"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath"></param>
        /// <param name="rootDir"></param>
        /// <param name="newIdOrPath">修改路径或名称，功能相当于重命名</param>
        /// <returns></returns>
        public bool MoveFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            FileInfo[] files;
            string realName;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                dir = Storage.PathHelper.GetDictionary(dir, realName);
            }
            files = dir.GetFiles($"{realName}.*");
            if (files.Length > 0)
            {
                dir = RootDirectory;
                if (newRootDir != null)
                {
                    dir = rootDir;
                }
                if (newPath.StartsWith("path:"))
                {
                    realName = newPath.Substring(5);
                }
                else
                {
                    realName = newPath.StartsWith("id:") ? newPath.Substring(3) : newPath;
                    dir = Storage.PathHelper.GetDictionary(dir, realName);
                }
                var path = Path.Combine(dir.FullName, $"{realName}{ files[0].Extension}");
                dir = new FileInfo(path).Directory;
                if (!dir.Exists) { dir.Create(); }
                files[0].MoveTo(path);
                return true;
            }
            throw new FileNotFoundException(Path.Combine(dir.FullName, $"{realName}.*"));
        }

        public bool CopyFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            FileInfo[] files;
            string realName;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                dir = Storage.PathHelper.GetDictionary(dir, realName);
            }
            files = dir.GetFiles($"{realName}.*");
            if (files.Length > 0)
            {
                dir = RootDirectory;
                if (newRootDir != null)
                {
                    dir = rootDir;
                }
                if (newPath.StartsWith("path:"))
                {
                    realName = newPath.Substring(5);
                }
                else
                {
                    realName = newPath.StartsWith("id:") ? newPath.Substring(3) : newPath;
                    dir = Storage.PathHelper.GetDictionary(dir, realName);
                }
                var path = Path.Combine(dir.FullName, $"{realName}{ files[0].Extension}");
                dir = new FileInfo(path).Directory;
                if (!dir.Exists) { dir.Create(); }
                files[0].CopyTo(path);
                return true;
            }
            throw new FileNotFoundException(Path.Combine(dir.FullName, $"{realName}.*"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath">id or Path: id 的格式为："id:***",path 的格式为:"path:***",若无前缀标识则为id</param>
        /// <param name="img"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        public bool UpdateImage(string idOrPath, Image img, string extension, DirectoryInfo rootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            FileInfo[] files;
            string realName;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
                files = dir.GetFiles($"{realName}.*");
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                dir = Storage.PathHelper.GetDictionary(dir, realName);
                files = dir.GetFiles($"{realName}.*");
            }

            string oldPath = string.Empty;
            if (files.Length > 0)
            {
                files[0].MoveTo(oldPath = (files[0].FullName + ".old"));
            }
            var format = GetImgFormat(img, extension);
            img.Save(Path.Combine(dir.FullName, $"{realName}.{extension}"), format);
            if (oldPath != string.Empty)
            {
                File.Delete(oldPath);
            }
            return true;
        }

        /// <summary>
        /// file with id exist
        /// </summary>
        /// <param name="idOrPath"></param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        public bool Exist(string idOrPath,  DirectoryInfo rootDir = null)
        {
            var dir = RootDirectory;
            if (rootDir != null)
            {
                dir = rootDir;
            }
            FileInfo[] files;
            string realName;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                dir = Storage.PathHelper.GetDictionary(dir, realName);
            }
            files = dir.GetFiles($"{realName}.*");
            return files.Length > 0;

        }


        //public void SetRootDirectory(DirectoryInfo dir)
        //{
        //    RootDirectory = dir;
        //}

        //LocalConfiguration _config;
        //public LocalConfiguration MyProperty { get => _config ?? (_config = new LocalConfiguration()); }
    }
}
