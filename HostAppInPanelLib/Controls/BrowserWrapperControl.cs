using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Browser;
using OpenQA.Selenium;
using HostAppInPanelLib.Utility.Selenium;
using OpenQA.Selenium.Remote;

namespace HostAppInPanelLib.Controls
{
    public class BrowserWrapperControl : WrapperControl
    {
        protected readonly DriverService DriverService;

        public BrowserWrapperControl(GetHiddenDriver getHiddenDriver, string processName)
        {
            var window = IntPtr.Zero;

            void SetWindow(IntPtr value)
            {
                window = value;
            }

            IntPtr GetWindow()
            {
                return window;
            }

            if (!ContainerPanel.IsHandleCreated)
            {
                ContainerPanel.CreateControl();
            }
            var invoke = getHiddenDriver?.Invoke(out DriverService, ContainerPanel.Handle,
                SetWindow);
            WebDriver = invoke?.Item2;
            //var windows = WebDriver?.WindowHandles.Select(e => long.Parse(e)).ToArray();
            Thread = invoke?.Item1;
            var processById = BrowserUtility.GetBrowserProcess(GetWindow, processName);
            Process = processById;
            KillProcessOnClose = false;
            Loaded += Window_Loaded;
        }

        public IWebDriver WebDriver { get; }

        public Thread Thread { get; }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            SeleniumUtility.KillSeleniumProcesses(DriverService.ProcessId);
        }

        protected virtual void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit += CurrentOnExit;
        }
    }
}