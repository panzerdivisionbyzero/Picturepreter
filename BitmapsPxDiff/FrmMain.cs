using MoonSharp.Interpreter;

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
            for (y = 0; y < resultImage.Height; y++)
            {
                for (x = 0; x < resultImage.Width; x++)
                {
                    c1 = src1.GetPixel(x, y);
                    c2 = src2.GetPixel(x, y);
                    if (!LuaChangeColor(c1, c2, ref cr, ref errorMessage))
                    {
                        tbScriptOutput.Text = errorMessage;
                        return false;
                    }
                    resultImage.SetPixel(x, y, cr);
                }
                this.Text = ((y+1)*100/resultImage.Height).ToString()+" %; color = "+cr.ToString();
            }
            tbScriptOutput.Text = "Script processed successfully";
            return true;   
        }
        private bool LuaChangeColor(Color c1, Color c2, ref Color result, ref string errorMessage)
        {
            uint i = 0;
            string script = "";
            try
            {
                script = @"  
                function CastToByte(i)
                    --if i<0 then i = i % 256 + 255 end
                    if i>255 then i = i % 256 end
                    return i
                end
		        function ChangeColor (image1A,image1R,image1G,image1B,image2A,image2R,image2G,image2B)"+"\r\n"
                    + tbScriptInput.Text + "\r\n"
                    + @"return CastToByte(resultR) + CastToByte(resultG)*256 + CastToByte(resultB)*65536 + CastToByte(resultA)*16777216
		        end
                return ChangeColor(" + c1.A.ToString() + "," + c1.R.ToString() + "," + c1.G.ToString() + "," + c1.B.ToString() + ","
                                     + c2.A.ToString() + "," + c2.R.ToString() + "," + c2.G.ToString() + "," + c2.B.ToString() + ")";

                DynValue res = Script.RunString(script);
                i = Convert.ToUInt32(res.Number);
            }
            catch (Exception e)
            {
                errorMessage = "Script error:\r\n" + e.Message + "\r\nGenerated script:\r\n"+script;
                return false;
            }

            result = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));
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