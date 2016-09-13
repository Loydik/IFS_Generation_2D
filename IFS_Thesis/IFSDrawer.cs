﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace IFS_Thesis
{
    public class IfsDrawer
    {
        public void SaveIfsImage(List<IfsFunction> ifsMappings, int imgx, int imgy, string path)
        {
            var pixels = GetIfsPixels(ifsMappings, imgx, imgy);
            var bmp = CreateImageFromPixels(pixels);
            bmp.Save(path, ImageFormat.Png);
            bmp.Dispose();
        }

        public List<Point> GetIfsPixels(List<IfsFunction> ifsMappings, int imgx, int imgy)
        {
            List<PointF> resultPoints = new List<PointF>();

            var randomGen = new Random();

            var length = ifsMappings.Count;

            //we start at E and F
            var currentPoint = new PointF(ifsMappings[0].E, ifsMappings[0].F);

            for (int k = 0; k < imgx*imgy; k++)
            {
                var p = randomGen.NextDouble();
                var psum = 0.0;

                var i = 0;

                for (int j = 0; j < length; j++)
                {
                    psum += ifsMappings[j].P;

                    i = j;

                    if (p <= psum)
                        break;
                }

                currentPoint = ApplyIFSTransformation(ifsMappings[i], currentPoint);

                resultPoints.Add(currentPoint);
            }

            var pixels = ConvertPointsToPixels(resultPoints, imgx, imgy );

            return pixels;
        }

        private List<Point> ConvertPointsToPixels(List<PointF> points, int imgx, int imgy)
        {
            List<Point> pixels =new List<Point>();

            var xMin = points.Min(x => x.X);
            var xMax = points.Max(x => x.X);
            var yMin = points.Min(x => x.Y);
            var yMax = points.Max(x => x.Y);

            imgy = Convert.ToInt32(imgy * (yMax - yMin) / (xMax - xMin)); //auto-re-adjust the aspect ratio

            foreach (var point in points)
            {
                var jx = Convert.ToInt32((point.X - xMin) / (xMax - xMin) * (imgx - 1));
                var jy = imgy - 1 - Convert.ToInt32((point.Y - yMin) / (yMax - yMin) * (imgy - 1));

                pixels.Add(new Point(jx, jy));
            }

            pixels = pixels.Distinct().ToList();

            return pixels;
        }

        public Bitmap CreateImageFromPixels(List<Point> pixels)
        {
            var bmpImage = DrawFilledRectangle(pixels.Max(x => x.X)+1, pixels.Max(x => x.Y)+1);

            foreach (var pixel in pixels )
            {
                bmpImage.SetPixel(pixel.X, pixel.Y, Color.Black);
            }

            return bmpImage;

        }

        public PointF[] CreateIfsPointsMyVersion(List<IfsFunction> ifsMappings, int numberOfIterations)
        {
            List<PointF> resultPoints = new List<PointF>();

            var numberOfFunctions = ifsMappings.Count;

            var q0 = new PointF(10, 10);

            for (int i = 0; i < numberOfIterations; i++)
            {
                var r = new Random().Next(1, numberOfFunctions);

                var q = ApplyIFSTransformation(ifsMappings[r], q0);

                q0 = q;

                resultPoints.Add(q0);

            }

            var result = resultPoints.Distinct().ToList();

            return result.ToArray();
        }

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
    }
}
