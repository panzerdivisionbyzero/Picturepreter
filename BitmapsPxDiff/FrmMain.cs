using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private enum ImagesIndexes { image1, image2, imageResult };
        private int currentImageIndex = 0;
        private Bitmap[] images = new Bitmap[3];
        private Renderer renderer;
        
        private static readonly object controlsLocker = new object();
        public FrmMain()
        {
            InitializeComponent();
            renderer = new Renderer(OnRefreshRenderingProgress, OnRenderingFinished);
    }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((renderer != null) && (renderer.Running))
            {
                renderer.StopRendering();
            }
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

            if (renderer.Running)
            {
                renderer.StopRendering(); 
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

            RefreshPreview(renderer.Running);
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
                && (renderer.Running))
            {
                renderer.StopRendering();
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
                    renderer.StartRendering(images[0], images[1], tbScriptInput.Text);
                    btnRunStopScript.Text = "Stop script execution";
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
            if (renderer.Running)
            {
                renderer.StopRendering();
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
            if (renderer.Running)
            {
                renderer.StopRendering();
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
        private void OnRefreshRenderingProgress(Bitmap newImage, string newStatus)
        {
            lock (controlsLocker)
            {
                this.tbScriptOutput.BeginInvoke((MethodInvoker)delegate
                {
                    tbScriptOutput.Text = newStatus;
                });
                if ((newImage != null) && (currentImageIndex == (int)ImagesIndexes.imageResult))
                {
                    this.pb.BeginInvoke((MethodInvoker)delegate
                    {
                        images[2] = (Bitmap)newImage.Clone();
                        RefreshPreview(false);
                    });
                }
            }
        }
        private void OnRenderingFinished()
        {
            lock (controlsLocker)
            {
                this.btnRunStopScript.BeginInvoke((MethodInvoker)delegate
                {
                    btnRunStopScript.Text = "Run script";
                });
            }
        }
    }
}