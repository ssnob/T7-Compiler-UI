using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace T7Installer
{
    internal class Program
    {
        static public string InstallLocation = "c:\\Program Files (x86)\\Compiler UI";

        static private void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
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

        static private void InstallCompiler(string url)
        {
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo3 = Process.GetProcessesByName("blackops3");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo4 = Process.GetProcessesByName("blackops4");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            var usertemp = Path.GetTempPath();
            var installertemp = usertemp + "\\installer_temp";
            var extractpath = usertemp + "update_t7.zip" ;
            var compileFolder = "c:\\t7compiler";

            if (Directory.Exists(extractpath))
                Directory.Delete(extractpath, true);

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, extractpath);
            }

            if (Directory.Exists(installertemp))
                Directory.Delete(installertemp, true);


            ZipFile.ExtractToDirectory(extractpath, installertemp);

            if (Directory.Exists(compileFolder))
                Directory.Delete(compileFolder, true);

            CopyDirectory(installertemp + "\\t7compiler", "c:\\t7compiler", true);
            CopyDirectory(installertemp + "\\defaultproject", "c:\\t7compiler\\defaultproject", true);
            Console.WriteLine("Complete.");

            // cleanup

            Directory.Delete(installertemp, true);
        }
        static private void InstallUI(string url)
        {
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo3 = Process.GetProcessesByName("blackops3");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo4 = Process.GetProcessesByName("blackops4");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            var usertemp = Path.GetTempPath();
            var UItemp = usertemp + "\\installerUI_temp";
            var extractpath = usertemp + "UI_SSNOB.zip";

            if (Directory.Exists(extractpath))
                Directory.Delete(extractpath, true);

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, extractpath);
            }

            if (Directory.Exists(UItemp))
                Directory.Delete(UItemp, true);

            ZipFile.ExtractToDirectory(extractpath, InstallLocation);

            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Idea.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Compiler UI";
            shortcut.TargetPath = @"C:\Program Files (x86)\Compiler UI\win-x64\Idea.exe";
            shortcut.IconLocation = @"C:\Program Files (x86)\Compiler UI\file.ico";
            shortcut.Save();

            WshShell SHELL = new WshShell();
            string SHORTADDR = (string)@"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Idea.lnk";
            IWshShortcut SHORTCUT = (IWshShortcut)SHELL.CreateShortcut(SHORTADDR);
            SHORTCUT.Description = "Compiler UI";
            SHORTCUT.TargetPath = @"C:\Program Files (x86)\Compiler UI\win-x64\Idea.exe";
            SHORTCUT.IconLocation = @"C:\Program Files (x86)\Compiler UI\file.ico";
            SHORTCUT.Save();

            Process.Start(@"C:\Program Files (x86)\Compiler UI\win-x64\Idea.exe");
        }

        static void Main(string[] args)
        {
            try
            {
                if (Directory.Exists(InstallLocation))
                    Directory.Delete(InstallLocation, true);

                Console.WriteLine("Installing.");
                
                InstallUI("https://ssnob.github.io/compiler.zip");
                InstallCompiler("https://gsc.dev/t7c_package");
                Console.WriteLine("Complete.");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Source:{ex.Source}\nMessage:{ex.Message}\nException:{ex.InnerException}\n---------Stack Trace---------\n{ex.StackTrace}\n");
                Console.ReadKey(true);
            }
            Environment.Exit(0);
            return;

        }
    }
}
