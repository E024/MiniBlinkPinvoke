
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        Timer timer = new Timer { Interval = 25 };
        string url = string.Empty;

        IntPtr bits = IntPtr.Zero;
        static IntPtr jsHandle = IntPtr.Zero;
        Size oldSize;
        public object GlobalObjectJs = new object();

        //public event wkeDocumentReadyCallback DocumentReadyCallback;

        static UrlChangedCallback urlChangedCallback;
        static AlertBoxCallback AlertBoxCallback;
        static TitleChangedCallback titleChangeCallback;
        public BlinkBrowser()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            GlobalObjectJs = this;
        }

        void OnUrlChangedCallback(IntPtr webView, IntPtr param, IntPtr url)
        {
            Console.WriteLine(Marshal.PtrToStringAuto(url));
        }
        void OnTitleChangeCallback(IntPtr webView, IntPtr param, IntPtr title)
        {
            //Console.WriteLine(Marshal.PtrToStringAnsi(title));
        }
        static wkeDocumentReadyCallback wkeDocumentReadyCallback;
        jsNativeFunction jsnav;
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                timer.Tick += Timer_Tick;
                timer.Start();
                BlinkBrowserPInvoke.wkeInitialize();
                handle = BlinkBrowserPInvoke.wkeCreateWebView();
                BlinkBrowserPInvoke.wkeResize(handle, Width, Height);
                //BindJsFunc();
                AlertBoxCallback = new AlertBoxCallback((a, b) =>
                {
                    MessageBox.Show(Marshal.PtrToStringAuto(b), "alert 调用");
                });
                BlinkBrowserPInvoke.wkeOnAlertBox(handle, AlertBoxCallback);

                //IntPtr param = IntPtr.Zero;

                //urlChangedCallback = new UrlChangedCallback(OnUrlChangedCallback);
                //BlinkBrowserPInvoke.wkeOnURLChanged(handle, urlChangedCallback,IntPtr.Zero);


                //titleChangeCallback = new TitleChangedCallback(OnTitleChangeCallback);
                //BlinkBrowserPInvoke.wkeOnTitleChanged(handle, titleChangeCallback, this.Handle);

                //EwePInvoke.wkeSetUserAgentW(handle, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                jsnav = new jsNativeFunction((es) =>
               {
                   var count = BlinkBrowserPInvoke.jsArgCount(es);//返回0？？？
                   MessageBox.Show(count.ToString());
                   var paramnow = BlinkBrowserPInvoke.jsArg(es, 1);
                   Console_WriteLine(Marshal.PtrToStringAnsi(BlinkBrowserPInvoke.jsToString(es, paramnow)));
                   return paramnow;
               });
                BlinkBrowserPInvoke.jsBindFunction(("Console_WriteLine"), jsnav, 1);
                // BlinkBrowserPInvoke.jsBindFunction(("Console_WriteLine"), jsnav, 2);

            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (handle != IntPtr.Zero && BlinkBrowserPInvoke.wkeIsDirty(handle))
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
            if (DesignMode)
            {
                e.Graphics.DrawString("MiniBlinkBrowser", this.Font, Brushes.Red, new Point());
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));
            }
            Application.DoEvents();
            GC.Collect();
        }
        void SetCursors()
        {
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
        //public void BindJsFunc()
        //{
        //    var att = GlobalObjectJs.GetType().GetMethods();
        //    jsnaviteList.Clear();
        //    var result = new ArrayList();
        //    foreach (var item in att)
        //    {
        //        var xx = item.GetCustomAttributes(typeof(JSFunctin), true);
        //        if (xx != null && xx.Length != 0)
        //        {
        //            var jsnav = new jsNativeFunction((es) =>
        //            {
        //                var xp = item.GetParameters();
        //                var argcount = BlinkBrowserPInvoke.jsArgCount(es);
        //                if (xp != null && xp.Length != 0 && argcount != 0)
        //                {

        //                    object[] listParam = new object[BlinkBrowserPInvoke.jsArgCount(es)];
        //                    for (int i = 0; i < argcount; i++)
        //                    {
        //                        Type tType = xp[i].ParameterType;

        //                        var paramnow = BlinkBrowserPInvoke.jsArg(es, i);

        //                        if (tType == typeof(int))
        //                        {
        //                            listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToInt(es, paramnow), tType);
        //                        }
        //                        else
        //                        if (tType == typeof(double))
        //                        {
        //                            listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToDouble(es, paramnow), tType);
        //                        }
        //                        else
        //                        if (tType == typeof(float))
        //                        {
        //                            listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToFloat(es, paramnow), tType);
        //                        }
        //                        else
        //                        if (tType == typeof(bool))
        //                        {
        //                            listParam[i] = Convert.ChangeType(BlinkBrowserPInvoke.jsToBoolean(es, paramnow), tType);
        //                        }
        //                        else
        //                        {
        //                            listParam[i] = Convert.ChangeType(Marshal.PtrToStringUni(BlinkBrowserPInvoke.jsToString(es, paramnow)), tType);
        //                        }

        //                    }

        //                    try
        //                    {
        //                        item.Invoke(GlobalObjectJs, listParam);
        //                    }
        //                    catch (Exception)
        //                    {
        //                    }
        //                }
        //                else
        //                {
        //                    item.Invoke(GlobalObjectJs, null);
        //                }
        //                return 0L;
        //            });
        //            BlinkBrowserPInvoke.jsBindFunction(Marshal.StringToCoTaskMemAnsi(item.Name), jsnav, (uint)item.GetParameters().Length);
        //            jsnaviteList.Add(jsnav);
        //        }
        //    }
        //}
        [JSFunctin]
        public void Console_WriteLine(string msg)
        {
            MessageBox.Show("Console_WriteLine 方法被调用了：" + msg);
        }
    }
    public class JSFunctin : Attribute
    {
        public JSFunctin() { }
    }
}
