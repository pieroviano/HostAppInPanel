using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{
    namespace OpenQA.Selenium.Firefox
    {
        using OpenQA.Selenium;
        using System;
        using System.Globalization;
        using System.Text;

        public sealed class FirefoxDriverServiceEx : DriverService
        {

            internal static class PortUtilities
            {
                public static int FindFreePort()
                {
                    int port = 0;
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
                        socket.Bind(localEP);
                        localEP = (IPEndPoint)socket.LocalEndPoint;
                        port = localEP.Port;
                    }
                    finally
                    {
                        socket.Close();
                    }
                    return port;
                }
            }

            private string browserBinaryPath;
            private int browserCommunicationPort;
            private bool connectToRunningBrowser;
            private const string DefaultFirefoxDriverServiceFileName = "geckodriver";
            private static readonly Uri FirefoxDriverDownloadUrl = new Uri("https://github.com/mozilla/geckodriver/releases");
            private string host;
            private FirefoxDriverLogLevel loggingLevel;

            internal FirefoxDriverServiceEx(string executablePath, string executableFileName, int port) : base(executablePath, port, executableFileName, FirefoxDriverDownloadUrl)
            {
                this.browserCommunicationPort = -1;
                this.browserBinaryPath = string.Empty;
                this.host = string.Empty;
                this.loggingLevel = FirefoxDriverLogLevel.Default;
            }

            public static FirefoxDriverServiceEx CreateDefaultService() =>
                CreateDefaultService(DriverService.FindDriverServiceExecutable(FirefoxDriverServiceFileName(), FirefoxDriverDownloadUrl));

            public static FirefoxDriverServiceEx CreateDefaultService(string driverPath) =>
                CreateDefaultService(driverPath, FirefoxDriverServiceFileName());

            public static FirefoxDriverServiceEx CreateDefaultService(string driverPath, string driverExecutableFileName) =>
                new FirefoxDriverServiceEx(driverPath, driverExecutableFileName, PortUtilities.FindFreePort());

            private static string FirefoxDriverServiceFileName()
            {
                string str = "geckodriver";
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                    case PlatformID.WinCE:
                        return (str + ".exe");

                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        return str;
                }
                if (Environment.OSVersion.Platform != ((PlatformID)0x80))
                {
                    throw new WebDriverException("Unsupported platform: " + Environment.OSVersion.Platform);
                }
                return str;
            }

            public int BrowserCommunicationPort
            {
                get =>
                    this.browserCommunicationPort;
                set
                {
                    this.browserCommunicationPort = value;
                }
            }

            protected override string CommandLineArguments
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    if (this.connectToRunningBrowser)
                    {
                        builder.Append(" --connect-existing");
                    }
                    if (this.browserCommunicationPort > 0)
                    {
                        object[] args = new object[] { this.browserCommunicationPort };
                        builder.AppendFormat(CultureInfo.InvariantCulture, " --marionette-port {0}", args);
                    }
                    if (base.Port > 0)
                    {
                        object[] objArray2 = new object[] { base.Port };
                        builder.AppendFormat(CultureInfo.InvariantCulture, " --port {0}", objArray2);
                    }
                    if (!string.IsNullOrEmpty(this.browserBinaryPath))
                    {
                        object[] objArray3 = new object[] { this.browserBinaryPath };
                        builder.AppendFormat(CultureInfo.InvariantCulture, " --binary \"{0}\"", objArray3);
                    }
                    if (!string.IsNullOrEmpty(this.host))
                    {
                        object[] objArray4 = new object[] { this.host };
                        builder.AppendFormat(CultureInfo.InvariantCulture, " --host \"{0}\"", objArray4);
                    }
                    if (this.loggingLevel != FirefoxDriverLogLevel.Default)
                    {
                        object[] objArray5 = new object[] { this.loggingLevel.ToString().ToLowerInvariant() };
                        builder.Append(string.Format(CultureInfo.InvariantCulture, " --log {0}", objArray5));
                    }
                    return builder.ToString().Trim();
                }
            }

            public bool ConnectToRunningBrowser
            {
                get =>
                    this.connectToRunningBrowser;
                set
                {
                    this.connectToRunningBrowser = value;
                }
            }

            public string FirefoxBinaryPath
            {
                get =>
                    this.browserBinaryPath;
                set
                {
                    this.browserBinaryPath = value;
                }
            }

            protected override bool HasShutdown =>
                false;

            public string Host
            {
                get =>
                    this.host;
                set
                {
                    this.host = value;
                }
            }

            protected override TimeSpan InitializationTimeout =>
                TimeSpan.FromSeconds(2.0);

            protected override TimeSpan TerminationTimeout =>
                TimeSpan.FromMilliseconds(100.0);
        }
    }
}
