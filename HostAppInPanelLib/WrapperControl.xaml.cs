using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Win32;
using WpfAdornedControl;
using WpfAdornedControl.WpfControls.Extensions;
using Panel = System.Windows.Forms.Panel;

namespace HostAppInPanelLib
{
    /// <summary>
    ///     Interaction logic for WrapperControl.xaml
    /// </summary>
    public partial class WrapperControl : UserControl
    {
        private readonly TimedAction _timedAction;
        private int _delay;

        public WrapperControl()
        {
            InitializeComponent();
            // Create the interop host control.
            FormsHost = new WindowsFormsHost();
            ContainerPanel = new Panel();


            _timedAction = new TimedAction();
        }

        public AdornedControl LoadingAdornerControl => LoadingAdorner;

        public Panel ContainerPanel { get; set; }
        public int WidthHeightIncrease { get; set; }
        public WindowsFormsHost FormsHost { get; }

        public Grid ContainerGrid => grid;

        public Grid ExternalGrid => externalGrid;

        public Thickness GridTickness
        {
            get => grid.Margin;
            set => grid.Margin = value;
        }

        public bool KillProcessOnClose { get; set; } = false;

        public Process Process { get; set; }

        public string Arguments { get; set; } = "";
        public string ProcessPath { get; set; } = "notepad.exe";

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Assign the MaskedTextBox control as the host control's child.
            FormsHost.Child = ContainerPanel;

            // Add the interop host control to the Grid
            // control's collection of child controls.
            grid.Children.Add(FormsHost);

            if (Process == null)
            {
                Process = new Process();
                var info = new ProcessStartInfo
                {
                    FileName = ProcessPath,
                    Arguments = Arguments,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };
                Process.StartInfo = new ProcessStartInfo();
                Process.StartInfo = info;
                Process.Start();
            }

            var processExitWaiter = new ProcessExitWaiter(Process);
            processExitWaiter.ProcessExited += ProcessOnExited;
            processExitWaiter.WaitProcessExit();

            if (Process != null)
            {
                try
                {
                    Process.WaitForInputIdle();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
                Thread.Sleep(500);

                Win32Interop.SetParent(Process.MainWindowHandle, ContainerPanel.Handle);
                WindowHelper.MakeExternalWindowBorderless(Process.MainWindowHandle, 0, 0, ContainerPanel.Width,
                    ContainerPanel.Height);
            }
        }

        public void ProcessExited()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                });
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        private void ProcessOnExited(object sender, EventArgs e)
        {
            ProcessExited();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow.SizeChanged += Window_SizeChanged;
            parentWindow.Closing += Window_Closing;
            parentWindow.Closed += Window_Closed;
            Window_Loaded(parentWindow, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!Process.HasExited)
            {
                Process.Kill();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (KillProcessOnClose)
            {
                Process.Kill();
            }
            if (Process.HasExited)
            {
                return;
            }
            if (KillProcessOnClose)
            {
                e.Cancel = true;
            }
            var hWnd = Process.MainWindowHandle;
            if (hWnd.ToInt32() != 0)
            {
                Win32Interop.PostMessage(hWnd, Win32Interop.WmClose, 0, 0);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingAdorner.StartStopWait(grid);

            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(2)};
            timer.Start();
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                LoadingAdorner.StartStopWait(grid);
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Width += 1;
                    parentWindow.Width -= 1;
                }
                _delay = 200;
            };
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Action action = delegate
            {
                if (Process != null && ContainerPanel != null)
                {
                    WindowHelper.ResizeExternalWindow(Process.MainWindowHandle, 0, 0,
                        ContainerPanel.Width + WidthHeightIncrease, ContainerPanel.Height + WidthHeightIncrease);
                }
            };
            if (_delay == 0)
            {
                action.Invoke();
            }
            else
            {
                _timedAction.ExecuteWithDelay(action, TimeSpan.FromMilliseconds(_delay));
            }
        }
    }
}