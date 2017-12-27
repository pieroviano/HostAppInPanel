using System;
using System.Collections.Generic;
using System.Text;
using HostAppInPanelLib.Utility.Win32;

namespace HostAppInPanelLib.Utility
{
    public class WindowsByClassFinder
    {
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        private readonly StringBuilder _apiResult = new StringBuilder(1024);

        private readonly string _className;
        private readonly List<IntPtr> _result = new List<IntPtr>();

        private WindowsByClassFinder(string className)
        {
            _className = className;
            Win32Interop.EnumWindows(callback, IntPtr.Zero);
        }

        private bool callback(IntPtr hWnd, IntPtr lparam)
        {
            if (Win32Interop.GetClassName(hWnd, _apiResult, _apiResult.Capacity) != 0)
            {
                if (string.CompareOrdinal(_apiResult.ToString(), _className) == 0)
                {
                    _result.Add(hWnd);
                }
            }

            return true; // Keep enumerating.
        }

        public static IEnumerable<IntPtr> WindowHandlesForClass(string className)
        {
            foreach (var windowHandle in WindowsMatchingClassName(className))
            {
                yield return windowHandle;
            }
        }

        /// <summary>Find the windows matching the specified class name.</summary>
        public static IEnumerable<IntPtr> WindowsMatching(string className)
        {
            return new WindowsByClassFinder(className)._result;
        }

        public static IEnumerable<IntPtr> WindowsMatchingClassName(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                throw new ArgumentOutOfRangeException("className", className, "className can't be null or blank.");
            }

            return WindowsMatching(className);
        }

        public static IEnumerable<string> WindowTitlesForClass(string className)
        {
            foreach (var windowHandle in WindowsMatchingClassName(className))
            {
                var length = Win32Interop.GetWindowTextLength(windowHandle);
                var sb = new StringBuilder(length + 1);
                Win32Interop.GetWindowText(windowHandle, sb, sb.Capacity);
                yield return sb.ToString();
            }
        }
    }
}