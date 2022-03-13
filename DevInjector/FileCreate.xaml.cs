using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DevInjector
{
    /// <summary>
    /// Interaction logic for FileCreate.xaml
    /// </summary>
    public partial class FileCreate : Window
    {
        string path = null;
        public FileCreate()
        {
            InitializeComponent();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TabButtonEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void TabButtonLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = new SolidColorBrush(Colors.White);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        public void addToFileType(string toadd)
        {
            FileType.Items.Add(toadd);
        }
        public void resetFileType(string toadd)
        {
            FileType.Items.Clear();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();

            var name = FileNameTo.Text.Replace(".gsc", "");
            var gsc = path + "\\scripts\\" + name + FileType.Text;
            File.WriteAllText(gsc, "New File!");

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplySelection(object sender, SelectionChangedEventArgs e)
        {
            if (FileType.SelectedIndex == 0)
            {
                FileType.Text = ".gsc";
            }
            if (FileType.SelectedIndex == 1)
            {
                FileType.Text = ".txt";
            }
        }

        public void PopUp(string PATH)
        {
            path = PATH;
            this.Show();
        }
    }
}
