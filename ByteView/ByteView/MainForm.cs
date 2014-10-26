using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

			this.filePaths = new string[] { filePath };
			this.Worker_DoWork(this, new DoWorkEventArgs(this));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ComboBitDepths.SelectedIndex)
            {
                case 0:
                    this.bitDepth = BitDepth.OneBpp;
                    this.RadioGrayscale.Enabled = true;
                    this.RadioRGB.Enabled = false;
                    this.RadioARGB.Enabled = false;
                    this.RadioPaletted.Enabled = true;
                    this.RadioGrayscale.Checked = true;
                    break;
                case 1:
                    this.bitDepth = BitDepth.TwoBpp;
                    this.RadioGrayscale.Enabled = true;
                    this.RadioRGB.Enabled = false;
                    this.RadioARGB.Enabled = false;
                    this.RadioPaletted.Enabled = true;
                    this.RadioGrayscale.Checked = true;
                    break;
                case 2:
                    this.bitDepth = BitDepth.FourBpp;
                    this.RadioGrayscale.Enabled = true;
                    this.RadioRGB.Enabled = true;
                    this.RadioARGB.Enabled = false;
                    this.RadioPaletted.Enabled = true;
                    this.RadioGrayscale.Checked = true;
                    break;
                case 3:
                    this.bitDepth = BitDepth.EightBpp;
                    this.RadioGrayscale.Enabled = true;
                    this.RadioRGB.Enabled = true;
                    this.RadioARGB.Enabled = true;
                    this.RadioPaletted.Enabled = true;
                    this.RadioGrayscale.Checked = true;
                    break;
                case 4:
                    this.bitDepth = BitDepth.SixteenBpp;
                    this.RadioGrayscale.Enabled = false;
                    this.RadioRGB.Enabled = true;
                    this.RadioARGB.Enabled = true;
                    this.RadioPaletted.Enabled = false;
                    this.RadioRGB.Checked = true;
                    break;
                case 5:
                    this.bitDepth = BitDepth.TwentyFourBpp;
                    this.RadioGrayscale.Enabled = false;
                    this.RadioRGB.Enabled = true;
                    this.RadioARGB.Enabled = true;
                    this.RadioPaletted.Enabled = false;
                    this.RadioRGB.Checked = true;
                    break;
                case 6:
                    this.bitDepth = BitDepth.ThirtyTwoBpp;
                    this.RadioGrayscale.Enabled = false;
                    this.RadioRGB.Enabled = false;
                    this.RadioARGB.Enabled = true;
                    this.RadioPaletted.Enabled = false;
                    this.RadioARGB.Checked = true;
                    break;
                default:
                    break;
            }
        }

        private void RadioGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioGrayscale.Checked)
            {
                this.colorMode = ColorMode.Grayscale;
            }
        }

        private void RadioRGB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioRGB.Checked)
            {
                this.colorMode = ColorMode.RGB;
            }
        }

        private void RadioARGB_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioARGB.Checked)
            {
                this.colorMode = ColorMode.ARGB;
            }
        }

        private void RadioPaletted_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioPaletted.Checked)
            {
                this.colorMode = ColorMode.Paletted;
            }
        }

        private void TSBOpenFolder_Click(object sender, EventArgs e)
        {
            if (this.FolderSelector.ShowDialog() == DialogResult.OK)
            {
                this.filePaths = System.IO.Directory.EnumerateFiles(this.FolderSelector.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories).ToArray();
                this.filePaths = this.filePaths.OrderBy(s => s).ToArray();
                this.Worker.RunWorkerAsync();
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
            if (this.OpenFile.ShowDialog() == DialogResult.OK)
            {
                this.filePaths = this.OpenFile.FileNames;
                this.filePaths = this.filePaths.OrderBy(s => s).ToArray();
                this.Worker.RunWorkerAsync();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.filePaths != null && this.filePaths.Length != 0)
            {
                FileSource source = new FileSource(this.filePaths);
                int[] palette = null;
                if (this.bitDepth != BitDepth.TwentyFourBpp && this.bitDepth != BitDepth.ThirtyTwoBpp)
                {
                    palette = DefaultPalettes.GetPalette(this.bitDepth, this.colorMode);
                }

                Drawer drawer = new Drawer();
                this.image = drawer.Draw(source, this.bitDepth, this.colorMode, palette, this.Worker);
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
                this.Image.Image = this.image;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Progress.Value = e.ProgressPercentage;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ComboBitDepths.SelectedIndex = 0;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void TSBRefresh_Click(object sender, EventArgs e)
        {
            if (this.filePaths != null)
            {
                this.Worker.RunWorkerAsync();
            }
        }

        private void TSBOpenPicture_Click(object sender, EventArgs e)
        {
            if (this.OpenPicture.ShowDialog() == DialogResult.OK)
            {
                string filePath = this.OpenPicture.FileName;
                this.image = (Bitmap)System.Drawing.Image.FromFile(filePath);
                this.Image.Image = this.image;
            }
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
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
            if (this.image != null && this.SaveFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.SaveFile.FileName;
                string extension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                if (extension != null)
                {
                    if (extension == "png")
                    {
                        this.image.Save(fileName, ImageFormat.Png);
                    }
                    else if (extension == "jpg" || extension == "jpeg")
                    {
                        this.image.Save(fileName, ImageFormat.Jpeg);
                    }
                    else if (extension == "gif")
                    {
                        this.image.Save(fileName, ImageFormat.Gif);
                    }
                    else if (extension == "raw")
                    {
                        byte[] bytes = this.BitmapToByteArray(this.image);
                        File.WriteAllBytes(fileName, bytes);
                    }
                }
            }
        }

        private void TSBOpenRaw_Click(object sender, EventArgs e)
        {
            if (this.OpenFile.ShowDialog() == DialogResult.OK)
            {
                string filePath = this.OpenFile.FileName;
                byte[] bytes = this.FixAlpha(File.ReadAllBytes(filePath));
                int width = 0, height = 0, length = bytes.Length;
                RawImageSizeForm sizeForm = new RawImageSizeForm();

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

                Bitmap result = new Bitmap(width, height);
                BitmapData data = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                IntPtr scan0 = data.Scan0;
                Marshal.Copy(bytes, 0, scan0, length);
                result.UnlockBits(data);
                this.image = result;
                this.Image.Image = this.image;
            }
        }

        private byte[] FixAlpha(byte[] bytes)
        {
            for (int i = 3; i < bytes.Length; i += 4)
            {
                bytes[i] = 0xFF;
            }
            return bytes;
        }

        private void TSBCancel_Click(object sender, EventArgs e)
        {
            this.Worker.CancelAsync();
        }

        private void TSBLargeFileProcessor_Click(object sender, EventArgs e)
        {
            new LargeFileProcessorForm().ShowDialog();
        }

		private void TSBSort_Click(object sender, EventArgs e)
		{
			if (this.image == null) return;
			this.image = new Drawer().Sort(this.image);
			this.Image.Image = this.image;
		}

		private void TSBUnique_Click(object sender, EventArgs e)
		{
			if (this.image == null) return;
			string colorCount;
			this.image = new Drawer().UniqueColors(this.image, out colorCount);
			this.Image.Image = this.image;
			this.Text = string.Format("ByteView - {0}", colorCount);
		}
    }
}
