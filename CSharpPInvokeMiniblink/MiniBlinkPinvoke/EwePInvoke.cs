using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace MiniBlinkPinvoke
{
    public delegate bool wkeJSSetPropertyCallback(IntPtr es, long @object, [MarshalAs(UnmanagedType.LPWStr)] [In] string propertyName, long value);
    public delegate long wkeJSGetPropertyCallback(IntPtr es, long @object, [MarshalAs(UnmanagedType.LPWStr)] [In] string propertyName);
    public delegate void wkeJSFinalizeCallback(ref wkeJSData data);
    public delegate long wkeJSCallAsFunctionCallback(IntPtr es, long @object, ref long args, int argCount);
    public delegate void AlertBoxCallback(IntPtr webView, IntPtr msg);
    public delegate bool ConfirmBoxCallback(IntPtr webView, IntPtr msg);
    public delegate bool PromptBoxCallback(IntPtr webView, IntPtr msg, IntPtr defaultResult, IntPtr result);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void wkeDocumentReadyCallback(IntPtr webView, ref wkeDocumentReadyInfo info);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ReadFileCallback(IntPtr _caller, [MarshalAs(UnmanagedType.LPStr)]string szFile, SetDataCallback setData);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetDataCallback(IntPtr _caller, IntPtr data, uint length);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool BeforeSendCallback(IntPtr url, IntPtr method, IntPtr data, long dataSize);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void EweCallBack(IntPtr param0);

    public struct wkeDocumentReadyInfo
    {
        public IntPtr url;

        public IntPtr frameJSState;

        public IntPtr mainFrameJSState;
    }
    public struct wkeJSData
    {

        public IntPtr userdata;

        public wkeJSGetPropertyCallback propertyGet;

        public wkeJSSetPropertyCallback propertySet;

        public wkeJSFinalizeCallback finalize;

        public wkeJSCallAsFunctionCallback callAsFunction;
    }
    public enum wkeLoadingResult
    {
        WKE_LOADING_SUCCEEDED,
        WKE_LOADING_FAILED,
        WKE_LOADING_CANCELED
    }

    public class LoadingFinishEventArgs : EventArgs
    {
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;
        [CompilerGenerated]
        private wkeLoadingResult wkeLoadingResult_0;

        public string FailedReason
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        public wkeLoadingResult LoadingResult
        {
            [CompilerGenerated]
            get
            {
                return this.wkeLoadingResult_0;
            }
            [CompilerGenerated]
            set
            {
                this.wkeLoadingResult_0 = value;
            }
        }

        public string Url
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }
    }

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
    public struct wkeConsoleMessage
    {
        public wkeMessageSource source;

        public wkeMessageType type;

        public wkeMessageLevel level;

        public IntPtr message;

        public IntPtr url;

        public uint lineNumber;
    }
    public enum wkeMessageSource
    {
        WKE_MESSAGE_SOURCE_HTML,
        WKE_MESSAGE_SOURCE_XML,
        WKE_MESSAGE_SOURCE_JS,
        WKE_MESSAGE_SOURCE_NETWORK,
        WKE_MESSAGE_SOURCE_CONSOLE_API,
        WKE_MESSAGE_SOURCE_OTHER
    }
    public enum wkeMessageType
    {
        WKE_MESSAGE_TYPE_LOG,
        WKE_MESSAGE_TYPE_DIR,
        WKE_MESSAGE_TYPE_DIR_XML,
        WKE_MESSAGE_TYPE_TRACE,
        WKE_MESSAGE_TYPE_START_GROUP,
        WKE_MESSAGE_TYPE_START_GROUP_COLLAPSED,
        WKE_MESSAGE_TYPE_END_GROUP,
        WKE_MESSAGE_TYPE_ASSERT
    }
    public enum wkeMessageLevel
    {
        WKE_MESSAGE_LEVEL_TIP,
        WKE_MESSAGE_LEVEL_LOG,
        WKE_MESSAGE_LEVEL_WARNING,
        WKE_MESSAGE_LEVEL_ERROR,
        WKE_MESSAGE_LEVEL_DEBUG
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void wkeConsoleMessageCallback(IntPtr webView, ref wkeConsoleMessage message);
    public delegate wkeNavigationAction wkeNavigationCallback(IntPtr webView, wkeNavigationType navigationType, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr url, wkeLoadingResult result, IntPtr failedReason);

    /// <summary>
    /// 下载回调
    /// </summary>
    /// <param name="webView"></param>
    /// <param name="url"></param>
    /// <param name="result"></param>
    /// <param name="failedReason"></param>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool wkeDownloadFileCallback(IntPtr webView, IntPtr url, IntPtr mimeType);

    public delegate void TitleChangedCallback(IntPtr webView, IntPtr title);
    public enum wkeNavigationAction
    {
        WKE_NAVIGATION_CONTINUE,
        WKE_NAVIGATION_ABORT,
        WKE_NAVIGATION_DOWNLOAD
    }
    public enum wkeNavigationType
    {
        WKE_NAVIGATION_TYPE_LINKCLICK,
        WKE_NAVIGATION_TYPE_FORMSUBMITTE,
        WKE_NAVIGATION_TYPE_BACKFORWARD,
        WKE_NAVIGATION_TYPE_RELOAD,
        WKE_NAVIGATION_TYPE_FORMRESUBMITT,
        WKE_NAVIGATION_TYPE_OTHER
    }

    public enum WkeCursorInfo
    {
        WkeCursorInfoPointer = 0,
        WkeCursorInfoCross = 1,
        WkeCursorInfoHand = 2,
        WkeCursorInfoIBeam = 3,
        WkeCursorInfoWait = 4,
        WkeCursorInfoHelp = 5,
        WkeCursorInfoEastResize = 6,
        WkeCursorInfoNorthResize = 7,
        WkeCursorInfoNorthEastResize = 8,
        WkeCursorInfoNorthWestResize = 9,
        WkeCursorInfoSouthResize = 10,
        WkeCursorInfoSouthEastResize = 11,
        WkeCursorInfoSouthWestResize = 12,
        WkeCursorInfoWestResize = 13,
        WkeCursorInfoNorthSouthResize = 14,
        WkeCursorInfoEastWestResize = 15,
        WkeCursorInfoNorthEastSouthWestResize = 16,
        WkeCursorInfoNorthWestSouthEastResize = 17,
        WkeCursorInfoColumnResize = 18,
        WkeCursorInfoRowResize = 19,
    }

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

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, ref wkeNewViewInfo info);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnDidDownloadCallback([MarshalAs(UnmanagedType.LPWStr)] [In] string url, IntPtr data, uint size);
    /// <summary>
    /// 如需要更完善的功能，请根据Pinvoke封装规则把ewe.h里的API进行封装
    /// </summary>
    public class EwePInvoke
    {
        const string Ewedll = "node.dll";
        private static ReadFileCallback readFileCallback_0;
        [CompilerGenerated]
        private static OnDidDownloadCallback didDownloadCallback_2;
        private static OnDidDownloadCallback didDownloadCallback_1;
        private static OnDidDownloadCallback didDownloadCallback_0;


        static EwePInvoke()
        {
            if (System.IO.File.Exists("ewe.dll"))
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    wkeDisableWOFF(false);
                }
                readFileCallback_0 = new ReadFileCallback(smethod_0);
                wkeOnReadFile(readFileCallback_0);
                if (didDownloadCallback_2 == null)
                {
                    didDownloadCallback_2 = new OnDidDownloadCallback(smethod_1);
                }
                didDownloadCallback_0 = didDownloadCallback_2;
                wkeOnDidDownloadCallback(didDownloadCallback_0);
            }
        }


        //DidDownloadCallback([In, MarshalAs(UnmanagedType.LPWStr)] string url, IntPtr data, uint size);
        [CompilerGenerated]
        private static void smethod_1(string url, IntPtr data, uint size)
        {
            if (didDownloadCallback_1 != null)
            {
                didDownloadCallback_1(url, data, size);
            }
        }

        public static Dictionary<string, Assembly> ResourceAssemblys
        {
            get
            {
                return dictionary_0;
            }
        }
        public static string PageNameSpace { get; set; }
        private static Func<string, byte[]> func_0;
        private static Dictionary<string, Assembly> dictionary_0 = new Dictionary<string, Assembly>();
        private static void smethod_0(IntPtr intptr_0, string string_1, SetDataCallback setDataCallback_0)
        {
            byte[] buffer;
            IntPtr ptr;
            string str = string_1;
            if (func_0 != null)
            {
                buffer = func_0(string_1);
                if ((buffer != null) && (buffer.Length > 0))
                {
                    ptr = Marshal.AllocHGlobal(buffer.Length);
                    Marshal.Copy(buffer, 0, ptr, buffer.Length);
                    setDataCallback_0(intptr_0, ptr, (uint)buffer.Length);
                    return;
                }
            }
            if ((dictionary_0.Count != 0) && (string_1.IndexOf(':') < 0))
            {
                try
                {
                    Assembly assembly;
                    string_1 = string_1.TrimStart(new char[] { '\\', '/' });
                    string_1 = string_1.Replace('/', '.');
                    string_1 = string_1.Replace('\\', '.');
                    string key = string_1.Substring(0, string_1.IndexOf('.'));
                    if (dictionary_0.TryGetValue(key, out assembly))
                    {
                        Stream manifestResourceStream = assembly.GetManifestResourceStream(string_1);
                        if (manifestResourceStream != null)
                        {
                            using (manifestResourceStream)
                            {
                                if (manifestResourceStream.Length > 0L)
                                {
                                    buffer = new byte[manifestResourceStream.Length];
                                    manifestResourceStream.Read(buffer, 0, (int)manifestResourceStream.Length);
                                    ptr = Marshal.AllocHGlobal(buffer.Length);
                                    Marshal.Copy(buffer, 0, ptr, buffer.Length);
                                    setDataCallback_0(intptr_0, ptr, (uint)buffer.Length);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message + " ：" + str + "路径有错，无法读取文件数据");
                }
            }
        }

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDidDownloadCallback(OnDidDownloadCallback _callback);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDisableWOFF([MarshalAs(UnmanagedType.I1)] bool isDisable);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnReadFile(ReadFileCallback _callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeBeforeSendCallback(BeforeSendCallback _callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeExecCommand(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string command, [In] [MarshalAs(UnmanagedType.LPWStr)] string args);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetUserAgent(IntPtr handle);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetUserAgent(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetUserAgentW(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnJsCall(IntPtr handle, JsCallCallback js);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
       // public static extern void wkeJSSimpleBind(IntPtr es, [MarshalAs(UnmanagedType.LPWStr)] [In] string name, jsNativeFunction fn);
        public static extern void jsBindFunction([MarshalAs(UnmanagedType.LPWStr)] [In] string name, jsNativeFunction fn);



        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSString(IntPtr es, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSCallGlobal(IntPtr es, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToString(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string jsToStringW(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeJSData wkeJSGetData(IntPtr es, long jsValue);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetWTFString(IntPtr wtfstr, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);

        /// <summary>
        /// 直接执行JS代码
        /// </summary>
        /// <param name="es"></param>
        /// <param name="js"></param>
        /// <returns></returns>
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSEval(IntPtr es, [MarshalAs(UnmanagedType.LPWStr)] [In] string js);


        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetWTFString(IntPtr wtfstr);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint(IntPtr webView, IntPtr bits, int pitch);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern WkeCursorInfo wkeGetCursorInfoType(IntPtr webView);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint2(IntPtr webView, IntPtr bits, int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, [MarshalAs(UnmanagedType.I1)]bool bCopyAlpha);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaintDC(IntPtr handle, IntPtr hdc);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsArg(IntPtr es, int argIdx);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsArgCount(IntPtr wkeJSState);


        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGlobalExec(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeJSType jsTypeOf(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsNumber(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsString(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsBool(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsObject(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsFunction(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsUndefined(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsNull(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsArray(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsTrue(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsFalse(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSGetAt(IntPtr es, long @object, int @index);



        #region 创建JS基础变量

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsInt(IntPtr es, int n);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsFloat(IntPtr es, float f);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsDouble(IntPtr es, double d);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsBoolean(IntPtr es, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsUndefined(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsNull(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsTrue(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long jsFalse(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSEmptyObject(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSEmptyArray(IntPtr es);

        #endregion


        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double jsToDouble(IntPtr es, long v);
        /// <summary>
        /// WebView关联操作
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeJSGetWebView(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float jsToFloat(IntPtr es, long v);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsToInt(IntPtr es, long v);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool jsToBoolean(IntPtr es, long v);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeRunJS(IntPtr webView, IntPtr script);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeRunJSW(IntPtr webView, [In] [MarshalAs(UnmanagedType.LPWStr)] string script);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnAlertBox(IntPtr webView, AlertBoxCallback callback);
        /// <summary>
        /// 下载回调
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="callback"></param>
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDownloadFile(IntPtr webView, wkeDownloadFileCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConfirmBox(IntPtr webView, ConfirmBoxCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPromptBox(IntPtr webView, PromptBoxCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetViewDC(IntPtr webView);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseEvent(IntPtr webView, uint message, int x, int y, uint flags);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseWheelEvent(IntPtr webView, int x, int y, int delta, uint flags);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyUpEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireKeyDownEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyPressEvent(IntPtr webView, uint charCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetFocus(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGetCaretRect(IntPtr webView, ref wkeRect rect);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeKillFocus(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeResize(IntPtr webView, int w, int h);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeGetCookie(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetCookieW(IntPtr webView);

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Utf8StringToWkeChar([In, MarshalAs(UnmanagedType.LPStr)] string param0);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCallOnMainThread(EweCallBack _callback, IntPtr context);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCallOnMainThreadAndWait(EweCallBack _callback, IntPtr context);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeCanGoBack(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeCanGoForward(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr WkeCharToUtf8String([In, MarshalAs(UnmanagedType.LPWStr)] string param0);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeContextMenuEvent(IntPtr webView, int x, int y, uint flags);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWebView();
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWindow(IntPtr hParent);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDestroyWebView(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEnableContextMenu(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeFree(IntPtr ptr);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentHeight(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentWidth(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetHeight(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeGetMediaVolume(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetWidth(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeGetZoomFactor(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGoBack(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGoForward(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInitialize();
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsCookieEnabled(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsDirty(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadComplete(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoading(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingCompleted(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingFailed(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingSucceeded(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsTransparent(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJSCollectGarbge();

        public delegate void ContextCreateCallback(IntPtr es);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJSContextCreateCallback(ContextCreateCallback cb);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long wkeJSGet(IntPtr es, long jsValue, [In, MarshalAs(UnmanagedType.LPWStr)] string proName);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLimitPlugins(IntPtr handle, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadFile(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string filename);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadHTML(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string html);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadURL(IntPtr webView, IntPtr url);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadURLW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeMalloc(int size);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConsoleMessage(IntPtr webView, wkeConsoleMessageCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnCreateView(IntPtr webView, wkeCreateViewCallback callback, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void DidDownloadCallback([In, MarshalAs(UnmanagedType.LPWStr)] string url, IntPtr data, uint size);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDidDownloadCallback(DidDownloadCallback callback_);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDocumentReady(IntPtr webView, wkeDocumentReadyCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadingFinish(IntPtr webView, wkeLoadingFinishCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnNavigation(IntPtr webView, wkeNavigationCallback callback);

        public delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPaintUpdated(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr callbackParam);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnTitleChanged(IntPtr webView, TitleChangedCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnURLChanged(IntPtr webView, UrlChangedCallback callback);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePostURL(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url, IntPtr data, int dataBytes);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeReload(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMediaVolume(IntPtr webView, float volume);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMenuItemText([In, MarshalAs(UnmanagedType.LPWStr)] string item, [In, MarshalAs(UnmanagedType.LPWStr)] string text);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMenuItemVisible([In, MarshalAs(UnmanagedType.LPWStr)] string item, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetTransparent(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool transparent);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetZoomFactor(IntPtr webView, float factor);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeStopLoading(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeTitleW(IntPtr webView);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeWindowOnPaint(IntPtr webView, IntPtr bits, int pitch);


        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeFinalize();

        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetStoragePath([MarshalAs(UnmanagedType.LPWStr)] [In] string directory);
        [DllImport("node.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern string wkeGetVersionString();



    }

    public enum wkeMouseMessage : uint
    {
        WKE_MSG_MOUSEMOVE = 0x0200,
        WKE_MSG_LBUTTONDOWN = 0x0201,
        WKE_MSG_LBUTTONUP = 0x0202,
        WKE_MSG_LBUTTONDBLCLK = 0x0203,
        WKE_MSG_RBUTTONDOWN = 0x0204,
        WKE_MSG_RBUTTONUP = 0x0205,
        WKE_MSG_RBUTTONDBLCLK = 0x0206,
        WKE_MSG_MBUTTONDOWN = 0x0207,
        WKE_MSG_MBUTTONUP = 0x0208,
        WKE_MSG_MBUTTONDBLCLK = 0x0209,
        WKE_MSG_MOUSEWHEEL = 0x020A,
    }
    public enum wkeMouseFlags
    {
        WKE_LBUTTON = 0x01,
        WKE_RBUTTON = 0x02,
        WKE_SHIFT = 0x04,
        WKE_CONTROL = 0x08,
        WKE_MBUTTON = 0x10,
    }
    public delegate long JsCallCallback(IntPtr es);
    public delegate void UrlChangedCallback(IntPtr webView, IntPtr url);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate long jsNativeFunction(IntPtr es, long jsObject, long args, int argCount);

    public enum wkeJSType
    {
        JSTYPE_NUMBER,
        JSTYPE_STRING,
        JSTYPE_BOOLEAN,
        JSTYPE_OBJECT,
        JSTYPE_FUNCTION,
        JSTYPE_UNDEFINED
    }
    public class JsValue
    {
        private IntPtr intptr_0;
        private long long_0;

        public JsValue(long value, IntPtr jsExecState)
        {
            this.long_0 = value;
            this.intptr_0 = jsExecState;
        }

        public JsValue(IntPtr jsExecState, int argIndex)
        {
            this.intptr_0 = jsExecState;
            this.long_0 = EwePInvoke.jsArg(this.intptr_0, argIndex);
        }

        public static int ArgCount(IntPtr jsExecState)
        {
            return EwePInvoke.jsArgCount(jsExecState);
        }

        public static long JsDouble(IntPtr jsExecState, double d)
        {
            return EwePInvoke.jsDouble(jsExecState, d);
        }

        public static long JsFalse(IntPtr jsExecState)
        {
            return EwePInvoke.jsFalse(jsExecState);
        }

        public static long JsFloat(IntPtr jsExecState, float f)
        {
            return EwePInvoke.jsFloat(jsExecState, f);
        }

        public static long JsInt(IntPtr jsExecState, int n)
        {
            return EwePInvoke.jsInt(jsExecState, n);
        }

        public static long JsNull(IntPtr jsExecState)
        {
            return EwePInvoke.jsNull(jsExecState);
        }

        public static long JsString(IntPtr jsExecState, string str)
        {
            return EwePInvoke.wkeJSString(jsExecState, str);
        }

        public static long JsTrue(IntPtr jsExecState)
        {
            return EwePInvoke.jsTrue(jsExecState);
        }

        public static long JsUndefined(IntPtr jsExecState)
        {
            return EwePInvoke.jsUndefined(jsExecState);
        }

        public bool ToBool()
        {
            return EwePInvoke.jsToBoolean(this.intptr_0, this.long_0);
        }

        public double ToDouble()
        {
            return EwePInvoke.jsToDouble(this.intptr_0, this.long_0);
        }

        public float ToFloat()
        {
            return EwePInvoke.jsToFloat(this.intptr_0, this.long_0);
        }

        public int ToInt()
        {
            return EwePInvoke.jsToInt(this.intptr_0, this.long_0);
        }

        public override string ToString()
        {
            if (this.intptr_0 == IntPtr.Zero)
            {
                return string.Empty;
            }
            var data = new List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(EwePInvoke.jsToString(this.intptr_0, this.long_0), off++);
                if (ch == 0)
                {
                    break;
                }
                data.Add(ch);
            }
            return Encoding.UTF8.GetString(data.ToArray());

        }

        public wkeJSType JsType
        {
            get
            {
                return EwePInvoke.jsTypeOf(this.intptr_0, this.long_0);
            }
        }

        public long Value
        {
            get
            {
                return this.long_0;
            }
        }
    }



}
