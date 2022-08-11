namespace BitmapsPxDiff
{
    public partial class FrmMain : Form
    {
        private Bitmap bmp;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnLoadBitmap_Click(object sender, EventArgs e)
        {
            if (od.ShowDialog() != DialogResult.OK) return;
            this.btnLoadBitmap.Text = Path.GetFileName(od.FileName);
            bmp = new Bitmap(od.FileName);
            pb.Image = bmp;
        }

    }
}