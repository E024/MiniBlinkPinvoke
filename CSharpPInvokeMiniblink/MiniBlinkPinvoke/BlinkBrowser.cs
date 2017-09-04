
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static MiniBlinkPinvoke.BlinkBrowserPInvoke;

namespace MiniBlinkPinvoke
{
    public class BlinkBrowser : Control
    {


        public IntPtr handle = IntPtr.Zero;
        //Timer timer = new Timer { Interval = 25 };
        string url = string.Empty;

        IntPtr bits = IntPtr.Zero;
        static IntPtr jsHandle = IntPtr.Zero;
        Size oldSize;
        public object GlobalObjectJs = new object();


        public event TitleChangedCallback OnTitleChangeCall;

        //public event wkeDocumentReadyCallback DocumentReadyCallback;

        List<object> listObj = new List<object>();

        static UrlChangedCallback urlChangedCallback;
        static AlertBoxCallback AlertBoxCallback;
        static TitleChangedCallback titleChangeCallback;
        static wkeNavigationCallback _wkeNavigationCallback;
        static wkeConsoleMessageCallback _wkeConsoleMessageCallback;
        static wkePaintUpdatedCallback _wkePaintUpdatedCallback;
        static wkeDocumentReadyCallback _wkeDocumentReadyCallback;
        static wkeLoadingFinishCallback _wkeLoadingFinishCallback;
        static wkeDownloadFileCallback _wkeDownloadFileCallback;
        static wkeCreateViewCallback _wkeCreateViewCallback;
        static wkeLoadUrlBeginCallback _wkeLoadUrlBeginCallback;
        private System.ComponentModel.IContainer components;
        static wkeLoadUrlEndCallback _wkeLoadUrlEndCallback;


        void OnwkeLoadUrlEndCallback(IntPtr webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len)
        {
            Console.WriteLine("call OnwkeLoadUrlEndCallback url:" + url);
            Console.WriteLine(buf.IntptrToString().Length);
            //  Marshal.Release(buf);
        }

        bool OnwkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, string url, IntPtr job)
        {
            var _url = url;
            if (_url.StartsWith("https://news.cnblogs.com/"))
            {
                //
                string data = "<html><head><title>hook test</title></head><body><h1>hook!</h1></body></html>";
                wkeNetSetMIMEType(job, Marshal.StringToCoTaskMemAnsi("text/html"));
                wkeNetSetURL(job, url);
                //var dataByte = Encoding.UTF8.GetBytes(data);
                //IntPtr dataPtr = Marshal.AllocHGlobal(dataByte.Length);
                //Marshal.Copy(Encoding.UTF8.GetBytes(data), 0, dataPtr, dataByte.Length);
                //wkeNetSetData(job, dataPtr, Encoding.UTF8.GetBytes(data).Length);
                //Marshal.Release(dataPtr);

                wkeNetSetData(job, Marshal.StringToCoTaskMemAnsi(data), Encoding.UTF8.GetBytes(data).Length);

                return true;
            }
            else
            {
                //如果需要 OnwkeLoadUrlEndCallback 回调，需要取消注释下面的 hook
                //wkeNetHookRequest(job);

            }
            Console.WriteLine("OnwkeLoadUrlBeginCallback url:" + url);
            return false;
        }

        IntPtr OnwkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url)
        {
            Console.WriteLine("OnwkeCreateViewCallback url:" + wkeGetString(url).IntptrToString());
            Console.WriteLine("OnwkeCreateViewCallback navigationType:" + navigationType);
            return webView;
        }
        bool OnwkeDownloadFileCallback(IntPtr webView, IntPtr param, string url)
        {
            Console.WriteLine("call OnwkeDownloadFileCallback:" + (url));
            return true;
        }

        void OnwkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason)
        {
            Console.WriteLine("call OnwkeLoadingFinishCallback:" + wkeGetString(url).IntptrToString());
            Console.WriteLine("call OnwkeLoadingFinishCallback result:" + result);
            if (result == wkeLoadingResult.WKE_LOADING_FAILED)
            {
                Console.WriteLine("call OnwkeLoadingFinishCallback 加载失败 failedReason:" + wkeGetString(failedReason).IntptrToString());
            }
            else
            {
                _url = wkeGetString(url).IntptrToString();
                Console.WriteLine("call OnwkeLoadingFinishCallback:成功加载完成。" + wkeGetString(url).IntptrToString());
            }
        }
        void OnwkeDocumentReadyCallback(IntPtr webView, IntPtr param)
        {
            Console.WriteLine("call OnwkeDocumentReadyCallback:" + Marshal.PtrToStringUni(param));//.IntptrToString());
        }
        void OnwkeConsoleMessageCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, int sourceLine, IntPtr stackTrace)
        {
            //Console.WriteLine("Console level" + level);
            Console.WriteLine("Console Msg:" + wkeGetString(message).IntptrToString());
            //Console.WriteLine("Console sourceName:" + wkeGetString(sourceName).IntptrToString());
            //Console.WriteLine("Console stackTrace:" + wkeGetString(stackTrace).IntptrToString());
            //Console.WriteLine("Console sourceLine:" + sourceLine);
        }
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        public BlinkBrowser()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
      ControlStyles.DoubleBuffer |
           ControlStyles.AllPaintingInWmPaint |
           ControlStyles.ResizeRedraw |
           //  ControlStyles.EnableNotifyMessage|
           ControlStyles.UserPaint, true);
            UpdateStyles();

            MouseEnter += ((s, ea) =>
            {
                lock (LockObj)
                {
                    if (!this.Focused)
                    {
                        this.Focus();
                    }
                }
            });


            contextMenuStrip.Opening += ContextMenuStrip1_Opening;

            ContextMenuStrip = contextMenuStrip;
            ToolStripMenuItem tsmiGoBack = new ToolStripMenuItem("返回", null, (x, y) =>
            {
                wkeGoBack(handle);
            });
            ToolStripMenuItem tsmiForward = new ToolStripMenuItem("前进", null, (x, y) =>
            {
                wkeGoForward(handle);
            });
            ToolStripMenuItem tsmiReload = new ToolStripMenuItem("重新加载", null, (x, y) =>
            {
                wkeReload(handle);
            });
            ToolStripMenuItem tsmiSelectAll = new ToolStripMenuItem("全选", null, (x, y) =>
            {
                wkeSelectAll(handle);
            });
            ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("复制", null, (x, y) =>
            {
                wkeCopy(handle);
            });
            ToolStripMenuItem tsmiCut = new ToolStripMenuItem("剪切", null, (x, y) =>
            {
                wkeCut(handle);
            });
            ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("粘贴", null, (x, y) =>
            {
                wkePaste(handle);
            });
            ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("删除", null, (x, y) =>
            {
                wkeDelete(handle);
            });

            contextMenuStrip.Items.Add(tsmiGoBack);
            contextMenuStrip.Items.Add(tsmiForward);
            contextMenuStrip.Items.Add(tsmiReload);
            contextMenuStrip.Items.Add(tsmiSelectAll);
            contextMenuStrip.Items.Add(tsmiCopy);
            contextMenuStrip.Items.Add(tsmiCut);
            contextMenuStrip.Items.Add(tsmiPaste);
            contextMenuStrip.Items.Add(tsmiDelete);

            GlobalObjectJs = this;
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.handle != IntPtr.Zero)
            {
                lock (LockObj)
                {
                    foreach (ToolStripMenuItem item in contextMenuStrip.Items)
                    {
                        if (item.Text == "返回")
                        {
                            item.Enabled = wkeCanGoBack(this.handle);
                        }
                        if (item.Text == "前进")
                        {
                            item.Enabled = wkeCanGoForward(this.handle);
                        }
                        //if (item.Text == "全选")
                        //{
                        //    item.Enabled = wkeCanGoForward(this.handle);
                        //}
                    }

                }
            }
        }

        void OnTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title)
        {
            //Console.WriteLine(BlinkBrowserPInvoke.wkeGetStringW(title));
            //base.Text = BlinkBrowserPInvoke.wkeGetStringW(title);
            OnTitleChangeCall?.Invoke(webView, param, title);
        }
        bool OnwkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url)
        {

            Console.WriteLine(navigationType);
            Console.WriteLine("OnwkeNavigationCallback:URL:" + BlinkBrowserPInvoke.wkeGetStringW(url));
            //Console.WriteLine(Marshal.PtrToStringAnsi(url));
            //Console.WriteLine(Marshal.PtrToStringAuto(url));
            //Console.WriteLine(Marshal.PtrToStringBSTR(url));
            //Console.WriteLine(Marshal.PtrToStringUni(url));
            return true;
        }

        private string _url = string.Empty;
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                if (handle != IntPtr.Zero)
                {
                    wkeLoadURLW(this.handle, _url);
                }
            }
        }

        void OnUrlChangedCallback(IntPtr webView, IntPtr param, IntPtr url)
        {
            Console.WriteLine("OnUrlChangedCallback:URL:" + wkeGetStringW(url));
        }
        void OnTitleChangeCallback(IntPtr webView, IntPtr param, IntPtr title)
        {
            Console.WriteLine(Marshal.PtrToStringAnsi(title));
        }

        void OnWkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            //Console.WriteLine(string.Format("call OnWkePaintUpdatedCallback {0},{1},{2},{3},{4},{5}", param, hdc, x, y, cx, cy));
            if (handle != IntPtr.Zero && BlinkBrowserPInvoke.wkeIsDirty(handle))
            {
                Invalidate(new Rectangle(x, y, cx, cy), false);
                //Invalidate();




            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                //timer.Tick += Timer_Tick;
                //timer.Start();
                BlinkBrowserPInvoke.wkeInitialize();
                handle = BlinkBrowserPInvoke.wkeCreateWebView();

                BlinkBrowserPInvoke.wkeSetCookieEnabled(handle, true);
                BlinkBrowserPInvoke.wkeSetCookieJarPath(handle, Application.StartupPath + "\\cookie\\");

                BlinkBrowserPInvoke.wkeResize(handle, Width, Height);
                BindJsFunc();
                AlertBoxCallback = new AlertBoxCallback((a, b) =>
                {
                    MessageBox.Show(Marshal.PtrToStringAuto(b), "alert 调用");
                });
                BlinkBrowserPInvoke.wkeOnAlertBox(handle, AlertBoxCallback);

                //设置声音
                //BlinkBrowserPInvoke.wkeSetMediaVolume(handle, 20);

                _wkeNavigationCallback = OnwkeNavigationCallback;
                BlinkBrowserPInvoke.wkeOnNavigation(handle, _wkeNavigationCallback, IntPtr.Zero);
                listObj.Add(_wkeNavigationCallback);



                //BlinkBrowserPInvoke.wkeSetCookieEnabled(handle, false);

                titleChangeCallback = OnTitleChangedCallback;
                BlinkBrowserPInvoke.wkeOnTitleChanged(this.handle, titleChangeCallback, IntPtr.Zero);
                listObj.Add(titleChangeCallback);

                urlChangedCallback = OnUrlChangedCallback;
                BlinkBrowserPInvoke.wkeOnURLChanged(this.handle, urlChangedCallback, IntPtr.Zero);
                listObj.Add(urlChangedCallback);

                _wkeConsoleMessageCallback = OnwkeConsoleMessageCallback;
                BlinkBrowserPInvoke.wkeOnConsole(this.handle, _wkeConsoleMessageCallback, IntPtr.Zero);
                listObj.Add(_wkeConsoleMessageCallback);

                _wkePaintUpdatedCallback = OnWkePaintUpdatedCallback;
                BlinkBrowserPInvoke.wkeOnPaintUpdated(this.handle, _wkePaintUpdatedCallback, IntPtr.Zero);
                listObj.Add(_wkePaintUpdatedCallback);

                _wkeDocumentReadyCallback = OnwkeDocumentReadyCallback;
                var pa = Marshal.StringToCoTaskMemUni("我传的值：：：：：");
                BlinkBrowserPInvoke.wkeOnDocumentReady(this.handle, _wkeDocumentReadyCallback, pa);
                listObj.Add(_wkeDocumentReadyCallback);


                _wkeLoadingFinishCallback = OnwkeLoadingFinishCallback;
                BlinkBrowserPInvoke.wkeOnLoadingFinish(this.handle, _wkeLoadingFinishCallback, IntPtr.Zero);
                listObj.Add(_wkeLoadingFinishCallback);

                ////会导致 taobao 加载图片异常
                //_wkeDownloadFileCallback = OnwkeDownloadFileCallback;
                //BlinkBrowserPInvoke.wkeOnDownload(this.handle, _wkeDownloadFileCallback, IntPtr.Zero);
                //listObj.Add(_wkeDownloadFileCallback);

                _wkeCreateViewCallback = OnwkeCreateViewCallback;
                BlinkBrowserPInvoke.wkeOnCreateView(this.handle, _wkeCreateViewCallback, handle);
                listObj.Add(_wkeCreateViewCallback);

                _wkeLoadUrlBeginCallback = OnwkeLoadUrlBeginCallback;
                BlinkBrowserPInvoke.wkeOnLoadUrlBegin(this.handle, _wkeLoadUrlBeginCallback, handle);
                listObj.Add(_wkeLoadUrlBeginCallback);

                #region JS 动态绑定，并返回值
                jsNativeFunction jsnav = new jsNativeFunction((es) =>
                {
                    return jsStringW(es, "这是C#返回值:" + jsToString(es, jsArg(es, 0)).IntptrToString());
                });
                BlinkBrowserPInvoke.jsBindFunction("jsReturnValueTest", jsnav, 1);
                listObj.Add(jsnav);
                #endregion

                // get
                jsNativeFunction jsnavGet = new jsNativeFunction((es) =>
                {
                    Console.WriteLine("call jsBindGetter");
                    return jsStringW(es, "{ \"name\": \"he\" }");
                    //return jsStringW(es, "这是C#返回值:" + jsToString(es, jsArg(es, 0)).IntptrToString());
                });
                BlinkBrowserPInvoke.jsBindGetter("testJson", jsnavGet);
                listObj.Add(jsnavGet);

                // set
                jsNativeFunction jsnavSet = new jsNativeFunction((es) =>
                {
                    Console.WriteLine("call jsBindSetter");

                    Int64 testJson = jsArg(es, 0);
                    IntPtr argStr = jsToStringW(es, testJson);
                    string argString = Marshal.PtrToStringUni(argStr);
                    MessageBox.Show(argString, "alert setter");

                    return jsUndefined(es);
                });
                BlinkBrowserPInvoke.jsBindSetter("testJson", jsnavSet);
                listObj.Add(jsnavSet);

                _wkeLoadUrlEndCallback = OnwkeLoadUrlEndCallback;
                BlinkBrowserPInvoke.wkeOnLoadUrlEnd(this.handle, _wkeLoadUrlEndCallback, handle);
                listObj.Add(_wkeLoadUrlEndCallback);
            }
        }
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    if (handle != IntPtr.Zero && BlinkBrowserPInvoke.wkeIsDirty(handle))
        //    {
        //        Invalidate();
        //    }
        //}

        public string GetCookiesByFile
        {
            get
            {
                StringBuilder sbCookie = new StringBuilder();
                if (File.Exists("cookies.dat"))
                {

                    var uri = new Uri(URL);
                    var host = uri.Host;
                    var allCookies = File.ReadLines("cookies.dat").ToList();
                    for (int i = 4; i < allCookies.Count(); i++)
                    {
                        var listCookie = allCookies[i].Split('\t');
                        if (listCookie != null && listCookie.Count() != 0 && listCookie.Count() == 7)
                        {
                            if (listCookie[0] == host)
                            {
                                sbCookie.AppendFormat("{0}={1};",listCookie[5],listCookie[6]);
                            }
                            //httponly
                            var httpOnly = "#HttpOnly_" + listCookie[0];
                            if (listCookie[0] == httpOnly)
                            {
                                sbCookie.AppendFormat("{0}={1};", listCookie[5], listCookie[6]);
                            }
                            //主域  ....
                           // Lable
                        }
                    
                    }
                }
                return sbCookie.ToString();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (handle != IntPtr.Zero)
            {

                if (bits == IntPtr.Zero || oldSize != Size)
                {
                    if (bits != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(bits);
                    }
                    oldSize = Size;
                    bits = Marshal.AllocHGlobal(Width * Height * 4);
                }

                BlinkBrowserPInvoke.wkePaint(handle, bits, 0);
                using (Bitmap bmp = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, bits))
                {
                    e.Graphics.DrawImage(bmp, 0, 0);
                }
                SetCursors();
            }
            else
            {
                base.OnPaint(e);
            }
            //base.OnPaint(e);
            if (DesignMode)
            {
                e.Graphics.DrawString("MiniBlinkBrowser", this.Font, Brushes.Red, new Point());
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
            }
            //Application.DoEvents();
            //GC.Collect();
        }
        void SetCursors()
        {
            //Console.WriteLine("wkeGetCursorInfoType:  " + BlinkBrowserPInvoke.wkeGetCursorInfoType(handle));
            switch (BlinkBrowserPInvoke.wkeGetCursorInfoType(handle))
            {
                case WkeCursorInfo.WkeCursorInfoPointer:
                    Cursor = Cursors.Default;
                    break;
                case WkeCursorInfo.WkeCursorInfoCross:
                    Cursor = Cursors.Cross;
                    break;
                case WkeCursorInfo.WkeCursorInfoHand:
                    Cursor = Cursors.Hand;
                    break;
                case WkeCursorInfo.WkeCursorInfoIBeam:
                    Cursor = Cursors.IBeam;
                    break;
                case WkeCursorInfo.WkeCursorInfoWait:
                    Cursor = Cursors.WaitCursor;
                    break;
                case WkeCursorInfo.WkeCursorInfoHelp:
                    Cursor = Cursors.Help;
                    break;
                case WkeCursorInfo.WkeCursorInfoEastResize:
                    Cursor = Cursors.PanEast;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthResize:
                    Cursor = Cursors.PanNorth;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthEastResize:
                    Cursor = Cursors.PanNE;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthWestResize:
                    Cursor = Cursors.PanNW;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthResize:
                    Cursor = Cursors.PanSouth;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthEastResize:
                    Cursor = Cursors.SizeNESW;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthWestResize:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case WkeCursorInfo.WkeCursorInfoWestResize:
                    Cursor = Cursors.PanWest;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthSouthResize:
                    Cursor = Cursors.SizeNS;
                    break;
                case WkeCursorInfo.WkeCursorInfoEastWestResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthEastSouthWestResize:
                    Cursor = Cursors.SizeAll;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthWestSouthEastResize:
                    Cursor = Cursors.SizeAll;
                    break;
                case WkeCursorInfo.WkeCursorInfoColumnResize:
                    Cursor = Cursors.Default;
                    break;
                case WkeCursorInfo.WkeCursorInfoRowResize:
                    Cursor = Cursors.Default;
                    break;
                default:
                    Cursor = Cursors.Default;
                    break;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (handle != IntPtr.Zero)
            {
                BlinkBrowserPInvoke.wkeResize(handle, Width, Height);
                Invalidate();
            }
        }
        static object LockObj = new object();
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            lock (LockObj)
            {
                base.OnMouseWheel(e);
                if (handle != IntPtr.Zero)
                {
                    uint flags = GetMouseFlags(e);
                    BlinkBrowserPInvoke.wkeFireMouseWheelEvent(handle, e.X, e.Y, e.Delta, flags);
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            lock (LockObj)
            {
                base.OnMouseDown(e);
                uint msg = 0;
                if (e.Button == MouseButtons.Left)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_LBUTTONDOWN;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_MBUTTONDOWN;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_RBUTTONDOWN;
                }
                uint flags = GetMouseFlags(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeFireMouseEvent(handle, msg, e.X, e.Y, flags);
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            lock (LockObj)
            {
                base.OnMouseUp(e);
                uint msg = 0;
                if (e.Button == MouseButtons.Left)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_LBUTTONUP;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_MBUTTONUP;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    msg = (uint)wkeMouseMessage.WKE_MSG_RBUTTONUP;
                }
                uint flags = GetMouseFlags(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeFireMouseEvent(handle, msg, e.X, e.Y, flags);
                    //if (e.Button == MouseButtons.Right)
                    //{
                    //    EwePInvoke.wkeFireContextMenuEvent(handle, e.X, e.Y, flags);
                    //}
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            lock (LockObj)
            {
                //base.OnMouseMove(e);
                //if (handle != IntPtr.Zero)
                //{
                //    //uint msg = (uint)wkeMouseMessage.WKE_MSG_MOUSEMOVE;
                //    uint flags = GetMouseFlags(e);
                //    //EwePInvoke.wkeFireMouseEvent(handle, msg, e.X, e.Y, flags);
                //    EwePInvoke.wkeFireMouseEvent(handle, 0x200, e.X, e.Y, flags);
                //}
                base.OnMouseMove(e);
                if (this.handle != IntPtr.Zero)
                {
                    uint flags = GetMouseFlags(e);
                    BlinkBrowserPInvoke.wkeFireMouseEvent(this.handle, 0x200, e.X, e.Y, flags);
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            lock (LockObj)
            {
                base.OnKeyDown(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeFireKeyDownEvent(handle, (uint)e.KeyValue, 0, false);
                }
            }
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            lock (LockObj)
            {
                base.OnKeyPress(e);
                if (handle != IntPtr.Zero)
                {
                    e.Handled = true;
                    BlinkBrowserPInvoke.wkeFireKeyPressEvent(handle, (uint)e.KeyChar, 0, false);
                }
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            lock (LockObj)
            {
                base.OnKeyUp(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeFireKeyUpEvent(handle, (uint)e.KeyValue, 0, false);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            lock (LockObj)
            {
                base.OnGotFocus(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeSetFocus(handle);
                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            lock (LockObj)
            {
                base.OnLostFocus(e);
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeKillFocus(handle);
                }
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }
        }
        private static uint GetMouseFlags(MouseEventArgs e)
        {
            uint flags = 0;
            if (e.Button == MouseButtons.Left)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_LBUTTON;
            }
            if (e.Button == MouseButtons.Middle)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_MBUTTON;
            }
            if (e.Button == MouseButtons.Right)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_RBUTTON;
            }
            if (Control.ModifierKeys == Keys.Control)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_CONTROL;
            }
            if (Control.ModifierKeys == Keys.Shift)
            {
                flags = flags | (uint)wkeMouseFlags.WKE_SHIFT;
            }
            return flags;
        }

        public JsValue InvokeJS(string js)
        {

            return new JsValue(BlinkBrowserPInvoke.wkeRunJS(handle, Marshal.StringToCoTaskMemAnsi(js)), BlinkBrowserPInvoke.wkeGlobalExec(handle));
            //Marshal.SecureStringToGlobalAllocAnsi(js)
            //return new JsValue(EwePInvoke.wkeRunJS(handle, Marshal.StringToCoTaskMemAnsi(js)), EwePInvoke.wkeGlobalExec(handle));
        }
        public JsValue InvokeJSW(string js)
        {
            //var value = EwePInvoke.wkeRunJSW(handle, js);
            return new JsValue(BlinkBrowserPInvoke.wkeRunJSW(handle, js), BlinkBrowserPInvoke.wkeGlobalExec(handle));
            //return Marshal.PtrToStringUni(EwePInvoke.jsToString(EwePInvoke.wkeGlobalExec(handle), xc));
            //return new JsValue(xc, EwePInvoke.wkeGlobalExec(handle)).ToString();
            //return Marshal.PtrToStringAnsi(xc);
        }



        private static List<jsNativeFunction> jsnaviteList = new List<jsNativeFunction>();
        public void BindJsFunc()
        {
            var att = GlobalObjectJs.GetType().GetMethods();
            jsnaviteList.Clear();
            var result = new ArrayList();
            foreach (var item in att)
            {
                var xx = item.GetCustomAttributes(typeof(JSFunctin), true);
                if (xx != null && xx.Length != 0)
                {
                    var jsnav = new jsNativeFunction((es) =>
                    {
                        var xp = item.GetParameters();
                        var argcount = BlinkBrowserPInvoke.jsArgCount(es);
                        long param = 0L;
                        if (xp != null && xp.Length != 0 && argcount != 0)
                        {

                            object[] listParam = new object[BlinkBrowserPInvoke.jsArgCount(es)];
                            for (int i = 0; i < argcount; i++)
                            {
                                Type tType = xp[i].ParameterType;

                                var paramnow = BlinkBrowserPInvoke.jsArg(es, i);
                                param = paramnow;
                                if (tType == typeof(int))
                                {
                                    listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToInt(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(double))
                                {
                                    listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToDouble(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(float))
                                {
                                    listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToFloat(es, paramnow), tType);
                                }
                                else
                                if (tType == typeof(bool))
                                {
                                    listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToBoolean(es, paramnow), tType);
                                }
                                else
                                {
                                    listParam[i] = Convert.ChangeType((BlinkBrowserPInvoke.jsToString(es, paramnow)).IntptrToString(), tType);
                                }
                            }
                            try
                            {
                                item.Invoke(GlobalObjectJs, listParam);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            item.Invoke(GlobalObjectJs, null);
                        }
                        return param;
                    });
                    BlinkBrowserPInvoke.jsBindFunction(item.Name, jsnav, (uint)item.GetParameters().Length);
                    jsnaviteList.Add(jsnav);
                }
            }
        }

        public string JsGetValue { get; set; }

        [JSFunctin]
        public void Console_WriteLine(string msg)
        {
            MessageBox.Show("Console_WriteLine 方法被调用了：" + msg);
        }
        [JSFunctin]
        public void Console_WriteLine2(int msg, string msg2)
        {
            MessageBox.Show("Console_WriteLine w 方法被调用了：" + msg2 + " " + msg);
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
    public class JSFunctin : Attribute
    {
        public JSFunctin() { }
    }
}
