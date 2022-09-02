/*
 * This unit is part of RPI Backing Track Player.
 * 
 * 
 * Licensed under the terms of the GNU GPL 2.0 license,
 * excluding used libraries:
 * -United Openlibraries of Sound(uos) licensed under LGPL 2.1;
 * and used code snippets marked with link to original source.
 * 
 * 
 * Copyright(c) 2018 - 2022 by Pawel Witkowski
 * 
 * pawel.vitek.witkowski @gmail.com 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picturepreter
{
    /// <summary>
    /// Contains helper methods for other classes;
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Calculates given images size intersection;
        /// </summary>
        /// <param name="images">Source images to compare</param>
        /// <returns>Intersection size</returns>
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
