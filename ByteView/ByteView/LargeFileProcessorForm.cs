using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteView
{
    public partial class LargeFileProcessorForm : Form
    {
        private List<string> sourceFiles;
        private ColorMode colorMode;
        private BitDepth bitDepth;
        private string outputFolder;
        private List<Bitmap> results = new List<Bitmap>();

        private int ImageWidth
        {
            get
            {
                return int.Parse(this.TextBoxImageWidth.Text);
            }
        }

        private int ImageHeight
        {
            get
            {
                return int.Parse(this.TextBoxImageHeight.Text);
            }
        }

        public LargeFileProcessorForm()
        {
            this.sourceFiles = new List<string>();
            InitializeComponent();
        }

        private void LargeFileProcessorForm_Load(object sender, EventArgs e)
        {
            this.ComboBoxBitDepths.SelectedIndex = 0;
            this.UpdateFileInfo();
            this.UpdateImageInfo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.outputFolder = this.textBox1.Text;
        }

        private void ButtonAddFiles_Click(object sender, EventArgs e)
        {
            if (this.OFDAddFile.ShowDialog() == DialogResult.OK)
            {
                this.sourceFiles.AddRange(this.OFDAddFile.FileNames);
                this.ListBoxFiles.Items.Clear();
                this.ListBoxFiles.Items.AddRange(this.sourceFiles.ToArray());
                this.UpdateFileInfo();
            }
        }

        private void ListBoxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.ListBoxFiles.SelectedIndex >= 0)
            {
                int index = this.ListBoxFiles.SelectedIndex;
                this.ListBoxFiles.Items.RemoveAt(index);
                this.sourceFiles.RemoveAt(index);

                if (this.ListBoxFiles.Items.Count > 0)
                {
                    this.ListBoxFiles.SelectedIndex = index - 1;
                }

                this.UpdateFileInfo();
            }
        }

        private void RadioGrayscale_CheckedChanged(object sender, EventArgs e)
        {
            this.colorMode = ColorMode.Grayscale;
        }

        private void RadioRGB_CheckedChanged(object sender, EventArgs e)
        {
            this.colorMode = ColorMode.RGB;
        }

        private void RadioARGB_CheckedChanged(object sender, EventArgs e)
        {
            this.colorMode = ColorMode.ARGB;
        }

        private void RadioPaletted_CheckedChanged(object sender, EventArgs e)
        {
            this.colorMode = ColorMode.Paletted;
        }

        private void TextBoxImageWidth_TextChanged(object sender, EventArgs e)
        {
            this.ValidateSize();
            this.UpdateImageInfo();
        }

        private void TextBoxImageHeight_TextChanged(object sender, EventArgs e)
        {
            this.ValidateSize();
            this.UpdateImageInfo();
        }

        private void ComboBoxBitDepths_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ComboBoxBitDepths.SelectedIndex)
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

            this.ValidateSize();
            this.UpdateImageInfo();
        }

        private string GenerateFileSizeAbbreviation(ulong fileSize, out int number)
        {
            char[] prefixes = { 'K', 'M', /* if you go past this, you're doing something wrong */ 'G', 'T', 'P', 'E', 'Z', 'Y' };

            if (fileSize < 1024UL)
            {
                number = (int)fileSize;
                return "B";
            }

            int prefixNumber = -1;
            while (fileSize >= 1024UL)
            {
                fileSize /= 1024UL;
                prefixNumber++;
            }

            number = (int)fileSize;
            return string.Concat(prefixes[prefixNumber], "B");
        }

        private void UpdateFileInfo()
        {
            ulong totalSize = 0UL;
            this.sourceFiles.ForEach(f => totalSize += (ulong)new FileInfo(f).Length);

            int size;
            string sizeSuffix = this.GenerateFileSizeAbbreviation(totalSize, out size);
            this.LabelFilesData.Text = string.Format("{0} file{1} loaded. Total size: {2} {3}.", this.sourceFiles.Count, (this.sourceFiles.Count == 1) ? "" : "s", size, sizeSuffix);
        }

        private void UpdateImageInfo()
        {
            if (!this.ButtonGenerate.Enabled)
            {
                return;
            }

            int pixels = this.ImageWidth * this.ImageHeight;
            int size = this.GetImageSize();
            int sizeNumber;
            string sizeString = this.GenerateFileSizeAbbreviation((ulong)size, out sizeNumber);

            this.LabelImageData.Text = string.Format("{0} pixels. Total size: {1} {2}.", pixels, sizeNumber, sizeString);
        }

        private void TextBoxFileEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!File.Exists(this.TextBoxFileEntry.Text))
                {
                    MessageBox.Show("The file path you entered is not valid.", "Invalid File Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.sourceFiles.Add(this.TextBoxFileEntry.Text);
                this.ListBoxFiles.Items.Add(this.TextBoxFileEntry.Text);
                this.UpdateFileInfo();
            }
        }

        private void ButtonSelectOutputFolder_Click(object sender, EventArgs e)
        {
            if (this.FBDOutputFolder.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.FBDOutputFolder.SelectedPath;
            }
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            // Verify the folder path.
            if (!Directory.Exists(this.textBox1.Text))
            {
                MessageBox.Show("The output folder does not exist.", "Output Folder Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.ButtonStop.Enabled = true;
            this.Worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Step 1: Set up some important variables.
            int imageSize = this.GetImageSize();
            string temporaryFilePath = Path.Combine(this.textBox1.Text, "temp.dat");

            ulong sourceFilesSize = 0;
            this.sourceFiles.ForEach(f => sourceFilesSize += (ulong)new FileInfo(f).Length);

            int totalImages, remainder = 0;
            if (sourceFilesSize % (ulong)imageSize == 0)
            {
                totalImages = (int)(sourceFilesSize / (ulong)imageSize);
            }
            else
            {
                totalImages = (int)((sourceFilesSize / (ulong)imageSize) + 1);
                remainder = (int)((ulong)(totalImages * imageSize) - sourceFilesSize);
            }

            // Step 2: Copy all the files into a temp file.
            using (FileStream writer = File.OpenWrite(temporaryFilePath))
            {
                for (int i = 0; i < sourceFiles.Count; i++)
                {
                    string statusText = string.Format("Copying files ({0} of {1}).", i + 1, sourceFiles.Count);
                    int progressValue = (int)(100m * ((i + 1m) / sourceFiles.Count));
                    this.Invoke((MethodInvoker)delegate { this.LabelStatus.Text = statusText; });
                    this.Invoke((MethodInvoker)delegate { this.Progress.Value = progressValue; });

                    using (FileStream reader = File.OpenRead(this.sourceFiles[i]))
                    {
                        reader.CopyTo(writer); // cooool
                    }

                    if (this.Worker.CancellationPending)
                    {
                        goto cleanup; // dun dun DUUUUNNN
                    }
                }
            }

            // Step 3: Create each image.
            int fileIndex = 0;
            byte[] currentImage = null;
            int[] palette = null;
            if (this.bitDepth != BitDepth.TwentyFourBpp && this.bitDepth != BitDepth.ThirtyTwoBpp)
            {
                palette = DefaultPalettes.GetPalette(this.bitDepth, this.colorMode);
            }

            using (FileStream reader = File.OpenRead(temporaryFilePath))
            {
                for (int i = 0; i < totalImages; i++)
                {
                    string statusText = string.Format("Creating images ({0} of {1}).", i + 1, totalImages);
                    int percentage = (int)(100m * ((i + 1m) / totalImages));
                    this.Invoke((MethodInvoker)delegate { this.LabelStatus.Text = statusText; });
                    this.Invoke((MethodInvoker)delegate { this.Progress.Value = percentage; });

                    if (remainder != 0 && i == totalImages - 1)
                    {
                        currentImage = new byte[imageSize - remainder];
                        reader.Read(currentImage, 0, imageSize - remainder);
                    }
                    else
                    {
                        currentImage = new byte[imageSize];
                        reader.Read(currentImage, 0, imageSize);
                    }

                    Drawer drawer = new Drawer();
                    Bitmap result = drawer.Draw(currentImage, this.bitDepth, palette, this.Worker, new Size(this.ImageWidth, this.ImageHeight));

                    string resultPath = Path.Combine(this.textBox1.Text, string.Format("image_{0:D4}.png", fileIndex));
                    result.Save(resultPath, ImageFormat.Png);
                    fileIndex++;

                    if (this.Worker.CancellationPending)
                    {
                        goto cleanup;
                    }
                }
            }

        cleanup:
            File.Delete(temporaryFilePath);
            string cleanupStatusText = "Waiting...";
            int cleanupProgress = 0;
            this.Invoke((MethodInvoker)delegate { this.LabelStatus.Text = cleanupStatusText; });
            this.Invoke((MethodInvoker)delegate { this.Progress.Value = cleanupProgress; });
            this.Invoke((MethodInvoker)delegate { this.ButtonStop.Enabled = false; });
        }

        private void ValidateSize()
        {
            bool valid = false;
            int whatever;
            if (!int.TryParse(this.TextBoxImageWidth.Text, out whatever) || !int.TryParse(this.TextBoxImageHeight.Text, out whatever))
            {
                this.LabelImageData.Text = "Invalid width or height.";
            }
            else if (this.GetImageSize() == 0)
            {
                this.LabelImageData.Text = "Image size is too low to produce files.";
            }
            else
            {
                valid = true;
            }

            this.ButtonGenerate.Enabled = valid;
        }

        private int GetImageSize()
        {
            int width = this.ImageWidth;
            int height = this.ImageHeight;
            decimal[] divisors = {decimal.MinValue, (1m / 8m), (1m / 4m), (1m / 2m), 1m, 2m, 3m, 4m };
            return (int)Math.Floor(width * height * divisors[(int)this.bitDepth]);
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            this.Worker.CancelAsync();
        }

        private void ButtonAddFolder_Click(object sender, EventArgs e)
        {
            if (this.FBDAddFolder.ShowDialog() == DialogResult.OK)
            {
                string[] filePaths = System.IO.Directory.EnumerateFiles(this.FBDAddFolder.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories).ToArray();
                filePaths = filePaths.OrderBy(s => s).ToArray();
                this.sourceFiles.AddRange(filePaths);
                this.ListBoxFiles.Items.AddRange(filePaths);
                this.UpdateFileInfo();
            }
        }
    }
}
