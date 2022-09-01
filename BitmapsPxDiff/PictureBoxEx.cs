using System.Drawing.Drawing2D;


namespace BitmapsPxDiff
{
    /// <summary>
    /// Inherits from PictureBox; Added features:
    /// - Interpolation Mode setting;
    /// - Pixel Offset Mode setting;
    /// - setting and displaying Image Pointer;
    /// </summary>
    public class PictureBoxEx : PictureBox
    {
        // stores original image assigned to component; needed to repaint image in ClearPointer():
        private Image? _imageBackup;

        // image pointer fields:
        private bool imagePointerSet = false;
        private Point _imagePointer;
        public Point? currentMouseImagePos;
        private bool disablePrintImagePointer = false; // prevents OnImageChange loop when drawing pointer

        // events:
        public event EventHandler? OnImageChange;

        // properties:
        public InterpolationMode InterpolationMode { get; set; }
        public PixelOffsetMode PixelOffsetMode { get; set; }

        /// <summary>
        /// Overrides PictureBox.Image property to hook up OnImageChange() event and to allow drawing on Image without affecting original Image ("get" returns _imageBackup);
        /// inspired by: https://www.codeproject.com/messages/3182303/re-image-changed-in-picturebox-event-question.aspx
        /// </summary>
        public new Image? Image
        {
            get { return _imageBackup; }
            set
            {
                base.Image = value;
                // _imageBackup update:
                _imageBackup = (value is null) ? _imageBackup = null : _imageBackup = (Image)value.Clone();

                if (OnImageChange != null) // event call
                {
                    OnImageChange(this, new EventArgs());
                }
                if (!disablePrintImagePointer) // draw pointer on the new image
                {
                    DrawImagePointer();
                }
            }
        }
        /// <summary>
        /// Pixel selection set by user;
        /// returns _imagePointer if imagePointerSet
        /// </summary>
        public Point? ImagePointer
        {
            get
            {
                if (imagePointerSet)
                {
                    return _imagePointer;
                }
                return null;
            }
        }
        // IMAGE POINTER METHODS *********************************************
        /// <summary>
        /// Clears pixel selection (ImagePointer);
        /// </summary>
        private void ClearPointer()
        {
            if (!imagePointerSet)
            {
                return;
            }
            imagePointerSet = false;
            base.Image = _imageBackup;
        }
        /// <summary>
        /// Redefines pixel selection (ImagePointer);
        /// </summary>
        /// <param name="p">new ImagePointer coordinates</param>
        private void RedefinePointer(Point p)
        {
            _imagePointer = p;
            imagePointerSet = true;
            DrawImagePointer();
        }
        /// <summary>
        /// Draws ImagePointer on Image; Gets original Image form _imageBackup;
        /// </summary>
        private void DrawImagePointer()
        {
            if (disablePrintImagePointer || (Image is null))
            {
                return;
            }
            disablePrintImagePointer = true;
            if (imagePointerSet && (_imageBackup != null))
            {
                Bitmap map = new Bitmap(_imageBackup.Width, _imageBackup.Height);
                Graphics g = Graphics.FromImage(map);
                g.DrawImage(_imageBackup, 0, 0);
                Pen pen = new Pen(Color.Red, 1.0f);
                int size = (int)Math.Round(Math.Max(2, Math.Max(_imageBackup.Width, _imageBackup.Height) * 0.025));
                g.DrawRectangle(pen, _imagePointer.X - 1, _imagePointer.Y - 1, 2, 2);
                g.DrawLine(pen, _imagePointer.X - 1, _imagePointer.Y, _imagePointer.X - size, _imagePointer.Y); // left line
                g.DrawLine(pen, _imagePointer.X + 1, _imagePointer.Y, _imagePointer.X + size, _imagePointer.Y); // right line
                g.DrawLine(pen, _imagePointer.X, _imagePointer.Y - 1, _imagePointer.X, _imagePointer.Y - size); // top line
                g.DrawLine(pen, _imagePointer.X, _imagePointer.Y + 1, _imagePointer.X, _imagePointer.Y + size); // bottom line

                base.Image = map;

                pen.Dispose();
                g.Dispose();
            }
            disablePrintImagePointer = false;
        }
        // EVENTS METHODS *********************************************
        /// <summary>
        /// Overrides PictureBox.OnPaint() event to change InterpolationMode and PixelOffsetMode;
        /// based on: https://stackoverflow.com/questions/29157/how-do-i-make-a-picturebox-use-nearest-neighbor-resampling
        /// </summary>
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode;

            base.OnPaint(paintEventArgs);
        }
        /// <summary>
        /// Overrides PictureBox.OnMouseMove() event to get currentMouseImagePos;
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            currentMouseImagePos = TranslateZoomMousePosition(new Point(e.X, e.Y));
            base.OnMouseMove(e);
        }
        /// <summary>
        /// Overrides PictureBox.OnMouseLeave() event to reset currentMouseImagePos;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            currentMouseImagePos = null;
            base.OnMouseLeave(e);
        }
        /// <summary>
        /// Overrides PictureBox.OnMouseClick() event to perform image pointer feature actions (RedefinePointer(), ClearPointer());
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            // processing image pointer change:
            if (Image != null)
            {
                // checking is mouse within image rect:
                Point p = TranslateZoomMousePosition(new Point(e.X, e.Y));
                if (!new Rectangle(0, 0, Image.Width, Image.Height).Contains(p))
                {
                    return;
                }
                // pointer redefine / clear:
                if (e.Button == MouseButtons.Left)
                {
                    RedefinePointer(p);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    ClearPointer();
                }
            }
        }
        // OTHER METHODS *********************************************
        /// <summary>
        /// Translates component mouse coordinates to scaled image pixel coordinates;
        /// taken from: https://www.codeproject.com/Articles/20923/Mouse-Position-over-Image-in-a-PictureBox
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public Point TranslateZoomMousePosition(Point coordinates)
        {
            if ((Image == null) || (Width == 0 || Height == 0 || Image.Width == 0 || Image.Height == 0))
            {
                return coordinates;
            }
            // need to check the aspect ratio of the image to the aspect ratio of the control
            // to determine how it is being rendered
            float imageAspect = (float)Image.Width / Image.Height;
            float controlAspect = (float)Width / Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)Image.Width / Width;
                newX *= ratioWidth;
                float scale = (float)Width / Image.Width;
                float displayHeight = scale * Image.Height;
                float diffHeight = Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)Image.Height / Height;
                newY *= ratioHeight;
                float scale = (float)Height / Image.Height;
                float displayWidth = scale * Image.Width;
                float diffWidth = Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }
    }
}