using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapsPxDiff
{
    //public static Helpers helpers;
    public static class Helpers
    {
        public static Point GetImagesSizeIntersection(List<Bitmap> images)
        {
            if ((images is null) || (images.Count == 0))
            {
                return new Point(1, 1);
            }
            Point result = new Point(int.MaxValue, int.MaxValue);
            for (int i = 0; i < images.Count; i++)
            {
                if (images[i].Width < result.X) { result.X = images[i].Width; }
                if (images[i].Height < result.Y) { result.Y = images[i].Height; }
            }
            return result;
        }
    }
}
