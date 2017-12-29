using System.Windows;
using HostAppInPanelLib.Controls;
using OpenQA.Selenium;

namespace HostAppInPanelLib
{
    /// <summary>
    ///     Interaction logic for ChromeWrapperWindow.xaml
    /// </summary>
    public partial class ChromeWrapperWindow : Window
    {
        public ChromeWrapperWindow()
        {
            InitializeComponent();
            ChromeWrapperControl = new ChromeWrapperControl();
            ChromeWrapperControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            ChromeWrapperControl.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.Children.Add(ChromeWrapperControl);
        }

        public ChromeWrapperControl ChromeWrapperControl { get; }

        public IWebDriver WebDriver => ChromeWrapperControl.WebDriver;
    }
}