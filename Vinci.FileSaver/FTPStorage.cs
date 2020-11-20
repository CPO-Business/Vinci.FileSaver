using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Vinci.FileSaver.Interface;

namespace Vinci.FileSaver
{
    class FTPStorage : IStorage
    {
        public FluentFTP.FtpClient Client;


        public bool CopyFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null)
        {
            throw new NotImplementedException();
        }

        public bool Exist(string idOrPath, DirectoryInfo rootDir = null)
        {
            throw new NotImplementedException();
        }

        public Stream GetFile(string idOrPath, DirectoryInfo rootDir = null)
        {

            var dir = new DirectoryInfo(".");
            string relativePath = "";
            if (rootDir != null)
                relativePath = Path.GetRelativePath(dir.FullName, rootDir.FullName);

            string realName;
            if (idOrPath.StartsWith("path:"))
            {
                realName = idOrPath.Substring(5);
            }
            else
            {
                realName = idOrPath.StartsWith("id:") ? idOrPath.Substring(3) : idOrPath;
                relativePath = Storage.PathHelper.GetDictionary(relativePath, realName);
            }

            var stream = new MemoryStream();

            if (Client.Download(stream, Path.Combine(relativePath, realName)))
            {
                return stream;
            }
            else
            {
                stream.Close();
            }
            throw new FileNotFoundException($"FTP:{realName}");
        }

        public Image GetImage(string idOrPath, DirectoryInfo rootDir = null)
        {
            throw new NotImplementedException();
        }

        public bool MoveFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// if there is a file with same name existed, save file will overwrite it
        /// </summary>
        /// <param name="fileToSave"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string SaveFile(Stream fileToSave, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {

            var dir = new DirectoryInfo(".");
            string relativePath = "";
            if (rootDir != null)
            {
                relativePath = Path.GetRelativePath(dir.FullName, rootDir.FullName);
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                var id = Storage.PathHelper.GetPathId();
                dir = Storage.PathHelper.GetDictionary(dir, id);
                Client.Upload(fileToSave, Path.Combine(relativePath, $"{id}.{extension}"), createRemoteDir: true);
                return $"id:{id}.{extension}";
            }
            else
            {
                Client.Upload(fileToSave, Path.Combine(relativePath, $"{filePath}.{extension}"), createRemoteDir: true);
                return $"path:{filePath}.{extension}";
            }
        }

        public string SaveFile(Action<FileStream> fileSaveAct, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {
            throw new NotImplementedException();
        }

        public string SaveImage(Image img, string extension, DirectoryInfo rootDir = null, string filePath = null)
        {
            throw new NotImplementedException();
        }

        public bool UpdateImage(string idOrPath, Image img, string extension, DirectoryInfo rootDir = null)
        {
            throw new NotImplementedException();
        }
    }
}
