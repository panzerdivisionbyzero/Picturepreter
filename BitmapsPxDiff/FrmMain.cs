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

            // TODO: TEMP
            Color c = Color.FromArgb(12, 23, 34, 45);
            int i = c.R + (c.G << 8) + (c.B << 16) + (c.A << 24);

            Color c2 = LuaChangeColor(c);

            //Color c2 = Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));  

            MessageBox.Show(c2.ToString());
            //MessageBox.Show(MoonSharpFactorial().ToString());
        }

        Color LuaChangeColor(Color c)
        {
            string script = @"    
		    function ChangeColor (a,r,g,b)
                a = a + 1
                r = r + 1
                g = g + 1
                b = b + 1
                return r + g*256 + b*65536 + a*16777216
		    end
            return ChangeColor(" + c.A.ToString() + "," + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
            //MessageBox.Show(script);
            DynValue res = Script.RunString(script);
            int i = Convert.ToInt32(res.Number);
            return Color.FromArgb((byte)(i >> 24), (byte)i, (byte)(i >> 8), (byte)(i >> 16));
        }

        double MoonSharpFactorial()
        {
            string script = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

	return fact(5)";

            DynValue res = Script.RunString(script);
            return res.Number;
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

            if (currentImageIndex == (int)ImagesIndexes.imageResult)
                images[2] = RenderResult(images[0], images[1]);

            pb.Image = images[currentImageIndex];
        }
        private Bitmap RenderResult(Bitmap src1, Bitmap src2)
        {
            if ((src1 is null) || (src2 is null)) return null;
            int x = Math.Min(src1.Width, src2.Width);
            int y = Math.Min(src1.Height, src2.Height);
            Bitmap resultImage = new Bitmap(x, y);

            Color c1, c2, cr;
            for (y = 0; y < resultImage.Height; y++)
                for (x = 0; x < resultImage.Width; x++)
                {
                    c1 = src1.GetPixel(x, y);
                    c2 = src2.GetPixel(x, y);
                    cr = Color.FromArgb(255,
                                        Math.Abs(c2.R - c1.R),
                                        Math.Abs(c2.G - c1.G),
                                        Math.Abs(c2.B - c1.B));
                    resultImage.SetPixel(x, y, cr);
                }
            return resultImage;
        }

    }
}