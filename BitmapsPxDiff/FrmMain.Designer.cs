namespace BitmapsPxDiff
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pb = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadImage2 = new System.Windows.Forms.Button();
            this.btnLoadImage1 = new System.Windows.Forms.Button();
            this.gbPreviewMode = new System.Windows.Forms.GroupBox();
            this.tbPreviewModeResult = new System.Windows.Forms.RadioButton();
            this.rbPreviewModeImg2 = new System.Windows.Forms.RadioButton();
            this.rbPreviewModeImg1 = new System.Windows.Forms.RadioButton();
            this.btnSwapImages = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadScript = new System.Windows.Forms.Button();
            this.btnSaveScript = new System.Windows.Forms.Button();
            this.btnSaveResultImage = new System.Windows.Forms.Button();
            this.tbScriptInput = new System.Windows.Forms.TextBox();
            this.tbScriptOutput = new System.Windows.Forms.TextBox();
            this.odLoadImage = new System.Windows.Forms.OpenFileDialog();
            this.odLoadScript = new System.Windows.Forms.OpenFileDialog();
            this.sdSaveResultImage = new System.Windows.Forms.SaveFileDialog();
            this.sdSaveScript = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbPreviewMode.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pb, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 671);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pb
            // 
            this.pb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb.Location = new System.Drawing.Point(336, 16);
            this.pb.Margin = new System.Windows.Forms.Padding(16);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(448, 639);
            this.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb.TabIndex = 1;
            this.pb.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnLoadImage2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnLoadImage1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbPreviewMode, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnSwapImages, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnSaveResultImage, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.tbScriptInput, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbScriptOutput, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(314, 665);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnLoadImage2
            // 
            this.btnLoadImage2.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadImage2.Location = new System.Drawing.Point(16, 71);
            this.btnLoadImage2.Margin = new System.Windows.Forms.Padding(16);
            this.btnLoadImage2.Name = "btnLoadImage2";
            this.btnLoadImage2.Size = new System.Drawing.Size(282, 23);
            this.btnLoadImage2.TabIndex = 1;
            this.btnLoadImage2.Text = "Load image 2";
            this.btnLoadImage2.UseVisualStyleBackColor = true;
            this.btnLoadImage2.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnLoadImage1
            // 
            this.btnLoadImage1.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadImage1.Location = new System.Drawing.Point(16, 16);
            this.btnLoadImage1.Margin = new System.Windows.Forms.Padding(16);
            this.btnLoadImage1.Name = "btnLoadImage1";
            this.btnLoadImage1.Size = new System.Drawing.Size(282, 23);
            this.btnLoadImage1.TabIndex = 0;
            this.btnLoadImage1.Text = "Load image 1";
            this.btnLoadImage1.UseVisualStyleBackColor = true;
            this.btnLoadImage1.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // gbPreviewMode
            // 
            this.gbPreviewMode.Controls.Add(this.tbPreviewModeResult);
            this.gbPreviewMode.Controls.Add(this.rbPreviewModeImg2);
            this.gbPreviewMode.Controls.Add(this.rbPreviewModeImg1);
            this.gbPreviewMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPreviewMode.Location = new System.Drawing.Point(3, 168);
            this.gbPreviewMode.Name = "gbPreviewMode";
            this.gbPreviewMode.Size = new System.Drawing.Size(308, 105);
            this.gbPreviewMode.TabIndex = 3;
            this.gbPreviewMode.TabStop = false;
            this.gbPreviewMode.Text = "Preview mode";
            // 
            // tbPreviewModeResult
            // 
            this.tbPreviewModeResult.AutoSize = true;
            this.tbPreviewModeResult.Location = new System.Drawing.Point(13, 72);
            this.tbPreviewModeResult.Name = "tbPreviewModeResult";
            this.tbPreviewModeResult.Size = new System.Drawing.Size(93, 19);
            this.tbPreviewModeResult.TabIndex = 2;
            this.tbPreviewModeResult.TabStop = true;
            this.tbPreviewModeResult.Text = "Result image";
            this.tbPreviewModeResult.UseVisualStyleBackColor = true;
            this.tbPreviewModeResult.CheckedChanged += new System.EventHandler(this.rbPreviewModeImg_CheckedChanged);
            // 
            // rbPreviewModeImg2
            // 
            this.rbPreviewModeImg2.AutoSize = true;
            this.rbPreviewModeImg2.Location = new System.Drawing.Point(13, 47);
            this.rbPreviewModeImg2.Name = "rbPreviewModeImg2";
            this.rbPreviewModeImg2.Size = new System.Drawing.Size(67, 19);
            this.rbPreviewModeImg2.TabIndex = 1;
            this.rbPreviewModeImg2.TabStop = true;
            this.rbPreviewModeImg2.Text = "Image 2";
            this.rbPreviewModeImg2.UseVisualStyleBackColor = true;
            this.rbPreviewModeImg2.CheckedChanged += new System.EventHandler(this.rbPreviewModeImg_CheckedChanged);
            // 
            // rbPreviewModeImg1
            // 
            this.rbPreviewModeImg1.AutoSize = true;
            this.rbPreviewModeImg1.Checked = true;
            this.rbPreviewModeImg1.Location = new System.Drawing.Point(13, 22);
            this.rbPreviewModeImg1.Name = "rbPreviewModeImg1";
            this.rbPreviewModeImg1.Size = new System.Drawing.Size(67, 19);
            this.rbPreviewModeImg1.TabIndex = 0;
            this.rbPreviewModeImg1.TabStop = true;
            this.rbPreviewModeImg1.Text = "Image 1";
            this.rbPreviewModeImg1.UseVisualStyleBackColor = true;
            this.rbPreviewModeImg1.CheckedChanged += new System.EventHandler(this.rbPreviewModeImg_CheckedChanged);
            // 
            // btnSwapImages
            // 
            this.btnSwapImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSwapImages.Location = new System.Drawing.Point(16, 126);
            this.btnSwapImages.Margin = new System.Windows.Forms.Padding(16);
            this.btnSwapImages.Name = "btnSwapImages";
            this.btnSwapImages.Size = new System.Drawing.Size(282, 23);
            this.btnSwapImages.TabIndex = 4;
            this.btnSwapImages.Text = "Swap images";
            this.btnSwapImages.UseVisualStyleBackColor = true;
            this.btnSwapImages.Click += new System.EventHandler(this.btnSwapImages_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnLoadScript, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSaveScript, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 554);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(314, 56);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // btnLoadScript
            // 
            this.btnLoadScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadScript.Location = new System.Drawing.Point(16, 16);
            this.btnLoadScript.Margin = new System.Windows.Forms.Padding(16);
            this.btnLoadScript.Name = "btnLoadScript";
            this.btnLoadScript.Size = new System.Drawing.Size(125, 23);
            this.btnLoadScript.TabIndex = 0;
            this.btnLoadScript.Text = "Load script";
            this.btnLoadScript.UseVisualStyleBackColor = true;
            this.btnLoadScript.Click += new System.EventHandler(this.btnLoadScript_Click);
            // 
            // btnSaveScript
            // 
            this.btnSaveScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveScript.Location = new System.Drawing.Point(173, 16);
            this.btnSaveScript.Margin = new System.Windows.Forms.Padding(16);
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.Size = new System.Drawing.Size(125, 23);
            this.btnSaveScript.TabIndex = 1;
            this.btnSaveScript.Text = "Save script";
            this.btnSaveScript.UseVisualStyleBackColor = true;
            this.btnSaveScript.Click += new System.EventHandler(this.btnSaveScript_Click);
            // 
            // btnSaveResultImage
            // 
            this.btnSaveResultImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveResultImage.Location = new System.Drawing.Point(16, 626);
            this.btnSaveResultImage.Margin = new System.Windows.Forms.Padding(16);
            this.btnSaveResultImage.Name = "btnSaveResultImage";
            this.btnSaveResultImage.Size = new System.Drawing.Size(282, 23);
            this.btnSaveResultImage.TabIndex = 6;
            this.btnSaveResultImage.Text = "Save result image";
            this.btnSaveResultImage.UseVisualStyleBackColor = true;
            this.btnSaveResultImage.Click += new System.EventHandler(this.btnSaveResultImage_Click);
            // 
            // tbScriptInput
            // 
            this.tbScriptInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScriptInput.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbScriptInput.Location = new System.Drawing.Point(3, 279);
            this.tbScriptInput.Multiline = true;
            this.tbScriptInput.Name = "tbScriptInput";
            this.tbScriptInput.Size = new System.Drawing.Size(308, 208);
            this.tbScriptInput.TabIndex = 7;
            this.tbScriptInput.Text = "resultA = 255\r\nresultR = image1R + 1\r\nresultG = image1G + 1\r\nresultB = image1B + " +
    "1";
            this.tbScriptInput.TextChanged += new System.EventHandler(this.tbScriptInput_TextChanged);
            // 
            // tbScriptOutput
            // 
            this.tbScriptOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScriptOutput.Location = new System.Drawing.Point(3, 493);
            this.tbScriptOutput.Multiline = true;
            this.tbScriptOutput.Name = "tbScriptOutput";
            this.tbScriptOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScriptOutput.Size = new System.Drawing.Size(308, 58);
            this.tbScriptOutput.TabIndex = 8;
            this.tbScriptOutput.Text = " \r\n";
            // 
            // odLoadImage
            // 
            this.odLoadImage.Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF|All files (*.*)|*.*";
            // 
            // odLoadScript
            // 
            this.odLoadScript.Filter = "Script Files(*.TXT;*.LUA)|*.TXT;*.LUA|All files (*.*)|*.*";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 671);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.gbPreviewMode.ResumeLayout(false);
            this.gbPreviewMode.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pb;
        private OpenFileDialog odLoadImage;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnLoadImage2;
        private Button btnLoadImage1;
        private GroupBox gbPreviewMode;
        private RadioButton tbPreviewModeResult;
        private RadioButton rbPreviewModeImg2;
        private RadioButton rbPreviewModeImg1;
        private Button btnSwapImages;
        private TableLayoutPanel tableLayoutPanel3;
        private Button btnLoadScript;
        private Button btnSaveScript;
        private Button btnSaveResultImage;
        private OpenFileDialog odLoadScript;
        private SaveFileDialog sdSaveResultImage;
        private SaveFileDialog sdSaveScript;
        private TextBox tbScriptInput;
        private TextBox tbScriptOutput;
    }
}