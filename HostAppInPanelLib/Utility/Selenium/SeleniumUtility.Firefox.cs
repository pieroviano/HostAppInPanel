using System;
using System.Collections.Generic;
using HostAppInPanelLib.Utility.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{
    public partial class SeleniumUtility
    {

        public static IWebDriver GetFirefoxDriverHidden(out DriverService firefoxDriverService, IntPtr containerHandle,
            Action<IntPtr> setChromeWindow)
        {
            firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;
            var options = new FirefoxOptions();
            options.AddArgument("--tray");
            options.AddArgument("--url about:blank");

            var tuple = new Tuple<Func<IEnumerable<IntPtr>>, string>(FirefoxWindowFinder.FirefoxWindows,
                "Mozilla Firefox");

            FindBrowserWindow(setChromeWindow, tuple);
            return new FirefoxDriver((FirefoxDriverService) firefoxDriverService, options, TimeSpan.FromSeconds(30));
        }
    }
}