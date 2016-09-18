using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis
{
    public class ImageParser
    {
        public List<Point> GetMatchingPixels(Bitmap image, Color color)
        {

            List<Point> resultPoints = new List<Point>();
            List<Color> resultColors = new List<Color>();

            try
            {
                LockBitmap lockBitmap = new LockBitmap(image);
                lockBitmap.LockBits();

                for (int y = 0; y < lockBitmap.Height; y++)
                {
                    for (int x = 0; x < lockBitmap.Width; x++)
                    {
                        resultColors.Add(lockBitmap.GetPixel(x, y));

                        if (lockBitmap.GetPixel(x, y).ToArgb() == color.ToArgb())
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

        public void ConvertBytesToPixelsByColor(int rgbColorValue, byte[] bytes)
        {
            List<Point> resultPoints = new List<Point>();

            foreach (var element in bytes)
            {
                if (element.Equals((byte)rgbColorValue))
                {
                    var point = new Point();
                }
            }
        }
    }
}
