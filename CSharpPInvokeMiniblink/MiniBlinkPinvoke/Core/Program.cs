using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MiniBlinkPinvoke
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //WKE_SETTING_PROXY = 1,
            //var x = 1 << 2;
            Application.SetCompatibleTextRenderingDefault(false);
            MiniBlinkPinvoke.BlinkBrowserPInvoke.ResourceAssemblys.Add("MiniBlinkPinvoke", System.Reflection.Assembly.GetExecutingAssembly());
            Application.Run(new Form1());
        }
    }
}
