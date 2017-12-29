using System;
using System.Collections.Generic;

namespace HostAppInPanelLib.Utility.Windows
{
    public static class FirefoxWindowFinder
    {
        public static IEnumerable<IntPtr> FirefoxWindows()
        {
            foreach (var title in WindowsByClassFinder.WindowHandlesForClass("MozillaWindowClass"))
            {
                if (title != IntPtr.Zero)
                {
                    yield return title;
                }
            }
        }

        public static IEnumerable<string> FirefoxWindowTitles()
        {
            foreach (var title in WindowsByClassFinder.WindowTitlesForClass("MozillaWindowClass"))
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    yield return title;
                }
            }
        }
    }
}