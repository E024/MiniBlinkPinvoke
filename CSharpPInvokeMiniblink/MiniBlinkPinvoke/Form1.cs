﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MiniBlinkPinvoke;

namespace MiniBlinkPinvoke
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////blinkBrowser1.handle
            //EwePInvoke.wkeLoadURLW(blinkBrowser1.handle, "http://www.w3school.com.cn/tiy/t.asp?f=jseg_isNaN");
            //BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "https://news.cnblogs.com/n/577094/");
            //   EwePInvoke.wkeLoadW(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
            // EwePInvoke.wkeLoadURL(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
           BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "file:///E:/Project/CSharpPInvokeMiniblink/MiniBlinkPinvoke/index.html");
            //EwePInvoke.wkeLoadURLW(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
            //BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "https://www.baidu.com/");
     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(BlinkBrowserPInvoke.wkeGetVersionString());
            //Console.WriteLine(blinkBrowser1.InvokeJSW("return document.body.innerHTML").ToString());
            //EwePInvoke.wkeLoadURL(blinkBrowser1.handle, Marshal.StringToHGlobalUni("file:///MiniBlinkPinvoke/index.html"));
            //EwePInvoke.wkeLoadFileW(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");

            //MessageBox.Show(blinkBrowser1.InvokeJSW("testString();").ToString());
            //MessageBox.Show(blinkBrowser1.InvokeJS("return testInt()").ToFloat() + "");

            //Console.WriteLine(BlinkBrowserPInvoke.wkeCreateStringW("123", "123".Length).IntptrToString());
            //MessageBox.Show(BlinkBrowserPInvoke.wkeGetCookie(blinkBrowser1.handle).IntptrToString());
            //MessageBox.Show(BlinkBrowserPInvoke.wkeGetCookieW(blinkBrowser1.handle));
            BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "http://tests/getsource");
            //Console.WriteLine(blinkBrowser1.InvokeJSW("return document.cookie").ToString());
            //var hax= BlinkBrowserPInvoke.wkeRunJS(blinkBrowser1.handle, Marshal.StringToHGlobalAnsi("return testInt();"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void blinkBrowser1_OnTitleChangeCall(IntPtr webView, IntPtr param, IntPtr title)
        {
            Text = BlinkBrowserPInvoke.wkeGetString(title).IntptrToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "https://news.cnblogs.com/n/577093/");
        }
    }
}
