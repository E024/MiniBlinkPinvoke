using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace MiniBlinkPinvoke
{
    /// <summary>
    /// 如需要更完善的功能，请根据Pinvoke封装规则把wke.h里的API进行封装
    /// </summary>
    public static class BlinkBrowserPInvoke
    {
        const string BlinkBrowserdll = "node.dll";//"node.dll";miniblink
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        private static Dictionary<string, Assembly> _ResourceAssemblys = new Dictionary<string, Assembly>();
        public static Dictionary<string, Assembly> ResourceAssemblys
        {
            get
            {
                return _ResourceAssemblys;
            }
        }
        static BlinkBrowserPInvoke()
        {
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        /// <summary>
        /// 设置页面控件的偏移量，一般用作控件时，需要制定位置，否则会出现事件响应位置有问题。
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandleOffset(IntPtr webView, int x, int y);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandle(IntPtr webView, IntPtr wnd);
        /// <summary>
        /// 是否启用插件
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNpapiPluginsEnabled(IntPtr webView, bool Enable);
        /// <summary>
        /// 是否渲染,True 禁止渲染，false 启用渲染
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHeadlessEnabled(IntPtr webView, bool Enable);
        /// <summary>
        /// 绑定View请求时，所用IP，比如一台电脑两个IP的情况下，可以指定某个IP访问
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="netInterface"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetViewNetInterface(IntPtr webView, IntPtr netInterface);
        /// <summary>
        /// 是否检查CSP，也就是跨域安全检查
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCspCheckEnable(IntPtr webView, bool Enable);
        /// <summary>
        /// 是否新开窗口
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNavigationToNewWindowEnable(IntPtr webView, bool Enable);

        /// <summary>
        /// 是否启用 Touch 触摸事件
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetTouchEnabled(IntPtr webView, bool Enable);
        /// <summary>
        /// 是否启用内存缓存
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="Enable"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMemoryCacheEnable(IntPtr webView, bool Enable);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeShowDevtools(IntPtr webView, [In] [MarshalAs(UnmanagedType.LPWStr)] string path, wkeOnShowDevtoolsCallback callback, IntPtr param);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadUrlBegin(IntPtr webView, wkeLoadUrlBeginCallback callback, IntPtr callbackParam);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadUrlEnd(IntPtr webView, wkeLoadUrlEndCallback callback, IntPtr callbackParam);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDidDownloadCallback(OnDidDownloadCallback _callback);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr wkeGetStringW(IntPtr @string);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr wkeToStringW(IntPtr @string);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetString(IntPtr @string);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSelectAll(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCopy(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCut(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaste(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDelete(IntPtr webView);



        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeMediaVolume(IntPtr webView);


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMediaVolume(IntPtr webView, Single volume);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Single wkeGetMediaVolume(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnReadFile(ReadFileCallback _callback);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeBeforeSendCallback(BeforeSendCallback _callback);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void wkeNetSetMIMEType(IntPtr job, IntPtr type);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetSetURL(IntPtr job, [MarshalAs(UnmanagedType.LPStr)]string url);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetSetData(IntPtr job, IntPtr buf, int len);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetOnResponse(IntPtr webView, wkeNetResponseCallback callback, IntPtr param);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetGetMIMEType(IntPtr job, IntPtr mime);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeNetHookRequest(IntPtr job);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeExecCommand(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string command, [In] [MarshalAs(UnmanagedType.LPWStr)] string args);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetUserAgent(IntPtr handle);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetUserAgent(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPStr)] string str);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetUserAgentW(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJsBindFunction([MarshalAs(UnmanagedType.LPStr)] [In] string name, wkeJsNativeFunction fn, IntPtr param, uint argCount);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJsBindGetter([MarshalAs(UnmanagedType.LPStr)] [In] string name, wkeJsNativeFunction fn, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJsBindSetter([MarshalAs(UnmanagedType.LPStr)] [In] string name, wkeJsNativeFunction fn, IntPtr param);

        /// <summary>
        /// 调试接口
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fn"></param>
        /// <param name="param"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetDebugConfig(IntPtr handler, [MarshalAs(UnmanagedType.LPStr)] [In] string debugString, [MarshalAs(UnmanagedType.LPStr)] [In] string param);


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsStringW(IntPtr es, IntPtr str);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsString(IntPtr es, IntPtr str);


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToString(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr jsToStringW(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeJSData wkeJSGetData(IntPtr es, Int64 jsValue);


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateStringW(IntPtr str, Int64 len);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDeleteString(IntPtr str);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetWTFString(IntPtr wtfstr, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);

        /// <summary>
        /// 直接执行JS代码
        /// </summary>
        /// <param name="es"></param>
        /// <param name="js"></param>
        /// <returns></returns>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeJSEval(IntPtr es, [MarshalAs(UnmanagedType.LPWStr)] [In] string js);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetWTFString(IntPtr wtfstr);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint(IntPtr webView, IntPtr bits, int pitch);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetDragFiles(IntPtr webView, Point clintPos, Point screenPos, IntPtr[] files, int filesCount);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern WkeCursorInfo wkeGetCursorInfoType(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaint2(IntPtr webView, IntPtr bits, int bufWid, int bufHei, int xDst, int yDst, int w, int h, int xSrc, int ySrc, [MarshalAs(UnmanagedType.I1)]bool bCopyAlpha);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePaintDC(IntPtr handle, IntPtr hdc);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsArg(IntPtr es, int argIdx);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsArgCount(IntPtr wkeJSState);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGlobalExec(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeJSType jsTypeOf(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsNumber(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsString(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsBool(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsObject(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsFunction(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsUndefined(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsNull(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsArray(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsTrue(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeJSIsFalse(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeJSGetAt(IntPtr es, Int64 @object, int @index);


        #region 创建JS基础变量

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsInt(IntPtr es, int n);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsFloat(IntPtr es, float f);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsDouble(IntPtr es, double d);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsBoolean(IntPtr es, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsUndefined(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsNull(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsTrue(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 jsFalse(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeJSEmptyObject(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeJSEmptyArray(IntPtr es);

        #endregion


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern double jsToDouble(IntPtr es, Int64 v);
        /// <summary>
        /// WebView关联操作
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeJSGetWebView(IntPtr es);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern float jsToFloat(IntPtr es, Int64 v);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int jsToInt(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool jsToBoolean(IntPtr es, Int64 v);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeRunJS(IntPtr webView, IntPtr script);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeRunJSW(IntPtr webView, [In] [MarshalAs(UnmanagedType.LPWStr)] string script);
        /// <summary>
        /// 可以在wkeOnDocumentReady2回调里注入JS执行 
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="frameId"></param>
        /// <param name="script"></param>
        /// <param name="isInClosure"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeRunJsByFrame(IntPtr webView, IntPtr frameId, IntPtr script, [MarshalAs(UnmanagedType.I1)] bool isInClosure);



        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnAlertBox(IntPtr webView, AlertBoxCallback callback, IntPtr callbackParam);
        /// <summary>
        /// 下载回调
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="callback"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDownloadFile(IntPtr webView, wkeDownloadFileCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDownload(IntPtr webView, wkeDownloadFileCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConfirmBox(IntPtr webView, ConfirmBoxCallback callback);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPromptBox(IntPtr webView, PromptBoxCallback callback);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetViewDC(IntPtr webView);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseEvent(IntPtr webView, uint message, int x, int y, uint flags);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseWheelEvent(IntPtr webView, int x, int y, int delta, uint flags);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyUpEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireKeyDownEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyPressEvent(IntPtr webView, uint charCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetFocus(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGetCaretRect(IntPtr webView, ref wkeRect rect);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeKillFocus(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeResize(IntPtr webView, int w, int h);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetCookie(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern string wkeGetCookieW(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieJarPath(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] [In] string path);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieJarFullPath(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] [In] string path);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookie(IntPtr webView, [MarshalAs(UnmanagedType.LPArray)]byte[] url, [MarshalAs(UnmanagedType.LPArray)]byte[] cookie);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Utf8StringToWkeChar([In, MarshalAs(UnmanagedType.LPStr)] string param0);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCallOnMainThread(EweCallBack _callback, IntPtr context);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeCallOnMainThreadAndWait(EweCallBack _callback, IntPtr context);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeCanGoBack(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeCanGoForward(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr WkeCharToUtf8String([In, MarshalAs(UnmanagedType.LPWStr)] string param0);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeContextMenuEvent(IntPtr webView, int x, int y, uint flags);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWebView();
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWindow(IntPtr hParent);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDestroyWebView(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEnableContextMenu(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEnableWindow(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeFree(IntPtr ptr);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentHeight(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetContentWidth(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetHeight(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkeGetWidth(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern float wkeGetZoomFactor(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGoBack(IntPtr webView);

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorSelectAll(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorUnSelect(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorCopy(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorCut(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorUndo(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorRedo(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeEditorDelete(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeGoForward(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInitialize();

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeInitializeEx(IntPtr settings);

        public static void wkeInitializeExWrap(wkeSettings settings)
        {
            int nSizeOfSettings = Marshal.SizeOf(settings);
            IntPtr intPtr = Marshal.AllocHGlobal(nSizeOfSettings);
            Marshal.StructureToPtr(settings, intPtr, true);
            BlinkBrowserPInvoke.wkeInitializeEx(intPtr);
            Marshal.DestroyStructure(intPtr, typeof(wkeSettings));
        }

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeConfigure(IntPtr settings); // wkeSettings

        public static void wkeConfigureWrap(wkeSettings settings)
        {
            int nSizeOfSettings = Marshal.SizeOf(settings);
            IntPtr intPtr = Marshal.AllocHGlobal(nSizeOfSettings);
            Marshal.StructureToPtr(settings, intPtr, true);
            BlinkBrowserPInvoke.wkeConfigure(intPtr);
            Marshal.DestroyStructure(intPtr, typeof(wkeSettings));
        }

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsCookieEnabled(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsDirty(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadComplete(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoading(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingCompleted(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingFailed(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsLoadingSucceeded(IntPtr webView);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeIsTransparent(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJSCollectGarbge();
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeJSContextCreateCallback(ContextCreateCallback cb);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 wkeJSGet(IntPtr es, Int64 jsValue, [In, MarshalAs(UnmanagedType.LPWStr)] string proName);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLimitPlugins(IntPtr handle, [MarshalAs(UnmanagedType.I1)] bool b);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadFile(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string filename);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadHTML(IntPtr webView, IntPtr html);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadHTMLW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string html);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadURL(IntPtr webView, IntPtr url);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void wkeLoadURLW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void wkeLoadFileW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void wkeLoadW(IntPtr webView, [In, MarshalAs(UnmanagedType.LPWStr)] string url);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeMalloc(int size);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConsole(IntPtr webView, wkeConsoleMessageCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnCreateView(IntPtr webView, wkeCreateViewCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDidDownloadCallback(DidDownloadCallback callback_);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDocumentReady(IntPtr webView, wkeDocumentReadyCallback callback, IntPtr param);
        /// <summary>
        /// 可判断是不是主 frame 加载完成
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnDocumentReady2(IntPtr webView, wkeDocumentReadyCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadingFinish(IntPtr webView, wkeLoadingFinishCallback callback, IntPtr param);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnNavigation(IntPtr webView, wkeNavigationCallback callback, IntPtr param);




        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPaintUpdated(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr callbackParam);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnTitleChanged(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnURLChanged(IntPtr webView, UrlChangedCallback callback, IntPtr callbackParam);
        /// <summary>
        /// 可以实现类似谷歌浏览器鼠标移动到带有超链接的地方，给出链接地址。
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="callback"></param>
        /// <param name="callbackParam"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnMouseOverUrlChanged(IntPtr webView, TitleChangedCallback callback, IntPtr callbackParam);
        /// <summary>
        /// URL 改变，可判断是否是主 frame 的url 改变
        /// </summary>
        /// <param name="webView"></param>
        /// <param name="callback"></param>
        /// <param name="callbackParam"></param>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnURLChanged2(IntPtr webView, UrlChangedCallback2 callback, IntPtr callbackParam);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkePostURL(IntPtr webView, [In, MarshalAs(UnmanagedType.LPStr)] string url, IntPtr data, int dataBytes);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeReload(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieEnabled(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMenuItemText([In, MarshalAs(UnmanagedType.LPStr)] string item, [In, MarshalAs(UnmanagedType.LPStr)] string text);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetMenuItemVisible([In, MarshalAs(UnmanagedType.LPStr)] string item, [MarshalAs(UnmanagedType.I1)] bool enable);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetTransparent(IntPtr webView, [MarshalAs(UnmanagedType.I1)] bool transparent);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetZoomFactor(IntPtr webView, float factor);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeStopLoading(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeTitleW(IntPtr webView);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeWindowOnPaint(IntPtr webView, IntPtr bits, int pitch);


        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeFinalize();

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetStoragePath([MarshalAs(UnmanagedType.LPWStr)] [In] string directory);
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetVersionString();

        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeNetGetPostBody(IntPtr job);

        /// <summary>
        /// wkeRequestType 
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [DllImport(BlinkBrowserdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeRequestType wkeNetGetRequestMethod(IntPtr job);

        public static string Utf8IntptrToString(IntPtr ptr)
        {
            var data = new List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, off++);
                if (ch == 0)
                {
                    break;
                }
                data.Add(ch);
            }
            return Encoding.UTF8.GetString(data.ToArray());
        }

    }
}
