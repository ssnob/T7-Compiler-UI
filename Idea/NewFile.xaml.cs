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

namespace Idea
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    /// 
    public partial class NewFile : Window
    {
        public static string PATH;
        public NewFile()
        {
            InitializeComponent();
            var esc = new RoutedCommand();
            esc.InputGestures.Add(new KeyGesture(Key.Escape, ModifierKeys.None));
            var esckey = new CommandBinding(esc, CancelClick);
            this.CommandBindings.Add(esckey);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            if (FileNAME.Text.Length <= 0)
                return;

            SelectedType.SelectedIndex = 0;
            string toendwith = null;
            switch (SelectedType.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    toendwith = "gsc";
                    break;
                case 1:
                    toendwith = "txt";
                    break;
            }
            if (!File.Exists($"{PATH}\\scripts\\{FileNAME.Text}.{toendwith}"))
                File.WriteAllText($"{PATH}\\scripts\\{FileNAME.Text}.{toendwith}", $"// File - {FileNAME.Text}.{toendwith}");
            else
            {
                MessageBox.Show($"File Already Exist ({FileNAME.Text}.{toendwith})", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.Close();
        }

        private void Test(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("RBM Pressed");
        }
    }
}
