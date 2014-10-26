using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
    public sealed class Drawer
    {
        private bool IsPerfectSquare(int n)
        {
            if (n < 1)
            {
                return false;
            }

            int squareRoot = (int)Math.Sqrt(n);
            return (squareRoot * squareRoot) == n;
        }

        private List<int> GetFactors(int n)
        {
            List<int> result = new List<int>();

            if (n < 1) return result;

            for (int i = 0; i < n / 2; i++)
            {
                if (n % i == 0)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private Size GetImageSize(int pixelCount)
        {
            int squareRoot = (int)Math.Sqrt((double)pixelCount);
            Size result;
            if (this.IsPerfectSquare(pixelCount))
            {
                result = new Size(squareRoot, squareRoot);
            }
            else
            {
                int height = squareRoot;
                int remainder = pixelCount - squareRoot * squareRoot;
                for (int remainderRows = (int)Math.Ceiling((double)remainder / (double)squareRoot); remainderRows > 0; remainderRows--)
                {
                    height++;
                }
                result = new Size(squareRoot, height);
            }
            return result;
        }

        public Bitmap Draw(FileSource source, BitDepth bitDepth, ColorMode mode, int[] palette, BackgroundWorker worker)
        {
            byte[] bytes = source.GetFiles();

            return this.Draw(bytes, bitDepth, palette, worker);
        }

        public Bitmap Draw(byte[] bytes, BitDepth bitDepth, int[] palette, BackgroundWorker worker)
        {
            switch (bitDepth)
            {
                case BitDepth.Invalid:
                    throw new InvalidOperationException("Cannot draw a bitmap using an invalid bit depth.");
                case BitDepth.OneBpp:
                    return this.ToBitmap(this.Create1BppImage(bytes, palette, worker), worker);
                case BitDepth.TwoBpp:
                    return this.ToBitmap(this.Create2BppImage(bytes, palette, worker), worker);
                case BitDepth.FourBpp:
                    return this.ToBitmap(this.Create4BppImage(bytes, palette, worker), worker);
                case BitDepth.EightBpp:
                    return this.ToBitmap(this.Create8BppImage(bytes, palette, worker), worker);
                case BitDepth.SixteenBpp:
                    return this.ToBitmap(this.Create16BppImage(bytes, palette, worker), worker);
                case BitDepth.TwentyFourBpp:
                    return this.ToBitmap(this.Create24BppImage(bytes, worker), worker);
                case BitDepth.ThirtyTwoBpp:
                    return this.ToBitmap(this.Create32BppImage(bytes, worker), worker);
                default:
                    return null;
            }
        }

        public Bitmap Draw(byte[] bytes, BitDepth bitDepth, int[] palette, BackgroundWorker worker, Size imageSize)
        {
            switch (bitDepth)
            {
                case BitDepth.Invalid:
                    throw new InvalidOperationException("Cannot draw a bitmap using an invalid bit depth.");
                case BitDepth.OneBpp:
                    return this.ToBitmap(this.Create1BppImage(bytes, palette, worker), worker, imageSize);
                case BitDepth.TwoBpp:
                    return this.ToBitmap(this.Create2BppImage(bytes, palette, worker), worker, imageSize);
                case BitDepth.FourBpp:
                    return this.ToBitmap(this.Create4BppImage(bytes, palette, worker), worker, imageSize);
                case BitDepth.EightBpp:
                    return this.ToBitmap(this.Create8BppImage(bytes, palette, worker), worker, imageSize);
                case BitDepth.SixteenBpp:
                    return this.ToBitmap(this.Create16BppImage(bytes, palette, worker), worker, imageSize);
                case BitDepth.TwentyFourBpp:
                    return this.ToBitmap(this.Create24BppImage(bytes, worker), worker, imageSize);
                case BitDepth.ThirtyTwoBpp:
                    return this.ToBitmap(this.Create32BppImage(bytes, worker), worker, imageSize);
                default:
                    return null;
            }
        }

		public Bitmap Sort(Bitmap bitmap)
		{
			int[] pixels = this.ToPixels(bitmap);
			Array.Sort(pixels);
			BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
			return this.ToBitmap(pixels, worker, new Size(bitmap.Width, bitmap.Height));
		}
		
		public Bitmap UniqueColors(Bitmap bitmap, out string colorCount)
		{
			int[] pixels = this.ToPixels(bitmap);
			pixels = pixels.Distinct().ToArray();
			colorCount = string.Format("{0} unique colors", pixels.Length);
			BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
			return this.ToBitmap(pixels, worker);
		}

        private Bitmap ToBitmap(int[] pixels, BackgroundWorker worker)
        {
            return this.ToBitmap(pixels, worker, this.GetImageSize(pixels.Length));
        }

        private Bitmap ToBitmap(int[] pixels, BackgroundWorker worker, Size imageSize)
        {
            if (pixels.Length == 0)
            {
                return new Bitmap(1, 1);
            }

            Bitmap result = new Bitmap(imageSize.Width, imageSize.Height);
            BitmapData data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = data.Stride;
            byte[] bytes = new byte[stride * result.Height];

            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    int pixelIndex = (y * result.Width) + x;
                    Color color = Color.FromArgb((pixelIndex < pixels.Length) ? pixels[pixelIndex]: 0);
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

		private int[] ToPixels(Bitmap bitmap)
		{
			BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int length = bitmap.Width * bitmap.Height;
			int[] result = new int[length];
			Marshal.Copy(data.Scan0, result, 0, length);
			bitmap.UnlockBits(data);
			return result;
		}

        private int[] Create1BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
        {
            int[] image = new int[bytes.Length * 8]; // good lord

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

        private int[] Create2BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
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

        private int[] Create4BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
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

        private int[] Create8BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
        {
            int[] image = new int[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                image[i] = palette[bytes[i]];
            }

            return image;
        }

        private int[] Create16BppImage(byte[] bytes, int[] palette, BackgroundWorker worker)
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
            }

            return image;
        }

        private int[] Create24BppImage(byte[] bytes, BackgroundWorker worker)
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
            }

            return image;
        }

        private int[] Create32BppImage(byte[] bytes, BackgroundWorker worker)
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
                image[i] = ((int)alpha << 24) + ((int)red << 16) + ((int)green << 8) + (int)blue;
            }

            return image;
        }
    }
}
