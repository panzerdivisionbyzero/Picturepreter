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

            btn.Text = Path.GetFileName(odLoadImage.FileName);

            images[btn.TabIndex] = new Bitmap(odLoadImage.FileName);

            if (btn.TabIndex == 0)
                rbPreviewModeImg1.Checked = true;
            else
                rbPreviewModeImg2.Checked = true;
            
            RefreshPreview();
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

            RefreshPreview();
        }

        private void rbPreviewModeImg_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("rbPreviewModeImg_CheckedChanged()");
            if (!(sender is RadioButton)) return;
            RadioButton rb = (RadioButton)sender;
            if ((!rb.Checked) 
                || (0 > rb.TabIndex || rb.TabIndex > images.Length)) return;
            currentImageIndex = rb.TabIndex;
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (0 > currentImageIndex || currentImageIndex > images.Length) return;

            if (currentImageIndex == (int)ImagesIndexes.imageResult)
                renderer.StartRenderer(images[0], images[1], tbScriptInput.Text, OnRenderFinish);
            else
                pb.Image = images[currentImageIndex];
        }

        

        private void OnRenderFinish(Bitmap newImage, string newStatus)
        {
            Debug.WriteLine("OnRenderFinish()");
            lock (controlsLocker)
            {
                this.tbScriptOutput.BeginInvoke((MethodInvoker)delegate
                {
                    tbScriptOutput.Text = newStatus;
                });
                if (newImage != null)
                {
                    this.pb.BeginInvoke((MethodInvoker)delegate
                    {
                        pb.Image = newImage;
                    });
                }
            }
            Debug.WriteLine("OnRenderFinish(); Set();");
        }
        

        private void tbScriptInput_TextChanged(object sender, EventArgs e)
        {
            if (currentImageIndex != (int)ImagesIndexes.imageResult) return;

            RefreshPreview();
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
    }
}