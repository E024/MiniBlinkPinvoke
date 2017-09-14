using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public struct wkeConsoleMessage
    {
        public wkeMessageSource source;
        public wkeMessageType type;
        public wkeMessageLevel level;
        public IntPtr message;
        public IntPtr url;
        public uint lineNumber;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkeNewViewInfo
    {
        public wkeNavigationType navigationType;
        public IntPtr url;
        public IntPtr target;
        public int x;
        public int y;
        public int width;
        public int height;
        [MarshalAs(UnmanagedType.I1)]
        public bool menuBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool statusBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool toolBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool locationBarVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool scrollbarsVisible;
        [MarshalAs(UnmanagedType.I1)]
        public bool resizable;
        [MarshalAs(UnmanagedType.I1)]
        public bool fullscreen;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct wkeDocumentReadyInfo
    {
        public IntPtr url;

        public IntPtr frameJSState;

        public IntPtr mainFrameJSState;
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
        public uint mask;
    }
}
