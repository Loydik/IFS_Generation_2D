using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mime;
using AForge.Imaging;
using Image = System.Drawing.Image;

namespace IFS_Thesis.EvolutionaryData
{
    public class FitnessFunction
    {
        //public static double CalculateFitness(Bitmap sourceImage, Individual individual)
        //{
        //    double similarity = 0.0;

        //        var generatedImage = new IfsDrawer().DrawIfs(individual, 512, 512);

        //        ResizeImage(generatedImage, sourceImage.Width, sourceImage.Height);

        //        ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
        //        // Compare two images
        //        TemplateMatch[] matchings = tm.ProcessImage(generatedImage, sourceImage);

        //        similarity = matchings[0].Similarity;


        //    return similarity;
        //}


        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
