using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace BitmapsPxDiff
{
    /// <summary>
    /// The main form of application. It stores and manages:
    /// - source images
    /// - result image
    /// - scriptRenderer
    /// </summary>
    public partial class FrmMain : Form
    {
        private const int resultImageFictionalIndex = -1; // exceptional "extension" to unify sourceImages[] indexing with imagesControlsPanel.imagesControlsItems[] (imagesControlsItems[] contains result image position)

        private int currentImageIndex = resultImageFictionalIndex; // index of image chosen by radio buttons;
        private List<Bitmap> sourceImages = new List<Bitmap>();
        private Bitmap resultImage = new Bitmap(1, 1);
        private ScriptRenderer scriptRenderer; // performs processing source images by script
        
        private static readonly object controlsLocker = new object(); // locks access to controls for threads
        /// <summary>
        /// Class constructor; initializes controls and scriptRenderer, generates build date;
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
            scriptRenderer = new ScriptRenderer(UpdateControls_OnChunkFinished, UpdateControls_OnRenderingStarted, UpdateControls_OnRenderingFinished);

            // display build date at form title:
            DateTime? dt = getAssemblyBuildDateTime();
            if (dt != null) {
                this.Text += " (built " + ((DateTime)dt).ToString("yyyyMMdd") + ")";
            }

            // create starting empty bitmaps and corresponding components:
            sourceImages.Add(new Bitmap(1, 1));
            sourceImages.Add(new Bitmap(1, 1));
            imagesControlsPanel.AddImageControlsSet();
            imagesControlsPanel.TryCheckPanelAtIndex(0);
            imagesControlsPanel.AddImageControlsSet();

            RefreshImagesPixelInfo(); // refresh pixel info components text
        }
        // CONTROLS EVENTS METHODS: *****************************************************************************
        /// <summary>
        /// Form Close event; stops scriptRenderer processing;
        /// </summary>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((scriptRenderer != null) && (scriptRenderer.Running))
            {
                scriptRenderer.StopRendering();
            }
        }
        /// <summary>
        /// Form KeyDown event captures hotkeys;
        /// </summary>
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // capture CTRL + [number] hotkey (switching between images):
            if (e.Control 
                && (e.KeyValue >= (int)Keys.D0) 
                && (e.KeyValue <= (int)Keys.D9))
            {
                if (imagesControlsPanel.TryCheckPanelAtIndex(e.KeyValue - (int)Keys.D1))
                {
                    e.SuppressKeyPress = true;
                }
            }
        }
        /// <summary>
        /// Captures keyboard event to override default RadioButton behaviour
        /// for imagesControlsPanel controls.
        /// Details:
        /// imagesControlsPanel radio buttons are scattered between separate panels,
        /// so it's impossible to move between them by arrows;
        /// instead of creating separate panel for radio buttons only, I decided to
        /// intercept key event for this particular case, and perform transition
        /// manually in imagesControlsPanel.rbSelectImage_ManualKeyDown();
        /// The second reason to intercept key event was the problem with default
        /// RadioButton action, which automatically changes the focus to other
        /// control on the same panel, which is much undesirable in this case;
        /// 
        /// Based on code snipped:
        /// https://stackoverflow.com/questions/22426390/disable-selection-of-controls-by-arrow-keys-in-a-form
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down)
            {
                int panelIndex = imagesControlsPanel.GetRadioButtonSenderIndex(ref msg);
                if (panelIndex >= 0)
                {
                    panelIndex += (keyData == Keys.Up) ? -1 : 1;
                    imagesControlsPanel.TryCheckPanelAtIndex(panelIndex);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// PictureBox pb MouseLeave event; 
        /// initiates refreshing pointed pixel info controls;
        /// </summary>
        private void pb_MouseLeave(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        /// <summary>
        /// PictureBox pb MouseMove event; 
        /// initiates refreshing pointed pixel info controls;
        /// </summary>
        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        /// <summary>
        /// Processes imagesControlPanel OnImageControlsRemoved event;
        /// removes image from sourceImages[] at index given in (Button)sender.Tag;
        /// stops scriptRenderer processing;
        /// </summary>
        private void imagesControlsPanel_OnImageControlsRemoved(object sender, EventArgs e)
        {
            if (!(sender is Button)) return; // sender type check

            int imageIndex = (int)((Button)sender).Tag; // get imageIndex

            if ((imageIndex < 0) || (imageIndex >= sourceImages.Count)) return; // check sourceImages[] bounds

            if (scriptRenderer.Running) { // stop renderer if running
                scriptRenderer.StopRendering();
            }

            sourceImages.RemoveAt(imageIndex); // remove image from sourceImages[]

            // adjust resultImage dimensions:
            Point newResultDimensions = Helpers.GetImagesSizeIntersection(sourceImages);
            resultImage = new Bitmap(newResultDimensions.X, newResultDimensions.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            // refresh controls:
            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        /// <summary>
        /// Processes imagesControlPanel OnNewImageControlsAdded event;
        /// extends sourceImages[] list and creates empty bitmap;
        /// </summary>
        private void imagesControlsPanel_OnNewImageControlsAdded(object sender, EventArgs e)
        {
            sourceImages.Add(new Bitmap(1, 1));
            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        /// <summary>
        /// Processes imagesControlPanel OnLoadImageButtonClick event;
        /// extends sourceImages[] list and creates empty bitmap;
        /// stops scriptRenderer processing;
        /// </summary>
        private void imagesControlsPanel_OnLoadImageButtonClick(object sender, EventArgs e)
        {
            if (!(sender is Button)) return; // check sender type
            Button btn = (Button)sender; // cast sender

            int imageIndex = (int)btn.Tag; // get image index
            if ((imageIndex<0)
                || (imageIndex > sourceImages.Count) // (new sourceImages[] index will be added if needed)
                || (imageIndex >= imagesControlsPanel.GetPanelsCount() - 1) // the last panel is reserved for result image index
                || (odLoadImage.ShowDialog() != DialogResult.OK)) // show open dialog and check result
            {
                return;
            }

            if (scriptRenderer.Running) { // stop renderer if running
                scriptRenderer.StopRendering(); 
            }

            try
            {
                // load bitmap to sourceImages[imageIndex] (add new index if list if needed)
                if (imageIndex < sourceImages.Count) {
                    sourceImages[imageIndex] = new Bitmap(odLoadImage.FileName);
                }else{
                    sourceImages.Add(new Bitmap(odLoadImage.FileName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not load file:\n\"{0}\"\n\nDetails:\n{1}", odLoadImage.FileName, ex.Message));
                return;
            }

            btn.Text = Path.GetFileName(odLoadImage.FileName); // show file name on the button

            imagesControlsPanel.TryCheckPanelAtIndex(imageIndex); // check loaded image radio button

            // adjust result image dimensions:
            Point newResultDimensions = Helpers.GetImagesSizeIntersection(sourceImages);
            resultImage = new Bitmap(newResultDimensions.X, newResultDimensions.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // refresh controls:
            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        /// <summary>
        /// Processes imagesControlPanel OnBeforeSwapImageControls event;
        /// replaces 2 items in sourceImages[] after some safety conditions
        /// </summary>
        private void imagesControlsPanel_OnBeforeSwapImageControls(object sender, EventArgs e)
        {
            if ((sourceImages.Count < 2) // swap requires more than 1 images
                || !(sender is Button)) { // and a Button sender is expected
                return;
            }
            int firstImageIndex = (int)((Button)sender).Tag; // get firstImageIndex

            if ((firstImageIndex >= 0)
                && (firstImageIndex < sourceImages.Count - 1)) // check sourceImages[] bounds
            {
                sourceImages.Reverse(firstImageIndex, 2); // reverse
                // refresh controls:
                RefreshImagesPixelInfo();
                RefreshPreview(scriptRenderer.Running);
            }
        }
        /// <summary>
        /// Processes imagesControlPanel OnImageChecked event;
        /// switches to image of given index (sourceImages[] or resultImage if index is beyond bounds);
        /// stops scriptRenderer processing if switching to one of sourceImages[];
        /// </summary>
        private void imagesControlsPanel_OnImageChecked(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return; // sender type check

            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked) return; // radio button must be checked

            int imageIndex = (int)rb.Tag; // get imageIndex

            if ((imageIndex < 0) || (imageIndex >= sourceImages.Count))
            { 
                currentImageIndex = resultImageFictionalIndex; // exceeded sourceImages[] bounds means checking resultImage
            }
            else 
            {
                currentImageIndex = imageIndex; // checking one of sourceImages[]

                if (scriptRenderer.Running) { // scriptRenderer should be stopped (user leaves resultImage preview)
                    scriptRenderer.StopRendering();
                }
            }
            RefreshPreview(false);
        }
        /// <summary>
        /// Changes preview PictureBox interpolation mode depending on RadioButton.TabIndex value;
        /// </summary>
        private void rbInterpolationMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return;

            RadioButton rb = (RadioButton)sender;
            switch (rb.TabIndex)
            {
                case 0: 
                    pb.InterpolationMode = InterpolationMode.Default;
                    break;
                case 1:
                    pb.InterpolationMode = InterpolationMode.NearestNeighbor;
                    break;
                case 2:
                    pb.InterpolationMode = InterpolationMode.High;
                    break;
                default:;
                    break;
            }
            pb.Refresh();
        }
        /// <summary>
        /// Enables or disables others pixel info checkboxes;
        /// initiates RefreshImagesPixelInfo(), which uses chbDisplayPixelInfo.Checked value
        /// to show or hide pixel info labels;
        /// </summary>
        private void chbDisplayPixelInfo_CheckedChanged(object sender, EventArgs e)
        {
            chbPixelInfoDisplayAlpha.Enabled = chbPixelInfoHex.Enabled = chbDisplayPixelInfo.Checked;
            RefreshImagesPixelInfo();
        }
        /// <summary>
        /// Initiates RefreshImagesPixelInfo(), which uses chbPixelInfoDisplayAlpha.Checked value
        /// to set pixel info format (with or without alpha channel);
        /// </summary>
        private void chbPixelInfoDisplayAlpha_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        /// <summary>
        /// Initiates RefreshImagesPixelInfo(), which uses chbPixelInfoHex.Checked value
        /// to set pixel info format (hex or dec);
        /// </summary>
        private void chbPixelInfoHex_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        /// <summary>
        /// Triggers scriptRenderer (via RefreshPreview()) if result image preview is active;
        /// </summary>
        private void tbScriptInput_TextChanged(object sender, EventArgs e)
        {
            if (currentImageIndex == resultImageFictionalIndex)
            {
                RefreshPreview(cbAutoRunScriptAfterChange.Checked);
            }            
        }
        /// <summary>
        /// Initiates or stops scriptRenderer;
        /// automatically switches currentImageIndex to result image preview;
        /// </summary>
        private void btnRunStopScript_Click(object sender, EventArgs e)
        {
            if (currentImageIndex != resultImageFictionalIndex) {
                imagesControlsPanel.TryCheckPanelAtIndex(imagesControlsPanel.GetPanelsCount() - 1);
            }

            if (scriptRenderer.Running) {
                scriptRenderer.StopRendering();
            }else{
                RefreshPreview(true);
            }
        }
        /// <summary>
        /// Opens up an open dialog and loads chosen script;
        /// </summary>
        private void btnLoadScript_Click(object sender, EventArgs e)
        {
            if (odLoadScript.ShowDialog() != DialogResult.OK) return;

            if (scriptRenderer.Running) {
                scriptRenderer.StopRendering();
            }

            try
            {
                tbScriptInput.Text = File.ReadAllText(odLoadScript.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not load file:\n\"{0}\"\n\nDetails:\n{1}", odLoadScript.FileName, ex.Message));
                return;
            }
        }
        /// <summary>
        /// Opens up a save dialog and saves script to chosen path;
        /// </summary>
        private void btnSaveScript_Click(object sender, EventArgs e)
        {
            if (sdSaveScript.ShowDialog() != DialogResult.OK) return;

            try
            {
                File.WriteAllText(sdSaveScript.FileName, tbScriptInput.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not save file:\n\"{0}\"\n\nDetails:\n{1}", sdSaveScript.FileName, ex.Message));
                return;
            }
        }
        /// <summary>
        /// Opens up a save dialog and saves resultImage to chosen path;
        /// </summary>
        private void btnSaveResultImage_Click(object sender, EventArgs e)
        {
            if (resultImage is null)
            {
                MessageBox.Show("Result image is empty.");
                return;
            }
            if (sdSaveResultImage.ShowDialog() != DialogResult.OK) return;

            // determine imgFormat:
            ImageFormat imgFormat;
            switch (Path.GetExtension(sdSaveResultImage.FileName).ToLower())
            {
                case ".bmp":
                    imgFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imgFormat = ImageFormat.Png;
                    break;
                case ".jpg":
                    imgFormat = ImageFormat.Jpeg;
                    break;
                case ".gif":
                    imgFormat = ImageFormat.Gif;
                    break;
                case ".tif":
                    imgFormat = ImageFormat.Tiff;
                    break;
                default:
                    MessageBox.Show("Unknown file format.");
                    return;
            }

            try // try save file:
            {
                resultImage.Save(sdSaveResultImage.FileName, imgFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Could not save file:\n\"{0}\"\n\nDetails:\n{1}", sdSaveResultImage.FileName, ex.Message));
                return;
            }
            MessageBox.Show("File saved.");
        }
        /// <summary>
        /// Initiates repainting of dependent controls (left and right panel (width change))
        /// moves tsslCursorCoords by changing tsslState.Width;
        /// </summary>
        private void splitterLeftPanelWidth_SplitterMoving(object sender, SplitterEventArgs e)
        {
            splitterLeftPanelWidth.SplitPosition = splitterLeftPanelWidth.SplitPosition; // refresh controls state
            tsslState.Width = splitterLeftPanelWidth.Left;
        }
        /// <summary>
        /// Initiates repainting of dependent controls (left panel (height change))
        /// </summary>
        private void splitterScriptOutputHeight_SplitterMoving(object sender, SplitterEventArgs e)
        {
            splitterScriptOutputHeight.SplitPosition = splitterScriptOutputHeight.SplitPosition; // refresh controls state
        }
        // THREADS EVENTS METHODS: *****************************************************************************
        /// <summary>
        /// Changes btnRunStopScript.Text and tsslState.Text to new context (scriptRenderer running)
        /// </summary>
        private void UpdateControls_OnRenderingStarted()
        {
            lock (controlsLocker)
            {
                this.btnRunStopScript.BeginInvoke((MethodInvoker)delegate
                {
                    btnRunStopScript.Text = "Stop script execution";
                });
                this.statusStrip.BeginInvoke((MethodInvoker)delegate
                {
                    tsslState.Text = "Running script...";
                });
            }
        }
        /// <summary>
        /// Changes btnRunStopScript.Text and tsslState.Text to new context (scriptRenderer idle)
        /// </summary>
        private void UpdateControls_OnRenderingFinished()
        {
            lock (controlsLocker)
            {
                this.btnRunStopScript.BeginInvoke((MethodInvoker)delegate
                {
                    btnRunStopScript.Text = "Run script";

                });
                this.statusStrip.BeginInvoke((MethodInvoker)delegate
                {
                    tsslState.Text = "Idle";
                });
            }
        }
        /// <summary>
        /// Updates processing status and result image preview;
        /// </summary>
        /// <param name=""></param>
        private void UpdateControls_OnChunkFinished(Bitmap newImage, string newStatus)
        {
            lock (controlsLocker)
            {
                if ((newImage != null) && (currentImageIndex == resultImageFictionalIndex))
                {
                    this.pb.BeginInvoke((MethodInvoker)delegate
                    {
                        resultImage = (Bitmap)newImage.Clone();
                        RefreshPreview(false);
                    });
                }
                this.tbScriptOutput.BeginInvoke((MethodInvoker)delegate
                {
                    tbScriptOutput.Text = newStatus;
                });
                this.statusStrip.BeginInvoke((MethodInvoker)delegate
                {
                    tsslState.Text = newStatus.Replace("\r\n", " ");
                    RefreshImagesPixelInfo();
                });
            }
        }
        // OTHER METHODS: *****************************************************************************
        /// <summary>
        /// Reads project build date (it's kind of automatic version number);
        /// Taken from: https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
        /// </summary>
        public static DateTime? getAssemblyBuildDateTime()
        { 
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var attr = Attribute.GetCustomAttribute(assembly, typeof(BuildDateTimeAttribute)) as BuildDateTimeAttribute;
            if (DateTime.TryParse(attr?.Date, out DateTime dt))
                return dt;
            else
                return null;
        }
        /// <summary>
        /// Refreshes checked source or result image display; Initiates scriptRenderer if switches to resultImage;
        /// </summary>
        /// <param name="startRendering">if "true", the script processing will be initiated if result image preview is active</param>
        private void RefreshPreview(bool startRendering)
        {
            // currentImageIndex value check:
            if ((currentImageIndex != resultImageFictionalIndex)
                && ((currentImageIndex<0) || (currentImageIndex >= sourceImages.Count))) {
                return;
            }

            if (currentImageIndex == resultImageFictionalIndex)
            {
                if (startRendering) 
                {
                    if (sourceImages.Count == 0) {
                        MessageBox.Show("No source images.");
                        return;
                    }
                    scriptRenderer.StartRendering(sourceImages, tbScriptInput.Text);
                }
                else
                {
                    lock (controlsLocker) {
                        pb.Image = resultImage;
                    }
                }
            }
            else
            {
                lock (controlsLocker) {
                    pb.Image = sourceImages[currentImageIndex];
                }
            }
        }
        /// <summary>
        /// Refreshes pixel info labels (imagesControlsPanel.imagesControlsItems[].lblPixelInfo);
        /// changes pixel info labels visibility depending on chbDisplayPixelInfo.Checked;
        /// formats pixel info string depending on chbPixelInfoHex.Checked and chbPixelInfoDisplayAlpha.Checked;
        /// </summary>
        private void RefreshImagesPixelInfo()
        {
            if (this.chbDisplayPixelInfo.Checked != imagesControlsPanel.PixelInfoLabelsVisible()) 
            {
                imagesControlsPanel.SetPixelInfoLabelsVisibility(this.chbDisplayPixelInfo.Checked);
            }

            if (!this.chbDisplayPixelInfo.Checked) return; // no controls to print at

            string pixelDigitFormat = (chbPixelInfoHex.Checked) ? "X2" : "D3";
            string pixelStringFormat = (chbPixelInfoDisplayAlpha.Checked)
                ? ("({0:XX} {1:XX} {2:XX} {3:XX})").Replace("XX", pixelDigitFormat)
                : ("({1:XX} {2:XX} {3:XX})").Replace("XX", pixelDigitFormat);

            if ((pb.ImagePointer is null) && (pb.currentMouseImagePos is null))
            {
                tsslCursorCoords.Text = "";
                for (int i = 0; i < imagesControlsPanel.GetPanelsCount(); i++)
                {
                    imagesControlsPanel.SetPixelInfoLabelText(i, "");
                }
            }
            else
            {
                Point p = (pb.ImagePointer != null) ? (Point)pb.ImagePointer : (Point)pb.currentMouseImagePos;
                tsslCursorCoords.Text = $"({p.X}; {p.Y})";

                for (int i = 0; i < sourceImages.Count; i++)
                {
                    imagesControlsPanel.SetPixelInfoLabelText(i, FormatBitmapPixelInfo(pixelStringFormat, sourceImages[i], p));
                }
                imagesControlsPanel.SetPixelInfoLabelText(imagesControlsPanel.GetPanelsCount() - 1, FormatBitmapPixelInfo(pixelStringFormat, resultImage, p));
            }
        }
        /// <summary>
        /// Generates pixel info string;
        /// </summary>
        /// <param name="pixelStringFormat">(string) output string format</param>
        /// <param name="bmp">(Bitmap) source bitmap to get specified pixel values</param>
        /// <param name="p">(Point) pixel coordinates</param>
        private string FormatBitmapPixelInfo(string pixelStringFormat, Bitmap bmp, Point p)
        {
            if ((bmp != null) && (new Rectangle(0, 0, bmp.Width, bmp.Height).Contains(p)))
            {
                Color c = bmp.GetPixel(p.X, p.Y);
                return string.Format(pixelStringFormat, c.A, c.R, c.G, c.B);
            }
            return "";
        }
    }
}