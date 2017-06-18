using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace IFS_Thesis.Ifs.IFSDrawers
{
    /// <summary>
    /// Class used for drawing IFS
    /// </summary>
    public class IfsDrawer
    {
        #region Private Methods

        private PointF ApplyIFSTransformation(IfsFunction currentFunction, PointF currentPoint)
        {
            var x0 = currentPoint.X;
            var y0 = currentPoint.Y;

            var x = currentFunction.A * x0 + currentFunction.B * y0 + currentFunction.E;
            var y = currentFunction.C * x0 + currentFunction.D * y0 + currentFunction.F;

            return new PointF(x, y);
        }

        private Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y, PixelFormat.Format32bppRgb);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                graph.Clear(Color.White);
            }

            return bmp;
        }

        private Tuple<int, List<Point>> ConvertPointsToPixels(List<PointF> points, int imgx, int imgy)
        {
            int redundantPixels = 0;

            List<Point> pixels = new List<Point>();

            var xMin = points.Min(x => x.X);
            var xMax = points.Max(x => x.X);
            var yMin = points.Min(x => x.Y);
            var yMax = points.Max(x => x.Y);

            if (Single.IsInfinity(xMax) || Single.IsInfinity(yMax) || Single.IsInfinity(xMin) || Single.IsInfinity(yMin) || Single.IsNaN(xMax) || Single.IsNaN(yMax) || Single.IsNaN(xMin) || Single.IsNaN(yMin))
            {
                redundantPixels = points.Count;
                return new Tuple<int, List<Point>>(redundantPixels, new List<Point>());
            }

            try
            {
                //imgy = Convert.ToInt32(imgy * (yMax - yMin) / (xMax - xMin)); //auto-re-adjust the aspect ratio
                Convert.ToInt32(imgy * (yMax - yMin) / (xMax - xMin));
            }
            catch (OverflowException e)
            {
                //If we cannot even adjust aspect ratio, all pixels are redundant
                redundantPixels = points.Count;
                return new Tuple<int, List<Point>>(redundantPixels, new List<Point>());
            }

            foreach (var point in points)
            {
                try
                {
                    var jx = Convert.ToInt32((point.X - xMin) / (xMax - xMin) * (imgx - 1));
                    var jy = imgy - 1 - Convert.ToInt32((point.Y - yMin) / (yMax - yMin) * (imgy - 1));

                    if (jx < 0 || jx > imgx || jy < 0 || jy > imgy)
                    {
                        redundantPixels++;
                    }
                    else
                    {
                        pixels.Add(new Point(jx, jy));
                    }

                }
                catch (OverflowException e)
                {
                    redundantPixels++;
                }

            }

            //Remove duplicated pixels
            pixels = pixels.Distinct().ToList();

            return new Tuple<int, List<Point>>(redundantPixels, pixels);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates an image from given IFS mappings and saves it 
        /// </summary>
        public void SaveIfsImage(List<IfsFunction> ifsMappings, int imgx, int imgy, string path)
        {
            var data = GetIfsPixels(ifsMappings, imgx, imgy);

            var pixels = data.Item2;

            if (pixels.Count != 0)
            {
                var bmp = CreateImageFromPixels(pixels);
                bmp.Save(path, ImageFormat.Png);
                bmp.Dispose();
            }
        }

        /// <summary>
        /// Gets generated IFS pixels based on given IFS functions
        /// </summary>
        public Tuple<int, List<Point>> GetIfsPixels(List<IfsFunction> ifsMappings, int imgx, int imgy)
        {
            bool ignoreProbabilities = Settings.Default.IgnoreProbabilities;

            List<PointF> resultPoints = new List<PointF>();

            var randomGen = new Random();

            var length = ifsMappings.Count;

            //we start at E and F
            var currentPoint = new PointF(ifsMappings[0].E, ifsMappings[0].F);

            var minIterations = 100;
            var maxIterations = imgx * imgy * Settings.Default.IfsGenerationMultiplier;

            for (int k = 0; k < maxIterations; k++)
            {
                var p = randomGen.NextDouble();
                var psum = 0.0;

                var i = 0;

                if (ignoreProbabilities)
                {
                    i = randomGen.Next(0, length);
                }

                else
                {
                    for (int j = 0; j < length; j++)
                    {
                        psum += ifsMappings[j].P;

                        i = j;

                        if (p <= psum)
                            break;
                    }
                }

                currentPoint = ApplyIFSTransformation(ifsMappings[i], currentPoint);

                if (k > minIterations)
                {
                    resultPoints.Add(currentPoint);
                }
            }

            var result = ConvertPointsToPixels(resultPoints, imgx, imgy);

            return result;
        }

        /// <summary>
        /// Creates a bitmap from a list of given pixels
        /// </summary>
        public Bitmap CreateImageFromPixels(List<Point> pixels)
        {
            Bitmap bitmapImage;

            if (pixels.Count != 0)
            {
                bitmapImage = DrawFilledRectangle(pixels.Max(x => x.X) + 1, pixels.Max(x => x.Y) + 1);

                foreach (var pixel in pixels)
                {
                    bitmapImage.SetPixel(pixel.X, pixel.Y, Color.Black);
                }
            }

            else
            {
                bitmapImage = DrawFilledRectangle(1, 1);
            }

            return bitmapImage;

        }

        #endregion

    }
}
