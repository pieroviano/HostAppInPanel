using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using HostAppInPanelLib.Utility.Selenium.OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Firefox;

namespace HostAppInPanelLib.Utility.Selenium
{

        public class FirefoxDriverEx : RemoteWebDriver
        {
            public static readonly bool AcceptUntrustedCertificates = true;
            public static readonly bool AssumeUntrustedCertificateIssuer = true;
            public static readonly string BinaryCapabilityName = "firefox_binary";
            public static readonly bool DefaultEnableNativeEvents = Platform.CurrentPlatform.IsPlatformType(PlatformType.Windows);
            public static readonly int DefaultPort = 0x1b8f;
            public static readonly string ProfileCapabilityName = "firefox_profile";

            public FirefoxDriverEx() : this(new FirefoxDriverServiceEx(null, null))
            {
            }

            public FirefoxDriverEx(FirefoxDriverServiceEx service) : this(service, new FirefoxDriverServiceEx(), RemoteWebDriver.DefaultCommandTimeout)
            {
            }

            public FirefoxDriverEx(FirefoxDriverServiceEx options) : this(CreateService(options), options, RemoteWebDriver.DefaultCommandTimeout)
            {
            }

            [Obsolete("FirefoxDriver should not be constructed with a FirefoxProfile object. Use FirefoxDriverServiceEx instead. This constructor will be removed in a future release.")]
            public FirefoxDriverEx(FirefoxProfile profile) : this(new FirefoxDriverServiceEx(profile, null))
            {
            }

            [Obsolete("FirefoxDriver should not be constructed with a raw ICapabilities or DesiredCapabilities object. Use FirefoxDriverServiceEx instead. This constructor will be removed in a future release.")]
            public FirefoxDriverEx(ICapabilities capabilities) : this(CreateOptionsFromCapabilities(capabilities))
            {
            }

            public FirefoxDriverEx(string geckoDriverDirectory) : this(geckoDriverDirectory, new FirefoxDriverServiceEx())
            {
            }

            [Obsolete("FirefoxDriver should not be constructed with a FirefoxBinary object. Use FirefoxDriverServiceEx instead. This constructor will be removed in a future release.")]
            public FirefoxDriverEx(FirefoxBinary binary, FirefoxProfile profile) : this(new FirefoxDriverServiceEx(profile, binary))
            {
            }

            public FirefoxDriverEx(string geckoDriverDirectory, FirefoxDriverServiceEx options) : this(geckoDriverDirectory, options, RemoteWebDriver.DefaultCommandTimeout)
            {
            }

            [Obsolete("FirefoxDriver should not be constructed  with a FirefoxBinary object. Use FirefoxDriverServiceEx instead. This constructor will be removed in a future release.")]
            public FirefoxDriverEx(FirefoxBinary binary, FirefoxProfile profile, TimeSpan commandTimeout) : this((FirefoxDriverServiceEx)null, new FirefoxDriverServiceEx(profile, binary), commandTimeout)
            {
            }

            public FirefoxDriverEx(FirefoxDriverServiceEx service, FirefoxDriverServiceEx options, TimeSpan commandTimeout) : base(CreateExecutor(service, options, commandTimeout), ConvertOptionsToCapabilities(options))
            {
            }

            public FirefoxDriverEx(string geckoDriverDirectory, FirefoxDriverServiceEx options, TimeSpan commandTimeout) : this(FirefoxDriverServiceEx.CreateDefaultService(geckoDriverDirectory), options, commandTimeout)
            {
            }

            private static ICapabilities ConvertOptionsToCapabilities(FirefoxDriverServiceEx options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException("options", "options must not be null");
                }
                ICapabilities capabilities = options.ToCapabilities();
                if (options.UseLegacyImplementation)
                {
                    capabilities = RemoveUnneededCapabilities(capabilities);
                }
                return capabilities;
            }

            protected override RemoteWebElement CreateElement(string elementId) =>
                new FirefoxWebElement(this, elementId);

            private static ICommandExecutor CreateExecutor(FirefoxDriverServiceEx service, FirefoxDriverServiceEx options, TimeSpan commandTimeout)
            {
                if (options.UseLegacyImplementation)
                {
                    FirefoxProfile profile = options.Profile;
                    if (profile == null)
                    {
                        profile = new FirefoxProfile();
                    }
                    return CreateExtensionConnection(new FirefoxBinary(options.BrowserExecutableLocation), profile, commandTimeout);
                }
                if (service == null)
                {
                    throw new ArgumentNullException("service", "You requested a service-based implementation, but passed in a null service object.");
                }
                return new DriverServiceCommandExecutor(service, commandTimeout);
            }

            private static ICommandExecutor CreateExtensionConnection(FirefoxBinary binary, FirefoxProfile profile, TimeSpan commandTimeout)
            {
                FirefoxProfile profile2 = profile;
                string environmentVariable = Environment.GetEnvironmentVariable("webdriver.firefox.profile");
                if ((profile2 == null) && (environmentVariable != null))
                {
                    profile2 = new FirefoxProfileManager().GetProfile(environmentVariable);
                }
                else if (profile2 == null)
                {
                    profile2 = new FirefoxProfile();
                }
                return new FirefoxDriverCommandExecutor(binary, profile2, "localhost", commandTimeout);
            }

            private static FirefoxDriverServiceEx CreateOptionsFromCapabilities(ICapabilities capabilities)
            {
                FirefoxBinary binary = ExtractBinary(capabilities);
                DesiredCapabilities capabilities2 = RemoveUnneededCapabilities(capabilities) as DesiredCapabilities;
                FirefoxDriverServiceEx options = new FirefoxDriverServiceEx(ExtractProfile(capabilities), binary);
                if (capabilities2 != null)
                {
                    foreach (KeyValuePair<string, object> pair in capabilities2.ToDictionary())
                    {
                        options.AddAdditionalCapability(pair.Key, pair.Value);
                    }
                }
                return options;
            }

            private static FirefoxDriverServiceEx CreateService(FirefoxDriverServiceEx options)
            {
                if ((options != null) && options.UseLegacyImplementation)
                {
                    return null;
                }
                return FirefoxDriverServiceEx.CreateDefaultService();
            }

            private static FirefoxBinary ExtractBinary(ICapabilities capabilities)
            {
                if (capabilities.GetCapability(BinaryCapabilityName) != null)
                {
                    return new FirefoxBinary(capabilities.GetCapability(BinaryCapabilityName).ToString());
                }
                return new FirefoxBinary();
            }

            private static FirefoxProfile ExtractProfile(ICapabilities capabilities)
            {
                FirefoxProfile profile = new FirefoxProfile();
                if (capabilities.GetCapability(ProfileCapabilityName) != null)
                {
                    object capability = capabilities.GetCapability(ProfileCapabilityName);
                    FirefoxProfile profile2 = capability as FirefoxProfile;
                    string str = capability as string;
                    if (profile2 != null)
                    {
                        profile = profile2;
                    }
                    else if (str != null)
                    {
                        try
                        {
                            profile = FirefoxProfile.FromBase64String(str);
                        }
                        catch (IOException exception)
                        {
                            throw new WebDriverException("Unable to create profile from specified string", exception);
                        }
                    }
                }
                if (capabilities.GetCapability(CapabilityType.Proxy) != null)
                {
                    Proxy proxy = null;
                    object obj2 = capabilities.GetCapability(CapabilityType.Proxy);
                    Proxy proxy2 = obj2 as Proxy;
                    Dictionary<string, object> settings = obj2 as Dictionary<string, object>;
                    if (proxy2 != null)
                    {
                        proxy = proxy2;
                    }
                    else if (settings != null)
                    {
                        proxy = new Proxy(settings);
                    }
                    profile.SetProxyPreferences(proxy);
                }
                if (capabilities.GetCapability(CapabilityType.AcceptSslCertificates) != null)
                {
                    bool flag = (bool)capabilities.GetCapability(CapabilityType.AcceptSslCertificates);
                    profile.AcceptUntrustedCertificates = flag;
                }
                return profile;
            }

            protected virtual void PrepareEnvironment()
            {
            }

            private static ICapabilities RemoveUnneededCapabilities(ICapabilities capabilities)
            {
                DesiredCapabilities capabilities1 = capabilities as DesiredCapabilities;
                capabilities1.CapabilitiesDictionary.Remove(ProfileCapabilityName);
                capabilities1.CapabilitiesDictionary.Remove(BinaryCapabilityName);
                return capabilities1;
            }

            public override IFileDetector FileDetector
            {
                get =>
                    base.FileDetector;
                set
                {
                }
            }

            public bool IsMarionette =>
                base.IsSpecificationCompliant;
        }
    }
}
