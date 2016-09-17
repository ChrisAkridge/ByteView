using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteView
{
	/// <summary>
	/// Generates multiple images of a certain size from a large file or group of files.
	/// </summary>
    public partial class LargeFileProcessorForm : Form
    {
		/// <summary>
		/// A list of file paths to all the source files.
		/// </summary>
        private List<string> sourceFiles;

		/// <summary>
		/// The desired color mode for the generated images.
		/// </summary>
        private ColorMode colorMode;

		/// <summary>
		/// The desired bit depth for the generated images.
		/// </summary>
        private BitDepth bitDepth;

		/// <summary>
		/// Gets the width of the desired image in pixels.
		/// </summary>
        private int ImageWidth
        {
            get
            {
                return int.Parse(TextBoxImageWidth.Text, CultureInfo.InvariantCulture);
            }
        }

		/// <summary>
		/// Gets the height of the desired image in pixels.
		/// </summary>
        private int ImageHeight
        {
            get
            {
                return int.Parse(TextBoxImageHeight.Text, CultureInfo.InvariantCulture);
            }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="LargeFileProcessorForm"/> class.
		/// </summary>
        public LargeFileProcessorForm()
        {
			sourceFiles = new List<string>();
            InitializeComponent();
        }

		/// <summary>
		/// Initializes the value of some controls on the form.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
        private void LargeFileProcessorForm_Load(object sender, EventArgs e)
        {
			ComboBoxBitDepths.SelectedIndex = 0;
			UpdateFileInfo();
			UpdateImageInfo();
        }

		/// <summary>
		/// Adds files selected in <see cref="OFDAddFile"/> to the source files.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void ButtonAddFiles_Click(object sender, EventArgs e)
        {
            if (OFDAddFile.ShowDialog() == DialogResult.OK)
            {
				sourceFiles.AddRange(OFDAddFile.FileNames);
				ListBoxFiles.Items.Clear();
				ListBoxFiles.Items.AddRange(sourceFiles.ToArray());
				UpdateFileInfo();
            }
        }

		/// <summary>
		/// Removes the selected file from the source files if the Delete key has been pressed.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void ListBoxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && ListBoxFiles.SelectedIndex >= 0)
            {
                int index = ListBoxFiles.SelectedIndex;
				ListBoxFiles.Items.RemoveAt(index);
				sourceFiles.RemoveAt(index);

                if (ListBoxFiles.Items.Count > 0)
                {
					ListBoxFiles.SelectedIndex = index - 1;
                }

				UpdateFileInfo();
            }
        }

		/// <summary>
		/// Sets the color mode to grayscale.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void RadioGrayscale_CheckedChanged(object sender, EventArgs e)
        {
			colorMode = ColorMode.Grayscale;
        }

		/// <summary>
		/// Sets the color mode to RGB.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void RadioRGB_CheckedChanged(object sender, EventArgs e)
        {
			colorMode = ColorMode.RGB;
        }

		/// <summary>
		/// Sets the color mode to ARGB.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void RadioARGB_CheckedChanged(object sender, EventArgs e)
        {
			colorMode = ColorMode.ARGB;
        }

		/// <summary>
		/// Sets the color mode to paletted.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void RadioPaletted_CheckedChanged(object sender, EventArgs e)
        {
			colorMode = ColorMode.Paletted;
        }

		/// <summary>
		/// Validates the image's width and height and updates the <see cref="LabelImageData"/> label.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void TextBoxImageWidth_TextChanged(object sender, EventArgs e)
        {
			ValidateSize();
			UpdateImageInfo();
        }

		/// <summary>
		/// Validates the image's height and updates the <see cref="LabelImageData"/> label.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void TextBoxImageHeight_TextChanged(object sender, EventArgs e)
        {
			ValidateSize();
			UpdateImageInfo();
        }

		/// <summary>
		/// Changes the bit depth to the one specified by <see cref="ComboBoxBitDepths"/> and toggles the color modes appropriately.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void ComboBoxBitDepths_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ComboBoxBitDepths.SelectedIndex)
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

			ValidateSize();
			UpdateImageInfo();
        }

		/// <summary>
		/// Generates a suffix for file sizes.
		/// </summary>
		/// <param name="fileSize"></param>
		/// <param name="number"></param>
		/// <returns></returns>
        private static string GenerateFileSizeAbbreviation(ulong fileSize, out int number)
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
			sourceFiles.ForEach(f => totalSize += (ulong)new FileInfo(f).Length);

            int size;
            string sizeSuffix = GenerateFileSizeAbbreviation(totalSize, out size);
			LabelFilesData.Text = string.Format("{0} file{1} loaded. Total size: {2} {3}.", sourceFiles.Count, (sourceFiles.Count == 1) ? "" : "s", size, sizeSuffix);
        }

        private void UpdateImageInfo()
        {
            if (!ButtonGenerate.Enabled)
            {
                return;
            }

            int pixels = ImageWidth * ImageHeight;
            int size = GetImageSize();
            int sizeNumber;
            string sizeString = GenerateFileSizeAbbreviation((ulong)size, out sizeNumber);

			LabelImageData.Text = string.Format("{0} pixels. Total size: {1} {2}.", pixels, sizeNumber, sizeString);
        }

        private void TextBoxFileEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!File.Exists(TextBoxFileEntry.Text))
                {
                    MessageBox.Show("The file path you entered is not valid.", "Invalid File Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

				sourceFiles.Add(TextBoxFileEntry.Text);
				ListBoxFiles.Items.Add(TextBoxFileEntry.Text);
				UpdateFileInfo();
            }
        }

        private void ButtonSelectOutputFolder_Click(object sender, EventArgs e)
        {
            if (FBDOutputFolder.ShowDialog() == DialogResult.OK)
            {
				TextOutputFolder.Text = FBDOutputFolder.SelectedPath;
            }
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            // Verify the folder path.
            if (!Directory.Exists(TextOutputFolder.Text))
            {
                MessageBox.Show("The output folder does not exist.", "Output Folder Doesn't Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			ButtonStop.Enabled = true;
			Worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Step 1: Set up some important variables.
            int imageSize = GetImageSize();
            string temporaryFilePath = Path.Combine(TextOutputFolder.Text, "temp.dat");

            ulong sourceFilesSize = 0;
			sourceFiles.ForEach(f => sourceFilesSize += (ulong)new FileInfo(f).Length);

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
					Invoke((MethodInvoker)delegate { LabelStatus.Text = statusText; });
					Invoke((MethodInvoker)delegate { Progress.Value = progressValue; });

                    using (FileStream reader = File.OpenRead(sourceFiles[i]))
                    {
                        reader.CopyTo(writer); // cooool
                    }

                    if (Worker.CancellationPending)
                    {
                        goto cleanup; // dun dun DUUUUNNN
                    }
                }
            }

            // Step 3: Create each image.
            int fileIndex = 0;
            byte[] currentImage = null;
            int[] palette = null;
            if (bitDepth != BitDepth.TwentyFourBpp && bitDepth != BitDepth.ThirtyTwoBpp)
            {
                palette = DefaultPalettes.GetPalette(bitDepth, colorMode);
            }

            using (FileStream reader = File.OpenRead(temporaryFilePath))
            {
                for (int i = 0; i < totalImages; i++)
                {
                    string statusText = string.Format("Creating images ({0} of {1}).", i + 1, totalImages);
                    int percentage = (int)(100m * ((i + 1m) / totalImages));
					Invoke((MethodInvoker)delegate { LabelStatus.Text = statusText; });
					Invoke((MethodInvoker)delegate { Progress.Value = percentage; });

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

                    Bitmap result = Drawer.Draw(currentImage, bitDepth, palette, Worker, new Size(ImageWidth, ImageHeight));

                    string resultPath = Path.Combine(TextOutputFolder.Text, string.Format("image_{0:D4}.png", fileIndex));
                    result.Save(resultPath, ImageFormat.Png);
                    fileIndex++;

                    if (Worker.CancellationPending)
                    {
                        goto cleanup;
                    }
                }
            }

        cleanup:
            File.Delete(temporaryFilePath);
            string cleanupStatusText = "Waiting...";
            int cleanupProgress = 0;
			Invoke((MethodInvoker)delegate { LabelStatus.Text = cleanupStatusText; });
			Invoke((MethodInvoker)delegate { Progress.Value = cleanupProgress; });
			Invoke((MethodInvoker)delegate { ButtonStop.Enabled = false; });
        }

        private void ValidateSize()
        {
            bool valid = false;
            int whatever;
            if (!int.TryParse(TextBoxImageWidth.Text, out whatever) || !int.TryParse(TextBoxImageHeight.Text, out whatever))
            {
				LabelImageData.Text = "Invalid width or height.";
            }
            else if (GetImageSize() == 0)
            {
				LabelImageData.Text = "Image size is too low to produce files.";
            }
            else
            {
                valid = true;
            }

			ButtonGenerate.Enabled = valid;
        }

        private int GetImageSize()
        {
            int width = ImageWidth;
            int height = ImageHeight;
            decimal[] divisors = {decimal.MinValue, (1m / 8m), (1m / 4m), (1m / 2m), 1m, 2m, 3m, 4m };
            return (int)Math.Floor(width * height * divisors[(int)bitDepth]);
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
			Worker.CancelAsync();
        }

        private void ButtonAddFolder_Click(object sender, EventArgs e)
        {
            if (FBDAddFolder.ShowDialog() == DialogResult.OK)
            {
                string[] filePaths = Directory.EnumerateFiles(FBDAddFolder.SelectedPath, "*.*", SearchOption.AllDirectories).ToArray();
                filePaths = filePaths.OrderBy(s => s).ToArray();
				sourceFiles.AddRange(filePaths);
				ListBoxFiles.Items.AddRange(filePaths);
				UpdateFileInfo();
            }
        }
    }
}
