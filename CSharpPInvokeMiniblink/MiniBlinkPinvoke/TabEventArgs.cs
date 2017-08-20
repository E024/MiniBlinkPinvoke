using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniBlinkPinvoke
{
    public class TabEventArgs : EventArgs
    {
        public readonly TabPage Tab;

        public TabEventArgs(TabPage tab)
        {
            this.Tab = tab;
        }
    }
}