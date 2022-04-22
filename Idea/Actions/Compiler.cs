using Idea.Games;
using Idea.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static Idea.Games.Game;

namespace Idea.Actions
{
    static class Compiler
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static string menu;
        public static string command = null;
        public static string[] symbols = { "", "", "", "", "", "", "" };
        static public string GetGame(GAMES games)
        {
            switch (games)
            {
                case GAMES.BLACKOPS3:
                    return "bo3";
                case GAMES.BLACKOPS4:
                    return "bo4";
            }
            return null;
        }
        static public string GetGamemode(GAMEMODE gamemode)
        {
            switch (gamemode)
            {
                case GAMEMODE.CAMPAIGN:
                    return "sp";
                case GAMEMODE.MULTIPLAYER:
                    return "mp";
                case GAMEMODE.ZOMBIES:
                    return "zm";
            }
            return null;
        }
        public static void InstallCompiler(string url)
        {
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            for (int i = 0; i < debugcompiler.Length; i++)
                try { debugcompiler[i].Kill(); } catch { }

            Process[] bo3 = Process.GetProcessesByName("blackops3");
            for (int i = 0; i < bo3.Length; i++)
                try { bo3[i].Kill(); } catch { }

            Process[] bo4 = Process.GetProcessesByName("blackops4");
            for (int i = 0; i < bo4.Length; i++)
                try { bo4[i].Kill(); } catch { }


            var usertemp = Path.GetTempPath();
            var installertemp = usertemp + "\\installer_temp";
            var extractpath = usertemp + "update_t7.zip";
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
                return;

            FileHelper.CopyDirectory(installertemp + "\\t7compiler", "c:\\t7compiler", true);
            FileHelper.CopyDirectory(installertemp + "\\defaultproject", "c:\\t7compiler\\defaultproject", true);
            // cleanup

            Directory.Delete(installertemp, true);

            MessageBox.Show("Compiler Updated/Installed", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        static string _infocheck;
        static string result__;
        static bool haveweinjected = false;
        public static void CompileScript(string path, GAMES game, GAMEMODE gamemode, bool folderselected)
        {
            haveweinjected = false;
            File.WriteAllText(Environment.CurrentDirectory + "\\injection.log", "");
            if (!folderselected)
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                return;
            }

            if (!Directory.Exists("c:\\t7compiler"))
            {
                MessageBox.Show("Compiler Is not installed. Cannot continue", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            symbols[0] = "symbols=";
            symbols[2] = "serious";

            symbols[1] = GetGame(game);
            symbols[3] = GetGamemode(gamemode);


            if (!File.Exists(path + @"\gsc.conf"))
            {
                File.Create(path + @"\gsc.conf");
            }

            var sp = "scripts\\core_common\\load_shared.gsc";
            var mp = "scripts\\mp_common\\bb.gsc";
            var zm = "scripts\\zm_common\\load.gsc";

            string bo4symbols = null;

            if (symbols[3] == "sp")
                bo4symbols = sp;

            if (symbols[3] == "mp")
                bo4symbols = mp;

            if (symbols[3] == "zm")
                bo4symbols = zm;

            // 0 = symbols, 1 = game, 2 = serious, 3 = gamemode | symbols=game,serious,gamemode
            switch (symbols[1])
            {
                case "bo3":
                    try
                    {
                        File.WriteAllText(path + @"\gsc.conf", symbols[0] + symbols[1] + "," + symbols[2] + "," + symbols[3]);
                    }
                    catch
                    {
                        return;
                    }
                    break;
                case "bo4":
                    try
                    {
                        File.WriteAllText(path + @"\gsc.conf", "game=t8\n" + "script=" + bo4symbols);
                    }
                    catch
                    {
                        return;
                    }
                    break;
            }

            File.WriteAllText(path + @"\compile.bat", "cd /d " + path.Replace(@"/", @"\") + "\nC:\\t7compiler\\debugcompiler --build");
            ProcessStartInfo startInfo = new ProcessStartInfo();


            startInfo.FileName = path + @"\compile.bat";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            string info = "";
            Process proc = Process.Start(startInfo);
            proc.OutputDataReceived += Proc_OutputDataReceived;

            proc.BeginOutputReadLine();
            proc.WaitForExit();

            var finalresult = Regex.Replace(File.ReadAllText(Environment.CurrentDirectory + "\\injection.log"), @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            if (haveweinjected)
            {
                bool bo3found = false;
                bool bo4found = false;

                var proc1 = Process.GetProcessesByName("blackops3");
                foreach (var p in proc1)
                {
                    if (p.Id > 0)
                        bo3found = true;
                }

                var proc2 = Process.GetProcessesByName("blackops4");
                foreach (var p in proc2)
                {
                    if (p.Id > 0)
                        bo4found = true;
                }

                if (bo3found)
                {
                    WindowHelper.BringProcessToFront(Process.GetProcessesByName("blackops3"));
                    BlackOps3.Popup($"^2{menu}\n^7Injected Succesfully - ^9{GetGamemode(gamemode)}");
                }

                if (bo4found)
                {
                    WindowHelper.BringProcessToFront(Process.GetProcessesByName("blackops4"));
                    BlackOps4.Popup($"^2{menu}\n^7Injected Succesfully - ^9{GetGamemode(gamemode)}");
                }

            }
            if (!haveweinjected) // :( an error occured
            {
                MessageBox.Show(finalresult, "Result", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
                _infocheck = e.Data;
                File.WriteAllText(Environment.CurrentDirectory + "\\injection.log", File.ReadAllText(Environment.CurrentDirectory + "\\injection.log") + e.Data);
                if (e.Data.Contains("If in game, you are probably going to crash."))
                {
                    ProcessEx dbc = "debugcompiler";
                    dbc.BaseProcess.Kill(); // chiefs kiss
                    haveweinjected = true;
                }
            }
        }

        private static void Proc_OutputDataReceivedPRECOMPILED(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Console.WriteLine(e.Data);
                _infocheck = e.Data;
                File.WriteAllText(Environment.CurrentDirectory + "\\injection.log", File.ReadAllText(Environment.CurrentDirectory + "\\injection.log") + e.Data);
                if (e.Data.Contains("Injected"))
                {
                    ProcessEx dbc = "debugcompiler";
                    dbc.BaseProcess.Kill(); // chiefs kiss
                    haveweinjected = true;
                }
            }
        }

        public static void InjectScript(string path, GAMES game)
        {
            haveweinjected = false;
            File.WriteAllText(Environment.CurrentDirectory + "\\injection.log", "");
            if (!Directory.Exists("c:\\t7compiler"))
            {
                MessageBox.Show("Compiler Is not installed. Cannot continue", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string _game = string.Empty;
            string _path = string.Empty;
            switch (GetGame(game))
            {
                default:
                    break;
                case "bo3":
                    _game = "\"T7\"";
                    _path = "\"scripts\\shared\\duplicaterender_mgr.gsc\"";
                    break;
                case "bo4":
                    _game = "\"T8\"";
                    _path = "\"scripts\\zm_common\\load.gsc\"";
                    break;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "c:\\t7compiler\\debugcompiler.exe";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = $"--inject \"{path}\" \"{_game}\" \"{_path}\"";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process proc = Process.Start(startInfo);
            proc.OutputDataReceived += Proc_OutputDataReceivedPRECOMPILED;

            proc.BeginOutputReadLine();
            proc.WaitForExit();

            var finalresult = Regex.Replace(File.ReadAllText(Environment.CurrentDirectory + "\\injection.log"), @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            if (haveweinjected)
            {
                bool bo3found = false;
                bool bo4found = false;

                var proc1 = Process.GetProcessesByName("blackops3");
                foreach (var p in proc1)
                {
                    if (p.Id > 0)
                        bo3found = true;
                }

                var proc2 = Process.GetProcessesByName("blackops4");
                foreach (var p in proc2)
                {
                    if (p.Id > 0)
                        bo4found = true;
                }

                
                if (bo3found)
                {
                    WindowHelper.BringProcessToFront(Process.GetProcessesByName("blackops3"));
                    BlackOps3.Popup($"^7Injected Succesfully - ^9Precompiled Script");
                }

                if (bo4found)
                {
                    WindowHelper.BringProcessToFront(Process.GetProcessesByName("blackops4"));
                    BlackOps4.Popup($"^7Injected Succesfully - ^9Precompiled Script");
                }
            }
            else
            {
                MessageBox.Show(finalresult, "Result", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
