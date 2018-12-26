using System;
using System.Threading;
using OpenQA.Selenium;

namespace HostAppInPanelLib.Utility
{
    public delegate Tuple<Thread, IWebDriver> GetHiddenDriver(out DriverService chromeDriverService, IntPtr containerHandle,
        Action<IntPtr> setChromeWindow);
}