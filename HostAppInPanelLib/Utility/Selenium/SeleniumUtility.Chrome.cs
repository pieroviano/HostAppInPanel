﻿using System;
using System.Collections.Generic;
using System.Threading;
using HostAppInPanelLib.Utility.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{
    public partial class SeleniumUtility
    {
        public static Tuple<Thread, IWebDriver> GetChromeDriverHidden(out DriverService chromeDriverService, IntPtr containerHandle,
            Action<IntPtr> setChromeWindow)
        {
            chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArgument("about:blank");
            options.AddArgument("--new-window");
            options.AddArgument("--window-position=-1000,-1000");
            options.AddArgument("--window-size=1,1");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--start-minimized");

            var tuple = new Tuple<Func<IEnumerable<IntPtr>>, string>(ChromeWindowFinder.ChromeWindows,
                "data:, - Google Chrome");

            var findBrowserWindow = FindBrowserWindow(setChromeWindow, tuple);
            return  new Tuple<Thread, IWebDriver>(findBrowserWindow, new ChromeDriver((ChromeDriverService) chromeDriverService, options));
        }

    }
}