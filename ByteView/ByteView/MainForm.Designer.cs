namespace ByteView
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSBOpenFiles = new System.Windows.Forms.ToolStripButton();
            this.TSBOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.TSBSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSBRefresh = new System.Windows.Forms.ToolStripButton();
            this.TSBCancel = new System.Windows.Forms.ToolStripButton();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.StaticLabelBitDepth = new System.Windows.Forms.Label();
            this.Worker = new System.ComponentModel.BackgroundWorker();
            this.ComboBitDepths = new System.Windows.Forms.ComboBox();
            this.StaticLabelColorMode = new System.Windows.Forms.Label();
            this.RadioGrayscale = new System.Windows.Forms.RadioButton();
            this.RadioRGB = new System.Windows.Forms.RadioButton();
            this.RadioARGB = new System.Windows.Forms.RadioButton();
            this.RadioPaletted = new System.Windows.Forms.RadioButton();
            this.ButtonPalette = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Image = new System.Windows.Forms.PictureBox();
            this.FolderSelector = new System.Windows.Forms.FolderBrowserDialog();
            this.TSBOpenPicture = new System.Windows.Forms.ToolStripButton();
            this.OpenPicture = new System.Windows.Forms.OpenFileDialog();
            this.TSBOpenRaw = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSBOpenFiles,
            this.TSBOpenPicture,
            this.TSBOpenFolder,
            this.TSBOpenRaw,
            this.TSBSaveAs,
            this.toolStripSeparator1,
            this.TSBRefresh,
            this.TSBCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(624, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TSBOpenFiles
            // 
            this.TSBOpenFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSBOpenFiles.Image = ((System.Drawing.Image)(resources.GetObject("TSBOpenFiles.Image")));
            this.TSBOpenFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBOpenFiles.Name = "TSBOpenFiles";
            this.TSBOpenFiles.Size = new System.Drawing.Size(99, 22);
            this.TSBOpenFiles.Text = "&Open File(s)...";
            this.TSBOpenFiles.Click += new System.EventHandler(this.TSBOpenFiles_Click);
            // 
            // TSBOpenFolder
            // 
            this.TSBOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("TSBOpenFolder.Image")));
            this.TSBOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBOpenFolder.Name = "TSBOpenFolder";
            this.TSBOpenFolder.Size = new System.Drawing.Size(101, 22);
            this.TSBOpenFolder.Text = "&Open Folder...";
            this.TSBOpenFolder.Click += new System.EventHandler(this.TSBOpenFolder_Click);
            // 
            // TSBSaveAs
            // 
            this.TSBSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("TSBSaveAs.Image")));
            this.TSBSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBSaveAs.Name = "TSBSaveAs";
            this.TSBSaveAs.Size = new System.Drawing.Size(76, 22);
            this.TSBSaveAs.Text = "Save &As...";
            this.TSBSaveAs.Click += new System.EventHandler(this.TSBSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TSBRefresh
            // 
            this.TSBRefresh.Image = ((System.Drawing.Image)(resources.GetObject("TSBRefresh.Image")));
            this.TSBRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBRefresh.Name = "TSBRefresh";
            this.TSBRefresh.Size = new System.Drawing.Size(66, 22);
            this.TSBRefresh.Text = "&Refresh";
            this.TSBRefresh.Click += new System.EventHandler(this.TSBRefresh_Click);
            // 
            // TSBCancel
            // 
            this.TSBCancel.Image = ((System.Drawing.Image)(resources.GetObject("TSBCancel.Image")));
            this.TSBCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBCancel.Name = "TSBCancel";
            this.TSBCancel.Size = new System.Drawing.Size(63, 22);
            this.TSBCancel.Text = "&Cancel";
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.Location = new System.Drawing.Point(13, 394);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(599, 12);
            this.Progress.TabIndex = 2;
            // 
            // StaticLabelBitDepth
            // 
            this.StaticLabelBitDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StaticLabelBitDepth.AutoSize = true;
            this.StaticLabelBitDepth.Location = new System.Drawing.Point(13, 416);
            this.StaticLabelBitDepth.Name = "StaticLabelBitDepth";
            this.StaticLabelBitDepth.Size = new System.Drawing.Size(59, 13);
            this.StaticLabelBitDepth.TabIndex = 3;
            this.StaticLabelBitDepth.Text = "Bit Depth:";
            // 
            // Worker
            // 
            this.Worker.WorkerReportsProgress = true;
            this.Worker.WorkerSupportsCancellation = true;
            this.Worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
            this.Worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
            this.Worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
            // 
            // ComboBitDepths
            // 
            this.ComboBitDepths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ComboBitDepths.FormattingEnabled = true;
            this.ComboBitDepths.Items.AddRange(new object[] {
            "1bpp",
            "2bpp",
            "4bpp",
            "8bpp",
            "16bpp",
            "24bpp",
            "32bpp"});
            this.ComboBitDepths.Location = new System.Drawing.Point(69, 413);
            this.ComboBitDepths.Name = "ComboBitDepths";
            this.ComboBitDepths.Size = new System.Drawing.Size(112, 21);
            this.ComboBitDepths.TabIndex = 4;
            this.ComboBitDepths.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // StaticLabelColorMode
            // 
            this.StaticLabelColorMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StaticLabelColorMode.AutoSize = true;
            this.StaticLabelColorMode.Location = new System.Drawing.Point(187, 416);
            this.StaticLabelColorMode.Name = "StaticLabelColorMode";
            this.StaticLabelColorMode.Size = new System.Drawing.Size(71, 13);
            this.StaticLabelColorMode.TabIndex = 5;
            this.StaticLabelColorMode.Text = "Color Mode:";
            // 
            // RadioGrayscale
            // 
            this.RadioGrayscale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioGrayscale.AutoSize = true;
            this.RadioGrayscale.Location = new System.Drawing.Point(264, 414);
            this.RadioGrayscale.Name = "RadioGrayscale";
            this.RadioGrayscale.Size = new System.Drawing.Size(73, 17);
            this.RadioGrayscale.TabIndex = 6;
            this.RadioGrayscale.TabStop = true;
            this.RadioGrayscale.Text = "&Grayscale";
            this.RadioGrayscale.UseVisualStyleBackColor = true;
            this.RadioGrayscale.CheckedChanged += new System.EventHandler(this.RadioGrayscale_CheckedChanged);
            // 
            // RadioRGB
            // 
            this.RadioRGB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioRGB.AutoSize = true;
            this.RadioRGB.Location = new System.Drawing.Point(343, 414);
            this.RadioRGB.Name = "RadioRGB";
            this.RadioRGB.Size = new System.Drawing.Size(47, 17);
            this.RadioRGB.TabIndex = 7;
            this.RadioRGB.TabStop = true;
            this.RadioRGB.Text = "&RGB";
            this.RadioRGB.UseVisualStyleBackColor = true;
            this.RadioRGB.CheckedChanged += new System.EventHandler(this.RadioRGB_CheckedChanged);
            // 
            // RadioARGB
            // 
            this.RadioARGB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioARGB.AutoSize = true;
            this.RadioARGB.Location = new System.Drawing.Point(396, 414);
            this.RadioARGB.Name = "RadioARGB";
            this.RadioARGB.Size = new System.Drawing.Size(54, 17);
            this.RadioARGB.TabIndex = 8;
            this.RadioARGB.TabStop = true;
            this.RadioARGB.Text = "ARG&B";
            this.RadioARGB.UseVisualStyleBackColor = true;
            this.RadioARGB.CheckedChanged += new System.EventHandler(this.RadioARGB_CheckedChanged);
            // 
            // RadioPaletted
            // 
            this.RadioPaletted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioPaletted.AutoSize = true;
            this.RadioPaletted.Location = new System.Drawing.Point(457, 414);
            this.RadioPaletted.Name = "RadioPaletted";
            this.RadioPaletted.Size = new System.Drawing.Size(67, 17);
            this.RadioPaletted.TabIndex = 9;
            this.RadioPaletted.TabStop = true;
            this.RadioPaletted.Text = "&Paletted";
            this.RadioPaletted.UseVisualStyleBackColor = true;
            this.RadioPaletted.CheckedChanged += new System.EventHandler(this.RadioPaletted_CheckedChanged);
            // 
            // ButtonPalette
            // 
            this.ButtonPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPalette.Location = new System.Drawing.Point(530, 411);
            this.ButtonPalette.Name = "ButtonPalette";
            this.ButtonPalette.Size = new System.Drawing.Size(82, 23);
            this.ButtonPalette.TabIndex = 10;
            this.ButtonPalette.Text = "&Palette...";
            this.ButtonPalette.UseVisualStyleBackColor = true;
            // 
            // OpenFile
            // 
            this.OpenFile.Filter = "All files|*.*";
            this.OpenFile.Multiselect = true;
            this.OpenFile.SupportMultiDottedExtensions = true;
            this.OpenFile.Title = "Open Files";
            // 
            // SaveFile
            // 
            this.SaveFile.DefaultExt = "png";
            this.SaveFile.Filter = "PNG Image|*.png|JPEG Image|*.jpg|GIF Image|*.gif|Bitmap|*.bmp|Raw File|*.raw";
            this.SaveFile.Title = "Save File As";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.Image);
            this.panel1.Location = new System.Drawing.Point(13, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 359);
            this.panel1.TabIndex = 11;
            this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
            // 
            // Image
            // 
            this.Image.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Image.Location = new System.Drawing.Point(0, 0);
            this.Image.Name = "Image";
            this.Image.Size = new System.Drawing.Size(599, 359);
            this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Image.TabIndex = 0;
            this.Image.TabStop = false;
            // 
            // FolderSelector
            // 
            this.FolderSelector.Description = "Select a folder to load files from. WARNING: Operates recursively. I wouldn\'t sug" +
    "gest selecting a folder with thousands of files.";
            // 
            // TSBOpenPicture
            // 
            this.TSBOpenPicture.Image = ((System.Drawing.Image)(resources.GetObject("TSBOpenPicture.Image")));
            this.TSBOpenPicture.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBOpenPicture.Name = "TSBOpenPicture";
            this.TSBOpenPicture.Size = new System.Drawing.Size(105, 22);
            this.TSBOpenPicture.Text = "Open &Picture...";
            this.TSBOpenPicture.Click += new System.EventHandler(this.TSBOpenPicture_Click);
            // 
            // OpenPicture
            // 
            this.OpenPicture.FileName = "openFileDialog1";
            this.OpenPicture.Filter = "JPEG Image|*.jpg|GIF Image|*.gif|PNG Image|*.png|Bitmap|*.bmp|All files|*.*";
            this.OpenPicture.Title = "Open Picture";
            // 
            // TSBOpenRaw
            // 
            this.TSBOpenRaw.Image = ((System.Drawing.Image)(resources.GetObject("TSBOpenRaw.Image")));
            this.TSBOpenRaw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSBOpenRaw.Name = "TSBOpenRaw";
            this.TSBOpenRaw.Size = new System.Drawing.Size(81, 22);
            this.TSBOpenRaw.Text = "Open &Raw";
            this.TSBOpenRaw.Click += new System.EventHandler(this.TSBOpenRaw_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ButtonPalette);
            this.Controls.Add(this.RadioPaletted);
            this.Controls.Add(this.RadioARGB);
            this.Controls.Add(this.RadioRGB);
            this.Controls.Add(this.RadioGrayscale);
            this.Controls.Add(this.StaticLabelColorMode);
            this.Controls.Add(this.ComboBitDepths);
            this.Controls.Add(this.StaticLabelBitDepth);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "ByteView";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TSBOpenFiles;
        private System.Windows.Forms.ToolStripButton TSBOpenFolder;
        private System.Windows.Forms.ToolStripButton TSBSaveAs;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton TSBRefresh;
        private System.Windows.Forms.ToolStripButton TSBCancel;
        private System.Windows.Forms.Label StaticLabelBitDepth;
        private System.ComponentModel.BackgroundWorker Worker;
        private System.Windows.Forms.ComboBox ComboBitDepths;
        private System.Windows.Forms.Label StaticLabelColorMode;
        private System.Windows.Forms.RadioButton RadioGrayscale;
        private System.Windows.Forms.RadioButton RadioRGB;
        private System.Windows.Forms.RadioButton RadioARGB;
        private System.Windows.Forms.RadioButton RadioPaletted;
        private System.Windows.Forms.Button ButtonPalette;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.SaveFileDialog SaveFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Image;
        private System.Windows.Forms.FolderBrowserDialog FolderSelector;
        private System.Windows.Forms.ToolStripButton TSBOpenPicture;
        private System.Windows.Forms.OpenFileDialog OpenPicture;
        private System.Windows.Forms.ToolStripButton TSBOpenRaw;
    }
}