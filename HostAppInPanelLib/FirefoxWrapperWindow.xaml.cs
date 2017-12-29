using System.Windows;
using HostAppInPanelLib.Controls;
using OpenQA.Selenium;

namespace HostAppInPanelLib
{
    /// <summary>
    ///     Interaction logic for ChromeWrapperWindow.xaml
    /// </summary>
    public partial class FirefoxWrapperWindow : Window
    {
        public FirefoxWrapperWindow()
        {
            InitializeComponent();
            FirefoxWrapperControl = new FirefoxWrapperControl();
            FirefoxWrapperControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            FirefoxWrapperControl.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.Children.Add(FirefoxWrapperControl);
        }

        public FirefoxWrapperControl FirefoxWrapperControl { get; }

        public IWebDriver WebDriver => FirefoxWrapperControl.WebDriver;
    }
}