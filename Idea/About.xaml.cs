using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Idea
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();     
        }

        private void HideAboutMe(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void ShowAboutMe(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void SeriousGitHub(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo proc = new ProcessStartInfo("https://github.com/shiversoftdev");
            proc.UseShellExecute = true;
            Process.Start(proc);
        }

        private void CompilerLink(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ProcessStartInfo proc = new ProcessStartInfo("https://github.com/shiversoftdev/t7-compiler");
            proc.UseShellExecute = true;
            Process.Start(proc);
        }

    }
}
