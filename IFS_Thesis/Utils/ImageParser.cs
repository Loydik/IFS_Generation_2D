using System;
using System.Collections.Generic;
using System.Drawing;

namespace IFS_Thesis.Utils
{
    /// <summary>
    /// Class for parsing Images
    /// </summary>
    public class ImageParser
    {
        /// <summary>
        /// Get pixels from an image which match a given color
        /// </summary>
        public List<Point> GetMatchingPixels(Bitmap image, Color color)
        {
            var resultPoints = new List<Point>();
            var colorArgb = color.ToArgb();

            try
            {
                var lockBitmap = new LockBitmap(image);
                lockBitmap.LockBits();

                for (int y = 0; y < lockBitmap.Height; y++)
                {
                    for (int x = 0; x < lockBitmap.Width; x++)
                    {
                        var pixel = lockBitmap.GetPixel(x, y);

                        if (pixel.ToArgb() == colorArgb)
                        {
                            resultPoints.Add(new Point(x,y));                            
                        }
                    }
                }

                lockBitmap.UnlockBits();
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("There was an error opening the bitmap." +
                    "Please check the path.");
            }

            return resultPoints;
        }

    }
}
