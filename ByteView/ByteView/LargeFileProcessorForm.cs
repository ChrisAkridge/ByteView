// LargeFileProcessorForm.cs
//
// Converts many files into many bitmaps of the same resolution.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private Dictionary<string, FileInfo> sourceFiles = new Dictionary<string, FileInfo>();

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
        private int ImageWidth => int.Parse(TextBoxImageWidth.Text, CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets the height of the desired image in pixels.
        /// </summary>
        private int ImageHeight => int.Parse(TextBoxImageHeight.Text, CultureInfo.InvariantCulture);

        /// <summary>
        /// Initializes a new instance of the <see cref="LargeFileProcessorForm"/> class.
        /// </summary>
        public LargeFileProcessorForm()
        {
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
				AddFiles(OFDAddFile.FileNames);
			}
		}
		private void ButtonAddFolder_Click(object sender, EventArgs e)
		{
			if (FBDAddFolder.ShowDialog() == DialogResult.OK)
			{
				string[] filePaths = Directory.EnumerateFiles(FBDAddFolder.SelectedPath, "*.*", 
					SearchOption.AllDirectories).ToArray();
				filePaths = filePaths.OrderBy(s => s).ToArray();

				AddFiles(filePaths);
			}
		}

		private void ButtonClose_Click(object sender, EventArgs e) => Close();

		private void ButtonGenerate_Click(object sender, EventArgs e)
		{
			// Verify the folder path.
			if (!Directory.Exists(TextOutputFolder.Text))
			{
				MessageBox.Show("The output folder does not exist.", "Output Folder Doesn't Exist",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ButtonStop.Enabled = true;
			Worker.RunWorkerAsync();
		}

		private void ButtonSelectOutputFolder_Click(object sender, EventArgs e)
		{
			if (FBDOutputFolder.ShowDialog() == DialogResult.OK)
			{
				TextOutputFolder.Text = FBDOutputFolder.SelectedPath;
			}
		}

		private void ButtonStop_Click(object sender, EventArgs e) => Worker.CancelAsync();

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
		/// Removes the selected file from the source files if the Delete key has been pressed.
		/// </summary>
		/// <param name="sender">The object that invoked this event handler.</param>
		/// <param name="e">Arguments for this event.</param>
		private void ListBoxFiles_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && ListBoxFiles.SelectedIndex >= 0)
			{
				int index = ListBoxFiles.SelectedIndex;
				string filePathToRemove = (string)ListBoxFiles.Items[index];
				ListBoxFiles.Items.RemoveAt(index);
				sourceFiles.Remove(filePathToRemove);

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

		private void TextBoxFileEntry_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				string filePath = TextBoxFileEntry.Text;
				if (!File.Exists(filePath))
				{
					MessageBox.Show("The file path you entered is not valid.", "Invalid File Path",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				sourceFiles.Add(filePath, new FileInfo(filePath));
				ListBoxFiles.Items.Add(filePath);
				UpdateFileInfo();
			}
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

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			// Step 1: Set up some important variables.
			int imageSize = GetImageSize();
			string temporaryFilePath = Path.Combine(TextOutputFolder.Text, "temp.dat");

			ulong sourceFilesSize = 0;

			sourceFilesSize = (ulong)sourceFiles.Sum(kvp => kvp.Value.Length);

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
				int i = 0;
				foreach (var kvp in sourceFiles)
				{
					string statusText = string.Format("Copying files ({0} of {1}).", i + 1, 
						sourceFiles.Count);
					int progressValue = (int)(100m * ((i + 1m) / sourceFiles.Count));
					Invoke((MethodInvoker)delegate { LabelStatus.Text = statusText; });
					Invoke((MethodInvoker)delegate { Progress.Value = progressValue; });

					using (FileStream reader = File.OpenRead(kvp.Key))
					{
						reader.CopyTo(writer);
					}

					if (Worker.CancellationPending)
					{
						goto cleanup;
					}
					i++;
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

					Bitmap result = Drawer.Draw(currentImage, bitDepth, palette, Worker, 
						new Size(ImageWidth, ImageHeight));

					string resultPath = Path.Combine(TextOutputFolder.Text, 
						string.Format("image_{0:D4}.png", fileIndex));
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

		/// <summary>
		/// Generates a suffix for file sizes.
		/// </summary>
		/// <param name="fileSize"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		private static string GenerateFileSizeAbbreviation(ulong fileSize, out int number)
        {
            // TODO: This method is a GREAT candidate to go into ChrisAkridge.Common.
            char[] prefixes = { 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };

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

		private void AddFiles(IEnumerable<string> paths)
		{
			foreach (var filePath in paths)
			{
				sourceFiles.Add(filePath, new FileInfo(filePath));
				ListBoxFiles.Items.Add(filePath);
			}
			UpdateFileInfo();
		}

		// Updates the LabelFilesData control when files are added or removed.
		private void UpdateFileInfo()
        {
            ulong totalSize = 0UL;
            foreach (var kvp in sourceFiles)
            {
                totalSize += (ulong)kvp.Value.Length;
            }

            int size;
            string sizeSuffix = GenerateFileSizeAbbreviation(totalSize, out size);
			LabelFilesData.Text = string.Format("{0} file{1} loaded. Total size: {2} {3}.", 
                sourceFiles.Count, (sourceFiles.Count == 1) ? "" : "s", size, sizeSuffix);
        }

		// Updates the LabelImageData control when the size TextBoxes are changed.
        private void UpdateImageInfo()
        {
            if (!ButtonGenerate.Enabled) { return; }

            int pixels = ImageWidth * ImageHeight;
            int size = GetImageSize();
            int sizeNumber;
            string sizeString = GenerateFileSizeAbbreviation((ulong)size, out sizeNumber);

			LabelImageData.Text = string.Format("{0} pixels. Total size: {1} {2}.", pixels, sizeNumber, sizeString);
        }

		// Validates that the size TextBoxes contain positive numbers.
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
			else if (GetImageSize() == int.MaxValue || GetImageSize() < 0)
			{
				LabelImageData.Text = "Image size is too large.";
			}
            else
            {
                valid = true;
            }

			ButtonGenerate.Enabled = valid;
        }

		// Gets the size of each image in bytes.
        private int GetImageSize()
        {
            int width = ImageWidth;
            int height = ImageHeight;
            decimal[] divisors = { decimal.MinValue, (1m / 8m), (1m / 4m), (1m / 2m), 1m, 2m, 3m, 4m };
			decimal bytes = Math.Floor(width * height * divisors[(int)bitDepth]);

			if (bytes < int.MaxValue)
			{
				return (int)bytes;
			}
			else
			{
				return int.MaxValue;
			}
        }
    }
}
