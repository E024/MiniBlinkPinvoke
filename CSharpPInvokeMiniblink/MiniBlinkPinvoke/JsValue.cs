using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniBlinkPinvoke
{
    public class JsValue
    {
        private IntPtr intptr_0;
        private long long_0;

        public JsValue(long value, IntPtr jsExecState)
        {
            this.long_0 = value;
            this.intptr_0 = jsExecState;
        }

        public JsValue(IntPtr jsExecState, int argIndex)
        {
            this.intptr_0 = jsExecState;
            this.long_0 = BlinkBrowserPInvoke.jsArg(this.intptr_0, argIndex);
        }

        public static int ArgCount(IntPtr jsExecState)
        {
            return BlinkBrowserPInvoke.jsArgCount(jsExecState);
        }

        public static long JsDouble(IntPtr jsExecState, double d)
        {
            return BlinkBrowserPInvoke.jsDouble(jsExecState, d);
        }

        public static long JsFalse(IntPtr jsExecState)
        {
            return BlinkBrowserPInvoke.jsFalse(jsExecState);
        }

        public static long JsFloat(IntPtr jsExecState, float f)
        {
            return BlinkBrowserPInvoke.jsFloat(jsExecState, f);
        }

        public static long JsInt(IntPtr jsExecState, int n)
        {
            return BlinkBrowserPInvoke.jsInt(jsExecState, n);
        }

        public static long JsNull(IntPtr jsExecState)
        {
            return BlinkBrowserPInvoke.jsNull(jsExecState);
        }

        public static long JsString(IntPtr jsExecState, string str)
        {
            return BlinkBrowserPInvoke.jsStringW(jsExecState, str);
        }

        public static long JsTrue(IntPtr jsExecState)
        {
            return BlinkBrowserPInvoke.jsTrue(jsExecState);
        }

        public static long JsUndefined(IntPtr jsExecState)
        {
            return BlinkBrowserPInvoke.jsUndefined(jsExecState);
        }

        public bool ToBool()
        {
            return BlinkBrowserPInvoke.jsToBoolean(this.intptr_0, this.long_0);
        }

        public double ToDouble()
        {
            return BlinkBrowserPInvoke.jsToDouble(this.intptr_0, this.long_0);
        }

        public float ToFloat()
        {
            return BlinkBrowserPInvoke.jsToFloat(this.intptr_0, this.long_0);
        }

        public int ToInt()
        {
            return BlinkBrowserPInvoke.jsToInt(this.intptr_0, this.long_0);
        }

        public override string ToString()
        {
            if (this.intptr_0 == IntPtr.Zero)
            {
                return string.Empty;
            }
            return BlinkBrowserPInvoke.jsToString(this.intptr_0, this.long_0).IntptrToString();
        }

        public wkeJSType JsType
        {
            get
            {
                return BlinkBrowserPInvoke.jsTypeOf(this.intptr_0, this.long_0);
            }
        }

        public long Value
        {
            get
            {
                return this.long_0;
            }
        }
    }

}
