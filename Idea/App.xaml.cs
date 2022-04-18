using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Idea
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CloseWindow_Event(object sender, RoutedEventArgs e)
        {
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            try { debugcompiler[0].Kill(); } catch { }

            Process[] injector = Process.GetProcessesByName("idea");
            try { injector[0].Kill(); } catch { }

            Environment.Exit(0);
        }
        private void AutoMinimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
                try { MaximizeRestore(Window.GetWindow((FrameworkElement)e.Source)); }
                catch { }
        }
        private void Minimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source != null)
                try { MinimizeWind(Window.GetWindow((FrameworkElement)e.Source)); }
                catch { }
        }

        public void CloseWind(Window window) => window.Close();
        public void MaximizeRestore(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;
            else if (window.WindowState == WindowState.Normal)
                window.WindowState = WindowState.Maximized;
        }
        public void MinimizeWind(Window window) => window.WindowState = WindowState.Minimized;

        private void OnWindowClosing(object sender, RoutedEventArgs e)
        {

        }
        private void CloseWindowNOQUIT_Event(object sender, RoutedEventArgs e)
        {
            About about = new About();
            Porter port = new Porter(null, null);
            about.Close();
            port.Close();
        }
    }
}
