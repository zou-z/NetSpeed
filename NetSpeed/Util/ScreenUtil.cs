using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetSpeed.Util
{
    /// <summary>
    /// 修改dpi后需要重启文件管理器，否则无法或者正确的dpi值
    /// </summary>
    internal static class ScreenUtil
    {
        public static int[] GetDpi()
        {
            int[] dpi = new int[2];
            IntPtr hdc = GetDC(IntPtr.Zero);
            dpi[0] = GetDeviceCaps(hdc, LOGPIXELSX);
            dpi[1] = GetDeviceCaps(hdc, LOGPIXELSY);
            ReleaseDC(IntPtr.Zero, hdc);
            return dpi;
        }

        public static int GetDpiX()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
            ReleaseDC(IntPtr.Zero, hdc);
            return dpiX;
        }

        public static int GetDpiY()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int dpiY = GetDeviceCaps(hdc, LOGPIXELSY);
            ReleaseDC(IntPtr.Zero, hdc);
            return dpiY;
        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;
    }
}
