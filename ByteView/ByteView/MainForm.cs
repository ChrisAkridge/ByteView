using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ByteView
{
	public partial class MainForm : Form
    {
        private BitDepth bitDepth;
        private ColorMode colorMode;
        private string[] filePaths;
        private Bitmap image;

		public MainForm()
		{
			InitializeComponent();
		}

        public MainForm(string filePath)
        {
            InitializeComponent();

			if (!File.Exists(filePath))
			{
				MessageBox.Show(string.Format("The file at {0} does not exist.", filePath), "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			filePaths = new string[] { filePath };
			Worker_DoWork(this, new DoWorkEventArgs(this));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ComboBitDepths.SelectedIndex)
            {
                case 0:
					bitDepth = BitDepth.OneBpp;
					RadioGrayscale.Enabled = true;
					RadioRGB.Enabled = false;
					RadioARGB.Enabled = false;
					RadioPaletted.Enabled = true;
					RadioGrayscale.Checked = true;
                    break;
                case 1:
					bitDepth = BitDepth.TwoBpp;
					RadioGrayscale.Enabled = true;
					RadioRGB.Enabled = false;
					RadioARGB.Enabled = false;
					RadioPaletted.Enabled = true;
					RadioGrayscale.Checked = true;
                    break;
                case 2:
					bitDepth = BitDepth.FourBpp;
					RadioGrayscale.Enabled = true;
					RadioRGB.Enabled = true;
					RadioARGB.Enabled = false;
					RadioPaletted.Enabled = true;
					RadioGrayscale.Checked = true;
                    break;
                case 3:
					bitDepth = BitDepth.EightBpp;
					RadioGrayscale.Enabled = true;
					RadioRGB.Enabled = true;
					RadioARGB.Enabled = true;
					RadioPaletted.Enabled = true;
					RadioGrayscale.Checked = true;
                    break;
                case 4:
					bitDepth = BitDepth.SixteenBpp;
					RadioGrayscale.Enabled = false;
					RadioRGB.Enabled = true;
					RadioARGB.Enabled = true;
					RadioPaletted.Enabled = false;
					RadioRGB.Checked = true;
                    break;
                case 5:
					bitDepth = BitDepth.TwentyFourBpp;
					RadioGrayscale.Enabled = false;
					RadioRGB.Enabled = true;
					RadioARGB.Enabled = true;
					RadioPaletted.Enabled = false;
					RadioRGB.Checked = true;
                    break;
                case 6:
					bitDepth = BitDepth.ThirtyTwoBpp;
					RadioGrayscale.Enabled = false;
					RadioRGB.Enabled = false;
					RadioARGB.Enabled = true;
					RadioPaletted.Enabled = false;
					RadioARGB.Checked = true;
                    break;
                default:
                    break;
            }
        }

        private void RadioGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioGrayscale.Checked)
            {
				colorMode = ColorMode.Grayscale;
            }
        }

        private void RadioRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioRGB.Checked)
            {
				colorMode = ColorMode.RGB;
            }
        }

        private void RadioARGB_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioARGB.Checked)
            {
				colorMode = ColorMode.ARGB;
            }
        }

        private void RadioPaletted_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioPaletted.Checked)
            {
				colorMode = ColorMode.Paletted;
            }
        }

        private void TSBOpenFolder_Click(object sender, EventArgs e)
        {
            if (FolderSelector.ShowDialog() == DialogResult.OK)
            {
				filePaths = System.IO.Directory.EnumerateFiles(FolderSelector.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories).ToArray();
				filePaths = filePaths.OrderBy(s => s).ToArray();
				Worker.RunWorkerAsync();
            }
        }

        private static string GetFileSizeString(long length)
        {
            char[] prefixes = new char[] { 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };
            int prefixIndex = -1;
            decimal lengthValue = length;

            while (lengthValue > 1024m)
            {
                lengthValue /= 1024m;
                if (prefixIndex != -1 && prefixes[prefixIndex] == 'Y')
                {
                    return string.Format("{0} YB", lengthValue);
                }
                else
                {
                    prefixIndex++;
                }
            }

            if (prefixIndex == -1)
            {
                return string.Format("{0} bytes", length);
            }
            else
            {
                return string.Format("{0} {1}B", Decimal.Round(lengthValue, 2), prefixes[prefixIndex]);
            }
        }

        private void TSBOpenFiles_Click(object sender, EventArgs e)
        {
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
				filePaths = OpenFile.FileNames;
				filePaths = filePaths.OrderBy(s => s).ToArray();
				Worker.RunWorkerAsync();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (filePaths != null && filePaths.Length != 0)
            {
                FileSource source = new FileSource(filePaths);
                int[] palette = null;
                if (bitDepth != BitDepth.TwentyFourBpp && bitDepth != BitDepth.ThirtyTwoBpp)
                {
                    palette = DefaultPalettes.GetPalette(bitDepth, colorMode);
                }

				image = Drawer.Draw(source, bitDepth, palette, Worker);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
				Image.Image = image;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
			Progress.Value = e.ProgressPercentage;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
			ComboBitDepths.SelectedIndex = 0;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void TSBRefresh_Click(object sender, EventArgs e)
        {
            if (filePaths != null)
            {
				Worker.RunWorkerAsync();
            }
        }

        private void TSBOpenPicture_Click(object sender, EventArgs e)
        {
            if (OpenPicture.ShowDialog() == DialogResult.OK)
            {
                string filePath = OpenPicture.FileName;
				image = (Bitmap)System.Drawing.Image.FromFile(filePath);
				Image.Image = image;
            }
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            byte[] result = new byte[data.Stride * bitmap.Height];
            IntPtr scan0 = data.Scan0;
            Marshal.Copy(scan0, result, 0, result.Length);
            bitmap.UnlockBits(data);
            return result;
        }

        private void TSBSaveAs_Click(object sender, EventArgs e)
        {
            if (image != null && SaveFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = SaveFile.FileName;
                string extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower(CultureInfo.InvariantCulture);
                if (extension != null)
                {
                    if (extension == "png")
                    {
						image.Save(fileName, ImageFormat.Png);
                    }
                    else if (extension == "jpg" || extension == "jpeg")
                    {
						image.Save(fileName, ImageFormat.Jpeg);
                    }
                    else if (extension == "gif")
                    {
						image.Save(fileName, ImageFormat.Gif);
                    }
                    else if (extension == "raw")
                    {
                        byte[] bytes = BitmapToByteArray(image);
                        File.WriteAllBytes(fileName, bytes);
                    }
                }
            }
        }

        private void TSBOpenRaw_Click(object sender, EventArgs e)
        {
			if (OpenFile.ShowDialog() == DialogResult.OK)
			{
				string filePath = OpenFile.FileName;
				byte[] bytes = FixAlpha(File.ReadAllBytes(filePath));
				int width = 0, height = 0, length = bytes.Length;

				using (var sizeForm = new RawImageSizeForm())
				{
					if (sizeForm.ShowDialog() == DialogResult.OK)
					{
						width = sizeForm.ImageWidth;
						height = sizeForm.ImageHeight;
						if (width == 0 || height == 0)
						{
							return;
						}
						else if (bytes.Length > width * height * 4)
						{
							length = width * height * 4;
						}
					}
					else
					{
						return;
					}
				}

                Bitmap result = new Bitmap(width, height);
                BitmapData data = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                IntPtr scan0 = data.Scan0;
                Marshal.Copy(bytes, 0, scan0, length);
                result.UnlockBits(data);
				image = result;
				Image.Image = image;
            }
        }

        private static byte[] FixAlpha(byte[] bytes)
        {
            for (int i = 3; i < bytes.Length; i += 4)
            {
                bytes[i] = 0xFF;
            }
            return bytes;
        }

        private void TSBCancel_Click(object sender, EventArgs e)
        {
			Worker.CancelAsync();
        }

        private void TSBLargeFileProcessor_Click(object sender, EventArgs e)
        {
			using (var lfpForm = new LargeFileProcessorForm())
			{
				lfpForm.ShowDialog();
			}
        }

		private void TSBSort_Click(object sender, EventArgs e)
		{
			if (image == null) return;
			image = Drawer.Sort(image);
			Image.Image = image;
		}

		private void TSBUnique_Click(object sender, EventArgs e)
		{
			if (image == null) return;
			string colorCount;
			image = Drawer.UniqueColors(image, out colorCount);
			Image.Image = image;
			Text = string.Format("ByteView - {0}", colorCount);
		}
    }
}
