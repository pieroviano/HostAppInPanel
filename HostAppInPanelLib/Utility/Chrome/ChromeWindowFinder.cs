using System;
using System.Collections.Generic;

namespace HostAppInPanelLib.Utility.Chrome
{
    public static class ChromeWindowFinder
    {
        public static IEnumerable<string> ChromeWindowTitles()
        {
            foreach (var title in WindowsByClassFinder.WindowTitlesForClass("Chrome_WidgetWin_0"))
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    yield return title;
                }
            }

            foreach (var title in WindowsByClassFinder.WindowTitlesForClass("Chrome_WidgetWin_1"))
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    yield return title;
                }
            }
        }

        public static IEnumerable<IntPtr> ChromeWindows()
        {
            foreach (var title in WindowsByClassFinder.WindowHandlesForClass("Chrome_WidgetWin_0"))
            {
                if (title != IntPtr.Zero)
                {
                    yield return title;
                }
            }

            foreach (var title in WindowsByClassFinder.WindowHandlesForClass("Chrome_WidgetWin_1"))
            {
                if (title!=IntPtr.Zero)
                {
                    yield return title;
                }
            }
        }

    }
}
