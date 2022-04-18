using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Updater
{
    class Program
    {
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
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

        static void Main(string[] args)
        {
            if (args[0] != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Executing path failed to pass");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please report this!");
                Console.ReadKey(true);
            }
            Directory.Delete(args[0]);

            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo3 = Process.GetProcessesByName("blackops3");
            for (int i = 0; i < bo3.Length; i++)
                try { bo3[i].Kill(); } catch { }

            Process[] bo4 = Process.GetProcessesByName("blackops4");
            for (int i = 0; i < bo4.Length; i++)
                try { bo4[i].Kill(); } catch { }

            Process[] ui = Process.GetProcessesByName("compiler ui");
            for (int i = 0; i < ui.Length; i++)
                try { ui[i].Kill(); } catch { }


            var usertemp = Path.GetTempPath();
            var installertemp = usertemp + "\\update_temp";
            var extractpath = usertemp + "update.zip";
            var compileFolder = "c:\\t7compiler";

            if (Directory.Exists(extractpath))
                Directory.Delete(extractpath, true);

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(@"https://ssnob.github.io/update.zip", extractpath);
            }

            if (Directory.Exists(installertemp))
                Directory.Delete(installertemp, true);


            ZipFile.ExtractToDirectory(extractpath, installertemp);

            if (Directory.Exists(compileFolder))
                return;

            CopyDirectory(installertemp + "\\t7compiler", "c:\\t7compiler", true);
            CopyDirectory(installertemp + "\\defaultproject", "c:\\t7compiler\\defaultproject", true);
            // cleanup

            Directory.Delete(installertemp, true);
        }
    }
}
