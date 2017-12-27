using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostAppInPanelLib.Utility.Win32;

namespace HostAppInPanelLib.Utility
{
    public static class WindowFinder
    {

        public static IEnumerable<IntPtr> FindWindowByTitle(this IEnumerable<IntPtr> handles, string title)
        {
            return handles.Where(windowHandle =>
            {
                var length = Win32Interop.GetWindowTextLength(windowHandle);
                var sb = new StringBuilder(length + 1);
                Win32Interop.GetWindowText(windowHandle, sb, sb.Capacity);
                if (sb.ToString() == title)
                    return true;
                return false;
            });
        }

    }
}
