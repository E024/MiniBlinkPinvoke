using System;
using System.Runtime.InteropServices;

namespace MiniBlinkPinvoke.Core
{
    public static class GraphicsWrapper
    {
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
        public static bool CopyTo(this System.Drawing.Graphics gr1, System.Drawing.Graphics gr2, System.Drawing.Rectangle rect)
        {
            bool result = false;
            var graaa1 = gr2.GetHdc();
            var graaa2 = gr1.GetHdc();
            result = BitBlt(graaa1, rect.X, rect.Y, rect.Width, rect.Height, graaa2, rect.X, rect.Y, 0x00CC0020);
            gr1.ReleaseHdc(graaa2);
            gr2.ReleaseHdc(graaa1);
            return result;
        }
    }
}
