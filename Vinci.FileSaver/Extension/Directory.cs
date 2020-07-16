using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vinci.FileSaver.Extension
{
    public static class DirectoryEx
    {
        /// <summary>
        /// Copy to extension
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"> </param>
        public static void CopyTo(this DirectoryInfo sourceDir, string destDirName, bool copySubDirs = false)
        {
            // Get the subdirectories for the specified directory.
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir.FullName);
            }
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                DirectoryInfo[] dirs = sourceDir.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    subdir.CopyTo(temppath, copySubDirs);
                }
            }
        }
    }
}
