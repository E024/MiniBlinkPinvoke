using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniBlinkPinvoke
{

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeRect
    {
        public int x;
        public int y;
        public int w;
        public int h;
        public Rectangle ToRectangle()
        {
            return new Rectangle(this.x, this.y, this.w, this.h);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct wkeJSData
    {

        public IntPtr userdata;

        public wkeJSGetPropertyCallback propertyGet;

        public wkeJSSetPropertyCallback propertySet;

        public wkeJSFinalizeCallback finalize;

        public wkeJSCallAsFunctionCallback callAsFunction;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkeProxy
    {
        public wkeProxyType type;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string hostname;
        public ushort port;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string username;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string password;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkeSettings
    {
        public wkeProxy proxy;
        public wkeSettingMask mask;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkePostBodyElement
    {
        public int size;
        public wkeHttBodyElementType type;
        /// <summary>
        /// 转 wkeMemBuf
        /// </summary>
        public IntPtr data;
        /// <summary>
        /// wkeString
        /// </summary>
        public IntPtr filePath;
        public Int64 fileStart;
        public Int64 fileLength;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkeMemBuf
    {
        public int size;
        public IntPtr data;
        public Int32 length;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkePostBodyElements
    {
        public int size;
        /// <summary>
        /// wkePostBodyElement**
        /// </summary>
        public IntPtr element;
        public int elementSize;
        [MarshalAs(UnmanagedType.I1)]
        public bool isDirty;
    }


}
