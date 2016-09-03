using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace IFS_Thesis
{
    public class IfsDrawer
    {
        public void SaveIfsImage(List<double[]> ifsMappings, int imgx, int imgy, string path)
        {
            var bmp = DrawIfs(ifsMappings, imgx, imgy);
            bmp.Save(path, ImageFormat.Png);
            bmp.Dispose();
        }

        public Bitmap DrawIfs(List<double[]> ifsMappings, int imgx, int imgy)
        {
            var randomGen = new Random();

            var length = ifsMappings.Count;

            var x = ifsMappings[0][4];
            var y = ifsMappings[0][5];
            var xa = x;
            var xb = x;
            var ya = y;
            var yb = y;

            for (int k = 0; k < imgx*imgy; k++)
            {
                var p = randomGen.NextDouble();
                var psum = 0.0;

                var i = 0;

                for (int j = 0; j < length; j++)
                {
                    psum += ifsMappings[j][6];

                    i = j;

                    if (p <= psum)
                        break;
                    }

                var x0 = x*ifsMappings[i][0] + y*ifsMappings[i][1] + ifsMappings[i][4];
                    y = x*ifsMappings[i][2] + y*ifsMappings[i][3] + ifsMappings[i][5];
                    x = x0;

                    if (x < xa)
                        xa = x;
                    if (x > xb)
                        xb = x;
                    if (y < ya)
                        ya = y;
                    if (y > yb)
                        yb = y;
                }

            imgy = Convert.ToInt32(imgy * (yb - ya) / (xb - xa)); //auto-re-adjust the aspect ratio

            var bmpImage = DrawFilledRectangle(imgx, imgy);

            x = 0.0;
            y = 0.0;

            for (int k = 0; k < imgx * imgy; k++)
            {
                var p = randomGen.NextDouble();
                var psum = 0.0;

                var i = 0;

                for (int j = 0; j < length; j++)
                {
                    psum += ifsMappings[j][6];

                    i = j;

                    if (p <= psum)
                        break;
                }

                var x0 = x * ifsMappings[i][0] + y * ifsMappings[i][1] + ifsMappings[i][4];
                    y = x * ifsMappings[i][2] + y * ifsMappings[i][3] + ifsMappings[i][5];
                    x = x0;

                    var jx = Convert.ToInt32((x - xa)/(xb - xa)*(imgx - 1));
                    var jy = (imgy - 1) - Convert.ToInt32((y - ya)/(yb - ya)*(imgy - 1));

                    bmpImage.SetPixel(jx, jy, Color.Black);
             }
            

            return bmpImage;
        }

        private Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y, PixelFormat.Format24bppRgb);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle imageSize = new Rectangle(0, 0, x, y);
                graph.FillRectangle(Brushes.White, imageSize);
            }
            return bmp;
        }
    }
}
