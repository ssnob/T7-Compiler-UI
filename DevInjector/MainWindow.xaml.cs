using DevInjector.Actions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Management;

namespace DevInjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region window
        string selectedbutton = "";
        #endregion

        public bool isfolderselected = false;
        public bool infile = false;

        public string path;
        public string filename;

        Games.GAMES GAME;
        Games.GAMEMODE GAME_MODE;
        public MainWindow()
        {
            InitializeComponent();
            OnStartUp();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs Event)
        {
            base.OnMouseLeftButtonDown(Event);
            this.DragMove();
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public override void OnApplyTemplate()
        {
            foreach (string s in "FocusVisualElement,MouseOverBorder".Split(','))
            {
                var bdr = GetTemplateChild(s) as Border;
                if (bdr != null)
                {
                    bdr.BorderThickness = new Thickness(0);
                }
            }
            base.OnApplyTemplate();
        }
        #region Window

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            try { debugcompiler[0].Kill(); } catch { }
            Application.Current.Shutdown();
        }

        private void MinClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TabButtonEnter(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Name == "exitButton")
            {
                ((Button)sender).Foreground = new SolidColorBrush(Color.FromArgb(255, 247, 11, 20));
            }
            else
            {
                ((Button)sender).Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void TabButtonLeave(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Name == "exitButton")
            {
                ((Button)sender).Background = new SolidColorBrush(Color.FromArgb(0, 247, 11, 20));
            }
            ((Button)sender).Foreground = new SolidColorBrush(Colors.White);
        }

        // Setup Button
        public void OnMouseEnterButton(object sender, EventArgs e)
        {
            var currentButton = sender as Button;

            if (currentButton.Name != "MenuIcon")
                if (selectedbutton != currentButton.Name)
                    currentButton.Background = new SolidColorBrush(Color.FromArgb(100, 74, 74, 74));

            if (currentButton.Name == "MenuIcon")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));
            }


            if (currentButton.Name == "CampaignButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 45, 45, 45));
            }
            
            if (currentButton.Name == "MultiplayerButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 45, 45, 45));
            }
            
            if (currentButton.Name == "ZombiesButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 45, 45, 45));
            }
            
        }

        // Restore button
        public void OnMouseLeaveButton(object sender, EventArgs e)
        {
            var currentButton = sender as Button;

            if (currentButton.Name != "MenuIcon")
                if (selectedbutton != currentButton.Name)
                    currentButton.Background = new SolidColorBrush(Color.FromArgb(100, 12, 12, 12));

            if (currentButton.Name == "MenuIcon")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 12, 12, 12));
            }

            if (currentButton.Name == "CompileBtn")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            }

            if (currentButton.Name == "CampaignButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            }

            if (currentButton.Name == "MultiplayerButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            }

            if (currentButton.Name == "ZombiesButton")
            {
                currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            }
            
            

        }


        // Setup Button
        private void OnMouseEnterBlueButton(object sender, EventArgs e)
        {
            var currentButton = sender as Button;
            currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 79, 158, 234));
        }

        // Restore button
        private void OnMouseLeaveBlueButton(object sender, EventArgs e)
        {
            var currentButton = sender as Button;
            currentButton.Background = new SolidColorBrush(Color.FromArgb(255, 30, 144, 255));
        }

        // Setup Label
        private void OnMouseEnterLabel(object sender, EventArgs e)
        {
            var currentLable = sender as Label;
            if (selectedbutton != currentLable.Name)
            {
                if (currentLable.Name.Contains("MenuIcon"))
                {
                    MenuIcon.Background = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));
                }
                if (currentLable.Name.StartsWith("BlackOps3"))
                {
                    BlackOps3.Background = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));
                }
                if (currentLable.Name.StartsWith("BlackOps4"))
                {
                    BlackOps4.Background = new SolidColorBrush(Color.FromArgb(255, 23, 23, 23));
                }
            }
        }

        // Restore Label
        private void OnMouseLeaveLabel(object sender, EventArgs e)
        {
           var currentLable = sender as Label;
            if (selectedbutton != currentLable.Name)
            {
                if (currentLable.Name.Contains("MenuIcon"))
                {
                    MenuIcon.Background = new SolidColorBrush(Color.FromArgb(255, 12, 12, 12));
                }
                if (currentLable.Name.StartsWith("BlackOps3"))
                {
                    BlackOps3.Background = new SolidColorBrush(Color.FromArgb(255, 12, 12, 12));
                }
                if (currentLable.Name.StartsWith("BlackOps4"))
                {
                    BlackOps4.Background = new SolidColorBrush(Color.FromArgb(255, 12, 12, 12));
                }
            }

        }

        // Setup State
        private void GetButtonState(Button button)
        {
            byte[] colors = { 74, 74, 74 };
            SetButtonState(button, button.Name, colors);
        }

        // Set background according to name of button
        private void SetButtonState(Button button, string name, byte[] colors)
        {
            button.Background = new SolidColorBrush(Color.FromArgb(100, colors[0], colors[1], colors[2]));
            selectedbutton = button.Name;
        }

        // Set stroke to existing rectangle
        private void SetButtonBorder(Rectangle rec)
        {
            rec.Stroke = new SolidColorBrush(Color.FromArgb(100, 225, 225, 225));
        }

        // Reset Buttons & Rectangles
        private void ResetButtonStates()
        {
            byte[] oldcolors = { 12, 12, 12 };
            foreach (Button button in SideBar.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush(Color.FromArgb(100, oldcolors[0], oldcolors[1], oldcolors[2]));
            }
            foreach (Rectangle rec in SideBar.Children.OfType<Rectangle>())
            {
                rec.Stroke = new SolidColorBrush(Color.FromArgb(100, oldcolors[0], oldcolors[1], oldcolors[2]));
            }
            selectedbutton = "";
        }

        // Tab Control
        private void SetupButton(Button button, Rectangle rec)
        {
            ResetButtonStates();
            GetButtonState(button);
            SetButtonBorder(rec);
        }

        private void DisableLabel(Label label)
        {
            label.IsEnabled = false; 
        }
        private void EnableLable(Label label)
        {
            label.IsEnabled = true;
        }
        #endregion

        #region Imports
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            string text = null;
            if (File.Exists(Environment.CurrentDirectory + "\\log.txt"))
                text = File.ReadAllText(Environment.CurrentDirectory + "\\log.txt");

            File.WriteAllText(Environment.CurrentDirectory + "\\log.txt", text + "\n" + ex.StackTrace);
        }

        private static void DispatcherEvent(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            string text = null;
            if (File.Exists(Environment.CurrentDirectory + "\\log.txt"))
                text = File.ReadAllText(Environment.CurrentDirectory + "\\log.txt");

            File.WriteAllText(Environment.CurrentDirectory + "\\log.txt", text + "\n" + ex.StackTrace);
        }


        [STAThread]
        private void OnStartUp()
        {
            AllocConsole();
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);
#if (!DEBUG)
            // Ensure we are running as admin.
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Program Must be ran as admin", "Error");
                Application.Current.Shutdown();
                return;
            }

            // Force an install
            if (!Directory.Exists("c:\\t7compiler")) 
            {
                var msg = MessageBox.Show("T7Compiler is not installed. \nInstall It now? (REQUIRED)", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (msg != MessageBoxResult.Yes) Application.Current.Shutdown();

                var updatepath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\netcoreapp3.1\\T7Installer.exe";

                if (!File.Exists(updatepath))
                {
                    MessageBox.Show("Extract ALL contents of the zip into a folder and try again.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    return;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = updatepath;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = Process.Start(startInfo);

                string info = null;
                info = proc.StandardOutput.ReadToEnd();

                Task.WaitAll();
                MessageError me = new MessageError();
                if (info.Contains("Complete."))
                {
                    me.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    me.Topmost = true;
                    me.SendMessage("Result", "Compiler Sucessfully Installed.");
                    me.Focus();
                }
                else
                {
                    me.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    me.Topmost = true;
                    me.SendMessage("Result", "One or more errors Occured.");
                    me.Focus();
                }
            }
#endif
            Closing += OnWindowClosing;
            Dispatcher.UnhandledException += DispatcherEvent;
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            // Events & Hide label
            SavedOrNotSavedLbl.Visibility = Visibility.Hidden;
            this.KeyUp += new KeyEventHandler(MainWindow_KeyUp);
            GSCText.KeyUp += new KeyEventHandler(MainWindow_KeyUp);

            FileCreate fileType = new FileCreate();
            fileType.addToFileType(".gsc");
            fileType.addToFileType(".txt");
            fileType.FileType.Text = ".gsc";
            
            // Default to BlackOps3 Directory 
            SetupButton(BlackOps3, BlackOps3Border);
            DisableLabel(BlackOps31);
            DisableLabel(BlackOps32);
            DisableLabel(BlackOps33);

            GAME = Games.GAMES.BLACKOPS3;

            // Minimize, Exit buttons
            foreach (Button button in TopBar.Children.OfType<Button>())
            {
                button.MouseEnter += OnMouseEnterButton;
                button.MouseLeave += OnMouseLeaveButton;
            }
            // Sidebar
            foreach (Button button in SideBar.Children.OfType<Button>())
            {
                button.MouseEnter += OnMouseEnterButton;
                button.MouseLeave += OnMouseLeaveButton;
            }
            // 
            foreach (Label label in SideBar.Children.OfType<Label>())
            {
                label.MouseEnter += OnMouseEnterLabel;
                label.MouseLeave += OnMouseLeaveLabel;
            }
            // Compile Area
            foreach (Button button in MWindow.Children.OfType<Button>())
            {
                button.MouseEnter += OnMouseEnterButton;
                button.MouseLeave += OnMouseLeaveButton;
            }
            SelectFolderBtn.MouseEnter += OnMouseEnterBlueButton;
            SelectFolderBtn.MouseLeave += OnMouseLeaveBlueButton;
            CreateFile.MouseEnter += OnMouseEnterBlueButton;
            CreateFile.MouseLeave += OnMouseLeaveBlueButton;
            RefreshList.MouseEnter += OnMouseEnterBlueButton;
            RefreshList.MouseLeave += OnMouseLeaveBlueButton;
            DeleteFile.MouseEnter += OnMouseEnterBlueButton;
            DeleteFile.MouseLeave += OnMouseLeaveBlueButton;

        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            var gsc = path + "\\scripts\\" + filename;

            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (!infile) return;
                this.Cursor = Cursors.Wait;
                Keyboard.ClearFocus();
                File.WriteAllText(gsc, GSCText.Text);
                Thread.Sleep(300);
                this.Cursor = Cursors.Arrow;

                if (SavedOrNotSavedLbl.Visibility == Visibility.Visible)
                {
                    SavedOrNotSavedLbl.Content = "Saved";
                }
            }

        }
        private void BarLClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void BlackOps3_Click(object sender, MouseButtonEventArgs e)
        {
            SetupButton(BlackOps3, BlackOps3Border);
            DisableLabel(BlackOps31);
            DisableLabel(BlackOps32);
            DisableLabel(BlackOps33);
            EnableLable(BlackOps41);
            EnableLable(BlackOps42);
            EnableLable(BlackOps43);
            EnableLable(BlackOps44);

            GAME = Games.GAMES.BLACKOPS3;
        }

        private void BlackOps4_Click(object sender, MouseButtonEventArgs e)
        {
            SetupButton(BlackOps4, BlackOps4Border);
            DisableLabel(BlackOps41);
            DisableLabel(BlackOps42);
            DisableLabel(BlackOps43);
            DisableLabel(BlackOps44);
            EnableLable(BlackOps31);
            EnableLable(BlackOps32);
            EnableLable(BlackOps33);

            GAME = Games.GAMES.BLACKOPS4;
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            MessageError messageError = new MessageError();
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select Folder";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = "C:\\";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {

                    Files.Children.Clear();

                    isfolderselected = true;
                    path = dialog.FileName;

                    MenuName.Content = path.Substring(path.LastIndexOf('\\') + 1);
                    string scripts = path + "\\scripts";
                    string[] gscfiles;
                    try
                    {
                        gscfiles = Directory.GetFiles(scripts);
                    }
                    catch
                    {
                        Directory.CreateDirectory(scripts);
                        gscfiles = Directory.GetFiles(scripts);
                    }
                    string[] directorys = Directory.GetDirectories(scripts);
                    if (gscfiles == null) return;

                    if (directorys.Length >= 1)
                    {
                        messageError.SendMessage("Error",
                            "Contents Must be moved to the \n" +
                            "scripts folder");
                    }

                    foreach (string gscfile in gscfiles)
                    {
                        var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                        if (filename.EndsWith(".gsc"))
                        {
                            Button filebutton = new Button();
                            Files.Children.Add(filebutton);
                            filebutton.Height = 23;
                            filebutton.Style = (System.Windows.Style)FindResource("FlatButton");
                            filebutton.Background = null;
                            filebutton.Cursor = Cursors.Hand;
                            filebutton.Content = filename;
                            filebutton.MouseEnter += OnMouseEnterButton;
                            filebutton.MouseLeave += OnMouseLeaveButton;
                            filebutton.Click += Filebutton_Click;
                        }
                    }
                }
            }
        }

        public void Filebutton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var gsc = path + "\\scripts\\" + btn.Content;

            var text = File.ReadAllText(gsc);
            var length = File.ReadLines(gsc).Count();

            LineList.Text = null;
            GSCText.Text = null;
            GSCText.Text = text;

            Helper.StaThreadWrapper(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    for (int i = 1; i < length; i++)
                    {
                        LineList.Text = LineList.Text + i.ToString() + "\n";
                    }
                }), DispatcherPriority.ContextIdle);
            });
            SelectedFile.Content = btn.Content;
            filename = btn.Content.ToString();
            infile = true;
        }

        private void CompileMenu(object sender, RoutedEventArgs e)
        {
            var gsc = path + "\\scripts\\" + filename;
            if (SavedOrNotSavedLbl.Visibility == Visibility.Visible)
            {
                SavedOrNotSavedLbl.Content = "Saved";
            }
            Helper.StaThreadWrapper(() =>
            {
                Compile compile = new Compile();
                try { File.WriteAllText(gsc, GSCText.Text); } catch { }
                Thread.Sleep(300);
                compile.CompileFinal(path, GAME, GAME_MODE, isfolderselected);
            });
        }

        private void CampaignClick(object sender, RoutedEventArgs e)
        {
            GAME_MODE = Games.GAMEMODE.CAMPAIGN;
        }

        private void Null_Mouse(object sender, MouseEventArgs e)
        {
            
        }

        private void MultiplayerClick(object sender, RoutedEventArgs e)
        {
            GAME_MODE = Games.GAMEMODE.MULTIPLAYER;
        }

        private void ZombiesClick(object sender, RoutedEventArgs e)
        {
            GAME_MODE = Games.GAMEMODE.ZOMBIES;
        }

        private void GSCMouse(object sender, MouseWheelEventArgs e)
        {

        }

        private void CheckSaved(object sender, KeyEventArgs e)
        {
            if (SavedOrNotSavedLbl.Visibility == Visibility.Hidden)
            {
                SavedOrNotSavedLbl.Visibility = Visibility.Visible;
                SavedOrNotSavedLbl.Content = "Changes Made";
            }

            if (SavedOrNotSavedLbl.Visibility == Visibility.Visible)
            {
                SavedOrNotSavedLbl.Visibility = Visibility.Visible;
                SavedOrNotSavedLbl.Content = "Changes Made";
            }
        }

        public void refreshList(bool force)
        {
            if (!force)
                if (!isfolderselected) return;

            Files.Children.Clear();
    
            string scripts = path + "\\scripts";
            string[] gscfiles = Directory.GetFiles(scripts);
            if (gscfiles == null) return;

            foreach (string gscfile in gscfiles)
            {
                var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                if (filename.EndsWith(".gsc") || filename.EndsWith(".txt"))
                {
                    Button filebutton = new Button();
                    Files.Children.Add(filebutton);
                    filebutton.Height = 23;
                    filebutton.Style = (System.Windows.Style)FindResource("FlatButton");
                    filebutton.Background = null;
                    filebutton.Cursor = Cursors.Hand;
                    filebutton.Content = filename;
                    filebutton.MouseEnter += OnMouseEnterButton;
                    filebutton.MouseLeave += OnMouseLeaveButton;
                    filebutton.Click += Filebutton_Click;
                }
            }
        }

        public void RefreshGSCList(object sender, RoutedEventArgs e)
        {
            refreshList(false);
        }

        private void DeleteExistingFile(object sender, RoutedEventArgs e)
        {
            if (!isfolderselected) return;
            if (!infile) return;

            var msg = MessageBox.Show("This will delete your current selected file! " + "(" + filename + ")" + "\nAre you sure you wish to continue?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msg != MessageBoxResult.Yes) return;

            var gsc = path + "\\scripts\\" + filename;
            File.Delete(gsc);

            RefreshGSCList(sender, null);
            GSCText.Text = null;
            LineList.Text = null;
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            if (!isfolderselected) return;
            FileCreate fileType = new FileCreate();
            fileType.PopUp(path);
        }

    }

}

