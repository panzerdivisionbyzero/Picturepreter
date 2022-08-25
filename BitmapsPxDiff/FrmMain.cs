using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private const int resultImageFictionalIndex = -1;
        private int currentImageIndex = resultImageFictionalIndex; // index of image chosen by radio buttons;
        private List<Bitmap> sourceImages = new List<Bitmap>();
        private Bitmap resultImage = new Bitmap(1, 1);
        private ScriptRenderer scriptRenderer; // performs processing source images by script
        
        private static readonly object controlsLocker = new object(); // locks access to controls for threads
        public FrmMain()
        {
            InitializeComponent();
            scriptRenderer = new ScriptRenderer(RefreshRenderingProgress, UpdateControlsOnRenderingStarted, UpdateControls_OnRenderingFinished);

            // display built date:
            DateTime? dt = getAssemblyBuildDateTime();
            if (dt != null)
            {
                this.Text += " (built " + ((DateTime)dt).ToString("yyyyMMdd") + ")";
            }

            CreateStartupBitmaps();
            RefreshImagesPixelInfo(); // refresh pixel info components text
        }
        private void CreateStartupBitmaps()
        {
            sourceImages.Add(new Bitmap(1, 1));
            sourceImages.Add(new Bitmap(1, 1));
            imagesControlsPanel.AddImageControlsSet();
            imagesControlsPanel.TryCheckPanelAtIndex(0);
            imagesControlsPanel.AddImageControlsSet();
        }
        // CONTROLS EVENTS METHODS: *****************************************************************************
        // https://stackoverflow.com/questions/22426390/disable-selection-of-controls-by-arrow-keys-in-a-form
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // overrides default RadioButton action for imagesControlsPanel controls
        {
            // imagesControlsPanel radio buttons are scattered between separate panels,
            // so it's impossible to move between them by arrows;
            // instead of creating separate panel for radio buttons only, I decided to
            // intercept key event for this particular case, and perform transition
            // manually in imagesControlsPanel.rbSelectImage_ManualKeyDown();
            // The second reason to intercept key event was the problem with default
            // RadioButton action, which automatically changes the focus to other
            // control on the same panel, which is much undesirable in this case;
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
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((scriptRenderer != null) && (scriptRenderer.Running))
            {
                scriptRenderer.StopRendering();
            }
        }
        private void pb_MouseLeave(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        private void imagesControlsPanel_OnRemoveImageClick(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;

            int index = (int)btn.Tag;

            if ((index < 0) || (index >= sourceImages.Count))
            {
                return;
            }

            if (scriptRenderer.Running)
            {
                scriptRenderer.StopRendering();
            }

            sourceImages.RemoveAt(index);

            // adjust result image dimensions:
            Point newResultDimensions = Helpers.GetImagesSizeIntersection(sourceImages);
            resultImage = new Bitmap(newResultDimensions.X, newResultDimensions.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        private void imagesControlsPanel_OnAddImageClick(object sender, EventArgs e)
        {
            sourceImages.Add(new Bitmap(1, 1));
            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        private void imagesControlsPanel_OnLoadImageClick(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;

            int index = (int)btn.Tag;

            if ((index<0) || (index > imagesControlsPanel.GetPanelsCount() - 2)
                || (odLoadImage.ShowDialog() != DialogResult.OK))
            {
                return;
            }

            if (scriptRenderer.Running)
            {
                scriptRenderer.StopRendering(); 
            }

            btn.Text = Path.GetFileName(odLoadImage.FileName);

            if (index >= sourceImages.Count) { sourceImages.Add(new Bitmap(odLoadImage.FileName)); }
                                        else { sourceImages[index] = new Bitmap(odLoadImage.FileName); }
            imagesControlsPanel.TryCheckPanelAtIndex(index);

            // adjust result image dimensions:
            Point newResultDimensions = Helpers.GetImagesSizeIntersection(sourceImages);
            resultImage = new Bitmap(newResultDimensions.X, newResultDimensions.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        private void imagesControlsPanel_OnSwitchImageClick(object sender, EventArgs e)
        {
            if (sourceImages.Count < 2)
            {
                return;
            }
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;

            int index = (int)btn.Tag;
            if ((index >= 0) && (index < sourceImages.Count))
            {
                sourceImages.Reverse(index, 2);
                RefreshImagesPixelInfo();
                RefreshPreview(scriptRenderer.Running);
            }
        }
        private void imagesControlsPanel_OnImageSelected(object sender, EventArgs e)
        {
            if (!(sender is RadioButton))
            {
                return;
            }
            RadioButton rb = (RadioButton)sender;
            if (!rb.Checked)
            {
                return;
            }
            int index = (int)rb.Tag;

            if ((index < 0) || (index >= sourceImages.Count))
            { 
                currentImageIndex = resultImageFictionalIndex; 
            }
            else 
            {
                currentImageIndex = index;
                if (scriptRenderer.Running)
                {
                    scriptRenderer.StopRendering(); 
                }
            }
            RefreshPreview(false);
        }
        private void rbInterpolationMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton))
            {
                return;
            }
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
        private void chbDisplayPixelInfo_CheckedChanged(object sender, EventArgs e)
        {
            chbPixelInfoDisplayAlpha.Enabled = chbPixelInfoHex.Enabled = chbDisplayPixelInfo.Checked;
            RefreshImagesPixelInfo();
        }
        private void chbPixelInfoDisplayAlpha_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        private void chbPixelInfoHex_CheckedChanged(object sender, EventArgs e)
        {
            RefreshImagesPixelInfo();
        }
        private void tbScriptInput_TextChanged(object sender, EventArgs e)
        {
            if (currentImageIndex != resultImageFictionalIndex)
            {
                return;
            }
            RefreshPreview(cbAutoRunScriptAfterChange.Checked);
        }
        private void btnRunStopScript_Click(object sender, EventArgs e)
        {
            if (currentImageIndex != resultImageFictionalIndex)
            {
                imagesControlsPanel.TryCheckPanelAtIndex(imagesControlsPanel.GetPanelsCount() - 1);
            }
            if (scriptRenderer.Running)
            {
                scriptRenderer.StopRendering();
            }
            else
            {
                RefreshPreview(true);
            }
        }
        private void btnLoadScript_Click(object sender, EventArgs e)
        {
            if (odLoadScript.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (scriptRenderer.Running)
            {
                scriptRenderer.StopRendering();
            }
            tbScriptInput.Text = File.ReadAllText(odLoadScript.FileName);
        }
        private void btnSaveScript_Click(object sender, EventArgs e)
        {
            if (sdSaveScript.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            File.WriteAllText(sdSaveScript.FileName, tbScriptInput.Text);
        }
        private void btnSaveResultImage_Click(object sender, EventArgs e)
        {
            if (resultImage is null)
            {
                MessageBox.Show("Result image is empty.");
                return;
            }
            if (sdSaveResultImage.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ImageFormat imgFormat;

            switch (Path.GetExtension(sdSaveResultImage.FileName).ToLower())
            {
                case ".bmp":
                    imgFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imgFormat = ImageFormat.Png;
                    break ;
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

            resultImage.Save(sdSaveResultImage.FileName, imgFormat);
            MessageBox.Show("File saved.");
        }
        private void splitter_SplitterMoving(object sender, SplitterEventArgs e)
        {
            mainSplitter.SplitPosition = mainSplitter.SplitPosition; // refresh controls state
            tsslState.Width = mainSplitter.Left;
        }
        private void leftPanelSplitter_SplitterMoving(object sender, SplitterEventArgs e)
        {
            leftPanelSplitter.SplitPosition = leftPanelSplitter.SplitPosition; // refresh controls state
        }
        // THREADS EVENTS METHODS: *****************************************************************************
        private void UpdateControlsOnRenderingStarted()
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
        // OTHER METHODS: *****************************************************************************
        public static DateTime? getAssemblyBuildDateTime() // build version method
        { // https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var attr = Attribute.GetCustomAttribute(assembly, typeof(BuildDateTimeAttribute)) as BuildDateTimeAttribute;
            if (DateTime.TryParse(attr?.Date, out DateTime dt))
                return dt;
            else
                return null;
        }
        private void RefreshPreview(bool startRendering)
        {
            if ((currentImageIndex != resultImageFictionalIndex)
                && ((currentImageIndex<0) || (currentImageIndex >= sourceImages.Count)))
            {
                return;
            }

            if (currentImageIndex == resultImageFictionalIndex)
            {
                if (startRendering)
                {
                    if (sourceImages.Count == 0)
                    {
                        MessageBox.Show("No source images.");
                        return;
                    }
                    scriptRenderer.StartRendering(sourceImages, tbScriptInput.Text); // TODO: TEMP
                }
                else
                {
                    lock (controlsLocker)
                    {
                        pb.Image = resultImage;
                    }
                }
            }
            else
            {
                lock (controlsLocker)
                {
                    pb.Image = sourceImages[currentImageIndex];
                }
            }
        }
        private void RefreshRenderingProgress(Bitmap newImage, string newStatus)
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
                    tsslState.Text = newStatus.Replace("\r\n"," ");
                    RefreshImagesPixelInfo();
                });                
            }
        }        
        private void RefreshImagesPixelInfo()
        {
            if (this.chbDisplayPixelInfo.Checked)
            {
                if (!imagesControlsPanel.PixelFormatLabelsVisible())
                {
                    imagesControlsPanel.ShowPixelFormatLabels();
                }
            }
            else
            {
                if (imagesControlsPanel.PixelFormatLabelsVisible())
                {
                    imagesControlsPanel.HidePixelFormatLabels();
                }
                return;
            }
            string pixelDigitFormat = (chbPixelInfoHex.Checked) ? "X2" : "D3";
            string pixelStringFormat = (chbPixelInfoDisplayAlpha.Checked)
                ? ("({0:XX} {1:XX} {2:XX} {3:XX})").Replace("XX", pixelDigitFormat)
                : ("({1:XX} {2:XX} {3:XX})").Replace("XX", pixelDigitFormat);

            if ((pb.ImagePointer is null) && (pb.currentMouseImagePos is null))
            {
                tsslCursorCoords.Text = "";
                for (int i = 0; i < imagesControlsPanel.GetPanelsCount(); i++)
                {
                    imagesControlsPanel.SetPixelFormatLabelText(i, "");
                }
            }
            else
            {
                Point p = (pb.ImagePointer != null) ? (Point)pb.ImagePointer : (Point)pb.currentMouseImagePos;
                tsslCursorCoords.Text = $"({p.X}; {p.Y})";

                for (int i = 0; i < sourceImages.Count; i++)
                {
                    imagesControlsPanel.SetPixelFormatLabelText(i, FormatBitmapPixelInfo(pixelStringFormat, sourceImages[i], p));
                }
                imagesControlsPanel.SetPixelFormatLabelText(imagesControlsPanel.GetPanelsCount() - 1, FormatBitmapPixelInfo(pixelStringFormat, resultImage, p));
            }
        }
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