using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DebugCompiler;

namespace DebugCompiler
{
    public partial class UI : Form
    {
        List<string> Logging = new List<string>();
        DateTime dTIME = DateTime.Now;
        public string injectpath = string.Empty;
        private string selectedFolder = string.Empty;
        public string todelete = string.Empty;
        public string gm = string.Empty;
        public string gscpath = string.Empty;
        public string gamemode = string.Empty;
        public string game = string.Empty;
        public string menu = string.Empty;

        public string foldersel = string.Empty;

        bool folderselect = false;

        public UI()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            //resetGsc.Hide();
            //InjectButton.Hide();
//#if DEBUG
//            InjectButton.Show();
//            resetGsc.Show();
//#endif
        }

        private void LoadZip_Click(object sender, EventArgs e)
        {
            folderselect = false;
            // filedialog
            ZIPSelector.Filter = "Zip files (*.zip)|";
            System.Windows.Forms.DialogResult result = ZIPSelector.ShowDialog();

            if (!ZIPSelector.FileName.Contains(".zip"))
            {
                MessageBox.Show("Please Select A Zip File!");
            }
            else if (result == System.Windows.Forms.DialogResult.OK)
            {
                // get our current directory
                string directory = Environment.CurrentDirectory;
                gscpath = directory + "\\temp\\gsc\\";
                // zip stuff
                if (Directory.Exists(gscpath))
                {
                    // pass
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(gscpath);
                }

            retry:
                try { System.IO.Compression.ZipFile.ExtractToDirectory(ZIPSelector.FileName, gscpath); }
                catch { Directory.Delete(gscpath, true); goto retry; }

                Logging.Add("Loaded Zip");

                injectpath = gscpath + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
                todelete = injectpath + "\\.vscode";
                Directory.Delete(todelete, true);

                gm = System.IO.File.ReadAllText(injectpath + "\\gsc.conf");


                //GmBox.Show();

                //if (gm.Contains("zm"))
                //{
                //    GmBox.Text = "Zombies";
                //}
                //if (gm.Contains("mp"))
                //{
                //    GmBox.Text = "Multiplayer";
                //}

                //InjectButton.Show();

                CurrentMenu.Text = "Selected File: " + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
            }
        }


        private void UI_Load(object sender, EventArgs e)
        {

        }
        private void resetGsc_Click(object sender, EventArgs e)
        {
            Root root = new Root();
            root.LastGameInjected = TreyarchCompiler.Enums.Games.T7;
            root.NoExcept(root.FreeActiveScript);
            MessageBox.Show("Tree Reset", "GSC");
        }

        public void RESETGSCSHOW()
        {
           // resetGsc.Show();
        }

        // c
        static void CopyDirectory(string sourceDir, string destDir)
        {
            var sourceDirInfo = new DirectoryInfo(sourceDir);
            if (!sourceDirInfo.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: '{sourceDir}'");
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sourceDirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Copy subdirectories
            DirectoryInfo[] subDirs = sourceDirInfo.GetDirectories();
            foreach (DirectoryInfo subdir in subDirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Zip File Must be structured like this\n" +
                "------------------\n" +
                "ZIP\n" +
                "Menu Folder\n" +
                "scripts\n" +
                "gsc.conf\n" +
                "------------------\n" + "Hit Ok to open an example, if you understood hit Cancel", "Help", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk
            ) == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start("https://ibb.co/RT12Qs5");
                    }
            }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            // filedialog
            ZIPSelector.Filter = "Zip files (*.zip)|";
            System.Windows.Forms.DialogResult result = ZIPSelector.ShowDialog();

            if (!ZIPSelector.FileName.Contains(".zip"))
            {
                //MessageBox.Show("Please Select A Zip File!");
            }
            else if (result == System.Windows.Forms.DialogResult.OK)
            {
                // get our current directory
                string directory = Environment.CurrentDirectory;
                gscpath = directory + "\\temp\\gsc\\";
                // zip stuff
                if (Directory.Exists(gscpath))
                {
                    // pass
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(gscpath);
                }

            retry:
                try { System.IO.Compression.ZipFile.ExtractToDirectory(ZIPSelector.FileName, gscpath); menu = ZIPSelector.FileName; }
                catch { Directory.Delete(gscpath, true); goto retry; }

                Logging.Add("Loaded Zip");
                menu = ZIPSelector.FileName;
                injectpath = gscpath + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
                todelete = injectpath + "\\.vscode";
                try
                {
                    Directory.Delete(todelete, true);
                }
                catch { }

                gm = System.IO.File.ReadAllText(injectpath + "\\gsc.conf");


                //GmBox.Show();

                if (gm.Contains("zm"))
                {
                    Zombies.BackColor = Color.FromArgb(28, 28, 28);
                    MultiPlayer.BackColor = Color.Transparent;
                    gamemode = "ZM";
                }
                if (gm.Contains("mp"))
                {
                    MultiPlayer.BackColor = Color.FromArgb(28, 28, 28);
                    Zombies.BackColor = Color.Transparent;
                    gamemode = "MP";
                }
                else if (!gm.Contains("mp") || !gm.Contains("zm"))
                {
                    gamemode = "ZM";
                }

                //InjectButton.Show();
                folderselect = false;
                CurrentMenu.Text = "Menu: " + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
                callzomb();
            }
        }

        private void Compile_Click(object sender, EventArgs e)
        {
            Root root = new Root();
            ErrorForm ef = new ErrorForm();

            if (!folderselect)
            {
                string sourceDirectory = injectpath + "\\scripts";
                string destinationDirectory = Environment.CurrentDirectory + "\\scripts";
            again:
                try
                {
                    root.cmd_Compile(new string[] { sourceDirectory, "pc", null, "false", "--build" });
                }
                catch { System.IO.Compression.ZipFile.ExtractToDirectory(menu, gscpath); goto again; }
            }
            if (folderselect)
            {
                string sourceDirectory = selectedFolder + "\\scripts";
                root.cmd_Compile(new string[] { sourceDirectory, "pc", null, "false", "--build" });
            }
            return;
        }

        private void HelpBut_Click(object sender, EventArgs e)
        {
            ErrorForm errorform = new ErrorForm();
            errorform.setTitle("Help");
            errorform.openme("Zip File Must be structured like this\n------------------\nZIP\nMenu Folder\nscripts\ngsc.conf\n------------------\nExample in the link below.");
            errorform.setlinks("https://i.ibb.co/QvZ9YT5/Example.png");
        }

        private void CurrentMenu_Click(object sender, EventArgs e)
        {

        }

        public void Zombies_Click(object sender, EventArgs e)
        {
            ErrorForm errorform = new ErrorForm();
            try
            {
                callzomb();
            }
            catch
            {
                errorform.setTitle("Error");
                errorform.openme("Please load a menu first.\n");
                errorform.hidelink();
            }
        }
        public void callzomb()
        {
            if (folderselect)
            {
                File.WriteAllText(gm, String.Empty);
                File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,zm");
                Zombies.BackColor = Color.FromArgb(28, 28, 28);
                MultiPlayer.BackColor = Color.Transparent;
                gamemode = "ZM";
            }
            else
            {
                File.WriteAllText(gm, String.Empty);
                File.WriteAllText("gsc.conf", "symbols=serious,zm");
                Zombies.BackColor = Color.FromArgb(28, 28, 28);
                MultiPlayer.BackColor = Color.Transparent;
                gamemode = "ZM";
                CurrentMenu.Text = "Menu: " + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
            }
        }
        private void MultiPlayer_Click(object sender, EventArgs e)
        {
            ErrorForm errorform = new ErrorForm();
            try
            {
                if (folderselect)
                {
                    File.WriteAllText(gm, String.Empty);
                    File.WriteAllText(selectedFolder + "\\gsc.conf", "symbols=serious,mp");
                    MultiPlayer.BackColor = Color.FromArgb(28, 28, 28);
                    Zombies.BackColor = Color.Transparent;
                    gamemode = "MP";
                }
                else
                {
                    File.WriteAllText(gm, String.Empty);
                    File.WriteAllText("gsc.conf", "symbols=serious,mp");
                    MultiPlayer.BackColor = Color.FromArgb(28, 28, 28);
                    Zombies.BackColor = Color.Transparent;
                    gamemode = "MP";
                    CurrentMenu.Text = "Menu: " + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
                }
                
            }
            catch
            {
                errorform.setTitle("Error");
                errorform.openme("Please load a menu first.\n");
                errorform.hidelink();
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
             Root root = new Root();
             root.NoExcept(root.FreeT7Script);
            
        }

        private void BO3_Click(object sender, EventArgs e)
        {

        }

        private void Folder_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolder = dialog.SelectedPath;
                }

                gm = System.IO.File.ReadAllText(selectedFolder + "\\gsc.conf");


                //GmBox.Show();

                if (gm.Contains("zm"))
                {
                    Zombies.BackColor = Color.FromArgb(28, 28, 28);
                    MultiPlayer.BackColor = Color.Transparent;
                    gamemode = "ZM";
                }
                if (gm.Contains("mp"))
                {
                    MultiPlayer.BackColor = Color.FromArgb(28, 28, 28);
                    Zombies.BackColor = Color.Transparent;
                    gamemode = "MP";
                }
                else if (!gm.Contains("mp") || !gm.Contains("zm"))
                {
                    gamemode = "ZM";
                }
                folderselect = true;
                CurrentMenu.Text = "Menu: " + Path.GetFileName(selectedFolder);
                callzomb();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }       

}           

            

            

            
