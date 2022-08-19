using System.Diagnostics;
namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private enum ImagesIndexes { image1, image2, imageResult };
        private int currentImageIndex = 0;
        private Bitmap[] images = new Bitmap[3];
        private Renderer renderer = new Renderer();
        
        private static readonly object controlsLocker = new object();
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button btn = (Button)sender;
            if ((0 > btn.TabIndex || btn.TabIndex > 1)
                || (odLoadImage.ShowDialog() != DialogResult.OK)) return;

            if (renderer.Running) renderer.StopRendering();

            btn.Text = Path.GetFileName(odLoadImage.FileName);

            images[btn.TabIndex] = new Bitmap(odLoadImage.FileName);

            if (btn.TabIndex == 0)
                rbPreviewModeImg1.Checked = true;
            else
                rbPreviewModeImg2.Checked = true;

            if ((images[0] != null) && (images[1] != null))
                images[2] = new Bitmap(Math.Min(images[0].Width, images[1].Width),
                                       Math.Min(images[0].Height, images[1].Height));

            RefreshPreview(true);
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

            RefreshPreview(true);
        }

        private void rbPreviewModeImg_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return;
            RadioButton rb = (RadioButton)sender;
            if ((!rb.Checked)
                || (0 > rb.TabIndex || rb.TabIndex > images.Length)) return;
            currentImageIndex = rb.TabIndex;
            if ((currentImageIndex != (int)ImagesIndexes.imageResult)
                && (renderer.Running))
            {
                renderer.StopRendering();
            }

            RefreshPreview(false);
        }

        private void RefreshPreview(bool restartRendererIfRunning)
        {
            if (0 > currentImageIndex || currentImageIndex > images.Length) return;

            if (currentImageIndex == (int)ImagesIndexes.imageResult)
            {
                if (renderer.Running && (!restartRendererIfRunning))
                    pb.Image = images[2];
                else
                    renderer.StartRendering(images[0], images[1], tbScriptInput.Text, OnRenderFinish);
            }
            else
            {
                lock (controlsLocker)
                    pb.Image = images[currentImageIndex];
            }
        }        

        private void tbScriptInput_TextChanged(object sender, EventArgs e)
        {
            if (currentImageIndex != (int)ImagesIndexes.imageResult) return;

            RefreshPreview(true);
        }

        private void btnLoadScript_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveScript_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveResultImage_Click(object sender, EventArgs e)
        {

        }
        private void OnRenderFinish(Bitmap newImage, string newStatus)
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
                        Graphics g = Graphics.FromImage(images[2]);
                        g.DrawImage(newImage, 0, 0);
                        g.Dispose();
                        RefreshPreview(false);
                    });
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((renderer != null) && (renderer.Running))
                renderer.StopRendering();
        }
    }
}