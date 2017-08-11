using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ByteView
{
	internal static class NativeMethods
	{
		// http://stackoverflow.com/a/1483963/2709212
		public static Bitmap ScreenPixel { get; private set; } = 
			new Bitmap(1, 1, PixelFormat.Format32bppArgb);


		[DllImport("user32.dll")]
		static extern bool GetCursorPos(ref Point lpPoint);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight,
			IntPtr hSrcDC, int xSrc, int ySec, int dwRop);

		public static Color GetColorAt(Point location)
		{
			using (var gdest = Graphics.FromImage(ScreenPixel))
			{
				using (var gsrc = Graphics.FromHwnd(IntPtr.Zero))
				{
					var hSrcDC = gsrc.GetHdc();
					var hDC = gdest.GetHdc();
					int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y,
						(int)CopyPixelOperation.SourceCopy);
					gdest.ReleaseHdc();
					gsrc.ReleaseHdc();
				}
			}

			return ScreenPixel.GetPixel(0, 0);
		}
	}
}
