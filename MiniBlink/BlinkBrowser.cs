
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static MiniBlinkPinvoke.BlinkBrowserPInvoke;

namespace MiniBlinkPinvoke
{
    public class BlinkBrowser : Control
    {


        string CookiePath { get; set; }

        public IntPtr handle = IntPtr.Zero;
        string url = string.Empty;

        IntPtr bits = IntPtr.Zero;
        static IntPtr jsHandle = IntPtr.Zero;
        Size oldSize;
        public object GlobalObjectJs = new object();


        public event TitleChangedCallback OnTitleChangeCall;

        public delegate void URLChange(string url);
        public event URLChange OnUrlChangeCall;


        public delegate void DocumentReady();
        public event DocumentReady DocumentReadyCallback;



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
        static wkeLoadUrlEndCallback _wkeLoadUrlEndCallback;

        void OnwkeLoadUrlEndCallback(IntPtr webView, IntPtr param, string url, IntPtr job, IntPtr buf, int len)
        {
            Console.WriteLine("call OnwkeLoadUrlEndCallback url:" + url);
            //  Marshal.Release(buf);
        }

   
        IntPtr OnwkeCreateViewCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url)
        {
            Console.WriteLine("OnwkeCreateViewCallback navigationType:" + navigationType);
            return webView;
        }
        bool OnwkeDownloadFileCallback(IntPtr webView, IntPtr param, string url)
        {
            Console.WriteLine("call OnwkeDownloadFileCallback:" + (url));
            return false;
        }

        void OnwkeLoadingFinishCallback(IntPtr webView, IntPtr param, IntPtr url, wkeLoadingResult result, IntPtr failedReason)
        {
            Console.WriteLine("call OnwkeLoadingFinishCallback result:" + result);
        }
        void OnwkeDocumentReadyCallback(IntPtr webView, IntPtr param)
        {
            Console.WriteLine("call OnwkeDocumentReadyCallback:" + Marshal.PtrToStringUni(param));//.Utf8IntptrToString());
            DocumentReadyCallback?.Invoke();

        }
        void OnwkeConsoleMessageCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, int sourceLine, IntPtr stackTrace)
        {
            Console.WriteLine("Console Msg:" + message);
        }
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        public BlinkBrowser()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
            UpdateStyles();


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

            //GlobalObjectJs = this;
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
                    }

                }
            }
        }

        void OnTitleChangedCallback(IntPtr webView, IntPtr param, IntPtr title)
        {
            OnTitleChangeCall?.Invoke(webView, param, title);
        }
        bool OnwkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url)
        {

            IntPtr urlPtr = BlinkBrowserPInvoke.wkeGetStringW(url);
            Console.WriteLine(navigationType);

            Console.WriteLine("OnwkeNavigationCallback:URL:" + Marshal.PtrToStringUni(urlPtr));
            return true;
        }

        void OnUrlChangedCallback(IntPtr webView, IntPtr param, IntPtr url)
        {
            Console.WriteLine("OnUrlChangedCallback:URL:" + wkeGetStringW(url));
        }
        void OnTitleChangeCallback(IntPtr webView, IntPtr param, IntPtr title)
        {
            Console.WriteLine(Marshal.PtrToStringUni(title));
        }

        void OnWkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy)
        {
            Invalidate(new Rectangle(x, y, cx, cy), false);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                BlinkBrowserPInvoke.wkeInitialize();

                handle = BlinkBrowserPInvoke.wkeCreateWebView();
                BlinkBrowserPInvoke.wkeSetCookieEnabled(handle, true);
                CookiePath = Application.StartupPath + "\\cookie\\";
                if (!Directory.Exists(CookiePath))
                {
                    Directory.CreateDirectory(CookiePath);
                }
                BlinkBrowserPInvoke.wkeSetCookieJarPath(handle, CookiePath);

                BlinkBrowserPInvoke.wkeResize(handle, Width, Height);
                BlinkBrowserPInvoke.wkeSetUserAgentW(this.handle, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.75 Safari/537.36");


                AlertBoxCallback = new AlertBoxCallback((a, b, c) =>
                {
                    MessageBox.Show(Marshal.PtrToStringUni(wkeToStringW(c)), Url + "提示");
                });
                BlinkBrowserPInvoke.wkeOnAlertBox(handle, AlertBoxCallback, IntPtr.Zero);

                _wkeNavigationCallback = OnwkeNavigationCallback;
                BlinkBrowserPInvoke.wkeOnNavigation(handle, _wkeNavigationCallback, IntPtr.Zero);
                listObj.Add(_wkeNavigationCallback);


                titleChangeCallback = OnTitleChangedCallback;
                BlinkBrowserPInvoke.wkeOnTitleChanged(this.handle, titleChangeCallback, IntPtr.Zero);
                listObj.Add(titleChangeCallback);

                _wkeDocumentReadyCallback = OnwkeDocumentReadyCallback;
                BlinkBrowserPInvoke.wkeOnDocumentReady(this.handle, _wkeDocumentReadyCallback, IntPtr.Zero);
                listObj.Add(_wkeDocumentReadyCallback);

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

                _wkeDownloadFileCallback = OnwkeDownloadFileCallback;
                BlinkBrowserPInvoke.wkeOnDownload(this.handle, _wkeDownloadFileCallback, IntPtr.Zero);
                listObj.Add(_wkeDownloadFileCallback);

                _wkeCreateViewCallback = OnwkeCreateViewCallback;
                BlinkBrowserPInvoke.wkeOnCreateView(this.handle, _wkeCreateViewCallback, handle);
                listObj.Add(_wkeCreateViewCallback);

                _wkeLoadUrlEndCallback = OnwkeLoadUrlEndCallback;
                BlinkBrowserPInvoke.wkeOnLoadUrlEnd(this.handle, _wkeLoadUrlEndCallback, handle);
                listObj.Add(_wkeLoadUrlEndCallback);

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
            if (DesignMode)
            {
                e.Graphics.DrawString("MiniBlinkBrowser", this.Font, Brushes.Red, new Point());
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }

        void SetCursors()
        {
            Console.WriteLine("wkeGetCursorInfoType:  " + BlinkBrowserPInvoke.wkeGetCursorInfoType(handle));
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
                    Cursor = Cursors.SizeWE;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthResize:
                    Cursor = Cursors.SizeNS;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthEastResize:
                    Cursor = Cursors.SizeNESW;
                    break;
                case WkeCursorInfo.WkeCursorInfoNorthWestResize:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthResize:
                    Cursor = Cursors.SizeWE;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthEastResize:
                    Cursor = Cursors.SizeNWSE;
                    break;
                case WkeCursorInfo.WkeCursorInfoSouthWestResize:
                    Cursor = Cursors.SizeNESW;
                    break;
                case WkeCursorInfo.WkeCursorInfoWestResize:
                    Cursor = Cursors.SizeWE;
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

        /// <summary>
        /// 修复加载后点击没焦点导致操作无反映问题
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            base.OnClick(e);
        }

        public float _ZoomFactor { get; set; }
        [Browsable(false), DefaultValue(1)]
        public float ZoomFactor
        {
            get
            {
                if (this.handle == IntPtr.Zero)
                {
                    return this._ZoomFactor;
                }
                return MiniBlinkPinvoke.BlinkBrowserPInvoke.wkeGetZoomFactor(this.handle);
            }
            set
            {
                this._ZoomFactor = value;
                if (this.handle != IntPtr.Zero)
                {
                    MiniBlinkPinvoke.BlinkBrowserPInvoke.wkeSetZoomFactor(this.handle, value);
                }
            }
        }
        public string JsGetValue { get; set; }

   

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        public string Url
        {
            get => url;
            set
            {
                url = value;
                if (handle != IntPtr.Zero)
                {
                    BlinkBrowserPInvoke.wkeLoadURLW(handle, url);
                }
            }
        }
    }



}
