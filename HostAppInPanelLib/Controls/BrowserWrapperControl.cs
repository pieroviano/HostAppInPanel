using System;
using System.Windows;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Browser;
using OpenQA.Selenium;
using HostAppInPanelLib.Utility.Selenium;

namespace HostAppInPanelLib.Controls
{
    public class BrowserWrapperControl : WrapperControl
    {
        protected readonly DriverService _driverService;

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
            WebDriver = getHiddenDriver?.Invoke(out _driverService, ContainerPanel.Handle,
                SetWindow);
            var processById = BrowserUtility.GetBrowserProcess(GetWindow, processName);
            Process = processById;
            KillProcessOnClose = false;
            Loaded += Window_Loaded;
        }

        public IWebDriver WebDriver { get; }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            SeleniumUtility.KillSeleniumProcesses(_driverService.ProcessId);
        }

        protected virtual void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit += CurrentOnExit;
        }
    }
}