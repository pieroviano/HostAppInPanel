using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{
    public class FirefoxOptionsEx : FirefoxOptions
    {
        public FirefoxOptionsEx()
        {

        }

        public FirefoxOptionsEx(FirefoxProfile profile, FirefoxBinary binary)
        {
            KnownCapabilityNames.Clear();
            if (profile != null)
            {
                this.Profile = profile;
            }
            if (binary != null)
            {
                var executable = typeof(FirefoxBinary).GetField("executable", BindingFlags.NonPublic).GetValue(binary);
                var s = executable.GetType().GetProperty("ExecutablePath").GetValue(executable) as string;
                this.BrowserExecutableLocation = s;
            }
        }

        private Dictionary<string, string> KnownCapabilityNames => (Dictionary<string, string>)typeof(FirefoxOptions).BaseType.GetField("knownCapabilityNames", BindingFlags.NonPublic).GetValue(this);
    }
}
