using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Idea.Helpers
{
    static internal class FileHelper
    {
        static public void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        static public long DirSize(string path)
        {
            long size = 0;
            // Add file sizes.

            DirectoryInfo INFO = new DirectoryInfo(path);
            FileInfo[] fis = INFO.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = INFO.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di.ToString());
            }
            return size;
        }
    }
}
