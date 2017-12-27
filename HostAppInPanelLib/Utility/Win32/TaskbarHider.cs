using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostAppInPanelLib.Utility.Win32
{
    public class TaskbarHider
    {
        private const int GwlExstyle = -0x14;
        private const int WsExToolwindow = 0x0080;

        public static void HideAppinTaskBar(IntPtr handle)
        {
            Win32Interop.ShowWindowAsync(handle, ShowInfo.Hide);
            WindowHelper.SetWindowLong(handle, GwlExstyle, WindowHelper.GetWindowLong(handle, GwlExstyle | WsExToolwindow));
            Win32Interop.ShowWindowAsync(handle, ShowInfo.Show);
        }
    }
}
