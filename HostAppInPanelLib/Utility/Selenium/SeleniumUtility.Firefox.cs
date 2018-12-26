using System;
using System.Diagnostics;
using System.Threading;
using CreateHiddenProcessLib.CreateWindowUtility;
using CreateHiddenProcessLib.CreateWindowUtility.Model;
using HostAppInPanelLib.Utility.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{
    public class ReferenceContainer<T>
    {
        public T Value { get; set; }
    }

    public partial class SeleniumUtility
    {
        public static Tuple<Thread, IWebDriver> GetFirefoxDriverHidden(out DriverService firefoxDriverService,
            IntPtr containerHandle,
            Action<IntPtr> setFirefoxWindow)
        {
            WindowCreationHookerByClassName windowCreationHookerByClassName = WindowCreationHookerByClassName.GetInstance("MozillaWindowClass");

            void InstanceFirefowWindowCreated(object sender, WindowHookEventArgs e)
            {
                Win32Interop.SetParent(e.Handle, containerHandle);
                setFirefoxWindow(e.Handle);
                windowCreationHookerByClassName.Dispose();
            }

            var process = new ByRef<Process>();
            windowCreationHookerByClassName
                .HookFirefoxCreation(process, InstanceFirefowWindowCreated);

            var referenceContainer = new ReferenceContainer<Tuple<Thread, IWebDriver>>();
            firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;
            var options = new FirefoxOptions();
            options.AddArgument("--url about:blank");

            //var tuple = new Tuple<Func<IEnumerable<IntPtr>>, string>(FirefoxWindowFinder.FirefoxWindows,
            //    "Mozilla Firefox");

            var firefoxDriverHidden = new FirefoxDriver((FirefoxDriverService) firefoxDriverService, options,
                TimeSpan.FromSeconds(30));

            var capabilities = firefoxDriverHidden.Capabilities;
            var processId = Convert.ToInt32(capabilities.GetCapability("moz:processID"));
            if (processId != 0)
            {
                var byId = Process.GetProcessById(processId);
                process.Value = byId;
            }
            //if(mainWindowHandle == null)
            //    findBrowserWindow = FindBrowserWindow(setFirefoxWindow, tuple);

            var driverHidden = new Tuple<Thread, IWebDriver>(windowCreationHookerByClassName.Thread, firefoxDriverHidden);
            referenceContainer.Value = driverHidden;
            return referenceContainer.Value;
        }
    }
}