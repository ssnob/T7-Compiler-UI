using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DevInjector.Actions.Games;
namespace DevInjector.Actions
{
    internal class Compile
    {
        public string[] symbols = {"", "", "", "", "", "" , "" };

        public string GetGame(GAMES games)
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
        public string GetGamemode(GAMEMODE gamemode)
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
        public void CompileFinal(string path, GAMES game, GAMEMODE gamemode, bool folderselected)
        {
            MessageError merror = new MessageError();

            if (!folderselected)
            {
                merror.SendMessage("Error", "Folder Not selected.");
                return;
            }
            if (!Directory.Exists(path))
            {
                merror.SendMessage("Error", "Folder does not exist.");
                return;
            }

            if (!Directory.Exists("c:\\t7compiler"))
            {
                merror.SendMessage("Error", "Compiler Installation does not exist. (did you delete your t7compiler directory?)");
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
                        merror.SendMessage("Error", "gsc.conf is being used by another program.");
                        return;
                    }
                    break;
                case "bo4":
                    try
                    {
                        File.WriteAllText(path + @"\gsc.conf", "game=t8\n" + "scripts=" + bo4symbols);
                    }
                    catch
                    {
                        merror.SendMessage("Error", "gsc.conf is being used by another program.");
                        return;
                    }
                    break;
            }

            File.WriteAllText(path + @"\compile.bat", "cd " + path.Replace(@"/", @"\") + "\nC:\\t7compiler\\debugcompiler --build");
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = path + @"\compile.bat";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process proc = Process.Start(startInfo);

            string info = string.Empty;


            new Task(() =>
            {
                System.Threading.Thread.Sleep(2400);
                Process[] proca = Process.GetProcessesByName("debugcompiler");
                proca[0].Kill();
            }).Start();

            info = proc.StandardOutput.ReadToEnd();

            Task.WaitAll();
            #region replacespam
            info = info.Replace(Environment.CurrentDirectory, "");
            info = info.Replace(path, "");
            info = info.Replace(">cd", "");
            info = info.Replace(">C:\\t7compiler\\debugcompiler --build", "");
            info = info.Replace("\\" + new DirectoryInfo(System.IO.Path.GetDirectoryName(path + @"\\")).Name, "");
            info = info.Replace("Script compiled. Press I to inject or anything else to continue", "");
            info = info.Replace("compiled.gscc", "");
            info = info.Replace("Press any key to reset gsc parsetree...", "");
            info = info.Replace("If in game, you are probaly going to crash...", "");
            #endregion

            var resultString = Regex.Replace(info, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            File.WriteAllText(Environment.CurrentDirectory + "\\output.txt", resultString + "---------------------------");

            //if (info.Contains("No game process found"))
            //{
            //    merror.SendMessage("Result", "No game process found");
            //    return;
            //}
            var SplitBy = "\n";
            TextReader tr = new StreamReader(Environment.CurrentDirectory + "\\output.txt");
            var fullLog = tr.ReadToEnd();

            String[] sections = fullLog.Split(new string[] { SplitBy }, StringSplitOptions.None);

            merror.SetTextSize(16);
            try
            {
                merror.SendMessage("Result", sections[5] + "\n" + sections[6] + "\n" + sections[7]);
            }
            catch
            {
                foreach(var s in sections)
                {
                    if(s.Contains("If in game, you are probaly going to crash..."))
                    {
                        merror.SendMessage("Sucess", "Menu Injected Succesfully");
                    }
                    else
                    {
                        string text = "";
                        for (int i = 2; i < sections.Length; i++)
                        {
                            text = text + sections[i].ToString();
                        }
                        merror.SendMessage("Result", text);
                    }
                }
            }
        }
    } 
}
