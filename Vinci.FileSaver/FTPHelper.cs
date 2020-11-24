using System;
using System.Collections.Generic;
using System.Text;
using Vinci.FileSaver.Interface;
using System.Linq;

namespace Vinci.FileSaver
{
    public static class FTPHelper
    {

        public static string[] ListFile(IStorage storage, string rootDir = null)
        {
            if (storage is FTPStorage)
            {
                var ftpClient = (storage as FTPStorage).Client;
                FluentFTP.FtpListItem[] items;
                if (string.IsNullOrWhiteSpace(rootDir)) {
                    items= ftpClient.GetListing(".");
                }
                else
                {
                    items = ftpClient.GetListing(rootDir);
                }
                return items.Where(i=>i.Type== FluentFTP.FtpFileSystemObjectType.File&&!i.Name.StartsWith(".")).Select(i => i.Name).ToArray();
                //return ftpClient.GetNameListing(rootDir);
            }
            else
            {
                throw new ArgumentException("argument 'storage' is not a validate FTP Storage", nameof(storage));
            }
        }

        public static void Clear(IStorage storage, string rootDir = null)
        {
            if (storage is FTPStorage)
            {
                var ftpClient = (storage as FTPStorage).Client;
                var files = ListFile(storage, rootDir);
                foreach (var item in files)
                {
                    ftpClient.DeleteFile(item);
                }
            }
            else
            {
                throw new ArgumentException("argument 'storage' is not a validate FTP Storage", nameof(storage));
            }
        }
    }
}
