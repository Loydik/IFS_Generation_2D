using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using IFS_Thesis.Utils;
using log4net;
using log4net.Repository.Hierarchy;
using Image = System.Drawing.Image;

namespace IFS_Thesis.EvolutionaryData
{
    public class FitnessFunction
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private float GetAverageFitnessForDegree(List<Individual> individuals, int degree)
        {
            var average = individuals.Where(x => x.Degree == degree).Select(x => x.ObjectiveFitness).Average();

            return average;
        }

        /// <summary>
        /// ToDO - test/ Needs rework
        /// </summary>
        public List<float> UpdateVectorOfProbabilitiesBasedOnFitness(List<Individual> individuals, List<float> vector )
        {
            //brute add method

            //foreach (var individual in individuals)
            //{
            //    vector[individual.Degree - 1] = vector[individual.Degree - 1] + individual.ObjectiveFitness;
            //}

            var degrees = OtherUtils.GetDegreesOfIndividuals(individuals);

            foreach (var degree in degrees)
            {
                var averageFitnessForDegree = GetAverageFitnessForDegree(individuals, degree);

                vector[degree - 1] = vector[degree - 1] + averageFitnessForDegree;
            }

            vector = OtherUtils.NormalizeVector(vector);

            Log.Info($"Updated the probability vector, current values are: [{string.Join(",", vector)}]");

            return vector;
        }



        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, Bitmap sourceImage)
        {

            foreach (var individual in individuals)
            {
                float fitness = CalculateFitness(sourceImage, individual, sourceImage.Width);

                individual.ObjectiveFitness = fitness;
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
