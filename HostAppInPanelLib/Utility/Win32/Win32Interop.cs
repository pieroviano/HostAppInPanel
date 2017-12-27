using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace HostAppInPanelLib.Utility.Win32
{
    public static class Win32Interop
    {
        // An enumeration containing all the possible ShowInfo values.

        public const uint WmClose = 0x0010;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(WindowsByClassFinder.EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        ///     Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        ///     A pointer to the WINDOWPLACEMENT structure that receives the show state and position information.
        ///     <para>
        ///         Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails
        ///         if lpwndpl-> length is not set correctly.
        ///     </para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     <para>
        ///         If the function fails, the return value is zero. To get extended error information, call GetLastError.
        ///     </para>
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("User32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr windowHandle, StringBuilder stringBuilder, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hwnd);

        [DllImport("user32.Dll")]
        public static extern int PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        ///     Sets the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="lpwndpl">
        ///     A pointer to a WindowPlacement structure that specifies the new show state and window positions.
        ///     <para>
        ///         Before calling SetWindowPlacement, set the length member of the WindowPlacement structure to
        ///         sizeof(WindowPlacement). SetWindowPlacement fails if the length member is not set correctly.
        ///     </para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     <para>
        ///         If the function fails, the return value is zero. To get extended error information, call GetLastError.
        ///     </para>
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd,
            [In] ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowInfo nCmdShow);
    }
}