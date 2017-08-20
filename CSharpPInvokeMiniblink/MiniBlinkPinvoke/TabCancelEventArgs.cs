using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniBlinkPinvoke
{
    public class TabCancelEventArgs : CancelEventArgs
    {
        public readonly TabPage Tab;

        public TabCancelEventArgs(TabPage tab)
        {
            this.Tab = tab;
        }
    }
}
