using System.Collections.Generic;
using System.Drawing;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Utils;
using NUnit.Framework;

namespace IFS_Thesis_Tests.Manual_Tests
{
    /// <summary>
    /// Class for testing some functionality manually
    /// </summary>
    [TestFixture]
    public class ManualTests
    {
        #region Private Methods

        /// <summary>
        /// Creates individual from signels string in logs
        /// </summary>
        /// <remarks>string in format [0.2611,0.2358,-0.7685,0.163,-1.5512,-9.0696,0];[-0.9634,0.2324,-0.4512,0.7583,1.7809,-0.0628,0];[-0.6634,0.3488,0.0202,0.3246,4.8702,-2.9171,0];[0.0018,0.5933,-0.9078,0.5663,7.1813,-4.2975,0]</remarks>
        private Individual CreateIndividualFromSingelsString(string singelsString)
        {
            var individualSingels = new List<IfsFunction>();
            var singels = singelsString.Split(';');

            foreach (var singel in singels)
            {
                char[] charsToTrim = { '[', ']'};
                var coefficientts = singel.Trim(charsToTrim).Split(',');

                individualSingels.Add(new IfsFunction(float.Parse(coefficientts[0]), float.Parse(coefficientts[1]), float.Parse(coefficientts[2]), float.Parse(coefficientts[3]), float.Parse(coefficientts[4]), float.Parse(coefficientts[5]), float.Parse(coefficientts[6])));
            }

            return new Individual(individualSingels);
        }
        #endregion

        #region Tests

        [Test, Category("Manual"), Ignore("Manual Test")]
        public void TestFitnessForIndividual()
        {
            var fitnesses  = new List<float>();
            var individual = CreateIndividualFromSingelsString("[0.3187,0.8946,0.3675,-0.1502,7.1641,3.5426,0];[0.2131,-0.8507,-0.2005,0.7513,4.2749,9.9805,0];[0.3297,-0.6026,-0.9373,0.8636885,-1.4526,-5.5306,0];[-0.9453,-0.9164,-0.5955,0.9657,6.6942,2.1566,0];[0.0716,-0.0509,0.9484,0.52,8.6859,7.2074,0];[-0.8213352,-0.5575,-0.2881,-0.5935,7.0869,-3.16,0]");
            var initialImagePath = "C:/tmp/IFS Images/tested_ifs.png";

            var image = (Bitmap)Image.FromFile(initialImagePath, true);
            var sourcePixels = new ImageParser().GetMatchingPixels(image, Color.Black);

            for (int i = 0; i < 10; i++)
            {
                var fitness = new WeightedPointsCoverageFitnessFunction().CalculateFitnessForIndividual(sourcePixels, individual, image.Width, image.Height);
                fitnesses.Add(fitness);
            }
            
            Assert.That(fitnesses, Is.Not.Null);
        }


        [Test, Category("Manual"), Ignore("Manual Test")]
        public void TestCloningOfIndividual()
        {
            var individual = CreateIndividualFromSingelsString("[0.2611,0.2358,-0.7685,0.163,-1.5512,-9.0696,0];[-0.9634,0.2324,-0.4512,0.7583,1.7809,-0.0628,0];[-0.6634,0.3488,0.0202,0.3246,4.8702,-2.9171,0];[0.0018,0.5933,-0.9078,0.5663,7.1813,-4.2975,0]");
            var clone = (Individual) individual.Clone();

            Assert.That(individual.GetHashCode(), Does.Not.EqualTo(clone.GetHashCode()));
        }

        #endregion
    }
}
