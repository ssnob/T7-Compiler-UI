using Idea.Games;
using Idea.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
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

        public static void CompileScript(string path, GAMES game, GAMEMODE gamemode, bool folderselected)
        {

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


            Process proc = Process.Start(startInfo);
            try
            {
                System.Threading.Thread.Sleep(3760); // if you have a better way of doing this please let me know
                foreach (var process in Process.GetProcessesByName("debugcompiler"))
                {
                    process.Kill();
                }
            }
            catch { }


            string info = "";
            info = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            #region replacespam
            info = info.Replace(Environment.CurrentDirectory, "");
            info = info.Replace(path, "");
            info = info.Replace(">cd /d", "");
            info = info.Replace(">C:\\t7compiler\\debugcompiler --build", "");
            info = info.Replace("\\" + new DirectoryInfo(System.IO.Path.GetDirectoryName(path + @"\\")).Name, "");
            info = info.Replace("Script compiled. Press I to inject or anything else to continue", "");
            info = info.Replace("compiled.gscc", "");
            info = info.Replace("Press any key to reset gsc parsetree...", "");
            info = info.Replace("If in game, you are probaly going to crash...", "");
            #endregion


            var finalresult = Regex.Replace(info, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            if (finalresult.Contains("If in game, you are probably going to crash.") || finalresult.Contains("_assetPool:ScriptParseTree =>"))
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
            else
                MessageBox.Show(finalresult, "Result", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            Console.WriteLine(info);
        }

        public static void InjectScript(string path, GAMES game)
        {
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

            try
            {
                System.Threading.Thread.Sleep(2560); // if you have a better way of doing this please let me know
                foreach (var process in Process.GetProcessesByName("debugcompiler"))
                {
                    process.Kill();
                }
            }
            catch { }


            string info = "";
            info = proc.StandardOutput.ReadToEnd();

            if (info.Contains("Injected") || info.Contains("_assetPool:ScriptParseTree =>"))
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
            Console.WriteLine(info);
        }
    }
}
