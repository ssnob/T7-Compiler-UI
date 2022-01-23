using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


        public int gamemode;

        public MainWindow()
        {
            InitializeComponent();
            System.Net.WebClient wc = new System.Net.WebClient();
            string version = wc.DownloadString(VersionUrl);
            MainWin.Title = "Compiler Ui";
            Version.Content = "Latest Version: " + version;
        }
        #region ghostprocessfix
        private void ByeBye(object sender, CancelEventArgs e)
        {
            Process[] compiler = Process.GetProcessesByName("t7 compiler ui");
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            try
            {
                debugcompiler[0].Kill();
                System.Threading.Thread.Sleep(200);
                compiler[0].Kill();
            }
            catch { }
            Environment.Exit(0);
        }
        #endregion
        private void Compile(object sender, RoutedEventArgs e)
        {
            string realgm = string.Empty;
            
            ErrorWindow ErrorWin = new ErrorWindow();

            Process[] compiler = Process.GetProcessesByName("debugcompiler");
            try
            {
                compiler[0].Kill();
            }               // cleanup from any previous compiles
            catch
            {
                // do nothing, this means there is no active ghost process
            }

            Process[] bo3 = Process.GetProcessesByName("blackops3");

            if (!menuselected)
            {
                MainWin.Hide(); // throw up a reminder to select a folder
                ErrorWin.Error("Please select a folder before trying to compile!");
            }
            else if (menuselected)
            {
                if (gamemode == 0) { System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,mp"); realgm = "mp"; } // set our gamemodes
                if (gamemode == 1) { System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,zm"); realgm = "zm"; }
                if (gamemode == 2) { System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,sp"); realgm = "sp"; }

                //                 this part gets a little confusing, what we're doing here is creating a batch file that will execute in the directory of our
                //                 selected menu folder, it will call the debugcompiler with the --build argument, this will tell it to compile & inject everything within the "scripts" folder
                File.WriteAllText(selectedFolder + "\\compile.bat", "cd " + selectedFolder + "\nc:/t7compiler/debugcompiler --build"); // create our batch

                ProcessStartInfo startInfo = new ProcessStartInfo(); // our class

                startInfo.FileName = selectedFolder + "\\compile.bat"; // this is what it will execute
                startInfo.UseShellExecute = true; // this will create the process from the folder its in
                startInfo.RedirectStandardOutput = false; // hides output
                startInfo.WindowStyle = ProcessWindowStyle.Hidden; // hides the entire window so we don't see it
                                                                   //Start the process
                if (bo3.Length == 0)
                {
                    MainWin.Hide();
                    ErrorWin.Title = "Error";
                    ErrorWin.Reason.Content = "Process";
                    ErrorWin.Error("Looks like your game isn't open!\nOpen your selected game then try again.");
                    return;
                }
                else
                {
                    Process proc = Process.Start(startInfo);

                    new Task(() =>
                    {
                        System.Threading.Thread.Sleep(15000); // this is a bit hacky but its all I could think of, this runs a background
                                                              // thread to kill the debug compiler after giving it
                                                              // more than enough time to compile/inject (note: may cause issues on slower pcs)
                        compiler[0].Kill();
                    }).Start(); // start our thread

                    // give the compiler some time to finish up
                    System.Threading.Thread.Sleep(700);


                    MainWin.Hide(); 

                    ErrorWin.Title = "Success";
                    ErrorWin.Reason.Content = "Success";
                    ErrorWin.Error("Your menu was sucessfully compiled and injected!\n" + selectedFolder.Replace("\t\\", "/") + "\nGamemode(" + gamemode + ")" + realgm);

                    menuselected = true;
                }

            }
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            GSCCONFIG = string.Empty; // used for qol

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select your menu folder";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK) { return; /* we dont care */ }
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    GSCCONFIG = selectedFolder + "\\gsc.conf";
                    selectedFolder = dialog.SelectedPath;
                }
                try
                {
                    GmTb.Text = System.IO.File.ReadAllText(selectedFolder + "\\gsc.conf").Replace("serious,", ""); // lets try to read this, if they dont have one lets make one!
                }
                catch
                {
                    System.IO.File.Create(selectedFolder + "\\gsc.conf");
                    System.IO.File.WriteAllText(GSCCONFIG, "symbols=serious,zm"); //default to zm just because we can
                    GmTb.Text = System.IO.File.ReadAllText(selectedFolder + "\\gsc.conf").Replace("serious,", ""); // just so it doesnt get confusing for newer people
                }

                isfolder = true;
                menuselected = true;
            }
        }

        public void SetMpInt(object sender, RoutedEventArgs e)
        {
            ErrorWindow ErrorWin = new ErrorWindow();
            if (!menuselected)
            {
                MainWin.Hide(); // throw up a reminder to select a folder
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }
            gamemode = 0;
            System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,mp");
            updateTextBox();
        }
        public void SetZmInt(object sender, RoutedEventArgs e)
        {
            ErrorWindow ErrorWin = new ErrorWindow();
            if (!menuselected)
            {
                MainWin.Hide(); // throw up a reminder to select a folder
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }
            gamemode = 1;
            System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,zm");
            updateTextBox();
        }

        public void updateTextBox()
        {
            GmTb.Text = System.IO.File.ReadAllText(selectedFolder + "\\gsc.conf").Replace("serious,", "");
        }

        private void UpdateClient(object sender, RoutedEventArgs e)
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
                ErrorWin.setTitle("Error");
                ErrorWin.Error("There was a problem updating.");
            }
        }

        private void SetSpInt(object sender, RoutedEventArgs e)
        {
            ErrorWindow ErrorWin = new ErrorWindow();
            if (!menuselected)
            {
                MainWin.Hide(); // throw up a reminder to select a folder
                ErrorWin.Error("Please select a folder before trying to change gamemodes!");
                return;
            }
            gamemode = 2;
            System.IO.File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,sp");
            updateTextBox();
        }
    }
}
