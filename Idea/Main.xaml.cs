using Idea.Actions;
using static Idea.Games.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Search;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.ComponentModel;
using DiscordRPC;
using Button = System.Windows.Controls.Button;

namespace Idea
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main : Window
    {
        GAMES GAME;
        GAMEMODE GAME_MODE;

        CompletionWindow completionWindow = null;
        public TextEditor[] edit = new TextEditor[225];

        public string path = string.Empty;

        public bool changesmade = false;
        bool folderopened = false;
        bool infile = false;
        bool firstfileclick = false;
        bool haveweadded = false;

        string selectedtabItem = string.Empty;
        string filename = string.Empty;

        string gamemode = string.Empty;
        string game = string.Empty; // deciding if i want to put this in the presense or not
        public DiscordRpcClient Client = new DiscordRpcClient("960318815760162876"); 

        #region Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        public Main()
        {
            // log unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (!Directory.Exists("c:\\t7compiler"))
                Compiler.InstallCompiler(@"https://gsc.dev/t7c_package");

            AllocConsole(); // we allocate a console to read the compilers output
            ShowWindow(GetConsoleWindow(), 0); // use 1 to show console

            InitializeComponent();

            // setup our syntax
            using (Stream s = typeof(Main).Assembly.GetManifestResourceStream("Idea.GSC.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    Editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }

            // Editor.Text = File.ReadAllText(Environment.CurrentDirectory + "\\guide.txt");

            // register our keybinds
            // save all keybind
            var ctrlShiftS = new RoutedCommand();
            ctrlShiftS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            var saveall = new CommandBinding(ctrlShiftS, SaveTextAllItems);
            this.CommandBindings.Add(saveall);

            // save changes to current file 
            var ctrlS = new RoutedCommand();
            ctrlS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            var savetext = new CommandBinding(ctrlS, SaveTextItem);
            this.CommandBindings.Add(savetext);

            // open folder 
            var ctrlO = new RoutedCommand();
            ctrlO.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            var openfol = new CommandBinding(ctrlO, OpenFolder);
            this.CommandBindings.Add(openfol);

            // create a new file 
            var ctrlN = new RoutedCommand();
            ctrlN.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            var createnewfile = new CommandBinding(ctrlN, CreateNewFile);
            this.CommandBindings.Add(createnewfile);

            // create a new project
            var ctrlShiftN = new RoutedCommand();
            ctrlShiftN.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control | ModifierKeys.Shift));
            var createnewFolder = new CommandBinding(ctrlShiftN, CreateNewFolder);
            this.CommandBindings.Add(createnewFolder);

            // port iL project
            var ctrlShiftAltO = new RoutedCommand();
            ctrlShiftAltO.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift));
            var portProject = new CommandBinding(ctrlShiftAltO, PortILProj);
            this.CommandBindings.Add(portProject);

            // compile
            var F9 = new RoutedCommand();
            F9.InputGestures.Add(new KeyGesture(Key.F9, ModifierKeys.None));
            var compile = new CommandBinding(F9, CompileMenu);
            this.CommandBindings.Add(compile);

            var ctrlR = new RoutedCommand();
            ctrlR.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            var refresh = new CommandBinding(ctrlR, RefList);
            this.CommandBindings.Add(refresh);

            // setup openeditor watcher
            DispatcherTimer tabTimer = new DispatcherTimer();
            tabTimer.Interval = TimeSpan.FromMilliseconds(100);
            tabTimer.Tick += delegate { UpdateSelectedTabItem(); };
            tabTimer.Start();

            // autosave 
            DispatcherTimer saveTimer = new DispatcherTimer();
            saveTimer.Interval = TimeSpan.FromMilliseconds(200);
            saveTimer.Tick += delegate { forcesave(); };
            saveTimer.Start();

            // update presense
            Client.Initialize();
            DispatcherTimer updatePresense = new DispatcherTimer();
            updatePresense.Interval = TimeSpan.FromMilliseconds(200);
            updatePresense.Tick += delegate { updatepresense(); };
            updatePresense.Start();
            // default to black ops 3 zombies

            CampaignHeader.IsChecked = false;
            MultiplayerHeader.IsChecked = false;
            ZombiesHeader.IsChecked = true;
            GAME_MODE = GAMEMODE.ZOMBIES;
            gamemode = "Zm";

            _BlackOps4Header.IsChecked = false;
            _BlackOps3Header.IsChecked = true;
            GAME = GAMES.BLACKOPS3;

            // Create Default Project 

            var scripts = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Compiler UI\\Projects\\Default Project (Black Ops 3)\\scripts";

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Compiler UI\\Projects\\Default Project (Black Ops 3)");
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Compiler UI\\Projects\\Default Project (Black Ops 3)\\scripts");
            if (!File.Exists(scripts + "\\main.gsc"))
                File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt7project.main", scripts + "\\main.gsc");
            if (!File.Exists(scripts + "\\headers.gsc"))
                File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt7project.headers", scripts + "\\headers.gsc");

            // loadup default project
            OpenFolder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Compiler UI\\Projects\\Default Project (Black Ops 3)");
            folderopened = true;
            infile = true;

            // set default size & position
            this.Width = 1280;
            this.Height = 720;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception
            File.WriteAllText(Environment.CurrentDirectory + "\\log.txt", (e.ExceptionObject as Exception).Message);

            if ((e.ExceptionObject as Exception).Message.Contains("is not a valid value for property"))
            {
                MessageBox.Show("You cannot have any special characters in your script names. (ie: !@#$%^&*()-+=[]{}\\|:;,.\"\'', or numbers.");
                ProcessStartInfo proc = new ProcessStartInfo(Environment.CurrentDirectory + "Idea.exe");
                proc.UseShellExecute = false;

                Process.Start(proc);
                System.Threading.Thread.Sleep(250);
                Environment.Exit(0);
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (changesmade)
            {
                var r = MessageBox.Show("Would you like to save your changes?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (r == MessageBoxResult.Yes)
                {
                    SaveTextAllItems(null, null);
                }
                changesmade = false;
            }

            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            try { debugcompiler[0].Kill(); } catch { }

            Process[] injector = Process.GetProcessesByName("idea");
            try { injector[0].Kill(); } catch { }

            Client.Dispose(); // safely disconnect from discord
            Environment.Exit(0);
        }

        public void CloseWind(Window window) => OnWindowClosing(null, null);
        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            if (changesmade)
            {
                var r = MessageBox.Show("Would you like to save your changes?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (r == MessageBoxResult.Yes)
                {
                    SaveTextAllItems(null, null);
                }
                changesmade = false;
            }
            Process[] debugcompiler = Process.GetProcessesByName("debugcompiler");
            try { debugcompiler[0].Kill(); } catch { }

            Process[] injector = Process.GetProcessesByName("Idea");
            try { injector[0].Kill(); } catch { }

            Client.Dispose(); // safely disconnect from discord
            CloseWind(GetWindow((FrameworkElement)e.Source));
        }

        private void SetGame(object sender, RoutedEventArgs e)
        {
            var head = sender as MenuItem;

            switch (head.Header)
            {
                default:
                    _BlackOps3Header.IsChecked = false;
                    _BlackOps4Header.IsChecked = false;
                    break;

                case "Black Ops 3":
                    GAME = GAMES.BLACKOPS3;
                    _BlackOps4Header.IsChecked = false;
                    break;
                case "Black Ops 4":
                    GAME = GAMES.BLACKOPS4;
                    _BlackOps3Header.IsChecked = false;
                    break;
            }
            return;
        }

        private void SetGameMode(object sender, RoutedEventArgs e)
        {
            var head = sender as MenuItem;

            switch (head.Header)
            {
                default:
                    break;

                case "Campaign":
                    GAME_MODE = GAMEMODE.CAMPAIGN;
                    MultiplayerHeader.IsChecked = false;
                    ZombiesHeader.IsChecked = false;
                    gamemode = "Sp";
                    break;
                case "Multiplayer":
                    GAME_MODE = GAMEMODE.MULTIPLAYER;
                    CampaignHeader.IsChecked = false;
                    ZombiesHeader.IsChecked = false;
                    gamemode = "Mp";
                    break;
                case "Zombies":
                    GAME_MODE = GAMEMODE.ZOMBIES;
                    CampaignHeader.IsChecked = false;
                    MultiplayerHeader.IsChecked = false;
                    gamemode = "Zm";
                    break;
            }
        }

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            changesmade = true;
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }

        private void MoveContentsToScripts(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists == false)
                Directory.CreateDirectory(path);

            List<String> Scripts = Directory
                               .GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();

            foreach (string file in Scripts)
            {
                FileInfo mFile = new FileInfo(file);
                // to remove name collisions
                if (new FileInfo(dirInfo + "\\" + mFile.Name).Exists == false)
                {
                    mFile.MoveTo(dirInfo + "\\" + mFile.Name);
                }
            }
        }
        private void Filebutton_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            int index = s.TabIndex;
            OpenEditors.SelectedIndex = index;
        }

        private void Xbtn_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            var m = MessageBox.Show($"Warning: This will delete the file ({s.Name}) Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (m != MessageBoxResult.Yes)
                return;

            if (File.Exists(path + $"\\scripts\\{s.Name}.gsc"))
                File.Delete(path + $"\\scripts\\{s.Name}.gsc");

            if (File.Exists(path + $"\\scripts\\{s.Name}.txt"))
                File.Delete(path + $"\\scripts\\{s.Name}.txt");


            refreshList(false);
            if (s.TabIndex > 0)
                OpenEditors.SelectedIndex = s.TabIndex - 1;
            else
                OpenEditors.SelectedIndex = 0;
        }

        public void refreshList(bool force)
        {
            bool haveweinstalled = false;
            int i = 0;
            if (!force)
                if (!folderopened) return;

            if (haveweadded)
                OpenEditors.Items.Clear();

            Scripts.Children.Clear();

            MoveContentsToScripts(path + "\\scripts");
            string scripts = path + "\\scripts";
            string[] gscfiles = Directory.GetFiles(scripts);
            if (gscfiles == null) return;

            foreach (string gscfile in gscfiles)
            {
                var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                if (filename.EndsWith(".gsc") || filename.EndsWith(".txt"))
                {
                    var removeafter = ".gsc";
                    var sorted = filename.Substring(filename.IndexOf(removeafter) + removeafter.Length).Replace(' ', '_');

                    Grid gridD = new Grid();
                    Button filebutton = new Button();
                    Button xbtn = new Button();
                    Scripts.Children.Add(gridD);
                    gridD.Children.Add(filebutton);
                    gridD.Children.Add(xbtn);

                    filebutton.Height = 23;
                    filebutton.Background = null;
                    filebutton.Cursor = Cursors.Hand;
                    filebutton.Content = filename.Replace(' ', '_');
                    filebutton.Click += Filebutton_Click;
                    filebutton.TabIndex = i;
                    xbtn.HorizontalAlignment = HorizontalAlignment.Right;
                    xbtn.Content = "X";
                    xbtn.Click += Xbtn_Click;
                    xbtn.MouseEnter += Xbtn_MouseEnter;
                    xbtn.MouseLeave += Xbtn_MouseLeave;
                    xbtn.Width = 20;
                    xbtn.TabIndex = i;
                    xbtn.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');

                    TabItem newTabItem = new TabItem
                    {
                        Header = filename.Replace(' ', '_'),
                        Name = sorted.Replace(".txt", ""),
                    };
                    TextEditor textedit = new TextEditor();
                    textedit.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                    textedit.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    textedit.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    textedit.ShowLineNumbers = true;
                    textedit.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                    textedit.Foreground = new SolidColorBrush(Color.FromRgb(225, 225, 225));


                    textedit.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');


                    if (!haveweinstalled)
                    {
                        var ctrlS = new RoutedCommand();
                        ctrlS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                        var savetext = new CommandBinding(ctrlS, SaveTextItem);

                        var ctrlShiftS = new RoutedCommand();
                        ctrlShiftS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
                        var saveall = new CommandBinding(ctrlShiftS, SaveTextAllItems);

                        this.CommandBindings.Add(savetext);
                        this.CommandBindings.Add(saveall);
                        haveweinstalled = true;
                    }

                    var t = SearchPanel.Install(textedit);
                    t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    textedit.Load(gscfile);
                    edit[i] = textedit;
                    using (Stream s = typeof(Main).Assembly.GetManifestResourceStream("Idea.GSC.xshd"))
                    {
                        if (s == null)
                            throw new InvalidOperationException("Could not find embedded resource");
                        using (XmlReader reader = new XmlTextReader(s))
                        {
                            textedit.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                        }
                    }

                    Grid grid = new Grid();
                    grid.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                    grid.Children.Add(textedit);

                    newTabItem.Content = grid;
                    OpenEditors.Items.Add(newTabItem);

                    i++;
                }
            }
        }

        private void OpenFolder(string __path)
        {
            if (!firstfileclick)
            {
                firstfileclick = true;
            }

            if (firstfileclick)
            {
                refreshList(false);
            }

            try
            {
                if (haveweadded)
                    OpenEditors.Items.Clear();
                Scripts.Children.Clear();
            }
            catch { }

            folderopened = true;
            path = __path;

            OpenEditors.Visibility = Visibility.Visible;
            Editor.Visibility = Visibility.Hidden;

            MenuOpened.Content = (string)path.Substring(path.LastIndexOf('\\') + 1);
            Compiler.menu = (string)path.Substring(path.LastIndexOf('\\') + 1);
            MoveContentsToScripts(path + "\\scripts");
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

            bool haveweinstalled = false;
            int i = 0;

            foreach (string gscfile in gscfiles)
            {
                var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                try
                {
                    if (filename.EndsWith(".gsc") || filename.EndsWith(".txt"))
                    {
                        var removeafter = ".gsc";
                        var sorted = filename.Substring(filename.IndexOf(removeafter) + removeafter.Length).Replace(' ', '_');

                        Grid gridD = new Grid();
                        Button filebutton = new Button();
                        Button xbtn = new Button();
                        Scripts.Children.Add(gridD);
                        gridD.Children.Add(filebutton);
                        gridD.Children.Add(xbtn);

                        filebutton.Height = 23;
                        filebutton.Background = null;
                        filebutton.Cursor = Cursors.Hand;
                        filebutton.Content = filename.Replace(' ', '_');
                        filebutton.Click += Filebutton_Click;
                        filebutton.TabIndex = i;
                        xbtn.HorizontalAlignment = HorizontalAlignment.Right;
                        xbtn.Content = "X";
                        xbtn.Click += Xbtn_Click;
                        xbtn.MouseEnter += Xbtn_MouseEnter;
                        xbtn.MouseLeave += Xbtn_MouseLeave;
                        xbtn.Width = 20;
                        xbtn.TabIndex = i;
                        xbtn.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');

                        TabItem newTabItem = new TabItem
                        {
                            Header = filename.Replace(' ', '_'),
                            Name = sorted.Replace(".txt", ""),
                        };
                        TextEditor textedit = new TextEditor();
                        textedit.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                        textedit.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        textedit.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                        textedit.ShowLineNumbers = true;
                        textedit.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                        textedit.Foreground = new SolidColorBrush(Color.FromRgb(225, 225, 225));


                        textedit.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');

                        if (!haveweinstalled)
                        {
                            var ctrlS = new RoutedCommand();
                            ctrlS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                            var savetext = new CommandBinding(ctrlS, SaveTextItem);

                            var ctrlShiftS = new RoutedCommand();
                            ctrlShiftS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
                            var saveall = new CommandBinding(ctrlShiftS, SaveTextAllItems);

                            this.CommandBindings.Add(savetext);
                            this.CommandBindings.Add(saveall);
                            haveweinstalled = true;
                        }

                        var t = SearchPanel.Install(textedit);
                        t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        textedit.Load(gscfile);
                        edit[i] = textedit;
                        using (Stream s = typeof(Main).Assembly.GetManifestResourceStream("Idea.GSC.xshd"))
                        {
                            if (s == null)
                                throw new InvalidOperationException("Could not find embedded resource");
                            using (XmlReader reader = new XmlTextReader(s))
                            {
                                textedit.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                            }
                        }

                        Grid grid = new Grid();
                        grid.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                        grid.Children.Add(textedit);

                        newTabItem.Content = grid;

                        OpenEditors.Items.Add(newTabItem);
                        i++;
                    }
                }
                catch { }
            }

            filename = selectedtabItem;
            infile = true;
            haveweadded = true;
        }

        private void Xbtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Colors.Red);
        }
        private void Xbtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            if (!firstfileclick)
            {
                firstfileclick = true;
            }

            if (firstfileclick)
            {
                refreshList(false);
            }

            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = "Select Folder";
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = "C:\\";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    try
                    {
                        if (haveweadded)
                            OpenEditors.Items.Clear();
                        Scripts.Children.Clear();
                    }
                    catch { }

                    folderopened = true;
                    path = dialog.FileName;

                    OpenEditors.Visibility = Visibility.Visible;
                    Editor.Visibility = Visibility.Hidden;
                    MenuOpened.Content = (string)path.Substring(path.LastIndexOf('\\') + 1);
                    Compiler.menu = (string)path.Substring(path.LastIndexOf('\\') + 1);
                    MoveContentsToScripts(path + "\\scripts");
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

                    bool haveweinstalled = false;
                    int i = 0;
                    foreach (string gscfile in gscfiles)
                    {
                        var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                        if (filename.EndsWith(".gsc") || filename.EndsWith(".txt"))
                        {
                            var removeafter = ".gsc";
                            var sorted = filename.Substring(filename.IndexOf(removeafter) + removeafter.Length).Replace(' ', '_');

                            Grid gridD = new Grid();
                            Button filebutton = new Button();
                            Button xbtn = new Button();
                            Scripts.Children.Add(gridD);
                            gridD.Children.Add(filebutton);
                            gridD.Children.Add(xbtn);

                            filebutton.Height = 23;
                            filebutton.Background = null;
                            filebutton.Cursor = Cursors.Hand;
                            filebutton.Content = filename.Replace(' ', '_');
                            filebutton.Click += Filebutton_Click;
                            filebutton.TabIndex = i;
                            xbtn.HorizontalAlignment = HorizontalAlignment.Right;
                            xbtn.Content = "X";
                            xbtn.Click += Xbtn_Click;
                            xbtn.MouseEnter += Xbtn_MouseEnter;
                            xbtn.MouseLeave += Xbtn_MouseLeave;
                            xbtn.Width = 20;
                            xbtn.TabIndex = i;
                            xbtn.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');

                            TabItem newTabItem = new TabItem
                            {
                                Header = filename.Replace(' ', '_'),
                                Name = sorted.Replace(".txt", ""),
                            };
                            TextEditor textedit = new TextEditor();
                            textedit.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                            textedit.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            textedit.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                            textedit.ShowLineNumbers = true;
                            textedit.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                            textedit.Foreground = new SolidColorBrush(Color.FromRgb(225, 225, 225));


                            textedit.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');


                            if (!haveweinstalled)
                            {
                                var ctrlS = new RoutedCommand();
                                ctrlS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                                var savetext = new CommandBinding(ctrlS, SaveTextItem);

                                var ctrlShiftS = new RoutedCommand();
                                ctrlShiftS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
                                var saveall = new CommandBinding(ctrlShiftS, SaveTextAllItems);

                                this.CommandBindings.Add(savetext);
                                this.CommandBindings.Add(saveall);
                                haveweinstalled = true;
                            }

                            var t = SearchPanel.Install(textedit);

                            textedit.Load(gscfile);
                            edit[i] = textedit;
                            using (Stream s = typeof(Main).Assembly.GetManifestResourceStream("Idea.GSC.xshd"))
                            {
                                if (s == null)
                                    throw new InvalidOperationException("Could not find embedded resource");
                                using (XmlReader reader = new XmlTextReader(s))
                                {
                                    textedit.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                                }
                            }

                            Grid grid = new Grid();
                            grid.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                            grid.Children.Add(textedit);

                            newTabItem.Content = grid;

                            OpenEditors.Items.Add(newTabItem);
                            i++;
                        }
                    }

                    filename = selectedtabItem;
                    infile = true;
                    haveweadded = true;
                }
            }
        }

        private void InjectPrecompiledScript(object sender, RoutedEventArgs e)
        {
            var head = sender as MenuItem;

            string _game = head.Header.ToString();
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = $"{_game} - Precompiled Script";
                dialog.IsFolderPicker = false;
                dialog.Multiselect = false;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (!dialog.FileName.EndsWith(".gsc"))
                    {
                        if (!dialog.FileName.EndsWith(".gscc"))
                        {
                            MessageBox.Show("Select a compiled script to inject.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            InjectPrecompiledScript(sender, e);
                            return;
                        }
                    }
                }
                else return;
                GAMES _GAME = GAMES.UNKNOWN;
                switch (_game)
                {
                    default:
                        _GAME = GAMES.UNKNOWN;
                        break;

                    case "Black Ops 3":
                        _GAME = GAMES.BLACKOPS3;
                        break;

                    case "Black Ops 4":
                        _GAME = GAMES.BLACKOPS4;
                        break;
                }

                Compiler.InjectScript(dialog.FileName, _GAME);
            }
        }

        private void AboutOpen(object sender, RoutedEventArgs e)
        {
            About.ShowAboutMe(null, null);
        }

        void UpdateSelectedTabItem()
        {
            if (!infile) return; if (!folderopened) return;
            try
            {
                // string manipulation
                var content = "Header:";
                var unsorteditem = OpenEditors.SelectedItem.ToString();
                var sorted = unsorteditem.Substring(unsorteditem.IndexOf(content) + content.Length);

                var removeafter = ".gsc";
                var sorted1 = sorted.Substring(sorted.IndexOf(removeafter) + removeafter.Length);

                var final = sorted.Replace(sorted1, null);
                selectedtabItem = final;
                filename = final;
            }
            catch
            {
                // pass
            }
        }

        private void forcesave()
        {
            if (!folderopened) return;
            try
            {
                string[] text = new string[edit.Length];
                for (int i = 0; i < edit.Length; i++)
                {
                    if (edit[i] == null)
                        continue;

                    text[i] = edit[i].Name;
                    if (text[i] == edit[i].Name)
                    {
                        var saved = $"{path}\\scripts\\{text[i]}.gsc";
                        if (File.Exists(saved))
                            edit[i].Save(saved);
                    }
                }
                changesmade = false;
            }
            catch
            {

            }
        }
        private void updatepresense()
        {
            RichPresence presence = new RichPresence()
            {
                Details = $"{(string)MenuOpened.Content} | {(string)gamemode}",
                State = $"Editing {selectedtabItem}",
                Assets = new Assets()
                {
                    LargeImageKey = "file",
                    LargeImageText = (string)MenuOpened.Content,
                    SmallImageKey = "file",
                    SmallImageText = (string)gamemode,
                }
            };
            Client.SetPresence(presence);
        }
        private void SaveTextItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (!infile) return;
            try
            {
                for (int i = 0; i < edit.Length; i++)
                {

                    if (edit[i].Name + ".gsc" != selectedtabItem)
                    {
                        continue;
                    }

                    edit[i].Save(path + $"\\scripts\\{selectedtabItem}");
                }
                changesmade = false;
            }
            catch
            {

            }
        }

        private void SaveTextAllItems(object sender, ExecutedRoutedEventArgs e)
        {
            if (!folderopened) return;
            try
            {
                string[] text = new string[edit.Length];
                for (int i = 0; i < edit.Length; i++)
                {
                    if (edit[i] == null)
                        continue;
                    text[i] = edit[i].Name;
                    if (text[i] == edit[i].Name)
                    {
                        var saved = $"{path}\\scripts\\{text[i]}.gsc";
                        if (File.Exists(saved))
                            edit[i].Save(saved);
                    }
                }
                changesmade = false;
            }
            catch
            {

            }
        }

        private void CompileMenu(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            CompileBTN.Cursor = Cursors.Wait;
            SaveTextAllItems(null, null);
            Compiler.CompileScript(path, GAME, GAME_MODE, folderopened);
            CompileBTN.Cursor = Cursors.Arrow;
            this.Cursor = Cursors.Arrow;
        }
        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            NewFile.PATH = path;
            NewFile np = new NewFile();
            np.ShowDialog();
            refreshList(false);
        }

        private void CreateNewFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Title = $"Select A Folder";
                dialog.IsFolderPicker = true;
                dialog.Multiselect = false;
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Compiler UI\\Projects\\";

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {

                    if (Directory.GetFiles(dialog.FileName).Length > 0)
                    {
                        MessageBox.Show("Expected an Empty Folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    switch (GAME)
                    {
                        case GAMES.BLACKOPS3:
                            Directory.CreateDirectory(dialog.FileName);
                            Directory.CreateDirectory(dialog.FileName + "\\scripts");
                            File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt7project.main", dialog.FileName + "\\scripts\\main.gsc");
                            File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt7project.headers", dialog.FileName + "\\scripts\\headers.gsc");
                            break;
                        case GAMES.BLACKOPS4:
                            Directory.CreateDirectory(dialog.FileName);
                            Directory.CreateDirectory(dialog.FileName + "\\scripts");
                            File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt8project.main", dialog.FileName + "\\scripts\\main.gsc");
                            File.Copy(Environment.CurrentDirectory + "\\Defaults\\defaultt8project.headers", dialog.FileName + "\\scripts\\headers.gsc");
                            break;
                    }

                    try
                    {
                        if (haveweadded)
                        {
                            SaveTextAllItems(null, null);
                            OpenEditors.Items.Clear();
                        }
                        Scripts.Children.Clear();
                    }
                    catch { }

                    path = dialog.FileName;

                    OpenEditors.Visibility = Visibility.Visible;
                    Editor.Visibility = Visibility.Hidden;
                    MenuOpened.Content = (string)path.Substring(path.LastIndexOf('\\') + 1);
                    Compiler.menu = (string)path.Substring(path.LastIndexOf('\\') + 1);
                    MoveContentsToScripts(path + "\\scripts");
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

                    bool haveweinstalled = false;
                    int i = 0;
                    foreach (string gscfile in gscfiles)
                    {
                        var filename = gscfile.Substring(gscfile.LastIndexOf('\\') + 1);
                        if (filename.EndsWith(".gsc") || filename.EndsWith(".txt"))
                        {
                            var removeafter = ".gsc";
                            var sorted = filename.Substring(filename.IndexOf(removeafter) + removeafter.Length).Replace(' ', '_');

                            Grid gridD = new Grid();
                            Button filebutton = new Button();
                            Button xbtn = new Button();
                            Scripts.Children.Add(gridD);
                            gridD.Children.Add(filebutton);
                            gridD.Children.Add(xbtn);

                            filebutton.Height = 23;
                            filebutton.Background = null;
                            filebutton.Cursor = Cursors.Hand;
                            filebutton.Content = filename.Replace(' ', '_');
                            filebutton.Click += Filebutton_Click;
                            filebutton.TabIndex = i;
                            xbtn.HorizontalAlignment = HorizontalAlignment.Right;
                            xbtn.Content = "X";
                            xbtn.Click += Xbtn_Click;
                            xbtn.MouseEnter += Xbtn_MouseEnter;
                            xbtn.MouseLeave += Xbtn_MouseLeave;
                            xbtn.Width = 20;
                            xbtn.TabIndex = i;
                            xbtn.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');

                            TabItem newTabItem = new TabItem
                            {
                                Header = filename.Replace(' ', '_'),
                                Name = sorted.Replace(".txt", ""),
                            };
                            TextEditor textedit = new TextEditor();
                            textedit.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                            textedit.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            textedit.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                            textedit.ShowLineNumbers = true;
                            textedit.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                            textedit.Foreground = new SolidColorBrush(Color.FromRgb(225, 225, 225));


                            textedit.Name = filename.Replace(".gsc", null).Replace(".txt", null).Replace(".txt", null).Replace(' ', '_');


                            if (!haveweinstalled)
                            {
                                var ctrlS = new RoutedCommand();
                                ctrlS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
                                var savetext = new CommandBinding(ctrlS, SaveTextItem);

                                var ctrlShiftS = new RoutedCommand();
                                ctrlShiftS.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
                                var saveall = new CommandBinding(ctrlShiftS, SaveTextAllItems);

                                this.CommandBindings.Add(savetext);
                                this.CommandBindings.Add(saveall);
                                haveweinstalled = true;
                            }

                            var t = SearchPanel.Install(textedit);
                            t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            textedit.Load(gscfile);
                            edit[i] = textedit;
                            using (Stream s = typeof(Main).Assembly.GetManifestResourceStream("Idea.GSC.xshd"))
                            {
                                if (s == null)
                                    throw new InvalidOperationException("Could not find embedded resource");
                                using (XmlReader reader = new XmlTextReader(s))
                                {
                                    textedit.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                                }
                            }

                            Grid grid = new Grid();
                            grid.Background = new SolidColorBrush(Color.FromRgb(16, 16, 16));
                            grid.Children.Add(textedit);

                            newTabItem.Content = grid;

                            OpenEditors.Items.Add(newTabItem);
                            i++;
                        }
                        filename = selectedtabItem;
                        infile = true;
                        haveweadded = true;
                        folderopened = true;
                    }
                }
            }
        }

        private void KillGame(object sender, RoutedEventArgs e)
        {
            var head = sender as MenuItem;

            switch (head.Header)
            {
                default:
                    break;

                case "Kill Black Ops 3":
                    ProcessEx bo3 = "blackops3";

                    if (bo3 == null)
                        return;

                    bo3.BaseProcess.Kill();
                    break;

                case "Kill Black Ops 4":
                    ProcessEx bo4 = "blackops4";

                    if (bo4 == null)
                        return;

                    bo4.BaseProcess.Kill();
                    break;
            }
        }

        private void PortILProj(object sender, RoutedEventArgs e)
        {
            Porter port = new Porter();
            port.ShowPortWindow();
        }

        private void SaveTextItem(object sender, RoutedEventArgs e)
        {
            SaveTextItem(null, null);
        }

        private void SaveTextAllItems(object sender, RoutedEventArgs e)
        {
            SaveTextAllItems(null, null);
        }

        private void RefList(object sender, RoutedEventArgs e)
        {
            refreshList(false);
        }

        private void UpdateComp(object sender, RoutedEventArgs e)
        {
            
            if (Directory.Exists("c:\\t7compiler"))
                Directory.Delete("c:\\t7compiler", true);

            Compiler.InstallCompiler(@"https://gsc.dev/t7c_package");
        }
    }
}
