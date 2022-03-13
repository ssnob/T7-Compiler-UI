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

namespace DevInjector
{
    /// <summary>
    /// Interaction logic for MessageError.xaml
    /// </summary>
    public partial class MessageError : Window
    {
        public double previousfontsize;
        public MessageError()
        {
            MainWindow win = new MainWindow();
            InitializeComponent();
            this.Focus();
            win.IsEnabled = false;
            Message.BorderBrush = null;
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

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.IsEnabled = true;
            this.Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        public void SetReason(string text)
        {
            Message.Text = text;
        }
        public void SetTitle(string text)
        {
            Title.Content = text;
        }
        public void SendMessage(string caption, string text)
        {
            Title.Content = caption;
            Message.Text = text;
            this.Show();
        }

        public string GetMessageText()
        {
            return Message.Text;
        }

        public void SetTextSize(double size)
        {
            previousfontsize = Message.FontSize;
            Message.FontSize = size;
        }
    }
}
