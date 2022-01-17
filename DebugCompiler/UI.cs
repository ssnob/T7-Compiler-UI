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

        public UI()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            resetGsc.Hide();
            InjectButton.Hide();
//#if DEBUG
//            InjectButton.Show();
//            resetGsc.Show();
//#endif
        }

        private void LoadZip_Click(object sender, EventArgs e)
        {
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


                GmBox.Show();

                if (gm.Contains("zm"))
                {
                    GmBox.Text = "Zombies";
                }
                if (gm.Contains("mp"))
                {
                    GmBox.Text = "Multiplayer";
                }

                InjectButton.Show();

                CurrentMenu.Text = "Selected File: " + ZIPSelector.SafeFileName.Replace(".zip", "").ToString();
            }
        }


        private void UI_Load(object sender, EventArgs e)
        {

        }

        private void InjectButton_Click(object sender, EventArgs e)
        {
            
            string sourceDirectory = injectpath + "\\scripts";
            string destinationDirectory = Environment.CurrentDirectory + "\\scripts";

            if (Directory.Exists(destinationDirectory))
            {
                Directory.Delete(destinationDirectory, true);
            }

            CopyDirectory(sourceDirectory, destinationDirectory);

            Root root = new Root();
            root.cmd_Compile(new string[] { "scripts", "pc", null, "false", "--build" });

            Directory.Delete(sourceDirectory, true);
            File.Delete("compiled.gsc");
            File.Delete("compiled.gscc");
            File.Delete("compiled.omap");

            resetGsc.Show();

            return;
        }

        private void FolderLoad_Click(object sender, EventArgs e)
        {

            // todo

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolder = dialog.SelectedPath;
                }
            }
        }

        private void GmBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GmBox.Text == "Zombies")
            {
                File.WriteAllText(gm, String.Empty);
                File.WriteAllText("gsc.conf", "symbols=serious,zm");
            }
            if (GmBox.Text == "Multiplayer")
            {
                File.WriteAllText(gm, String.Empty);
                File.WriteAllText("gsc.conf", "symbols=serious,mp");
            }
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
            resetGsc.Show();
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
    }
}
