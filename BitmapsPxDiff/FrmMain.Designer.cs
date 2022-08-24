﻿using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

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
            this.tlbSettingsControlsGroups = new System.Windows.Forms.TableLayoutPanel();
            this.gbInterpolationMode = new System.Windows.Forms.GroupBox();
            this.rbimOther = new System.Windows.Forms.RadioButton();
            this.rbimNearestNeighbour = new System.Windows.Forms.RadioButton();
            this.rbimDefault = new System.Windows.Forms.RadioButton();
            this.gbPixelInfoFormat = new System.Windows.Forms.GroupBox();
            this.chbPixelInfoHex = new System.Windows.Forms.CheckBox();
            this.chbPixelInfoDisplayAlpha = new System.Windows.Forms.CheckBox();
            this.chbDisplayPixelInfo = new System.Windows.Forms.CheckBox();
            this.tbScriptInput = new System.Windows.Forms.TextBox();
            this.cbAutoRunScriptAfterChange = new System.Windows.Forms.CheckBox();
            this.btnRunStopScript = new System.Windows.Forms.Button();
            this.imagesControlsPanel = new BitmapsPxDiff.ImagesControlsPanel();
            this.leftPanelSplitter = new System.Windows.Forms.Splitter();
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
            this.panelLeft = new System.Windows.Forms.Panel();
            this.mainSplitter = new System.Windows.Forms.Splitter();
            this.panelRight = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.tlbLeftTopPanels.SuspendLayout();
            this.tlbSettingsControlsGroups.SuspendLayout();
            this.gbInterpolationMode.SuspendLayout();
            this.gbPixelInfoFormat.SuspendLayout();
            this.tlbLeftBottomPanels.SuspendLayout();
            this.tlbLoadSaveScript.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelLeft.SuspendLayout();
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
            this.tlbLeftTopPanels.Controls.Add(this.tlbSettingsControlsGroups, 0, 1);
            this.tlbLeftTopPanels.Controls.Add(this.tbScriptInput, 0, 2);
            this.tlbLeftTopPanels.Controls.Add(this.cbAutoRunScriptAfterChange, 0, 3);
            this.tlbLeftTopPanels.Controls.Add(this.btnRunStopScript, 0, 4);
            this.tlbLeftTopPanels.Controls.Add(this.imagesControlsPanel, 0, 0);
            this.tlbLeftTopPanels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbLeftTopPanels.Location = new System.Drawing.Point(0, 0);
            this.tlbLeftTopPanels.Name = "tlbLeftTopPanels";
            this.tlbLeftTopPanels.RowCount = 5;
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftTopPanels.Size = new System.Drawing.Size(370, 490);
            this.tlbLeftTopPanels.TabIndex = 2;
            // 
            // tlbSettingsControlsGroups
            // 
            this.tlbSettingsControlsGroups.ColumnCount = 2;
            this.tlbSettingsControlsGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbSettingsControlsGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbSettingsControlsGroups.Controls.Add(this.gbInterpolationMode, 0, 0);
            this.tlbSettingsControlsGroups.Controls.Add(this.gbPixelInfoFormat, 1, 0);
            this.tlbSettingsControlsGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbSettingsControlsGroups.Location = new System.Drawing.Point(0, 128);
            this.tlbSettingsControlsGroups.Margin = new System.Windows.Forms.Padding(0);
            this.tlbSettingsControlsGroups.Name = "tlbSettingsControlsGroups";
            this.tlbSettingsControlsGroups.RowCount = 1;
            this.tlbSettingsControlsGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlbSettingsControlsGroups.Size = new System.Drawing.Size(370, 111);
            this.tlbSettingsControlsGroups.TabIndex = 9;
            // 
            // gbInterpolationMode
            // 
            this.gbInterpolationMode.Controls.Add(this.rbimOther);
            this.gbInterpolationMode.Controls.Add(this.rbimNearestNeighbour);
            this.gbInterpolationMode.Controls.Add(this.rbimDefault);
            this.gbInterpolationMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbInterpolationMode.Location = new System.Drawing.Point(3, 3);
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
            // gbPixelInfoFormat
            // 
            this.gbPixelInfoFormat.Controls.Add(this.chbPixelInfoHex);
            this.gbPixelInfoFormat.Controls.Add(this.chbPixelInfoDisplayAlpha);
            this.gbPixelInfoFormat.Controls.Add(this.chbDisplayPixelInfo);
            this.gbPixelInfoFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPixelInfoFormat.Location = new System.Drawing.Point(188, 3);
            this.gbPixelInfoFormat.Name = "gbPixelInfoFormat";
            this.gbPixelInfoFormat.Size = new System.Drawing.Size(179, 105);
            this.gbPixelInfoFormat.TabIndex = 4;
            this.gbPixelInfoFormat.TabStop = false;
            this.gbPixelInfoFormat.Text = "Pixel info format";
            // 
            // chbPixelInfoHex
            // 
            this.chbPixelInfoHex.AutoSize = true;
            this.chbPixelInfoHex.Checked = true;
            this.chbPixelInfoHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPixelInfoHex.Location = new System.Drawing.Point(15, 73);
            this.chbPixelInfoHex.Name = "chbPixelInfoHex";
            this.chbPixelInfoHex.Size = new System.Drawing.Size(85, 19);
            this.chbPixelInfoHex.TabIndex = 2;
            this.chbPixelInfoHex.Text = "Hex format";
            this.chbPixelInfoHex.UseVisualStyleBackColor = true;
            this.chbPixelInfoHex.CheckedChanged += new System.EventHandler(this.chbPixelInfoHex_CheckedChanged);
            // 
            // chbPixelInfoDisplayAlpha
            // 
            this.chbPixelInfoDisplayAlpha.AutoSize = true;
            this.chbPixelInfoDisplayAlpha.Checked = true;
            this.chbPixelInfoDisplayAlpha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPixelInfoDisplayAlpha.Location = new System.Drawing.Point(15, 48);
            this.chbPixelInfoDisplayAlpha.Name = "chbPixelInfoDisplayAlpha";
            this.chbPixelInfoDisplayAlpha.Size = new System.Drawing.Size(96, 19);
            this.chbPixelInfoDisplayAlpha.TabIndex = 1;
            this.chbPixelInfoDisplayAlpha.Text = "Display alpha";
            this.chbPixelInfoDisplayAlpha.UseVisualStyleBackColor = true;
            this.chbPixelInfoDisplayAlpha.CheckedChanged += new System.EventHandler(this.chbPixelInfoDisplayAlpha_CheckedChanged);
            // 
            // chbDisplayPixelInfo
            // 
            this.chbDisplayPixelInfo.AutoSize = true;
            this.chbDisplayPixelInfo.Checked = true;
            this.chbDisplayPixelInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbDisplayPixelInfo.Location = new System.Drawing.Point(15, 22);
            this.chbDisplayPixelInfo.Name = "chbDisplayPixelInfo";
            this.chbDisplayPixelInfo.Size = new System.Drawing.Size(115, 19);
            this.chbDisplayPixelInfo.TabIndex = 0;
            this.chbDisplayPixelInfo.Text = "Display pixel info";
            this.chbDisplayPixelInfo.UseVisualStyleBackColor = true;
            this.chbDisplayPixelInfo.CheckedChanged += new System.EventHandler(this.chbDisplayPixelInfo_CheckedChanged);
            // 
            // tbScriptInput
            // 
            this.tbScriptInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbScriptInput.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbScriptInput.Location = new System.Drawing.Point(3, 242);
            this.tbScriptInput.Multiline = true;
            this.tbScriptInput.Name = "tbScriptInput";
            this.tbScriptInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScriptInput.Size = new System.Drawing.Size(364, 187);
            this.tbScriptInput.TabIndex = 7;
            this.tbScriptInput.Text = "result[1] = 255\r\nresult[2] = colors[1][2] + 1\r\nresult[3] = colors[1][3] + 1\r\nresu" +
    "lt[4] = colors[1][4] + 1";
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
            // imagesControlsPanel
            // 
            this.imagesControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagesControlsPanel.Location = new System.Drawing.Point(3, 3);
            this.imagesControlsPanel.Name = "imagesControlsPanel";
            this.imagesControlsPanel.Size = new System.Drawing.Size(364, 122);
            this.imagesControlsPanel.TabIndex = 12;
            this.imagesControlsPanel.OnImageSelected += new System.EventHandler(this.imagesControlsPanel_OnImageSelected);
            this.imagesControlsPanel.OnAddImageClick += new System.EventHandler(this.imagesControlsPanel_OnAddImageClick);
            this.imagesControlsPanel.OnLoadImageClick += new System.EventHandler(this.imagesControlsPanel_OnLoadImageClick);
            this.imagesControlsPanel.OnRemoveImageClick += new System.EventHandler(this.imagesControlsPanel_OnRemoveImageClick);
            this.imagesControlsPanel.OnSwitchImageClick += new System.EventHandler(this.imagesControlsPanel_OnSwitchImageClick);
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
            // tlbLeftBottomPanels
            // 
            this.tlbLeftBottomPanels.ColumnCount = 1;
            this.tlbLeftBottomPanels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftBottomPanels.Controls.Add(this.tbScriptOutput, 0, 0);
            this.tlbLeftBottomPanels.Controls.Add(this.tlbLoadSaveScript, 0, 1);
            this.tlbLeftBottomPanels.Controls.Add(this.btnSaveResultImage, 0, 2);
            this.tlbLeftBottomPanels.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlbLeftBottomPanels.Location = new System.Drawing.Point(0, 496);
            this.tlbLeftBottomPanels.Name = "tlbLeftBottomPanels";
            this.tlbLeftBottomPanels.RowCount = 3;
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftBottomPanels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlbLeftBottomPanels.Size = new System.Drawing.Size(370, 174);
            this.tlbLeftBottomPanels.TabIndex = 4;
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
            this.tsslEmpty});
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
            this.tsslEmpty.Size = new System.Drawing.Size(737, 17);
            this.tsslEmpty.Spring = true;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.tlbLeftTopPanels);
            this.panelLeft.Controls.Add(this.leftPanelSplitter);
            this.panelLeft.Controls.Add(this.tlbLeftBottomPanels);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(370, 670);
            this.panelLeft.TabIndex = 2;
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
            this.tlbSettingsControlsGroups.ResumeLayout(false);
            this.gbInterpolationMode.ResumeLayout(false);
            this.gbInterpolationMode.PerformLayout();
            this.gbPixelInfoFormat.ResumeLayout(false);
            this.gbPixelInfoFormat.PerformLayout();
            this.tlbLeftBottomPanels.ResumeLayout(false);
            this.tlbLeftBottomPanels.PerformLayout();
            this.tlbLoadSaveScript.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PictureBoxEx pb;
        private OpenFileDialog odLoadImage;
        private TableLayoutPanel tlbLeftTopPanels;
        private Splitter leftPanelSplitter;
        private TableLayoutPanel tlbLeftBottomPanels;
        private TableLayoutPanel tlbLoadSaveScript;
        private Button btnLoadScript;
        private Button btnSaveScript;
        private Button btnSaveResultImage;
        private OpenFileDialog odLoadScript;
        private SaveFileDialog sdSaveResultImage;
        private SaveFileDialog sdSaveScript;
        private TextBox tbScriptInput;
        private TextBox tbScriptOutput;
        private TableLayoutPanel tlbSettingsControlsGroups;
        private GroupBox gbPixelInfoFormat;
        private GroupBox gbInterpolationMode;
        private RadioButton rbimOther;
        private RadioButton rbimNearestNeighbour;
        private RadioButton rbimDefault;
        private Button btnRunStopScript;
        private CheckBox cbAutoRunScriptAfterChange;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel tsslState;
        private ToolStripStatusLabel tsslCursorCoords;
        private ToolStripStatusLabel tsslEmpty;
        private Panel panelLeft;
        private Splitter mainSplitter;
        private Panel panelRight;
        private ImagesControlsPanel imagesControlsPanel;
        private CheckBox chbPixelInfoHex;
        private CheckBox chbPixelInfoDisplayAlpha;
        private CheckBox chbDisplayPixelInfo;
    }
}