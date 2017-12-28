using System;
using System.Windows;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace HostAppInPanelLib
{
    public class ChromeWrapperControl : WrapperControl
    {
        private readonly ChromeDriverService _chromeDriverService;

        public ChromeWrapperControl()
        {
            var chromeWindow = IntPtr.Zero;

            void SetChromeWindow(IntPtr value)
            {
                chromeWindow = value;
            }

            IntPtr GetChromeWindow()
            {
                return chromeWindow;
            }

            if (!ContainerPanel.IsHandleCreated)
            {
                ContainerPanel.CreateControl();
            }
            WebDriver = SeleniumUtility.GetChromeDriverHidden(out _chromeDriverService, ContainerPanel.Handle,
                SetChromeWindow);
            var processById = ChromeUtility.GetChromeProcess(GetChromeWindow);
            Process = processById;
            KillProcessOnClose = false;
            Loaded += ChromeWrapperWindow_Loaded;
        }

        public IWebDriver WebDriver { get; }

        private void ChromeWrapperWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WidthHeightIncrease = 2;
            GridTickness = new Thickness(-8, -6, -6, -6);
            Application.Current.Exit += CurrentOnExit;
        }

        private void CurrentOnExit(object sender, ExitEventArgs exitEventArgs)
        {
            SeleniumUtility.KillSeleniumProcesses(_chromeDriverService);
        }
    }
}