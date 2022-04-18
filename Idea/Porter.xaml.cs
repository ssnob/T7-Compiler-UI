using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Linq;

namespace Idea
{
    /// <summary>
    /// Interaction logic for Porter.xaml
    /// </summary>
    public partial class Porter : Window
    {
        private string _port = "";
        private string _output = "";

        public Porter(string input, string output)
        {
            InitializeComponent();
            Topmost = true;
            _port = input;
            _output = output;

            try
            {
                if (_port != "")
                        PortPro.Content = "IL Project: " + (string)input.Substring(input.LastIndexOf('\\') + 1);

                if (_output != "")
                        OutPut.Content = "Output Folder: " + (string)output.Substring(output.LastIndexOf('\\') + 1);
            }
            catch
            {

            }
        }

        private void HidePortWindow(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        public void ShowPortWindow()
        {
            Porter port = new Porter("unknown", "unknown");
            port.ShowDialog();
        }

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
                string targetFilePath = System.IO.Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = System.IO.Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        private void SelectILProject(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select IL Project";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = "C:\\";
                var result = dialog.ShowDialog();

                if (result != CommonFileDialogResult.Ok) return;

                
                _port = dialog.FileName;
                PortPro.Content = "IL Project:" + (string)dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\') + 1);
                Topmost = true;
            }
        }

        private void SelectOutputFolder(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select Output Folder";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = "C:\\";
                var result = dialog.ShowDialog();

                if (result != CommonFileDialogResult.Ok)
                {
                    Topmost = true;
                    return;
                }

                if (Directory.Exists(dialog.FileName) && !IsDirectoryEmpty(dialog.FileName))
                {
                    MessageBox.Show("The folder you select must be empty.", "Error: Folder has contents", MessageBoxButton.OK, MessageBoxImage.Error);
                    Topmost = true;
                    return;
                }
            
                _output = dialog.FileName;
                OutPut.Content = "Output Folder: " + (string)dialog.FileName.Substring(dialog.FileName.LastIndexOf('\\') + 1);
                Topmost = true;
            }
        }
        private bool Port()
        {
            Topmost = true;
            if (_port == null || _output == null)
                return false;

            if (_port.Contains(_output) || _output.Contains(_port))
            {
                MessageBox.Show("You must select two different folders (Your ouput folder must be different from your import folder/Import folder must be different from your output folder)", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                Topmost = false;
                return false;
            }

            string scripts = _output + "\\scripts";

            Directory.CreateDirectory(scripts);

            CopyDirectory(_port, _output + "\\scripts", true);

            foreach (var file in Directory.GetFiles(scripts, "*.il", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }

            // credits serious
            foreach (var file in Directory.GetFiles(scripts, "*.gsc", SearchOption.AllDirectories))
            {
                string source = File.ReadAllText(file);
                string lower = source.ToLower();
                int index;
                while ((index = lower.IndexOf("enableonlinematch")) > -1)
                {
                    source = new string(source.Take(index).ToArray()) + "getplayers" + new string(source.Skip(index + "enableonlinematch".Length).ToArray());
                    lower = source.ToLower();
                }
                File.WriteAllText(file, source);
            }

            File.WriteAllText(_output + "\\gsc.conf", "symbols=bo3,serious,zm");

            var s = MessageBox.Show("Project ported sucessfully.", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);

            _output = "";
            _port = "";
            this.Close();
            return false; 
        }
        private void PortBegin(object sender, RoutedEventArgs e)
        {
            Port();
        }

        public static void PortProject(string input, string output)
        {
            new Porter(input, output).ShowDialog();
        }
    }
}
