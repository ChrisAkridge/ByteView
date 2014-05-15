namespace ByteView
{
    partial class TextBoxOutputFolder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StaticLabelFiles = new System.Windows.Forms.Label();
            this.TextBoxFileEntry = new System.Windows.Forms.TextBox();
            this.ButtonAddFolder = new System.Windows.Forms.Button();
            this.ButtonAddFiles = new System.Windows.Forms.Button();
            this.ListBoxFiles = new System.Windows.Forms.ListBox();
            this.LabelFilesData = new System.Windows.Forms.Label();
            this.StaticLabelSeparatorA = new System.Windows.Forms.Label();
            this.StaticLabelBitDepth = new System.Windows.Forms.Label();
            this.ComboBoxBitDepths = new System.Windows.Forms.ComboBox();
            this.StaticLabelColorMode = new System.Windows.Forms.Label();
            this.RadioGrayscale = new System.Windows.Forms.RadioButton();
            this.RadioRGB = new System.Windows.Forms.RadioButton();
            this.RadioARGB = new System.Windows.Forms.RadioButton();
            this.RadioPaletted = new System.Windows.Forms.RadioButton();
            this.ButtonEditPalette = new System.Windows.Forms.Button();
            this.StaticLabelImageWidth = new System.Windows.Forms.Label();
            this.StaticLabelImageHeight = new System.Windows.Forms.Label();
            this.TextBoxImageWidth = new System.Windows.Forms.TextBox();
            this.TextBoxImageHeight = new System.Windows.Forms.TextBox();
            this.LabelImageData = new System.Windows.Forms.Label();
            this.LabelOutputFolder = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ButtonSelectOutputFolder = new System.Windows.Forms.Button();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonGenerate = new System.Windows.Forms.Button();
            this.StaticLabelSeparatorB = new System.Windows.Forms.Label();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // StaticLabelFiles
            // 
            this.StaticLabelFiles.AutoSize = true;
            this.StaticLabelFiles.Location = new System.Drawing.Point(13, 13);
            this.StaticLabelFiles.Name = "StaticLabelFiles";
            this.StaticLabelFiles.Size = new System.Drawing.Size(33, 13);
            this.StaticLabelFiles.TabIndex = 0;
            this.StaticLabelFiles.Text = "Files:";
            // 
            // TextBoxFileEntry
            // 
            this.TextBoxFileEntry.Location = new System.Drawing.Point(52, 10);
            this.TextBoxFileEntry.Name = "TextBoxFileEntry";
            this.TextBoxFileEntry.Size = new System.Drawing.Size(313, 22);
            this.TextBoxFileEntry.TabIndex = 1;
            // 
            // ButtonAddFolder
            // 
            this.ButtonAddFolder.Location = new System.Drawing.Point(452, 8);
            this.ButtonAddFolder.Name = "ButtonAddFolder";
            this.ButtonAddFolder.Size = new System.Drawing.Size(84, 23);
            this.ButtonAddFolder.TabIndex = 2;
            this.ButtonAddFolder.Text = "Add F&older...";
            this.ButtonAddFolder.UseVisualStyleBackColor = true;
            // 
            // ButtonAddFiles
            // 
            this.ButtonAddFiles.Location = new System.Drawing.Point(371, 8);
            this.ButtonAddFiles.Name = "ButtonAddFiles";
            this.ButtonAddFiles.Size = new System.Drawing.Size(75, 23);
            this.ButtonAddFiles.TabIndex = 3;
            this.ButtonAddFiles.Text = "Add &Files...";
            this.ButtonAddFiles.UseVisualStyleBackColor = true;
            // 
            // ListBoxFiles
            // 
            this.ListBoxFiles.FormattingEnabled = true;
            this.ListBoxFiles.Location = new System.Drawing.Point(16, 38);
            this.ListBoxFiles.Name = "ListBoxFiles";
            this.ListBoxFiles.Size = new System.Drawing.Size(520, 82);
            this.ListBoxFiles.TabIndex = 4;
            // 
            // LabelFilesData
            // 
            this.LabelFilesData.AutoSize = true;
            this.LabelFilesData.Location = new System.Drawing.Point(12, 127);
            this.LabelFilesData.Name = "LabelFilesData";
            this.LabelFilesData.Size = new System.Drawing.Size(171, 13);
            this.LabelFilesData.TabIndex = 5;
            this.LabelFilesData.Text = "{0} files loaded. Total size: {1} {2}.";
            // 
            // StaticLabelSeparatorA
            // 
            this.StaticLabelSeparatorA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StaticLabelSeparatorA.Location = new System.Drawing.Point(16, 144);
            this.StaticLabelSeparatorA.Name = "StaticLabelSeparatorA";
            this.StaticLabelSeparatorA.Size = new System.Drawing.Size(520, 1);
            this.StaticLabelSeparatorA.TabIndex = 6;
            // 
            // StaticLabelBitDepth
            // 
            this.StaticLabelBitDepth.AutoSize = true;
            this.StaticLabelBitDepth.Location = new System.Drawing.Point(13, 149);
            this.StaticLabelBitDepth.Name = "StaticLabelBitDepth";
            this.StaticLabelBitDepth.Size = new System.Drawing.Size(59, 13);
            this.StaticLabelBitDepth.TabIndex = 7;
            this.StaticLabelBitDepth.Text = "Bit Depth:";
            // 
            // ComboBoxBitDepths
            // 
            this.ComboBoxBitDepths.FormattingEnabled = true;
            this.ComboBoxBitDepths.Items.AddRange(new object[] {
            "1bpp",
            "2bpp",
            "4bpp",
            "8bpp",
            "16bpp",
            "24bpp",
            "32bpp"});
            this.ComboBoxBitDepths.Location = new System.Drawing.Point(78, 146);
            this.ComboBoxBitDepths.Name = "ComboBoxBitDepths";
            this.ComboBoxBitDepths.Size = new System.Drawing.Size(121, 21);
            this.ComboBoxBitDepths.TabIndex = 8;
            // 
            // StaticLabelColorMode
            // 
            this.StaticLabelColorMode.AutoSize = true;
            this.StaticLabelColorMode.Location = new System.Drawing.Point(220, 149);
            this.StaticLabelColorMode.Name = "StaticLabelColorMode";
            this.StaticLabelColorMode.Size = new System.Drawing.Size(71, 13);
            this.StaticLabelColorMode.TabIndex = 9;
            this.StaticLabelColorMode.Text = "Color Mode:";
            // 
            // RadioGrayscale
            // 
            this.RadioGrayscale.AutoSize = true;
            this.RadioGrayscale.Location = new System.Drawing.Point(297, 147);
            this.RadioGrayscale.Name = "RadioGrayscale";
            this.RadioGrayscale.Size = new System.Drawing.Size(73, 17);
            this.RadioGrayscale.TabIndex = 10;
            this.RadioGrayscale.TabStop = true;
            this.RadioGrayscale.Text = "&Grayscale";
            this.RadioGrayscale.UseVisualStyleBackColor = true;
            // 
            // RadioRGB
            // 
            this.RadioRGB.AutoSize = true;
            this.RadioRGB.Location = new System.Drawing.Point(371, 149);
            this.RadioRGB.Name = "RadioRGB";
            this.RadioRGB.Size = new System.Drawing.Size(47, 17);
            this.RadioRGB.TabIndex = 11;
            this.RadioRGB.TabStop = true;
            this.RadioRGB.Text = "&RGB";
            this.RadioRGB.UseVisualStyleBackColor = true;
            // 
            // RadioARGB
            // 
            this.RadioARGB.AutoSize = true;
            this.RadioARGB.Location = new System.Drawing.Point(297, 170);
            this.RadioARGB.Name = "RadioARGB";
            this.RadioARGB.Size = new System.Drawing.Size(54, 17);
            this.RadioARGB.TabIndex = 12;
            this.RadioARGB.TabStop = true;
            this.RadioARGB.Text = "&ARGB";
            this.RadioARGB.UseVisualStyleBackColor = true;
            // 
            // RadioPaletted
            // 
            this.RadioPaletted.AutoSize = true;
            this.RadioPaletted.Location = new System.Drawing.Point(371, 170);
            this.RadioPaletted.Name = "RadioPaletted";
            this.RadioPaletted.Size = new System.Drawing.Size(67, 17);
            this.RadioPaletted.TabIndex = 13;
            this.RadioPaletted.TabStop = true;
            this.RadioPaletted.Text = "&Paletted";
            this.RadioPaletted.UseVisualStyleBackColor = true;
            // 
            // ButtonEditPalette
            // 
            this.ButtonEditPalette.Location = new System.Drawing.Point(444, 149);
            this.ButtonEditPalette.Name = "ButtonEditPalette";
            this.ButtonEditPalette.Size = new System.Drawing.Size(92, 23);
            this.ButtonEditPalette.TabIndex = 14;
            this.ButtonEditPalette.Text = "&Edit Palette...";
            this.ButtonEditPalette.UseVisualStyleBackColor = true;
            // 
            // StaticLabelImageWidth
            // 
            this.StaticLabelImageWidth.AutoSize = true;
            this.StaticLabelImageWidth.Location = new System.Drawing.Point(12, 194);
            this.StaticLabelImageWidth.Name = "StaticLabelImageWidth";
            this.StaticLabelImageWidth.Size = new System.Drawing.Size(76, 13);
            this.StaticLabelImageWidth.TabIndex = 15;
            this.StaticLabelImageWidth.Text = "Image Width:";
            // 
            // StaticLabelImageHeight
            // 
            this.StaticLabelImageHeight.AutoSize = true;
            this.StaticLabelImageHeight.Location = new System.Drawing.Point(12, 221);
            this.StaticLabelImageHeight.Name = "StaticLabelImageHeight";
            this.StaticLabelImageHeight.Size = new System.Drawing.Size(79, 13);
            this.StaticLabelImageHeight.TabIndex = 16;
            this.StaticLabelImageHeight.Text = "Image Height:";
            // 
            // TextBoxImageWidth
            // 
            this.TextBoxImageWidth.Location = new System.Drawing.Point(94, 191);
            this.TextBoxImageWidth.Name = "TextBoxImageWidth";
            this.TextBoxImageWidth.Size = new System.Drawing.Size(100, 22);
            this.TextBoxImageWidth.TabIndex = 17;
            this.TextBoxImageWidth.Text = "640";
            // 
            // TextBoxImageHeight
            // 
            this.TextBoxImageHeight.Location = new System.Drawing.Point(94, 218);
            this.TextBoxImageHeight.Name = "TextBoxImageHeight";
            this.TextBoxImageHeight.Size = new System.Drawing.Size(100, 22);
            this.TextBoxImageHeight.TabIndex = 18;
            this.TextBoxImageHeight.Text = "480";
            // 
            // LabelImageData
            // 
            this.LabelImageData.AutoSize = true;
            this.LabelImageData.Location = new System.Drawing.Point(94, 247);
            this.LabelImageData.Name = "LabelImageData";
            this.LabelImageData.Size = new System.Drawing.Size(113, 13);
            this.LabelImageData.TabIndex = 19;
            this.LabelImageData.Text = "{0} pixels. Size: {1} {2}.";
            // 
            // LabelOutputFolder
            // 
            this.LabelOutputFolder.AutoSize = true;
            this.LabelOutputFolder.Location = new System.Drawing.Point(207, 198);
            this.LabelOutputFolder.Name = "LabelOutputFolder";
            this.LabelOutputFolder.Size = new System.Drawing.Size(84, 13);
            this.LabelOutputFolder.TabIndex = 20;
            this.LabelOutputFolder.Text = "Output Folder:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(297, 194);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 22);
            this.textBox1.TabIndex = 21;
            // 
            // ButtonSelectOutputFolder
            // 
            this.ButtonSelectOutputFolder.Location = new System.Drawing.Point(461, 193);
            this.ButtonSelectOutputFolder.Name = "ButtonSelectOutputFolder";
            this.ButtonSelectOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelectOutputFolder.TabIndex = 22;
            this.ButtonSelectOutputFolder.Text = "...";
            this.ButtonSelectOutputFolder.UseVisualStyleBackColor = true;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(461, 319);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 23;
            this.ButtonClose.Text = "&Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            // 
            // ButtonStop
            // 
            this.ButtonStop.Enabled = false;
            this.ButtonStop.Location = new System.Drawing.Point(381, 319);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 24;
            this.ButtonStop.Text = "&Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            // 
            // ButtonGenerate
            // 
            this.ButtonGenerate.Location = new System.Drawing.Point(266, 319);
            this.ButtonGenerate.Name = "ButtonGenerate";
            this.ButtonGenerate.Size = new System.Drawing.Size(109, 23);
            this.ButtonGenerate.TabIndex = 25;
            this.ButtonGenerate.Text = "Generate Pictures";
            this.ButtonGenerate.UseVisualStyleBackColor = true;
            // 
            // StaticLabelSeparatorB
            // 
            this.StaticLabelSeparatorB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StaticLabelSeparatorB.Location = new System.Drawing.Point(14, 267);
            this.StaticLabelSeparatorB.Name = "StaticLabelSeparatorB";
            this.StaticLabelSeparatorB.Size = new System.Drawing.Size(520, 1);
            this.StaticLabelSeparatorB.TabIndex = 26;
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.Location = new System.Drawing.Point(16, 272);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(57, 13);
            this.LabelStatus.TabIndex = 27;
            this.LabelStatus.Text = "Waiting...";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(19, 289);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(515, 24);
            this.Progress.TabIndex = 28;
            // 
            // TextBoxOutputFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 354);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.StaticLabelSeparatorB);
            this.Controls.Add(this.ButtonGenerate);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.ButtonSelectOutputFolder);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.LabelOutputFolder);
            this.Controls.Add(this.LabelImageData);
            this.Controls.Add(this.TextBoxImageHeight);
            this.Controls.Add(this.TextBoxImageWidth);
            this.Controls.Add(this.StaticLabelImageHeight);
            this.Controls.Add(this.StaticLabelImageWidth);
            this.Controls.Add(this.ButtonEditPalette);
            this.Controls.Add(this.RadioPaletted);
            this.Controls.Add(this.RadioARGB);
            this.Controls.Add(this.RadioRGB);
            this.Controls.Add(this.RadioGrayscale);
            this.Controls.Add(this.StaticLabelColorMode);
            this.Controls.Add(this.ComboBoxBitDepths);
            this.Controls.Add(this.StaticLabelBitDepth);
            this.Controls.Add(this.StaticLabelSeparatorA);
            this.Controls.Add(this.LabelFilesData);
            this.Controls.Add(this.ListBoxFiles);
            this.Controls.Add(this.ButtonAddFiles);
            this.Controls.Add(this.ButtonAddFolder);
            this.Controls.Add(this.TextBoxFileEntry);
            this.Controls.Add(this.StaticLabelFiles);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TextBoxOutputFolder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Large File Processor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StaticLabelFiles;
        private System.Windows.Forms.TextBox TextBoxFileEntry;
        private System.Windows.Forms.Button ButtonAddFolder;
        private System.Windows.Forms.Button ButtonAddFiles;
        private System.Windows.Forms.ListBox ListBoxFiles;
        private System.Windows.Forms.Label LabelFilesData;
        private System.Windows.Forms.Label StaticLabelSeparatorA;
        private System.Windows.Forms.Label StaticLabelBitDepth;
        private System.Windows.Forms.ComboBox ComboBoxBitDepths;
        private System.Windows.Forms.Label StaticLabelColorMode;
        private System.Windows.Forms.RadioButton RadioGrayscale;
        private System.Windows.Forms.RadioButton RadioRGB;
        private System.Windows.Forms.RadioButton RadioARGB;
        private System.Windows.Forms.RadioButton RadioPaletted;
        private System.Windows.Forms.Button ButtonEditPalette;
        private System.Windows.Forms.Label StaticLabelImageWidth;
        private System.Windows.Forms.Label StaticLabelImageHeight;
        private System.Windows.Forms.TextBox TextBoxImageWidth;
        private System.Windows.Forms.TextBox TextBoxImageHeight;
        private System.Windows.Forms.Label LabelImageData;
        private System.Windows.Forms.Label LabelOutputFolder;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ButtonSelectOutputFolder;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonGenerate;
        private System.Windows.Forms.Label StaticLabelSeparatorB;
        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.ProgressBar Progress;
    }
}