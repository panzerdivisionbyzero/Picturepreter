namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private enum ImagesIndexes { image1, image2, imageResult };
        private int currentImageIndex = 0;
        private Bitmap[] images = new Bitmap[3];
        private LuaScriptCalc luaScriptCalc = new LuaScriptCalc();
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

            bool refreshImage = true;

            if (currentImageIndex == (int)ImagesIndexes.imageResult)
                refreshImage = RenderResult(images[0], images[1], ref images[2]);

            if (refreshImage) pb.Image = images[currentImageIndex];
        }
        private bool RenderResult(Bitmap src1, Bitmap src2, ref Bitmap resultImage)
        {
            if ((src1 is null) || (src2 is null)) return false;
            int x = Math.Min(src1.Width, src2.Width);
            int y = Math.Min(src1.Height, src2.Height);
            resultImage = new Bitmap(x, y);

            Color c1, c2, cr;
            cr = Color.Black;
            string errorMessage = "";
            uint[] pixels1 = new uint[x * y];
            uint[] pixels2 = new uint[x * y];
            uint[] pixelsOut = new uint[x * y];
            for (y = 0; y < resultImage.Height; y++)
            {
                for (x = 0; x < resultImage.Width; x++)
                {
                    c1 = src1.GetPixel(x, y);
                    c2 = src2.GetPixel(x, y);
                    pixels1[y * resultImage.Width + x] = (uint)(c1.R + (c1.G << 8) + (c1.B << 16) + (c1.A << 24));
                    pixels2[y * resultImage.Width + x] = (uint)(c2.R + (c2.G << 8) + (c2.B << 16) + (c2.A << 24));
                }
            }
            if (!luaScriptCalc.LuaChangeColor(tbScriptInput.Text, ref pixels1, ref pixels2, ref pixelsOut, ref errorMessage))
            {
                tbScriptOutput.Text = errorMessage;
                return false;
            }
            uint i;
            for (y = 0; y < resultImage.Height; y++)
            {
                for (x = 0; x < resultImage.Width; x++)
                {
                    i = pixelsOut[y * resultImage.Width + x];
                    cr = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));  
                    resultImage.SetPixel(x, y, cr);
                }
            }

            tbScriptOutput.Text = "Script processed successfully";
            return true;   
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