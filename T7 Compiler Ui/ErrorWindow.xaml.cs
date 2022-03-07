using System;
using System.Collections.Generic;
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

namespace Infinity_Loader_3._0
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public bool hide = true;
        public ErrorWindow()
        {
            InitializeComponent();
            this.Title = "Error";
            this.Topmost = true;
        }

        private void AcceptPrompt(object sender, RoutedEventArgs e)
        {
            MainWindow mainwin = new MainWindow();

            resetme();

            this.Hide();

            if (hide)
                mainwin.hideshow("show");
        }
        
        public void setTitle(string title)
        {
            this.Title = title;
        }

        public void resetme()
        {
            ErrorText.Content = "";
            this.Title = "Error";
        }

        public void Error(string reason)
        {
            ErrorText.Content = reason;
            this.Show();
        }


    }
}
