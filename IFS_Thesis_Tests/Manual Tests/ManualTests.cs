using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Utils;
using IFS_Thesis_Tests.Properties;
using MoreLinq;
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
        /// <remarks>string in format [-0.2725,0.4525,0.327,0.6865,9.6825,-6.0006,0];[-0.5844,0.3935,0.7635,0.5305,0.0808,-7.0765,0];[-0.6267,-0.1776,0.1581,-0.822,5.8148,2.8991,0]</remarks>
        private Individual CreateIndividualFromSingelsString(string singelsString)
        {
            var individualSingels = new List<IfsFunction>();
            var singels = singelsString.Trim().Split(';');

            foreach (var singel in singels)
            {
                char[] charsToTrim = { '[', ']' };
                var coefficientts = singel.Trim(charsToTrim).Split(',');

                individualSingels.Add(new IfsFunction(float.Parse(coefficientts[0]), float.Parse(coefficientts[1]), float.Parse(coefficientts[2]), float.Parse(coefficientts[3]), float.Parse(coefficientts[4]), float.Parse(coefficientts[5]), float.Parse(coefficientts[6])));
            }

            return new Individual(individualSingels);
        }

        #endregion

        #region Tests

        [Test, Category("Manual")/*, Ignore("Manual Test")*/]
        [Apartment(ApartmentState.STA)]
        public void TestFitnessForIndividual()
        {
            var imageFolder = Settings.Default.WorkingDirectory;
            var ifsDrawer = new IfsDrawer();
            //var multiplier = 2;
            var dimensionX = 256;
            var dimensionY = 256;
            
            var sourceIndividual = CreateIndividualFromSingelsString("[0.382,0,0,0.382,0,0,0.2];[0.382,0,0,0.382,0.618,0,0.2];[0.382,0,0,0.382,0.809,0.588,0.2];[0.382,0,0,0.382,0.309,0.951,0.2];[0.382,0,0,0.382,-0.191,0.588,0.2]");

            var sourcePixels = ifsDrawer.GetIfsPixels(sourceIndividual.Singels, dimensionX, dimensionY).Item2;

            var fitnesses = new List<float>();
           
            var evolvedIndividual = CreateIndividualFromSingelsString("[-0.1778,0.6665001,0.4529,-0.2237,4.2472,4.661595,0];[-0.0389,-0.6991,-0.4011,0.0124,4.2782,-4.3041,0];[-0.5518503,0.4926,-0.4753044,-0.8586,-1.1821,-0.3416,0];[0.3762944,0.5922,-0.6496114,-0.0214,-5.6392,-2.4035,0];[-0.6245,0.3353,0.2827,0.3911,-0.339,-4.7897,0]");

            for (int i = 0; i < 10; i++)
            {
                var fitness = new WeightedPointsCoverageObjectiveFitnessFunction().CalculateFitnessForIndividual(sourcePixels, evolvedIndividual, dimensionX, dimensionY);
                fitnesses.Add(fitness);
            }

            Assert.That(fitnesses, Is.Not.Null);

            if (true)
            {
                var generatedPixels = ifsDrawer.GetIfsPixels(evolvedIndividual.Singels, dimensionX, dimensionY).Item2;
                var matchingPixels = generatedPixels.Intersect(sourcePixels).ToList();
                var pixelsDrawnOutside = generatedPixels.Except(matchingPixels).ToList();

                ifsDrawer.CreateImageFromPixels(sourcePixels).Save(imageFolder + "/source_voxels.png");
                ifsDrawer.CreateImageFromPixels(generatedPixels).Save(imageFolder + "/generated_voxels.png");
                ifsDrawer.CreateImageFromPixels(matchingPixels).Save(imageFolder + "/matching_voxels.png");
                ifsDrawer.CreateImageFromPixels(pixelsDrawnOutside).Save(imageFolder + "/voxels_drawn_outside.png");
            }
        }


        [Test, Category("Manual"), Ignore("Manual Test")]
        public void TestCloningOfIndividual()
        {
            var individual = CreateIndividualFromSingelsString("[0.3194,0.7139,0.5867,0.9837722,0.6427,0.5526701,0.0483,0.0322,0.0079,1.6835,2.862625,3.3435,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,-9.081965,7.8529,0];[0.7322,0.9465162,0.8317,0.7455,0.4996,-0.748,0.0454,0.0021,-0.0087,8.103401,3.1998,-1.2937,0];[0.0285,0.0434,0.0665,0.223,0.04479644,-0.2702,0.0454,-0.0063,0.0081,5.6749,3.0677,2.9059,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,3.7962,7.8529,0]");
            var clone = (Individual)individual.Clone();

            Assert.That(individual.GetHashCode(), Does.Not.EqualTo(clone.GetHashCode()));
        }


        [Test, Category("Manual")]
        public void TestLinearRanking()
        {
            var individual1 = new Individual { ObjectiveFitness = 0.9f };
            var individual2 = new Individual { ObjectiveFitness = 0.1f };
            var individual3 = new Individual { ObjectiveFitness = 1.2f };
            var individual4 = new Individual { ObjectiveFitness = 0.3f };
            var individual5 = new Individual { ObjectiveFitness = 0.001f };

            var testPopulation = new List<Individual> { individual1, individual2, individual3, individual4, individual5 };
            var expectedRanked = new List<Individual> {individual5, individual2, individual4, individual1, individual3};

            var rankedPopulation = new LinearRankingFitnessFunction().AssignRankingFitnessToIndividuals(testPopulation,
                1.5f);

            Assert.That(rankedPopulation.OrderBy(i => i.RankFitness).ToList(), Is.EqualTo(expectedRanked));
        }

        #endregion
    }
}
