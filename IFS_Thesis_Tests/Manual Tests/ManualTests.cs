using System;
using System.Collections.Generic;
using System.Drawing;
using IFS_Thesis.EvolutionaryData;
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
            var individual = CreateIndividualFromSingelsString("[0.4199,0.1155,0.3005,0.5313,-3.1733,-3.1937,0];[0.1701,-0.1785,0.3398,-0.1059,-3.2943,-6.8108,0];[0.0327,0.1881,-0.5201,-0.09676194,7.6954,-4.4345,0];[0.3389,-0.1501,-0.2864,-0.6797,6.8361,-4.6384,0];[0.5196,-0.2561,0.6439,-0.3201,6.450814,-2.2881,0];[-0.4216,0.9305,0.0156,0.0673,-3.0577,8.2746,0];[0.5319,0.219,-0.0793,0.0871,3.0154,3.4911,0]");
            var initialImagePath = "C:/tmp/IFS Images/tested_ifs.png";

            var image = (Bitmap)Image.FromFile(initialImagePath, true);
            var sourcePixels = new ImageParser().GetMatchingPixels(image, Color.Black);

            for (int i = 0; i < 10; i++)
            {
                var fitness = new FitnessFunction().CalculateFitnessForIndividual(sourcePixels, individual, image.Width, image.Height);
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
