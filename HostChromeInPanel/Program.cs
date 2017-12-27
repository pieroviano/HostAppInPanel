using System;
using System.Windows;
using HostAppInPanelLib;
using HostAppInPanelLib.Utility;

namespace HostChromeInPanel
{
    public static class Program
    {
        private static WpfRunner _wpfRunner;

        [STAThread]
        public static void Main(string[] args)
        {
            _wpfRunner = new WpfRunner(typeof(ChromeWrapperWindow), Window_Loaded);
            _wpfRunner.RunWpfFromMain();
        }


        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((ChromeWrapperWindow) _wpfRunner.WrapperWindow).WebDriver.Url = "http://www.google.com";
        }
    }
}