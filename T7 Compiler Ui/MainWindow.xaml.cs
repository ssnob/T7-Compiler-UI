using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using Path = System.IO.Path;

namespace Infinity_Loader_3._0 // lel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool menuselected = false;
        public bool iserroropen = false;
        public bool isfolder = false;

        private string selectedFolder = string.Empty;
        public string gm = string.Empty;
        public string GSCCONFIG = string.Empty;
        public static string VersionUrl = "https://gsc.dev/t7c_version";
        public static string UpdaterURL = "https://gsc.dev/t7c_updater";

        public string game = string.Empty;

        public int gamemode;

        string version;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        public MainWindow()
        {
            InitializeComponent();
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); // hide reading console

            GmTb.IsReadOnly = true;
            GmTb.IsHitTestVisible= false;

            System.Net.WebClient wc = new System.Net.WebClient();
            try
            {
                version = wc.DownloadString(VersionUrl);
                Version.Content = "Latest Version: " + version;
            }
            catch
            {
                Version.Content = "Offline";
            }
            MainWin.Title = "Compiler Ui";

            game = "bo3";
            gamemode = 1;
            setBO3.Background = new SolidColorBrush(Color.FromArgb(55, 225, 225, 225));

            if (!Directory.Exists(@"C:/t7compiler"))
            {
                update();
            }

            if (Directory.Exists(@"C:/t7compiler"))
            {
                if (!File.Exists(@"C:/t7compiler/TreyarchCompiler.dll") || !File.Exists(@"C:/t7compiler/debugcompiler.exe"))
                {
                    update();
                }
            }
        }
        #region ghostprocessfix
        private void ByeBye(object sender, CancelEventArgs e)
        {
            //Process[] compiler = Process.GetProcessesByName("t7 compiler ui");
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");

            try
            {
                try { debugcompiler[0].Kill(); } catch { }
            }
            catch { }

            
            Environment.Exit(0);
        }
        #endregion

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

        private void Compile(object sender, RoutedEventArgs e)
        {
            string realgm = string.Empty;
            
            ErrorWindow ErrorWin = new ErrorWindow();

            Process[] compiler = Process.GetProcessesByName("debugcompiler");         
            Process[] bo3 = Process.GetProcessesByName("blackops3");
            Process[] bo4 = Process.GetProcessesByName("blackops4");

            if (!menuselected)
            {
                MainWin.Hide(); // throw up a reminder to select a folder
                ErrorWin.Error("Please select a folder before trying to compile!");
                return;
            }

            string[] sp =
            {
                "game=T8",
                "script=scripts\\core_common\\load_shared.gsc",
            };
            string[] mp =
            {
                "game=T8",
                "script=scripts\\mp_common\\bb.gsc",
            };
            string[] zm =
            {
                "game=T8",
                "script=scripts\\zm_common\\load.gsc",
            };
            if (gamemode == 0) 
            { 
                if(game == "bo3")
                {
                    System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,mp");
                }
                if (game == "bo4")
                {
                    System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", mp);
                }
                realgm = "mp";
            } 
            if (gamemode == 1) 
            {
                if (game == "bo3")
                {
                    System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,zm");
                }
                if (game == "bo4")
                {
                    System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", zm);
                }
                realgm = "zm";
            }
            if (gamemode == 2) 
            {
                if (game == "bo3")
                {
                    System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,mp");
                }
                if (game == "bo4")
                {
                    System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", sp);
                }
                realgm = "sp";
            }

            string newinfo;

            if (selectedFolder.StartsWith(@"C:\"))
            {
                var direct = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + new DirectoryInfo(System.IO.Path.GetDirectoryName(selectedFolder + @"\\")).Name;

                if (Directory.Exists(direct))
                {
                    Directory.Delete(direct, true);
                }
                    
                Directory.CreateDirectory(direct);

                CopyDirectory(selectedFolder, direct, true);

                newinfo = direct;
            }

            else
            {
                newinfo = selectedFolder;
            }

            File.WriteAllText(newinfo + @"\compile.bat", "cd " + newinfo.Replace(@"/", @"\") + "\nC:\\t7compiler\\debugcompiler --build");
            ProcessStartInfo startInfo = new ProcessStartInfo(); 

            startInfo.FileName = newinfo + @"\compile.bat";                 // this is what it will execute
            startInfo.UseShellExecute = false;                              
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process proc = Process.Start(startInfo);
            new Task(() =>
            {
                System.Threading.Thread.Sleep(4000);
                Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
                try { debugcompiler[0].Kill(); } catch { }
                this.Cursor = Cursors.Arrow;
            }
            ).Start();

            var info = proc.StandardOutput.ReadToEnd();

            if (info.Contains("Press any key to reset gsc parsetree"))
            {
                #region replacespam
                info = info.Replace(Environment.CurrentDirectory, "");
                info = info.Replace(newinfo, "");
                info = info.Replace(">cd", "");
                info = info.Replace(">C:\\t7compiler\\debugcompiler --build", "");
                info = info.Replace("\\" + new DirectoryInfo(System.IO.Path.GetDirectoryName(selectedFolder + @"\\")).Name, "");
                info = info.Replace("Script compiled. Press I to inject or anything else to continue", "");
                info = info.Replace("compiled.gscc", "");
                info = info.Replace("Press any key to reset gsc parsetree...", "");
                #endregion

                ErrorWin.hide = false;
                ErrorWin.Title = "Success";
                ErrorWin.Reason.Content = "Success";
                ErrorWin.Error("Your menu was sucessfully compiled and injected!:" + info);  
            }


            
            if (!info.Contains("Press any key to reset gsc parsetree"))
            {
                #region replacespam
                info = info.Replace(Environment.CurrentDirectory, "");
                info = info.Replace(newinfo, "");
                info = info.Replace(">cd", "");
                info = info.Replace(">C:\\t7compiler\\debugcompiler --build", "");
                info = info.Replace("\\" + new DirectoryInfo(System.IO.Path.GetDirectoryName(selectedFolder + @"\\")).Name, "");
                info = info.Replace("Script compiled. Press I to inject or anything else to continue", "");
                info = info.Replace("compiled.gscc", "");
                #endregion

                ErrorWin.hide = false;
                ErrorWin.Title = "Error";
                ErrorWin.Reason.Content = "Warning";
                ErrorWin.ErrorText.FontSize = 15;
                ErrorWin.Error("An Error Occured:" + info);
            }
        }

        public void hideshowballs(string a1)
        {
            switch (a1)
            {
                case "hide":
                    this.Hide();
                    break;
                case "show":
                    this.Show();
                    break;
            }
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            ErrorWindow ErrorWin = new ErrorWindow();

            if(game == null)
            {
                ErrorWin.Error("Please select a gamemode before trying to select a menu!");
            }

            GSCCONFIG = string.Empty; // used for qol

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select Folder";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = "C:\\";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    GSCCONFIG = selectedFolder + "\\gsc.conf";
                    selectedFolder = dialog.FileName;
                    if (!Directory.Exists(selectedFolder + "\\scripts"))
                    {
                        ErrorWin.hide = false;
                        ErrorWin.setTitle("Error");
                        ErrorWin.Error("Folder does not contain a scripts folder.");
                        return;
                    }
                }
                try
                {
                    var read = System.IO.File.ReadLines(selectedFolder + "\\gsc.conf").First(); //  try to read this

                    if (read.Contains("mp"))
                        gamemode = 0;
                    if (read.Contains("zm"))
                        gamemode = 1;
                    if (read.Contains("sp"))
                        gamemode = 2;

                    GmTb.Text = read;
                }
                catch
                {
                    System.IO.File.Create(selectedFolder + "\\gsc.conf");
                    System.IO.File.WriteAllText(GSCCONFIG, "symbols=bo3,serious,zm"); //default to zm just because we can
                    GmTb.Text = System.IO.File.ReadLines(selectedFolder + "\\gsc.conf").First();
                    gamemode = 1;
                }

                MenuName.Content = new DirectoryInfo(System.IO.Path.GetDirectoryName(selectedFolder + @"\\")).Name;
                isfolder = true;
                menuselected = true;
            }
        }

        public void SetMpInt(object sender, RoutedEventArgs e)
        {
            string[] lines =
            {
                    "symbols=bo4,serious,mp",
                    "script=scripts\\mp_common\\bb.gsc",
            };

            ErrorWindow ErrorWin = new ErrorWindow();

            if (game == string.Empty)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a game before trying to change gamemodes!");
                return;
            }

            if (!menuselected)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }

            gamemode = 0;
            if (game == "bo3")
            {
                System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,mp");
                updateTextBox();
            }
            if (game == "bo4")
            {
                System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", lines);
                GmTb.Text = "symbols=bo4,serious,mp";
            }
    }
        public void SetZmInt(object sender, RoutedEventArgs e)
        {
            string[] lines =
            {
                    "game=T8",
                    "script=scripts\\zm_common\\load.gsc",
            };

            ErrorWindow ErrorWin = new ErrorWindow();

            if (game == string.Empty)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a game before trying to change gamemodes!");
                return;
            }

            if (!menuselected)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }

            gamemode = 1;
            if (game == "bo3")
            {
                System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,zm");
                updateTextBox();
            }
            if (game == "bo4")
            {
                System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", lines);
                GmTb.Text = "symbols=bo4,serious,zm";
            }
        }
        private void SetSpInt(object sender, RoutedEventArgs e)
        {
            ErrorWindow ErrorWin = new ErrorWindow();
            if (game == string.Empty)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a game before trying to change gamemodes!");
                return;
            }
            string[] lines =
            {
                    "symbols=bo4,serious,sp",
                    "script=scripts\\core_common\\load_shared.gsc",
            };

            if (!menuselected)
            {
                ErrorWin.hide = false;
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }
            gamemode = 2;
            if (game == "bo3")
            {
                System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=bo3,serious,sp");
                updateTextBox();
            }
            if (game == "bo4")
            {
                System.IO.File.WriteAllLines(selectedFolder + "\\gsc.conf", lines);
                GmTb.Text = "symbols=bo4,serious,sp";
            }
        }
        public void updateTextBox()
        {
            GmTb.Text = System.IO.File.ReadLines(selectedFolder + "\\gsc.conf").First();
        }

        public void update()
        {
            ErrorWindow ErrorWin = new ErrorWindow();
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string version = wc.DownloadString(VersionUrl);

                string filename = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "t7c_installer.exe");
                if (File.Exists(filename)) File.Delete(filename);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(UpdaterURL, filename);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = filename;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = false;
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(filename, "--install_silent");

                // prob a better way to update lmao its whatever tho
            }
            catch
            {
                this.Hide();
                ErrorWin.setTitle("Error");
                ErrorWin.Error("There was a problem updating.");
            }
        }

        private void UpdateClient(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void setGame3(object sender, RoutedEventArgs e)
        {
            if (game == "bo3")
                return;

            game = "bo3";

            setBO4.Background = new SolidColorBrush(Color.FromArgb(7, 255, 255, 255));
            setBO3.Background = new SolidColorBrush(Color.FromArgb(55, 225, 225, 225));
        }

        private void setGame4(object sender, RoutedEventArgs e)
        {
            if (game == "bo4")
                return;


            game = "bo4";

            setBO3.Background = new SolidColorBrush(Color.FromArgb(7, 255, 255, 255));
            setBO4.Background = new SolidColorBrush(Color.FromArgb(55, 225, 225, 225));
        }

        private void Discord(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discordapp.com/invite/gsc");
        }
    }
}
