using System;
using System.Windows;
using HostAppInPanelLib;
using OpenQA.Selenium;

namespace HostAppInPanel
{
    public static class Program
    {
        private static ChromeWrapperWindow _wrapperWindow;

        private static void Application_Startup(object sender, StartupEventArgs e)
        {
            _wrapperWindow = new ChromeWrapperWindow();
            _wrapperWindow.Loaded += Window_Loaded;
            _wrapperWindow.Show();
        }

        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            application.Startup += Application_Startup;
            application.Run();
        }


        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _wrapperWindow.WebDriver.Url = "http://www.google.com";
        }
    }
}