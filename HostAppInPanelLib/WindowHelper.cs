using System;
using System.Runtime.InteropServices;

namespace HostAppInPanelLib
{
    public static class WindowHelper
    {
        public static IntPtr WsBorder = (IntPtr) 8388608;
        public static IntPtr WsDlgFrame = (IntPtr) 4194304;
        public static IntPtr WsCaption = (IntPtr) ((long) WsBorder | (long) WsDlgFrame);
        public static IntPtr WsSysMenu = (IntPtr) 524288;
        public static IntPtr WsThickFrame = (IntPtr) 262144;
        public static IntPtr WsMinimize = (IntPtr) 536870912;
        public static IntPtr WsMaximizeBox = (IntPtr) 65536;
        public static int GwlStyle = -16;
        public static int GwlExStyle = -20;
        public static IntPtr WsExDlgModalFrame = (IntPtr) 0x1;
        public static IntPtr SwpNoMove = (IntPtr) 0x2;
        public static IntPtr SwpNoSize = (IntPtr) 0x1;
        public static IntPtr SwpFrameChanged = (IntPtr) 0x20;
        public static UIntPtr MfByPosition = (UIntPtr) 0x400;
        public static UIntPtr MfRemove = (UIntPtr) 0x1000;

        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr 
        public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLongPtr64(hWnd, nIndex);
            }
            return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static void MakeExternalWindowBorderless(IntPtr mainWindowHandle, int x, int y, int width, int height)
        {
            var style = GetWindowLong(mainWindowHandle, GwlStyle);
            style = (IntPtr)((long)style & ~(long)WsCaption);
            style = (IntPtr)((long)style & ~(long)WsSysMenu);
            style = (IntPtr)((long)style & ~(long)WsThickFrame);
            style = (IntPtr)((long)style & ~(long)WsBorder);
            style = (IntPtr)((long)style & ~(long)WsMinimize);
            style = (IntPtr)((long)style & ~(long)WsMaximizeBox);
            SetWindowLong(mainWindowHandle, GwlStyle, style);
            style = GetWindowLong(mainWindowHandle, GwlExStyle);
            SetWindowLong(mainWindowHandle, GwlExStyle, (IntPtr)((long)style | (long)WsExDlgModalFrame));
            ResizeExternalWindow(mainWindowHandle, x, y, width, height);
        }

        public static void ResizeExternalWindow(IntPtr mainWindowHandle, int x, int y, int width, int htight)
        {
            SetWindowPos(mainWindowHandle, new IntPtr(0), x, y, width, htight, 0);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        // This static method is required because legacy OSes do not support
        // SetWindowLongPtr 
        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            int uFlags);
    }
}