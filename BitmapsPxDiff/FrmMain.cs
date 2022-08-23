using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private enum ImagesIndexes { image1, image2, imageResult };
        private enum ImagePointerPixelInfoFormat { argbHex, argbDec, rgbHex , rgbDec };

        private int currentImageIndex = 0; // index of image chosen by radio buttons
        private Bitmap[] images = new Bitmap[3]; // source bitmaps + result (indexes described by enum ImagesIndexes)
        private ScriptRenderer scriptRenderer; // performs processing source images by script
        private ImagePointerPixelInfoFormat imagePointerPixelInfoFormat = ImagePointerPixelInfoFormat.argbHex; // chosen image pointed pixel info format (displayed on statusStrip)
        
        private static readonly object controlsLocker = new object(); // locks access to controls for threads
        public FrmMain()
        {
            InitializeComponent();
            scriptRenderer = new ScriptRenderer(RefreshRenderingProgress, UpdateControlsOnRenderingStarted, UpdateControls_OnRenderingFinished);
            RefreshImagesPixelInfo(); // refresh components text
            // display built date:
            DateTime? dt = getAssemblyBuildDateTime();
            if (dt != null)
            {
                this.Text += " (built " + ((DateTime)dt).ToString("yyyyMMdd") + ")";
            }
        }
        // built version method:
        public static DateTime? getAssemblyBuildDateTime()
        { // https://stackoverflow.com/questions/1600962/displaying-the-build-date?answertab=modifieddesc#tab-top
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var attr = Attribute.GetCustomAttribute(assembly, typeof(BuildDateTimeAttribute)) as BuildDateTimeAttribute;
            if (DateTime.TryParse(attr?.Date, out DateTime dt))
                return dt;
            else
                return null;
        }
        // CONTROLS EVENTS METHODS: *****************************************************************************
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
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button))
            {
                return;
            }
            Button btn = (Button)sender;

            if ((0 > btn.TabIndex || btn.TabIndex > 1)
                || (odLoadImage.ShowDialog() != DialogResult.OK))
            {
                return;
            }

            if (scriptRenderer.Running)
            {
                scriptRenderer.StopRendering(); 
            }

            btn.Text = Path.GetFileName(odLoadImage.FileName);

            images[btn.TabIndex] = new Bitmap(odLoadImage.FileName);

            if (btn.TabIndex == 0)
            {
                rbPreviewModeImg1.Checked = true;
            }
            else
            {
                rbPreviewModeImg2.Checked = true;
            }

            if ((images[0] != null) && (images[1] != null))
            {
                images[2] = new Bitmap(Math.Min(images[0].Width, images[1].Width),
                                       Math.Min(images[0].Height, images[1].Height),
                                       System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }
            RefreshImagesPixelInfo();
            RefreshPreview(false);
        }
        private void btnSwapImages_Click(object sender, EventArgs e)
        {
            string s;
            Bitmap img;
            s = btnLoadImage1.Text;
            img = images[0];
            btnLoadImage1.Text = btnLoadImage2.Text;
            images[0] = images[1];
            btnLoadImage2.Text = s;
            images[1] = img;

            RefreshImagesPixelInfo();
            RefreshPreview(scriptRenderer.Running);
        }
        private void rbPreviewModeImg_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return;
            RadioButton rb = (RadioButton)sender;
            if ((!rb.Checked)
                || (0 > rb.TabIndex || rb.TabIndex > images.Length))
            {
                return;
            }
            currentImageIndex = rb.TabIndex;

            if ((currentImageIndex != (int)ImagesIndexes.imageResult)
                && (scriptRenderer.Running))
            {
                scriptRenderer.StopRendering();
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
        private void tbScriptInput_TextChanged(object sender, EventArgs e)
        {
            if (currentImageIndex != (int)ImagesIndexes.imageResult)
            {
                return;
            }

            RefreshPreview(cbAutoRunScriptAfterChange.Checked);
        }
        private void btnRunStopScript_Click(object sender, EventArgs e)
        {
            if (currentImageIndex != (int)ImagesIndexes.imageResult)
            {
                rbPreviewModeResult.Checked = true;
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
            if (images[2] is null)
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

            images[2].Save(sdSaveResultImage.FileName, imgFormat);
            MessageBox.Show("File saved.");
        }
        private void argbHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePointerPixelInfoFormat = ImagePointerPixelInfoFormat.argbHex;
            RefreshImagesPixelInfo();
        }
        private void argbDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePointerPixelInfoFormat = ImagePointerPixelInfoFormat.argbDec;
            RefreshImagesPixelInfo();
        }
        private void rgbHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePointerPixelInfoFormat = ImagePointerPixelInfoFormat.rgbHex;
            RefreshImagesPixelInfo();
        }
        private void rgbDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imagePointerPixelInfoFormat = ImagePointerPixelInfoFormat.rgbDec;
            RefreshImagesPixelInfo();
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
        private void RefreshPreview(bool startRendering)
        {
            if (0 > currentImageIndex || currentImageIndex > images.Length)
            {
                return;
            }

            if (currentImageIndex == (int)ImagesIndexes.imageResult)
            {
                if (startRendering)
                {
                    if ((images[0] is null) || (images[1] is null))
                    {
                        MessageBox.Show("Source images cannot be empty.");
                        return;
                    }
                    scriptRenderer.StartRendering(images[0], images[1], tbScriptInput.Text);
                }
                else
                {
                    pb.Image = images[2];
                }
            }
            else
            {
                lock (controlsLocker)
                {
                    pb.Image = images[currentImageIndex];
                }
            }
        }
        private void RefreshRenderingProgress(Bitmap newImage, string newStatus)
        {
            lock (controlsLocker)
            {
                if ((newImage != null) && (currentImageIndex == (int)ImagesIndexes.imageResult))
                {
                    this.pb.BeginInvoke((MethodInvoker)delegate
                    {
                        images[2] = (Bitmap)newImage.Clone();
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
            if ((pb.ImagePointer is null) && (pb.currentMouseImagePos is null))
            {
                tsslCursorCoords.Text = "";
                tsslImage1argb.Text = "";
                tsslImage2argb.Text = "";
                tsslImageResultargb.Text = "";
            }
            else
            {
                Point p = (pb.ImagePointer != null) ? (Point)pb.ImagePointer : (Point)pb.currentMouseImagePos;
                tsslCursorCoords.Text = $"({p.X}; {p.Y})";
                tsslImage1argb.Text = FormatBitmapPixelInfo("Image1: ", images[0], p);
                tsslImage2argb.Text = FormatBitmapPixelInfo("Image2: ", images[1], p);
                tsslImageResultargb.Text = FormatBitmapPixelInfo("Result: ", images[2], p);
            }
            switch (imagePointerPixelInfoFormat)
            {
                case ImagePointerPixelInfoFormat.argbHex:
                    tsddbSwitchPixelInfoFormat.Text = "ARGB Hex";
                    break;
                case ImagePointerPixelInfoFormat.argbDec:
                    tsddbSwitchPixelInfoFormat.Text = "ARGB Dec";
                    break;
                case ImagePointerPixelInfoFormat.rgbHex:
                    tsddbSwitchPixelInfoFormat.Text = "RGB Hex";
                    break;
                case ImagePointerPixelInfoFormat.rgbDec:
                    tsddbSwitchPixelInfoFormat.Text = "RGB Dec";
                    break;
                default:
                    tsddbSwitchPixelInfoFormat.Text = "Unknown";
                    break;
            }
        }
        private string FormatBitmapPixelInfo(string description, Bitmap bmp, Point p)
        {
            if ((bmp != null) && (new Rectangle(0, 0, bmp.Width, bmp.Height).Contains(p)))
            {
                Color c = bmp.GetPixel(p.X, p.Y);
                switch (imagePointerPixelInfoFormat)
                {
                    case ImagePointerPixelInfoFormat.argbHex:
                        return string.Format("{0}({1:X2} {2:X2} {3:X2} {4:X2})", description, c.A, c.R, c.G, c.B);
                    case ImagePointerPixelInfoFormat.argbDec:
                        return string.Format("{0}({1:D3} {2:D3} {3:D3} {4:D3})", description, c.A, c.R, c.G, c.B);
                    case ImagePointerPixelInfoFormat.rgbHex:
                        return string.Format("{0}({1:X2} {2:X2} {3:X2})", description, c.R, c.G, c.B);
                    case ImagePointerPixelInfoFormat.rgbDec:
                        return string.Format("{0}({1:D3} {2:D3} {3:D3})", description, c.R, c.G, c.B);
                    default:
                        return string.Format("{0}(Unknown pixel format)", description);
                }
            }
            return "";
        }

        private void splitter_SplitterMoving(object sender, SplitterEventArgs e)
        {
            splitter.SplitPosition = splitter.SplitPosition; // refresh controls state
        }
    }
}