using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
//using PictureBoxEx;
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
            this.pb = new BitmapsPxDiff.PictureBoxEx();
            this.tlbLeftTopPanels = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadImage1 = new System.Windows.Forms.Button();
            this.btnLoadImage2 = new System.Windows.Forms.Button();
            this.btnSwapImages = new System.Windows.Forms.Button();
            this.tlbRadioButtonsGroups = new System.Windows.Forms.TableLayoutPanel();
            this.gbPreviewMode = new System.Windows.Forms.GroupBox();
            this.rbPreviewModeResult = new System.Windows.Forms.RadioButton();
            this.rbPreviewModeImg2 = new System.Windows.Forms.RadioButton();
            this.rbPreviewModeImg1 = new System.Windows.Forms.RadioButton();
            this.gbInterpolationMode = new System.Windows.Forms.GroupBox();
            this.rbimOther = new System.Windows.Forms.RadioButton();
            this.rbimNearestNeighbour = new System.Windows.Forms.RadioButton();
            this.rbimDefault = new System.Windows.Forms.RadioButton();
            this.tbScriptInput = new System.Windows.Forms.TextBox();
            this.cbAutoRunScriptAfterChange = new System.Windows.Forms.CheckBox();
            this.btnRunStopScript = new System.Windows.Forms.Button();
            this.tlbLeftBottomPanels = new System.Windows.Forms.TableLayoutPanel();
            this.tbScriptOutput = new System.Windows.Forms.TextBox();
            this.tlbLoadSaveScript = new System.Windows.Forms.TableLayoutPanel();
            this.btnLoadScript = new System.Windows.Forms.Button();
            this.btnSaveScript = new System.Windows.Forms.Button();
            this.btnSaveResultImage = new System.Windows.Forms.Button();
            this.odLoadImage = new System.Windows.Forms.OpenFileDialog();
            this.odLoadScript = new System.Windows.Forms.OpenFileDialog();
            this.sdSaveResultImage = new System.Windows.Forms.SaveFileDialog();
            this.sdSaveScript = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCursorCoords = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslEmpty = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsddbSwitchPixelInfoFormat = new System.Windows.Forms.ToolStripDropDownButton();
            this.argbHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.argbDecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rgbHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rgbDecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsslImage1argb = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslImage2argb = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslImageResultargb = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelLeftTop = new System.Windows.Forms.Panel();
            this.leftPanelSplitter = new System.Windows.Forms.Splitter();
            this.panelLeftBottom = new System.Windows.Forms.Panel();
            this.mainSplitter = new System.Windows.Forms.Splitter();
            this.panelRight = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.tlbLeftTopPanels.SuspendLayout();
            this.tlbRadioButtonsGroups.SuspendLayout();
            this.gbPreviewMode.SuspendLayout();
            this.gbInterpolationMode.SuspendLayout();
            this.tlbLeftBottomPanels.SuspendLayout();
            this.tlbLoadSaveScript.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelLeftTop.SuspendLayout();
            this.panelLeftBottom.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb
            // 
            this.pb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb.Image = null;
            this.pb.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pb.Location = new System.Drawing.Point(16, 16);
            this.pb.Margin = new System.Windows.Forms.Padding(0);
            this.pb.Name = "pb";
            this.pb.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            this.pb.Size = new System.Drawing.Size(810, 638);
            this.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb.TabIndex = 1;
            this.pb.TabStop = false;
            this.pb.MouseLeave += new System.EventHandler(this.pb_MouseLeave);
            this.pb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_MouseMove);
            // 
            // tlbLeftTopPanels
            // 
            this.tlbLeftTopPanels.ColumnCount = 1;
            this.tlbLeftTopPanels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftTopPanels.Controls.Add(this.btnLoadImage1, 0, 0);
            this.tlbLeftTopPanels.Controls.Add(this.btnLoadImage2, 0, 1);
            this.tlbLeftTopPanels.Controls.Add(this.btnSwapImages, 0, 2);
            this.tlbLeftTopPanels.Controls.Add(this.tlbRadioButtonsGroups, 0, 3);
            this.tlbLeftTopPanels.Controls.Add(this.tbScriptInput, 0, 4);
            this.tlbLeftTopPanels.Controls.Add(this.cbAutoRunScriptAfterChange, 0, 5);
            this.tlbLeftTopPanels.Controls.Add(this.btnRunStopScript, 0, 6);
            this.tlbLeftTopPanels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbLeftTopPanels.Location = new System.Drawing.Point(0, 0);
            this.tlbLeftTopPanels.Name = "tlbLeftTopPanels";
            this.tlbLeftTopPanels.RowCount = 7;
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.Size = new System.Drawing.Size(370, 490);
            this.tlbLeftTopPanels.TabIndex = 2;
            // 
            // btnLoadImage1
            // 
            this.btnLoadImage1.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadImage1.Location = new System.Drawing.Point(4, 4);
            this.btnLoadImage1.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadImage1.Name = "btnLoadImage1";
            this.btnLoadImage1.Size = new System.Drawing.Size(362, 23);
            this.btnLoadImage1.TabIndex = 0;
            this.btnLoadImage1.Text = "Load image 1";
            this.btnLoadImage1.UseVisualStyleBackColor = true;
            this.btnLoadImage1.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnLoadImage2
            // 
            this.btnLoadImage2.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadImage2.Location = new System.Drawing.Point(4, 35);
            this.btnLoadImage2.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadImage2.Name = "btnLoadImage2";
            this.btnLoadImage2.Size = new System.Drawing.Size(362, 23);
            this.btnLoadImage2.TabIndex = 1;
            this.btnLoadImage2.Text = "Load image 2";
            this.btnLoadImage2.UseVisualStyleBackColor = true;
            this.btnLoadImage2.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnSwapImages
            // 
            this.btnSwapImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSwapImages.Location = new System.Drawing.Point(4, 66);
            this.btnSwapImages.Margin = new System.Windows.Forms.Padding(4);
            this.btnSwapImages.Name = "btnSwapImages";
            this.btnSwapImages.Size = new System.Drawing.Size(362, 23);
            this.btnSwapImages.TabIndex = 4;
            this.btnSwapImages.Text = "Swap images";
            this.btnSwapImages.UseVisualStyleBackColor = true;
            this.btnSwapImages.Click += new System.EventHandler(this.btnSwapImages_Click);
            // 
            // tlbRadioButtonsGroups
            // 
            this.tlbRadioButtonsGroups.ColumnCount = 2;
            this.tlbRadioButtonsGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbRadioButtonsGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbRadioButtonsGroups.Controls.Add(this.gbPreviewMode, 0, 0);
            this.tlbRadioButtonsGroups.Controls.Add(this.gbInterpolationMode, 1, 0);
            this.tlbRadioButtonsGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbRadioButtonsGroups.Location = new System.Drawing.Point(0, 93);
            this.tlbRadioButtonsGroups.Margin = new System.Windows.Forms.Padding(0);
            this.tlbRadioButtonsGroups.Name = "tlbRadioButtonsGroups";
            this.tlbRadioButtonsGroups.RowCount = 1;
            this.tlbRadioButtonsGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbRadioButtonsGroups.Size = new System.Drawing.Size(370, 111);
            this.tlbRadioButtonsGroups.TabIndex = 9;
            // 
            // gbPreviewMode
            // 
            this.gbPreviewMode.Controls.Add(this.rbPreviewModeResult);
            this.gbPreviewMode.Controls.Add(this.rbPreviewModeImg2);
            this.gbPreviewMode.Controls.Add(this.rbPreviewModeImg1);
            this.gbPreviewMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPreviewMode.Location = new System.Drawing.Point(3, 3);
            this.gbPreviewMode.Name = "gbPreviewMode";
            this.gbPreviewMode.Size = new System.Drawing.Size(179, 105);
            this.gbPreviewMode.TabIndex = 4;
            this.gbPreviewMode.TabStop = false;
            this.gbPreviewMode.Text = "Preview mode";
            // 
            // rbPreviewModeResult
            // 
            this.rbPreviewModeResult.AutoSize = true;
            this.rbPreviewModeResult.Location = new System.Drawing.Point(13, 72);
            this.rbPreviewModeResult.Name = "rbPreviewModeResult";
            this.rbPreviewModeResult.Size = new System.Drawing.Size(93, 19);
            this.rbPreviewModeResult.TabIndex = 2;
            this.rbPreviewModeResult.TabStop = true;
            this.rbPreviewModeResult.Text = "Result image";
            this.rbPreviewModeResult.UseVisualStyleBackColor = true;
            this.rbPreviewModeResult.CheckedChanged += new System.EventHandler(this.rbPreviewModeImg_CheckedChanged);
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
            // gbInterpolationMode
            // 
            this.gbInterpolationMode.Controls.Add(this.rbimOther);
            this.gbInterpolationMode.Controls.Add(this.rbimNearestNeighbour);
            this.gbInterpolationMode.Controls.Add(this.rbimDefault);
            this.gbInterpolationMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbInterpolationMode.Location = new System.Drawing.Point(188, 3);
            this.gbInterpolationMode.Name = "gbInterpolationMode";
            this.gbInterpolationMode.Size = new System.Drawing.Size(179, 105);
            this.gbInterpolationMode.TabIndex = 5;
            this.gbInterpolationMode.TabStop = false;
            this.gbInterpolationMode.Text = "Interpolation mode";
            // 
            // rbimOther
            // 
            this.rbimOther.AutoSize = true;
            this.rbimOther.Location = new System.Drawing.Point(13, 72);
            this.rbimOther.Name = "rbimOther";
            this.rbimOther.Size = new System.Drawing.Size(51, 19);
            this.rbimOther.TabIndex = 2;
            this.rbimOther.Text = "High";
            this.rbimOther.UseVisualStyleBackColor = true;
            this.rbimOther.CheckedChanged += new System.EventHandler(this.rbInterpolationMode_CheckedChanged);
            // 
            // rbimNearestNeighbour
            // 
            this.rbimNearestNeighbour.AutoSize = true;
            this.rbimNearestNeighbour.Location = new System.Drawing.Point(13, 47);
            this.rbimNearestNeighbour.Name = "rbimNearestNeighbour";
            this.rbimNearestNeighbour.Size = new System.Drawing.Size(123, 19);
            this.rbimNearestNeighbour.TabIndex = 1;
            this.rbimNearestNeighbour.Text = "Nearest neighbour";
            this.rbimNearestNeighbour.UseVisualStyleBackColor = true;
            this.rbimNearestNeighbour.CheckedChanged += new System.EventHandler(this.rbInterpolationMode_CheckedChanged);
            // 
            // rbimDefault
            // 
            this.rbimDefault.AutoSize = true;
            this.rbimDefault.Checked = true;
            this.rbimDefault.Location = new System.Drawing.Point(13, 22);
            this.rbimDefault.Name = "rbimDefault";
            this.rbimDefault.Size = new System.Drawing.Size(63, 19);
            this.rbimDefault.TabIndex = 0;
            this.rbimDefault.TabStop = true;
            this.rbimDefault.Text = "Default";
            this.rbimDefault.UseVisualStyleBackColor = true;
            this.rbimDefault.CheckedChanged += new System.EventHandler(this.rbInterpolationMode_CheckedChanged);
            // 
            // tbScriptInput
            // 
            this.tbScriptInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScriptInput.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbScriptInput.Location = new System.Drawing.Point(3, 207);
            this.tbScriptInput.Multiline = true;
            this.tbScriptInput.Name = "tbScriptInput";
            this.tbScriptInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScriptInput.Size = new System.Drawing.Size(364, 222);
            this.tbScriptInput.TabIndex = 7;
            this.tbScriptInput.Text = "resultA = 255\r\nresultR = image1R + 1\r\nresultG = image1G + 1\r\nresultB = image1B + " +
    "1";
            this.tbScriptInput.TextChanged += new System.EventHandler(this.tbScriptInput_TextChanged);
            // 
            // cbAutoRunScriptAfterChange
            // 
            this.cbAutoRunScriptAfterChange.AutoSize = true;
            this.cbAutoRunScriptAfterChange.Checked = true;
            this.cbAutoRunScriptAfterChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoRunScriptAfterChange.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbAutoRunScriptAfterChange.Location = new System.Drawing.Point(16, 436);
            this.cbAutoRunScriptAfterChange.Margin = new System.Windows.Forms.Padding(16, 4, 16, 4);
            this.cbAutoRunScriptAfterChange.Name = "cbAutoRunScriptAfterChange";
            this.cbAutoRunScriptAfterChange.Size = new System.Drawing.Size(338, 19);
            this.cbAutoRunScriptAfterChange.TabIndex = 11;
            this.cbAutoRunScriptAfterChange.Text = "Automatically run script after change";
            this.cbAutoRunScriptAfterChange.UseVisualStyleBackColor = true;
            // 
            // btnRunStopScript
            // 
            this.btnRunStopScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRunStopScript.Location = new System.Drawing.Point(4, 463);
            this.btnRunStopScript.Margin = new System.Windows.Forms.Padding(4);
            this.btnRunStopScript.Name = "btnRunStopScript";
            this.btnRunStopScript.Size = new System.Drawing.Size(362, 23);
            this.btnRunStopScript.TabIndex = 10;
            this.btnRunStopScript.Text = "Run script";
            this.btnRunStopScript.UseVisualStyleBackColor = true;
            this.btnRunStopScript.Click += new System.EventHandler(this.btnRunStopScript_Click);
            // 
            // tlbLeftBottomPanels
            // 
            this.tlbLeftBottomPanels.ColumnCount = 1;
            this.tlbLeftBottomPanels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftBottomPanels.Controls.Add(this.tbScriptOutput, 0, 0);
            this.tlbLeftBottomPanels.Controls.Add(this.tlbLoadSaveScript, 0, 1);
            this.tlbLeftBottomPanels.Controls.Add(this.btnSaveResultImage, 0, 2);
            this.tlbLeftBottomPanels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbLeftBottomPanels.Location = new System.Drawing.Point(0, 0);
            this.tlbLeftBottomPanels.Name = "tlbLeftBottomPanels";
            this.tlbLeftBottomPanels.RowCount = 3;
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftBottomPanels.Size = new System.Drawing.Size(370, 174);
            this.tlbLeftBottomPanels.TabIndex = 2;
            // 
            // tbScriptOutput
            // 
            this.tbScriptOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScriptOutput.Location = new System.Drawing.Point(3, 3);
            this.tbScriptOutput.Multiline = true;
            this.tbScriptOutput.Name = "tbScriptOutput";
            this.tbScriptOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScriptOutput.Size = new System.Drawing.Size(364, 105);
            this.tbScriptOutput.TabIndex = 8;
            this.tbScriptOutput.Text = " \r\n";
            // 
            // tlbLoadSaveScript
            // 
            this.tlbLoadSaveScript.ColumnCount = 2;
            this.tlbLoadSaveScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbLoadSaveScript.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbLoadSaveScript.Controls.Add(this.btnLoadScript, 0, 0);
            this.tlbLoadSaveScript.Controls.Add(this.btnSaveScript, 1, 0);
            this.tlbLoadSaveScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbLoadSaveScript.Location = new System.Drawing.Point(0, 111);
            this.tlbLoadSaveScript.Margin = new System.Windows.Forms.Padding(0);
            this.tlbLoadSaveScript.Name = "tlbLoadSaveScript";
            this.tlbLoadSaveScript.RowCount = 1;
            this.tlbLoadSaveScript.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLoadSaveScript.Size = new System.Drawing.Size(370, 32);
            this.tlbLoadSaveScript.TabIndex = 5;
            // 
            // btnLoadScript
            // 
            this.btnLoadScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLoadScript.Location = new System.Drawing.Point(4, 4);
            this.btnLoadScript.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadScript.Name = "btnLoadScript";
            this.btnLoadScript.Size = new System.Drawing.Size(177, 23);
            this.btnLoadScript.TabIndex = 0;
            this.btnLoadScript.Text = "Load script";
            this.btnLoadScript.UseVisualStyleBackColor = true;
            this.btnLoadScript.Click += new System.EventHandler(this.btnLoadScript_Click);
            // 
            // btnSaveScript
            // 
            this.btnSaveScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveScript.Location = new System.Drawing.Point(189, 4);
            this.btnSaveScript.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.Size = new System.Drawing.Size(177, 23);
            this.btnSaveScript.TabIndex = 1;
            this.btnSaveScript.Text = "Save script";
            this.btnSaveScript.UseVisualStyleBackColor = true;
            this.btnSaveScript.Click += new System.EventHandler(this.btnSaveScript_Click);
            // 
            // btnSaveResultImage
            // 
            this.btnSaveResultImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveResultImage.Location = new System.Drawing.Point(4, 147);
            this.btnSaveResultImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveResultImage.Name = "btnSaveResultImage";
            this.btnSaveResultImage.Size = new System.Drawing.Size(362, 23);
            this.btnSaveResultImage.TabIndex = 6;
            this.btnSaveResultImage.Text = "Save result image";
            this.btnSaveResultImage.UseVisualStyleBackColor = true;
            this.btnSaveResultImage.Click += new System.EventHandler(this.btnSaveResultImage_Click);
            // 
            // odLoadImage
            // 
            this.odLoadImage.Filter = "Image Files(*.BMP;*.PNG;*.JPG;*.GIF)|*.BMP;*.PNG;*.JPG;*.GIF|All files (*.*)|*.*";
            // 
            // odLoadScript
            // 
            this.odLoadScript.Filter = "Script Files(*.TXT;*.LUA)|*.TXT;*.LUA|All files (*.*)|*.*";
            // 
            // sdSaveResultImage
            // 
            this.sdSaveResultImage.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPG (*.jpg)|*.jpg|GIF (*.gif)|*.gif|TIF (*" +
    ".tif)|*.tif";
            // 
            // sdSaveScript
            // 
            this.sdSaveScript.Filter = "LUA script (*.lua)|*.lua|Text file (*.txt)|*.txt";
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslState,
            this.tsslCursorCoords,
            this.tsslEmpty,
            this.tsddbSwitchPixelInfoFormat,
            this.tsslImage1argb,
            this.tsslImage2argb,
            this.tsslImageResultargb});
            this.statusStrip.Location = new System.Drawing.Point(0, 670);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1218, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tsslState
            // 
            this.tsslState.AutoSize = false;
            this.tsslState.Name = "tsslState";
            this.tsslState.Size = new System.Drawing.Size(370, 17);
            this.tsslState.Text = "Ready";
            this.tsslState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslCursorCoords
            // 
            this.tsslCursorCoords.AutoSize = false;
            this.tsslCursorCoords.Name = "tsslCursorCoords";
            this.tsslCursorCoords.Size = new System.Drawing.Size(96, 17);
            this.tsslCursorCoords.Text = "(0; 0)";
            this.tsslCursorCoords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslEmpty
            // 
            this.tsslEmpty.Name = "tsslEmpty";
            this.tsslEmpty.Size = new System.Drawing.Size(81, 17);
            this.tsslEmpty.Spring = true;
            // 
            // tsddbSwitchPixelInfoFormat
            // 
            this.tsddbSwitchPixelInfoFormat.AutoSize = false;
            this.tsddbSwitchPixelInfoFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbSwitchPixelInfoFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.argbHexToolStripMenuItem,
            this.argbDecToolStripMenuItem,
            this.rgbHexToolStripMenuItem,
            this.rgbDecToolStripMenuItem});
            this.tsddbSwitchPixelInfoFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbSwitchPixelInfoFormat.Name = "tsddbSwitchPixelInfoFormat";
            this.tsddbSwitchPixelInfoFormat.Size = new System.Drawing.Size(80, 20);
            this.tsddbSwitchPixelInfoFormat.Text = "ARGB fmt";
            // 
            // argbHexToolStripMenuItem
            // 
            this.argbHexToolStripMenuItem.Name = "argbHexToolStripMenuItem";
            this.argbHexToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.argbHexToolStripMenuItem.Text = "ARGB Hex";
            this.argbHexToolStripMenuItem.Click += new System.EventHandler(this.argbHexToolStripMenuItem_Click);
            // 
            // argbDecToolStripMenuItem
            // 
            this.argbDecToolStripMenuItem.Name = "argbDecToolStripMenuItem";
            this.argbDecToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.argbDecToolStripMenuItem.Text = "ARGB Dec";
            this.argbDecToolStripMenuItem.Click += new System.EventHandler(this.argbDecToolStripMenuItem_Click);
            // 
            // rgbHexToolStripMenuItem
            // 
            this.rgbHexToolStripMenuItem.Name = "rgbHexToolStripMenuItem";
            this.rgbHexToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.rgbHexToolStripMenuItem.Text = "RGB Hex";
            this.rgbHexToolStripMenuItem.Click += new System.EventHandler(this.rgbHexToolStripMenuItem_Click);
            // 
            // rgbDecToolStripMenuItem
            // 
            this.rgbDecToolStripMenuItem.Name = "rgbDecToolStripMenuItem";
            this.rgbDecToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.rgbDecToolStripMenuItem.Text = "RGB Dec";
            this.rgbDecToolStripMenuItem.Click += new System.EventHandler(this.rgbDecToolStripMenuItem_Click);
            // 
            // tsslImage1argb
            // 
            this.tsslImage1argb.AutoSize = false;
            this.tsslImage1argb.Name = "tsslImage1argb";
            this.tsslImage1argb.Size = new System.Drawing.Size(192, 17);
            this.tsslImage1argb.Text = "FFFFFFFF";
            this.tsslImage1argb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tsslImage2argb
            // 
            this.tsslImage2argb.AutoSize = false;
            this.tsslImage2argb.Name = "tsslImage2argb";
            this.tsslImage2argb.Size = new System.Drawing.Size(192, 17);
            this.tsslImage2argb.Text = "FFFFFFFF";
            this.tsslImage2argb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tsslImageResultargb
            // 
            this.tsslImageResultargb.AutoSize = false;
            this.tsslImageResultargb.Name = "tsslImageResultargb";
            this.tsslImageResultargb.Size = new System.Drawing.Size(192, 17);
            this.tsslImageResultargb.Text = "result: FFFFFFFF";
            this.tsslImageResultargb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelLeftTop);
            this.panelLeft.Controls.Add(this.leftPanelSplitter);
            this.panelLeft.Controls.Add(this.panelLeftBottom);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(370, 670);
            this.panelLeft.TabIndex = 2;
            // 
            // panelLeftTop
            // 
            this.panelLeftTop.Controls.Add(this.tlbLeftTopPanels);
            this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
            this.panelLeftTop.Name = "panelLeftTop";
            this.panelLeftTop.Size = new System.Drawing.Size(370, 490);
            this.panelLeftTop.TabIndex = 4;
            // 
            // leftPanelSplitter
            // 
            this.leftPanelSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.leftPanelSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.leftPanelSplitter.Location = new System.Drawing.Point(0, 490);
            this.leftPanelSplitter.Name = "leftPanelSplitter";
            this.leftPanelSplitter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.leftPanelSplitter.Size = new System.Drawing.Size(370, 6);
            this.leftPanelSplitter.TabIndex = 3;
            this.leftPanelSplitter.TabStop = false;
            this.leftPanelSplitter.SplitterMoving += new System.Windows.Forms.SplitterEventHandler(this.leftPanelSplitter_SplitterMoving);
            // 
            // panelLeftBottom
            // 
            this.panelLeftBottom.Controls.Add(this.tlbLeftBottomPanels);
            this.panelLeftBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLeftBottom.Location = new System.Drawing.Point(0, 496);
            this.panelLeftBottom.Name = "panelLeftBottom";
            this.panelLeftBottom.Size = new System.Drawing.Size(370, 174);
            this.panelLeftBottom.TabIndex = 2;
            // 
            // mainSplitter
            // 
            this.mainSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainSplitter.Location = new System.Drawing.Point(370, 0);
            this.mainSplitter.Name = "mainSplitter";
            this.mainSplitter.Size = new System.Drawing.Size(6, 670);
            this.mainSplitter.TabIndex = 3;
            this.mainSplitter.TabStop = false;
            this.mainSplitter.SplitterMoving += new System.Windows.Forms.SplitterEventHandler(this.splitter_SplitterMoving);
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.pb);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(376, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(16);
            this.panelRight.Size = new System.Drawing.Size(842, 670);
            this.panelRight.TabIndex = 4;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 692);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.mainSplitter);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.statusStrip);
            this.Name = "FrmMain";
            this.Text = "Picturepreter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.tlbLeftTopPanels.ResumeLayout(false);
            this.tlbLeftTopPanels.PerformLayout();
            this.tlbRadioButtonsGroups.ResumeLayout(false);
            this.gbPreviewMode.ResumeLayout(false);
            this.gbPreviewMode.PerformLayout();
            this.gbInterpolationMode.ResumeLayout(false);
            this.gbInterpolationMode.PerformLayout();
            this.tlbLeftBottomPanels.ResumeLayout(false);
            this.tlbLeftBottomPanels.PerformLayout();
            this.tlbLoadSaveScript.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeftTop.ResumeLayout(false);
            this.panelLeftBottom.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PictureBoxEx pb;
        private OpenFileDialog odLoadImage;
        private TableLayoutPanel tlbLeftTopPanels;
        private TableLayoutPanel tlbLeftBottomPanels;
        private Button btnLoadImage2;
        private Button btnLoadImage1;
        private Button btnSwapImages;
        private TableLayoutPanel tlbLoadSaveScript;
        private Button btnLoadScript;
        private Button btnSaveScript;
        private Button btnSaveResultImage;
        private OpenFileDialog odLoadScript;
        private SaveFileDialog sdSaveResultImage;
        private SaveFileDialog sdSaveScript;
        private TextBox tbScriptInput;
        private TextBox tbScriptOutput;
        private TableLayoutPanel tlbRadioButtonsGroups;
        private GroupBox gbPreviewMode;
        private RadioButton rbPreviewModeResult;
        private RadioButton rbPreviewModeImg2;
        private RadioButton rbPreviewModeImg1;
        private GroupBox gbInterpolationMode;
        private RadioButton rbimOther;
        private RadioButton rbimNearestNeighbour;
        private RadioButton rbimDefault;
        private Button btnRunStopScript;
        private CheckBox cbAutoRunScriptAfterChange;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel tsslState;
        private ToolStripStatusLabel tsslCursorCoords;
        private ToolStripStatusLabel tsslImage1argb;
        private ToolStripStatusLabel tsslImage2argb;
        private ToolStripStatusLabel tsslImageResultargb;
        private ToolStripStatusLabel tsslEmpty;
        private ToolStripDropDownButton tsddbSwitchPixelInfoFormat;
        private ToolStripMenuItem argbDecToolStripMenuItem;
        private ToolStripMenuItem argbHexToolStripMenuItem;
        private ToolStripMenuItem rgbHexToolStripMenuItem;
        private ToolStripMenuItem rgbDecToolStripMenuItem;
        private Panel panelLeft;
        private Splitter mainSplitter;
        private Panel panelRight;
        private Panel panelLeftTop;
        private Splitter leftPanelSplitter;
        private Panel panelLeftBottom;
    }
}