using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using Image = System.Drawing.Image;

namespace IFS_Thesis.EvolutionaryData
{
    public class FitnessFunction
    {

        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, Bitmap sourceImage)
        {

            foreach (var individual in individuals)
            {
                float fitness = CalculateFitness(sourceImage, individual, sourceImage.Width);

                individual.CurrentFintess = fitness;
            }

            return individuals;
        }

        public static float CalculateFitness(Bitmap sourceImage, Individual individual, int width /*int height*/)
        {
            var originalPixels = new ImageParser().GetMatchingPixels(sourceImage, Color.Black);

            //new IfsDrawer().CreateImageFromPixels(originalPixels).Save(@"C:/Users/Loydik94/Desktop/originalPixels.png");

            var generatedPixels = new IfsDrawer().GetIfsPixels(individual.Singels, width,
               width);

            //new IfsDrawer().CreateImageFromPixels(generatedPixels).Save(@"C:/Users/Loydik94/Desktop/generatedPixels.png");

            var matchingPixels = generatedPixels.Intersect(originalPixels).ToList();

            //new IfsDrawer().CreateImageFromPixels(matchingPixels).Save(@"C:/Users/Loydik94/Desktop/matchingPixels.png");

            var pixelsDrawnOutside = generatedPixels.Except(matchingPixels).ToList();

            //new IfsDrawer().CreateImageFromPixels(pixelsDrawnOutside).Save(@"C:/Users/Loydik94/Desktop/pixelsDrawnOutsidePixels.png");

            //Stupid formula

            var NA = generatedPixels.Count;

            var NI = originalPixels.Count;

            //NND
            var NotDrawnPoints = originalPixels.Except(generatedPixels).Count();

            //NNN
            var PointsNotNeeded = generatedPixels.Except(matchingPixels).Count();

            float RC = NotDrawnPoints/(float)NI;

            float RO = PointsNotNeeded/(float)NA;

            float fitness = (1 - RC) + (1 - RO);

            return fitness;
        }


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
