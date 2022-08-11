namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private enum ImagesIndexes { image1, image2, imageResult };
        private int currentImageIndex = 0;
        private Bitmap[] images = new Bitmap[3];
        public FrmMain()
        {
            InitializeComponent();
        }

        private void RefreshPreview()
        {
            if (0 > currentImageIndex || currentImageIndex > images.Length) return;

            pb.Image = images[currentImageIndex];
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button btn = (Button)sender;
            if ((0 > btn.TabIndex || btn.TabIndex > 1)
                || (od.ShowDialog() != DialogResult.OK)) return;

            btn.Text = Path.GetFileName(od.FileName);

            images[btn.TabIndex] = new Bitmap(od.FileName);

            if (btn.TabIndex == 0)
                rbPreviewModeImg1.Checked = true;
            else
                rbPreviewModeImg2.Checked = true;
            
            RefreshPreview();
        }

        private void rbPreviewModeImg_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton)) return;
            RadioButton rb = (RadioButton)sender;
            if ((!rb.Checked) 
                || (0 > rb.TabIndex || rb.TabIndex > images.Length)) return;
            currentImageIndex = rb.TabIndex;
            RefreshPreview();
        }
    }
}