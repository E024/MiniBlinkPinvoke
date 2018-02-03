using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniBlinkPinvoke
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool wkeJSSetPropertyCallback(IntPtr es, long @object, [MarshalAs(UnmanagedType.LPWStr)] [In] string propertyName, long value);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate long wkeJSGetPropertyCallback(IntPtr es, long @object, [MarshalAs(UnmanagedType.LPWStr)] [In] string propertyName);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeJSFinalizeCallback(ref wkeJSData data);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate long wkeJSCallAsFunctionCallback(IntPtr es, long @object, ref long args, int argCount);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AlertBoxCallback(IntPtr webView, IntPtr param, IntPtr msg);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool ConfirmBoxCallback(IntPtr webView, IntPtr msg);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool PromptBoxCallback(IntPtr webView, IntPtr msg, IntPtr defaultResult, IntPtr result);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeDocumentReadyCallback(IntPtr webView, IntPtr param);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReadFileCallback(IntPtr _caller, [MarshalAs(UnmanagedType.LPStr)]string szFile, SetDataCallback setData);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetDataCallback(IntPtr _caller, IntPtr data, uint length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool BeforeSendCallback(IntPtr url, IntPtr method, IntPtr data, long dataSize);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EweCallBack(IntPtr param0);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeConsoleMessageCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, int sourceLine, IntPtr stackTrace);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void wkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason);
    /// <summary>
    /// 下载回调
    /// </summary>
    /// <param name="webView"></param>
    /// <param name="url"></param>
    /// <param name="result"></param>
    /// <param name="failedReason"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool wkeDownloadFileCallback(IntPtr webView, IntPtr param, string url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void TitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr wkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnDidDownloadCallback([MarshalAs(UnmanagedType.LPWStr)] [In] string url, IntPtr data, uint size);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 JsCallCallback(IntPtr es);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UrlChangedCallback(IntPtr webView, IntPtr param, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool wkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate bool wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, [MarshalAs(UnmanagedType.LPStr)] string url, IntPtr job);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, [MarshalAs(UnmanagedType.LPStr)] string url, IntPtr job, IntPtr buf, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate Int64 jsNativeFunction(IntPtr es);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate Int64 wkeJsNativeFunction(IntPtr es, IntPtr param);
}
