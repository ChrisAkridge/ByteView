// Drawer.cs
//
// Takes files as byte arrays and converts them to bitmapped images.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ByteView
{
	/// <summary>
	/// Draws bitmaps using arbitary files or raw byte arrays.
	/// </summary>
	public static class Drawer
	{
		/// <summary>
		/// The number of colors in a 1 bit per pixel color palette.
		/// </summary>
		private const int NumberOf1BppColors = 2;

		/// <summary>
		/// The number of colors in a 2 bit per pixel color palette.
		/// </summary>
		private const int NumberOf2BppColors = 4;

		/// <summary>
		/// The number of colors in a 4 bit per pixel color palette.
		/// </summary>
		private const int NumberOf4BppColors = 16;

		/// <summary>
		/// The number of colors in an 8 bit per pixel color palette.
		/// </summary>
		private const int NumberOf8BppColors = 256;

		/// <summary>
		/// The number of colors in a 16 bit per pixel color palette.
		/// </summary>
		private const int NumberOf16BppColors = 65536;

		/// <summary>
		/// Draws a bitmap from a group of files.
		/// </summary>
		/// <param name="source">The group of files.</param>
		/// <param name="bitDepth">The bits per pixel of the resulting bitmap.</param>
		/// <param name="palette">
		/// The palette to use for the resulting bitmap. Pass null if the bit
		/// depth is 24 or 32 bits per pixel.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// A bitmap containing pixels made from all the bytes of all the files
		/// in the group.
		/// </returns>
		public static Bitmap Draw(FileSource source, BitDepth bitDepth, int[] palette,
			BackgroundWorker worker)
		{
			if (source == null || source.FilePaths.Count == 0)
			{
				throw new ArgumentException("The provided file source was null or had no files.", nameof(source));
			}
			if (bitDepth < 0 || (int)bitDepth > 7)
			{
				throw new ArgumentOutOfRangeException(nameof(bitDepth),
					$"The provided bit depth was not a valid value. Expected a value between 0 and 7, got {bitDepth}.");
			}
			if (worker == null)
			{
				throw new ArgumentNullException(nameof(worker), "The provided BackgroundWorker was null.");
			}

			byte[] sourceBytes = source.GetFiles();

			return Draw(sourceBytes, bitDepth, palette, worker);
		}

		/// <summary>
		/// Draws a bitmap of a certain size from a byte array.
		/// </summary>
		/// <param name="source">The byte array.</param>
		/// <param name="bitDepth">The number of bits per pixel in the bitmap.</param>
		/// <param name="palette">
		/// The palette used to draw the image. Pass null for 24 or 32 bit per
		/// pixel images.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <param name="imageSize">The desired size of the bitmap.</param>
		/// <returns>
		/// A bitmap, sized at most to be the desired size, containing pixels
		/// made from some or all of the bytes in the array.
		/// </returns>
		/// <remarks>
		/// If there aren't enough bytes to fill enough pixels to fill the
		/// desired size, all remaining pixels will have all-bits-zero (in
		/// paletted modes, all-bits-zero pixels use the first color in the
		/// provided palette). If there are too many bytes, the image will only
		/// display enough bytes to fill the image.
		/// </remarks>
		public static Bitmap Draw(byte[] source, BitDepth bitDepth, int[] palette,
			BackgroundWorker worker, Size imageSize)
		{
			if (source == null || source.Length == 0)
			{
				throw new ArgumentException("The provided source bytes were null or empty.",
					nameof(source));
			}
			if (bitDepth < 0 || (int)bitDepth > 7)
			{
				throw new ArgumentOutOfRangeException(nameof(bitDepth),
					$"The provided bit depth was not a valid value. Expected a value between 0 and 7, got {bitDepth}.");
			}
			if (worker == null)
			{
				throw new ArgumentNullException(nameof(worker),
					"The provided BackgroundWorker was null.");
			}
			if (imageSize.Width == 0 || imageSize.Height == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(imageSize),
					"The provided image size has a width or height of 0 pixels.");
			}

			int paletteSize = (palette != null) ? palette.Length : 0;

			switch (bitDepth)
			{
				case BitDepth.Invalid:
					throw new InvalidOperationException("Cannot draw a bitmap using an invalid bit depth.");
				case BitDepth.OneBpp:
					ValidatePaletteSize(NumberOf1BppColors, paletteSize);
					return ToBitmap(Create1BppImage(source, palette, worker), worker, imageSize);
				case BitDepth.TwoBpp:
					ValidatePaletteSize(NumberOf2BppColors, paletteSize);
					return ToBitmap(Create2BppImage(source, palette, worker), worker, imageSize);
				case BitDepth.FourBpp:
					ValidatePaletteSize(NumberOf4BppColors, paletteSize);
					return ToBitmap(Create4BppImage(source, palette, worker), worker, imageSize);
				case BitDepth.EightBpp:
					ValidatePaletteSize(NumberOf8BppColors, paletteSize);
					return ToBitmap(Create8BppImage(source, palette, worker), worker, imageSize);
				case BitDepth.SixteenBpp:
					ValidatePaletteSize(NumberOf16BppColors, paletteSize);
					return ToBitmap(Create16BppImage(source, palette, worker), worker, imageSize);
				case BitDepth.TwentyFourBpp:
					return ToBitmap(Create24BppImage(source, worker), worker, imageSize);
				case BitDepth.ThirtyTwoBpp:
					return ToBitmap(Create32BppImage(source, worker), worker, imageSize);
				default:
					throw new ArgumentException($"Provided bit depth {bitDepth} was not valid.",
						nameof(bitDepth));
			}
		}

		/// <summary>
		/// Draws a bitmap from a byte array.
		/// </summary>
		/// <param name="bytes">The byte array.</param>
		/// <param name="bitDepth">The number of bits per pixel.</param>
		/// <param name="palette">
		/// The palette used to draw the image. Pass null for 24 and 32 bit per
		/// pixel bitmaps.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>A bitmap containing pixels from all bytes in the array.</returns>
		private static Bitmap Draw(byte[] bytes, BitDepth bitDepth, int[] palette,
			BackgroundWorker worker)
		{
			int paletteSize = ((int)bitDepth < 6) ? palette.Length : 0; // not 24 or 32bpp
			switch (bitDepth)
			{
				case BitDepth.Invalid:
					throw new InvalidOperationException("Cannot draw a bitmap using an invalid bit depth.");
				case BitDepth.OneBpp:
					ValidatePaletteSize(NumberOf1BppColors, paletteSize);
					return ToBitmap(Create1BppImage(bytes, palette, worker), worker);
				case BitDepth.TwoBpp:
					ValidatePaletteSize(NumberOf2BppColors, paletteSize);
					return ToBitmap(Create2BppImage(bytes, palette, worker), worker);
				case BitDepth.FourBpp:
					ValidatePaletteSize(NumberOf4BppColors, paletteSize);
					return ToBitmap(Create4BppImage(bytes, palette, worker), worker);
				case BitDepth.EightBpp:
					ValidatePaletteSize(NumberOf8BppColors, paletteSize);
					return ToBitmap(Create8BppImage(bytes, palette, worker), worker);
				case BitDepth.SixteenBpp:
					ValidatePaletteSize(NumberOf16BppColors, paletteSize);
					return ToBitmap(Create16BppImage(bytes, palette, worker), worker);
				case BitDepth.TwentyFourBpp:
					return ToBitmap(Create24BppImage(bytes, worker), worker);
				case BitDepth.ThirtyTwoBpp:
					return ToBitmap(Create32BppImage(bytes, worker), worker);
				default:
					throw new ArgumentException($"The provided bit depth {bitDepth} was not valid.");
			}
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 1 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="palette">
		/// A palette describing the colors to use for each bit.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 1 bit
		/// per pixel image.
		/// </returns>
		/// <remarks>Each input byte expands to 8 pixels or 32 bytes.</remarks>
		private static int[] Create1BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
		{
			int[] image = new int[bytes.Length * 8];

			int pixelIndex = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				for (int bitIndex = 0; bitIndex < 8; bitIndex++)
				{
					byte bit = (byte)(bytes[i] & (1 << bitIndex));
					if (bit == 0)
					{
						image[pixelIndex] = palette[0];
					}
					else
					{
						image[pixelIndex] = palette[1];
					}
					pixelIndex++;
				}

				if (i % (bytes.Length / 100m) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}
					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 2 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="palette">
		/// A palette describing the colors to use for each bit.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 2 bit
		/// per pixel image.
		/// </returns>
		/// <remarks>Each input byte expands to 4 pixels or 16 bytes.</remarks>
		private static int[] Create2BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
		{
			int[] image = new int[bytes.Length * 4];

			int pixelIndex = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int mask = (3 << (j * 2));
					byte colorIndex = (byte)((bytes[i] & mask) >> (j * 2));
					image[pixelIndex] = palette[colorIndex];
					pixelIndex++;
				}

				if (i % (bytes.Length / 100m) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}
					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 4 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="palette">
		/// A palette describing the colors to use for each bit.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 4 bit
		/// per pixel image.
		/// </returns>
		/// <remarks>Each input byte expands to 2 pixels or 8 bytes.</remarks>
		private static int[] Create4BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
		{
			int[] image = new int[bytes.Length * 2];
			int pixelIndex = 0;

			for (int i = 0; i < bytes.Length; i++)
			{
				int high = (bytes[i] & 0xF0) >> 4;
				int low = (bytes[i] & 0x0F);

				image[pixelIndex] = palette[high];
				image[pixelIndex + 1] = palette[low];
				pixelIndex += 2;

				if (i % (bytes.Length / 100) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}
					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		private static int[] Create8BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
		{
			int[] image = new int[bytes.Length];

			for (int i = 0; i < bytes.Length; i++)
			{
				image[i] = palette[bytes[i]];

				if (i % (bytes.Length / 100) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}

					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 16 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="palette">
		/// A palette describing the colors to use for each bit.
		/// </param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 16 bit
		/// per pixel image.
		/// </returns>
		private static int[] Create16BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
		{
			int[] image;
			if (bytes.Length % 2 == 0)
			{
				image = new int[bytes.Length / 2];
			}
			else
			{
				image = new int[bytes.Length / 2 + 1];
			}

			int pixelIndex = 0;
			for (int i = 0; i < bytes.Length; i += 2)
			{
				int value;
				if (i + 1 < bytes.Length)
				{
					value = (bytes[i] << 8) + bytes[i + 1];
				}
				else
				{
					value = (bytes[i] << 8);
				}

				image[pixelIndex] = palette[value];
				pixelIndex++;

				if (i % (bytes.Length / 100) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}

					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 24 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 24 bit
		/// per pixel image.
		/// </returns>
		private static int[] Create24BppImage(byte[] bytes, BackgroundWorker worker)
		{
			int[] image;
			if (bytes.Length % 3 == 0)
			{
				image = new int[bytes.Length / 3];
			}
			else
			{
				int pixelCount = bytes.Length / 3;
				while (pixelCount % 3 != 0)
				{
					pixelCount++;
				}
				image = new int[pixelCount];
			}

			for (int i = 0; i < image.Length; i++)
			{
				byte red = (byte)((i * 3 < bytes.Length) ? bytes[i * 3] : 0);
				byte green = (byte)((i * 3 + 1 < bytes.Length) ? bytes[i * 3 + 1] : 0);
				byte blue = (byte)((i * 3 + 2 < bytes.Length) ? bytes[i * 3 + 2] : 0);
				image[i] = (255 << 24) + (red << 16) + (green << 8) + blue;

				if (i % (bytes.Length / 100) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}

					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Creates an array of 32-bit integers as ARGB values which represent
		/// the pixels of a 32 bit per pixel image made from an array of bytes.
		/// </summary>
		/// <param name="bytes">The array of bytes.</param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>
		/// An array of 32-bit integers as ARGB values which represent a 32 bit
		/// per pixel image.
		/// </returns>
		private static int[] Create32BppImage(byte[] bytes, BackgroundWorker worker)
		{
			int[] image;
			if (bytes.Length % 4 == 0)
			{
				image = new int[bytes.Length / 4];
			}
			else
			{
				int pixelCount = bytes.Length / 4;
				while (pixelCount % 4 != 0)
				{
					pixelCount++;
				}
				image = new int[pixelCount];
			}

			for (int i = 0; i < image.Length; i++)
			{
				byte alpha = (byte)((i * 4 < bytes.Length) ? bytes[i * 4] : 0);
				byte red = (byte)((i * 4 + 1 < bytes.Length) ? bytes[i * 4 + 1] : 0);
				byte green = (byte)((i * 4 + 2 < bytes.Length) ? bytes[i * 4 + 2] : 0);
				byte blue = (byte)((i * 4 + 3 < bytes.Length) ? bytes[i * 4 + 3] : 0);
				image[i] = (alpha << 24) + (red << 16) + (green << 8) + blue;

				if (i % (bytes.Length / 100) == 0)
				{
					if (worker.CancellationPending)
					{
						break;
					}

					worker.ReportProgress((int)((i * 100m) / bytes.Length));
				}
			}

			return image;
		}

		/// <summary>
		/// Checks if an integer's square root is an integer.
		/// </summary>
		/// <param name="n">The integer to check.</param>
		/// <returns>
		/// True if <paramref name="n" /> is a perfect square, false if it is not.
		/// </returns>
		private static bool IsPerfectSquare(int n)
		{
			if (n < 1) { return false; }

			int squareRoot = (int)Math.Sqrt(n);
			return (squareRoot * squareRoot) == n;
		}

		/// <summary>
		/// Returns the smallest size that contains at least a certain number of pixels.
		/// </summary>
		/// <param name="pixelCount">
		/// The number of pixels to generate a size for.
		/// </param>
		/// <returns>
		/// The smallest size that contains at least <paramref name="pixelCount"
		/// /> pixels.
		/// </returns>
		private static Size GetImageSize(int pixelCount)
		{
			int squareRoot = (int)Math.Sqrt(pixelCount);
			Size result;
			if (IsPerfectSquare(pixelCount))
			{
				result = new Size(squareRoot, squareRoot);
			}
			else
			{
				int height = squareRoot;
				int remainder = pixelCount - squareRoot * squareRoot;
				int remainderRows = (int)Math.Ceiling((double)remainder / squareRoot);
				while (remainderRows > 0)
				{
					height++;
					remainderRows--;
				}
				result = new Size(squareRoot, height);
			}
			return result;
		}

		/// <summary>
		/// Sorts all the pixels of a bitmap from lowest ARGB value to highest.
		/// </summary>
		/// <param name="bitmap">The bitmap to sort.</param>
		/// <returns>The sorted bitmap.</returns>
		/// <remarks>
		/// Sort order is determined by the value of each pixel, which is
		/// interpreted as a 32-bit signed integer in ARGB order. Generally,
		/// darker colors appear before brighter colors.
		/// </remarks>
		public static Bitmap Sort(Bitmap bitmap)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException(nameof(bitmap), "The provided bitmap was null.");
			}

			int[] pixels = ToPixels(bitmap);
			Array.Sort(pixels);
			using (var worker = new BackgroundWorker()
			{ WorkerReportsProgress = true, WorkerSupportsCancellation = true })
			{
				return ToBitmap(pixels, worker, new Size(bitmap.Width, bitmap.Height));
			}
		}

		/// <summary>
		/// Generates a bitmap containing only the unique colors from another bitmap.
		/// </summary>
		/// <param name="bitmap">
		/// The bitmap from which to generate the resulting bitmap.
		/// </param>
		/// <param name="colorCount">
		/// An out param that returns a formatted string stating the total number
		/// of colors in the resulting bitmap.
		/// </param>
		/// <returns>
		/// A bitmap of all the unique colors in <paramref name="bitmap" />.
		/// </returns>
		public static Bitmap UniqueColors(Bitmap bitmap, out string colorCount)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException(nameof(bitmap), "The provided bitmap was null.");
			}

			int[] pixels = ToPixels(bitmap);
			pixels = pixels.Distinct().ToArray();
			colorCount = string.Format("{0} unique colors", pixels.Length);
			using (var worker = new BackgroundWorker()
			{ WorkerReportsProgress = true, WorkerSupportsCancellation = true })
			{
				return ToBitmap(pixels, worker);
			}
		}

		/// <summary>
		/// Converts a bitmap image into a 32-bit-per pixel RGBu array where the
		/// u component is unused.
		/// </summary>
		/// <param name="bitmap">The bitmap to convert into a byte array.</param>
		/// <returns>A byte array made from the bitmap.</returns>
		public static byte[] BitmapToByteArray(Bitmap bitmap)
		{
			BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
			byte[] result = new byte[data.Stride * bitmap.Height];
			IntPtr scan0 = data.Scan0;
			Marshal.Copy(scan0, result, 0, result.Length);
			bitmap.UnlockBits(data);
			return result;
		}

		/// <summary>
		/// Sets every alpha byte in an RGBA array to 0xFF (fully opaque). This
		/// method mutates the original array.
		/// </summary>
		/// <param name="bytes">The array for which to fix the alpha bytes.</param>
		/// <returns>
		/// An array with all alpha values equal to 0xFF (fully opaque).
		/// </returns>
		public static byte[] FixAlpha(byte[] bytes)
		{
			for (int i = 3; i < bytes.Length; i += 4)
			{
				bytes[i] = 0xFF;
			}
			return bytes;
		}

		/// <summary>
		/// Creates an image of a given size made from the raw bytes of a file at
		/// a given path.
		/// </summary>
		/// <param name="filePath">A path to a file.</param>
		/// <param name="width">The width of the image, in pixels.</param>
		/// <param name="height">The height of the image, in pixels.</param>
		/// <returns>An image of the given sizes with pixels from the file.</returns>
		/// <remarks>
		/// If the image's given size is not large enough to store the entire
		/// file, only the first part of the image will be in the result. If the
		/// image's given size is larger than the file's size, the remainder of
		/// the space will be fully transparent.
		/// </remarks>
		public static Bitmap OpenRaw(string filePath, int width, int height)
		{
			// Validate file exists
			byte[] bytes = FixAlpha(File.ReadAllBytes(filePath));
			int length = Math.Min(bytes.Length,
				width * height * 4);

			Bitmap result = new Bitmap(width, height);
			BitmapData data = result.LockBits(new Rectangle(0, 0, width, height),
				ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
			IntPtr scan0 = data.Scan0;
			Marshal.Copy(bytes, 0, scan0, length);
			result.UnlockBits(data);
			return result;
		}

		/// <summary>
		/// Converts an array of 32-bit integers as ARGB pixels into a bitmap.
		/// </summary>
		/// <param name="pixels">The array of pixels.</param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <returns>A bitmap containing the pixels stored in the array.</returns>
		/// <remarks>
		/// The size of the bitmap is determined by the <see
		/// cref="Drawer.GetImageSize" /> method.
		/// </remarks>
		private static Bitmap ToBitmap(int[] pixels, BackgroundWorker worker)
		{
			return ToBitmap(pixels, worker, GetImageSize(pixels.Length));
		}

		/// <summary>
		/// Converts an array of 32-bit integers as ARGB pixels into a bitmap of
		/// a certain size.
		/// </summary>
		/// <param name="pixels">The array of pixels.</param>
		/// <param name="worker">
		/// A BackgroundWorker which receives progress reports and may cancel
		/// this method.
		/// </param>
		/// <param name="imageSize">The desired size of the image.</param>
		/// <returns>
		/// A bitmap of the desired size with each pixel, in order, set by the
		/// value in the array.
		/// </returns>
		/// <remarks>
		/// If there are less pixels in the array than in the bitmap, the
		/// remaining pixels in the bitmap will be all-bits-zero (in paletted
		/// modes, all-bits-zero pixels use the first color in the provided
		/// palette). If there are more pixels in the array than in the bitmap,
		/// remaining pixels in the array will be ignored.
		/// </remarks>
		private static Bitmap ToBitmap(int[] pixels, BackgroundWorker worker, Size imageSize)
		{
			if (pixels.Length == 0)
			{
				return new Bitmap(1, 1);
			}

			Bitmap result = new Bitmap(imageSize.Width, imageSize.Height);
			BitmapData data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height),
				ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			int stride = data.Stride;
			byte[] bytes = new byte[stride * result.Height];

			for (int y = 0; y < result.Height; y++)
			{
				for (int x = 0; x < result.Width; x++)
				{
					int pixelIndex = (y * result.Width) + x;
					Color color = Color.FromArgb(
						(pixelIndex < pixels.Length) ? pixels[pixelIndex] : 0);
					bytes[y * stride + x * 4] = color.B;
					bytes[y * stride + x * 4 + 1] = color.G;
					bytes[y * stride + x * 4 + 2] = color.R;
					bytes[y * stride + x * 4 + 3] = color.A;
				}
				if (worker.CancellationPending)
				{
					break;
				}

				worker.ReportProgress((int)((y * 100m) / result.Height));
			}

			IntPtr scan0 = data.Scan0;
			Marshal.Copy(bytes, 0, scan0, stride * result.Height);
			result.UnlockBits(data);
			return result;
		}

		/// <summary>
		/// Converts the pixels in a bitmap into an array of 32-bit integers as
		/// ARGB values.
		/// </summary>
		/// <param name="bitmap">The bitmap.</param>
		/// <returns>An array of pixels from the bitmap.</returns>
		private static int[] ToPixels(Bitmap bitmap)
		{
			BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int length = bitmap.Width * bitmap.Height;
			int[] result = new int[length];
			Marshal.Copy(data.Scan0, result, 0, length);
			bitmap.UnlockBits(data);
			return result;
		}

		/// <summary>
		/// Throws an <see cref="ArgumentOutOfRangeException" /> if the actual
		/// size of the palette was not the same as the expected size.
		/// </summary>
		/// <param name="expectedSize">
		/// The expected number of colors in the palette.
		/// </param>
		/// <param name="actualSize">The actual number of colors in the palette.</param>
		private static void ValidatePaletteSize(int expectedSize, int actualSize)
		{
			if (expectedSize != actualSize)
			{
				string determiner = (actualSize > expectedSize) ? "too many" : "too few";
				throw new ArgumentOutOfRangeException("palette",
					$"The palette has {determiner} colors defined. Expected {expectedSize} colors, got {actualSize} colors.");
			}
		}
	}
}
