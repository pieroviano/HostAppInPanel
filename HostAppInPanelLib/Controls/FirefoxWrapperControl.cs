using System.Windows;
using HostAppInPanelLib.Utility;
using HostAppInPanelLib.Utility.Selenium;

namespace HostAppInPanelLib.Controls
{
    public class FirefoxWrapperControl : BrowserWrapperControl
    {
        public FirefoxWrapperControl() : base(SeleniumUtility.GetFirefoxDriverHidden, "firefox")
        {
        }

        protected override void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WidthHeightIncrease = 2;
            GridTickness = new Thickness(-8, -6, -6, -6);
            base.Window_Loaded(sender, e);
        }
    }
}