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
            //blinkBrowser1.handle
            //    EwePInvoke.wkeLoadURLW(blinkBrowser1.handle, "https://www.baidu.com/");E
            //   EwePInvoke.wkeLoadW(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
            // EwePInvoke.wkeLoadURL(blinkBrowser1.handle, "file:///MiniBlinkPinvoke/index.html");
            EwePInvoke.wkeLoadURLW(blinkBrowser1.handle, "file:///F:/Project/CSharpPInvokeMiniblink/MiniBlinkPinvoke/index.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(EwePInvoke.wkeGetVersionString());
            //Console.WriteLine(blinkBrowser1.InvokeJSW("return document.body.innerHTML").ToString());
        }


    }
}
