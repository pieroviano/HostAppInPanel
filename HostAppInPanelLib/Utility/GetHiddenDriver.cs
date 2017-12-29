using System;
using OpenQA.Selenium;

namespace HostAppInPanelLib.Utility
{
    public delegate IWebDriver GetHiddenDriver(out DriverService chromeDriverService, IntPtr containerHandle,
        Action<IntPtr> setChromeWindow);
}