﻿using System;
using System.Diagnostics;
using System.IO;
using HostAppInPanelLib.Utility.Win32;
using Microsoft.Win32;

namespace HostAppInPanelLib.Utility.Browser
{
    public class BrowserUtility
    {
        public static Process GetBrowserProcess(Func<IntPtr> getBrowserWindow, string processName)
        {
            Process processById;
            try
            {
                do
                {
                    uint processId;
                    IntPtr browserWindow;
                    do
                    {
                        browserWindow = getBrowserWindow.Invoke();
                        processId = 0;
                        if (browserWindow != IntPtr.Zero)
                        {
                            Win32Interop.GetWindowThreadProcessId(browserWindow, out processId);
                        }
                    } while (processId == 0);
                    Win32Interop.GetWindowThreadProcessId(browserWindow, out processId);
                    processById = Process.GetProcessById(unchecked((int) processId));
                } while (processById.HasExited);
                processById.EnableRaisingEvents = true;
            }
            catch
            {
                var processesByName = Process.GetProcessesByName(processName);
                foreach (var processbyname in processesByName)
                {
                    if (!processbyname.HasExited)
                    {
                        processbyname.Kill();
                    }
                }
                return null;
            }
            return processById;
        }

        public static void RunChromeHidden(out Process process)
        {
            process = new Process();
            string keyName;
            if (Environment.Is64BitOperatingSystem)
            {
                keyName =
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Google Chrome";
            }
            else
            {
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Google Chrome";
            }

            var valueName = "InstallLocation";
            var defaultValue = "";

            var resolveExeUsingPath = (string) Registry.GetValue(keyName, valueName, defaultValue);
            var fileName = Path.Combine(resolveExeUsingPath, "chrome.exe");
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = "about:blank --new-window --window-position=-1000,-1000 --window-size=1,1",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Minimized,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo = info;
            process.EnableRaisingEvents = true;
            process.Start();
        }
    }
}