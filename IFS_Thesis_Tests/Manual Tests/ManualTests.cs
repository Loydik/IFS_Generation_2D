using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Ifs.IFSGenerators;
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
        /// <remarks>string in format [0.2611,0.2358,-0.7685,0.163,-1.5512,-9.0696,0];[-0.9634,0.2324,-0.4512,0.7583,1.7809,-0.0628,0];[-0.6634,0.3488,0.0202,0.3246,4.8702,-2.9171,0];[0.0018,0.5933,-0.9078,0.5663,7.1813,-4.2975,0]</remarks>
        private Individual CreateIndividualFromSingelsString(string singelsString)
        {
            var individualSingels = new List<IfsFunction>();
            var singels = singelsString.Split(';');

            foreach (var singel in singels)
            {
                char[] charsToTrim = { '[', ']'};
                var coefficientts = singel.Trim(charsToTrim).Split(',');

                individualSingels.Add(new IfsFunction(float.Parse(coefficientts[0]), float.Parse(coefficientts[1]), float.Parse(coefficientts[2]), float.Parse(coefficientts[3]), float.Parse(coefficientts[4]), float.Parse(coefficientts[5]), float.Parse(coefficientts[6]), float.Parse(coefficientts[7]), float.Parse(coefficientts[8]), float.Parse(coefficientts[9]), float.Parse(coefficientts[10]), float.Parse(coefficientts[11]), float.Parse(coefficientts[12])));
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
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();
            var multiplier = 3;
            var dimensionX = 256;
            var dimensionY = 256;
            var dimensionZ = 256;

            var sourceIndividual = CreateIndividualFromSingelsString("[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0.5,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0.5,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0.5,0]");

            var sourceVoxels = ifsGenerator.GenerateVoxelsForIfs(sourceIndividual.Singels, dimensionX, dimensionY, dimensionZ, multiplier);

            var fitnesses  = new List<float>();
            var evolvedIndividual = CreateIndividualFromSingelsString("[0.6,0,0,0,0.7,0,0,0.1,0.5,0,0,0,0];[0.6,0,0,0,0.54,0,0,0,0.3,0.5,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0.7,0,0];[0.5,0,0,0,0.5,0,0,0.1,0.5,0,0.2,0.5,0]");

            for (int i = 0; i < 5; i++)
            {
                var fitness = new WeightedPointsCoverageFitnessFunction().CalculateFitnessForIndividual(sourceVoxels, evolvedIndividual, ifsGenerator, 256, 256, 256, multiplier);
                fitnesses.Add(fitness);
            }
            
            Assert.That(fitnesses, Is.Not.Null);

            if (true)
            {
                var generatedVoxels = ifsGenerator.GenerateVoxelsForIfs(evolvedIndividual.Singels, dimensionX, dimensionY, dimensionZ, multiplier);
                var matchingVoxels = generatedVoxels.Intersect(sourceVoxels).ToHashSet();
                var voxelsDrawnOutside = generatedVoxels.Except(matchingVoxels).ToHashSet();

                ifsDrawer.SaveVoxelImage(imageFolder + "/source_voxels", sourceVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/generated_voxels", generatedVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/matching_voxels", matchingVoxels, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelImage(imageFolder + "/voxels_drawn_outside", voxelsDrawnOutside, ImageFormat3D.Obj);
                ifsDrawer.SaveVoxelOverlayImage(imageFolder + "/voxels_overlay", sourceVoxels, generatedVoxels, ImageFormat3D.Obj);
            }
        }


        [Test, Category("Manual"), Ignore("Manual Test")]
        public void TestCloningOfIndividual()
        {
            var individual = CreateIndividualFromSingelsString("[0.3194,0.7139,0.5867,0.9837722,0.6427,0.5526701,0.0483,0.0322,0.0079,1.6835,2.862625,3.3435,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,-9.081965,7.8529,0];[0.7322,0.9465162,0.8317,0.7455,0.4996,-0.748,0.0454,0.0021,-0.0087,8.103401,3.1998,-1.2937,0];[0.0285,0.0434,0.0665,0.223,0.04479644,-0.2702,0.0454,-0.0063,0.0081,5.6749,3.0677,2.9059,0];[0.3407471,0.7422,0.6245,0.8996,0.687,0.5735,0.6682,0.8002633,0.012,3.402,3.7962,7.8529,0]");
            var clone = (Individual) individual.Clone();

            Assert.That(individual.GetHashCode(), Does.Not.EqualTo(clone.GetHashCode()));
        }

        #endregion
    }
}
