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

            #region Weird shit

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

            imgy = Convert.ToInt32(imgy*(yb - ya)/(xb - xa)); //auto-re-adjust the aspect ratio

            var bmpImage = DrawFilledRectangle(imgx, imgy);

            #endregion

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

        public void DrawIFSImageMyVersion()
        {

        }

        public List<Point> CreateIfsPointsMyVersion(List<double[]> ifsMappings, int numberOfIterations)
        {
            List<Point> resultPoints = new List<Point>();

            var startingPoint = new Point(1,1);
            var numberOfFunctions = ifsMappings.Count;

            var currentPoint = startingPoint;
            var currentFunctionIndex = 0;

            for (int i = 0; i < numberOfIterations; i++)
            {
                if (numberOfFunctions > 1)
                {
                    var p = new Random().NextDouble();
                    var psum = 0.0;

                    currentFunctionIndex = 0;

                    for (int j = 0; j < numberOfFunctions; j++)
                    {
                        psum += ifsMappings[j][6];

                        currentFunctionIndex = j;

                        if (p <= psum)
                            break;
                    }


                }

                currentPoint = ApplyIFSTransformation(ifsMappings[currentFunctionIndex], currentPoint);

                resultPoints.Add(currentPoint);
            }

            return resultPoints;
        }

        private Point ApplyIFSTransformation(double[] currentFunction, Point currentPoint)
        {
            var x0 = currentPoint.X;
            var y0 = currentPoint.Y;

            int x = Convert.ToInt32(currentFunction[0] * x0 + currentFunction[1] * y0 + currentFunction[4]);
            int y = Convert.ToInt32(currentFunction[2] * x0 + currentFunction[3] * y0 + currentFunction[5]);

            return new Point(x, y);

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
