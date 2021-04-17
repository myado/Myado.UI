using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Myado.Helpers;
using PInvoke;
using static PInvoke.Gdi32;
using static PInvoke.User32;

namespace System.Windows.Forms
{
    public static class ScreenExtensions
    {
        public static IntPtr GetMonitorHandle(this Screen screen)
        {
            var point = new Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var pt = new POINT() { x = (int)point.X, y = (int)point.Y };
            var hmonitor = MonitorFromPoint(pt,MonitorOptions.MONITOR_DEFAULTTONEAREST);
            return hmonitor;
        }

        public static Rectangle GetBounds(this Screen screen)
        {
            var hmonitor = GetMonitorHandle(screen);
            MONITORINFO monitorInfo = new MONITORINFO();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            var b = GetMonitorInfo(hmonitor, ref monitorInfo);
            var rect = monitorInfo.rcMonitor;
            var rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            return rectangle;
        }

        public static double GetDpiFactor(this Screen screen)
        {
            //var className = screen.Primary ? "Shell_TrayWnd" : "Shell_SecondaryTrayWnd";
            //IntPtr taskbarHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, null);
            //var g = Graphics.FromHwnd(taskbarHandle);
            //var h = g.GetHdc();
            //SafeDCHandle sh = new SafeDCHandle(taskbarHandle, h);
            //int logpixelsy = Gdi32.GetDeviceCaps(sh, DeviceCap.LOGPIXELSY);
            var logpixelsy = GetDpi(screen);
            var dpiFactor = (float)logpixelsy / (float)96;

            //sh.Dispose();
            return dpiFactor;
        }
        public static double GetScreenFactor(this Screen screen)
        {
            //var className = screen.Primary ? "Shell_TrayWnd" : "Shell_SecondaryTrayWnd";
            //IntPtr taskbarHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, null);
            //var g = Graphics.FromHwnd(taskbarHandle);
            //var h = g.GetHdc();
            //SafeDCHandle sh = new SafeDCHandle(taskbarHandle, h);
            //int LogicalScreenHeight = Gdi32.GetDeviceCaps(sh, DeviceCap.VERTRES);
            //int PhysicalScreenHeight = Gdi32.GetDeviceCaps(sh, DeviceCap.DESKTOPVERTRES);
            //var screenFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            //sh.Dispose();
            GetScreenSize(screen, out int physicalScreenWidth, out int physicalScreenHeight, out int logicScreenWidth, out int logicScreenHeight);
            var screenFactor = (float)physicalScreenHeight / (float)logicScreenHeight;
            return screenFactor;
        }

        public static Rectangle GetDpiBounds(this Screen screen)
        {
            var rc = screen.GetBounds();
            double dpiFactor = GetDpiFactor(screen);
            var sb = new Rectangle((int)(rc.Left / dpiFactor), (int)(rc.Top / dpiFactor), (int)((rc.Width) / dpiFactor), (int)((rc.Height) / dpiFactor));
            return sb;
        }

        public static void GetScreenSize(this Screen screen,out int physicalScreenWidth,out int physicalScreenHeight,out int logicScreenWidth, out int logicScreenHeight)
        {
            //var hmonitor = GetHandle(screen);
            //var className = screen.Primary ? "Shell_TrayWnd" : "Shell_SecondaryTrayWnd";
            //IntPtr taskbarHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, null);
            //var g = Graphics.FromHwnd(taskbarHandle);
            //var h = g.GetHdc();
            var hdc = CreateDC(screen);
            physicalScreenHeight = GetDeviceCaps(hdc, (int)DeviceCap.DESKTOPVERTRES);//缩放后的像素
            physicalScreenWidth = GetDeviceCaps(hdc, (int)DeviceCap.DESKTOPHORZRES);

            logicScreenHeight = GetDeviceCaps(hdc, (int)DeviceCap.VERTRES); //物理像素
            logicScreenWidth = GetDeviceCaps(hdc, (int)DeviceCap.HORZRES);
            DeleteDC(hdc);
            //SafeDCHandle sh = new SafeDCHandle(taskbarHandle, h);
            //int PhysicalScreenHeight = GetDeviceCaps(sh, DeviceCap.DESKTOPVERTRES);
            //int PhysicalScreenWidth = GetDeviceCaps(sh, DeviceCap.DESKTOPHORZRES);
            //sh.Dispose();
        }

        public static Rectangle GetWorkArea(this Screen screen, out double screenFactor, out double dpiFactor,out int barEdge)
        {
            RECT barRect = new RECT();
            var bounds = screen.GetBounds();

            string[] tbClassNames = new string[] {"Shell_TrayWnd" , "Shell_SecondaryTrayWnd"};
            if(screen.Primary == false)
            {
                tbClassNames = new string[] {"Shell_SecondaryTrayWnd", "Shell_TrayWnd" };
            }
            IntPtr taskbarHandle;
            Rectangle logicBounds;
            Rectangle bRect;
            screenFactor = 1;
            dpiFactor = 1;
            barEdge = 3;
            foreach (var className in tbClassNames)
            {
                taskbarHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, null);
                GetWindowRect(taskbarHandle, out barRect);
                var g = Graphics.FromHwnd(taskbarHandle);
                var h = g.GetHdc();
                SafeDCHandle sh = new SafeDCHandle(taskbarHandle, h);
                int LogicalScreenHeight = Gdi32.GetDeviceCaps(sh, DeviceCap.VERTRES);
                int PhysicalScreenHeight = Gdi32.GetDeviceCaps(sh, DeviceCap.DESKTOPVERTRES);
                int logpixelsy = Gdi32.GetDeviceCaps(sh, DeviceCap.LOGPIXELSY);
                screenFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
                dpiFactor = (float)logpixelsy / (float)96;
                sh.Dispose();

                if (taskbarHandle == IntPtr.Zero)
                {
                    return bounds;
                }
                //}
                //else
                //{
                //    var data = new APPBARDATA();
                //    data.cbSize = Marshal.SizeOf(data);
                //    var ret = SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref data);
                //    barRect = data.rc;
                //}
                bRect = Rectangle.FromLTRB(barRect.left, barRect.top, barRect.right, barRect.bottom);
                var bRect2 = Rectangle.FromLTRB(barRect.left, barRect.top, barRect.right, barRect.bottom);
                bRect2.Inflate(-5, -5);
                var dx = 1;
                var dy = 1;
                logicBounds = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width * dx), (int)(bounds.Height * dy));
                //var logicBounds = bounds;
                if(logicBounds.Contains(bRect2) == false)
                {
                    continue;
                }
                barEdge = 3;
                var l = bounds.Left;
                var t = bounds.Top;
                var r = bounds.Right;
                var b = bounds.Bottom;
                if (Math.Abs(logicBounds.Right - bRect.Right) > 5)
                {
                    barEdge = 0;
                    l = (int)(bRect.Right / dx);
                }
                else if (Math.Abs(logicBounds.Bottom - bRect.Bottom) > 5)
                {
                    barEdge = 1;
                    t = (int)(bRect.Bottom / dy);
                }
                else if (Math.Abs(logicBounds.Left - bRect.Left) > 5)
                {
                    barEdge = 2;
                    r = (int)(bRect.Left / dx);
                }
                else if (Math.Abs(logicBounds.Top - bRect.Top) > 5)
                {
                    barEdge = 3;
                    b = (int)(bRect.Top / dy);
                }
                var clientBounds = Rectangle.FromLTRB(l, t, r, b);
                return clientBounds;
            }

            return bounds;
        }

        public static RECT GetTaskBarRect(this Screen screen)
        {
            RECT rect;
            IntPtr taskBarWnd = FindWindow("Shell_TrayWnd", null);
            IntPtr tray = FindWindowEx(taskBarWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr trayclock = FindWindowEx(tray, IntPtr.Zero, "TrayClockWClass", null);
            GetWindowRect(trayclock, out rect);
            return rect;
        }

        public static Monitor GetMonitor(this Screen screen)
        {
            var monitor = Monitor.AllMonitors.FirstOrDefault(el=>el.Name == screen.DeviceName);
            return monitor;
        }

        public static int GetDpi(this Screen screen)
        {
            var monitor = GetMonitor(screen);
            //var bounds = GetBounds(screen);
            //var pnt = new System.Drawing.Point(bounds.Left + 1, bounds.Top + 1);

            //var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            var mon = monitor.HMonitor;
            GetDpiForMonitor(mon, DpiType.Effective, out uint dpiX, out uint dpiY);
            return (int)dpiX;
        }

        public static void GetDpi(this Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
        {
            var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
        }

        public static IntPtr CreateDC(this Screen screen)
        {
            //var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            //var hMonitor = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            //screen.
            //GetMonitorInfo(hMonitor, out MONITORINFO lpmi);
            return CreateDC("DISPLAY", screen.DeviceName, null, IntPtr.Zero);
        }

    [DllImport("user32.dll")]
    static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
   EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

    [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(
           IntPtr hdc, // handle to DC
           int nIndex // index of capability
           );
        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern IntPtr CreateDC(
            string lpszDriver,//驱动名称
            string lpszDevice,//设备名称
            string lpszOutput,//无用，设为null
            IntPtr lpInitData//任意的打印机数据
            );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);
    }
    public enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
}