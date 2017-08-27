using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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
            // BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "https://www.baidu.com/");
            //   EwePInvoke.wkeLoadW(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
            // EwePInvoke.wkeLoadURL(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
           BlinkBrowserPInvoke.wkeLoadURLW(blinkBrowser1.handle, "file:///F:/Project/CSharpPInvokeMiniblink/MiniBlinkPinvoke/index.html");
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
           MessageBox.Show(blinkBrowser1.InvokeJS("return testInt()").ToFloat()+"");


            //var hax= BlinkBrowserPInvoke.wkeRunJS(blinkBrowser1.handle, Marshal.StringToHGlobalAnsi("return testInt();"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
