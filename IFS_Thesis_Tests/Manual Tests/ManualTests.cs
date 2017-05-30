using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Ifs.IFSGenerators;
using IFS_Thesis_Tests.Properties;
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
        public void TestFitnessForIndividual()
        {
            var imagePath = Settings.Default.WorkingDirectory + "/manual_tested_ifs";
            var ifsGenerator = new PointRecursiveIfsGenerator();

            var sourceIndividual = CreateIndividualFromSingelsString("[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0.5,0,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0.5,0,0];[0.5,0,0,0,0.5,0,0,0,0.5,0,0,0.5,0]");

            var sourceVoxels = new PointRecursiveIfsGenerator().GenerateVoxelsForIfs(sourceIndividual.Singels, 256, 256, 256);

            var fitnesses  = new List<float>();
            var individual = CreateIndividualFromSingelsString("[-0.028,0.0637,0.5636,-0.3166,-0.5621,0.7151,-0.0617,0.4357,0.3051,-3.6201,-1.3762,0.0286,0];[-0.7519,0.2488,0.0497,-0.2554,-0.0946,0.3691,-0.1365,0.2763,0.1917,-0.6295,-3.6185,-3.4825,0];[0.7056,0.1143,0.0002,-0.2973,-0.5573,0.5338,-0.0654,0.529,0.2854,-4.9092,-1.0659,0.8751,0];[0.2861,-0.5518,-0.6816,-0.8142,0.8623,-0.4619,-0.1592,0.6662,0.1943,-7.7947,3.8023,3.3423,0];[-0.7519,0.2488,0.0497,-0.2548,-0.0946,0.3691,-0.1365,0.2763,0.1917,-0.6274,-3.6169,-3.4803,0];[0.6468,0.1315,0.0083,-0.2984,-0.3687,0.5329,-0.0698,0.3615,0.1492,-7.8404,-2.7105,-2.9107,0]");

            for (int i = 0; i < 5; i++)
            {
                var fitness = new WeightedPointsCoverageFitnessFunction().CalculateFitnessForIndividual(sourceVoxels, individual, ifsGenerator, 256, 256, 256);
                fitnesses.Add(fitness);
            }
            
            Assert.That(fitnesses, Is.Not.Null);

            if (true)
            {
                var voxels = ifsGenerator.GenerateVoxelsForIfs(individual.Singels, 256, 256, 256);
                new IfsDrawer3D().SaveImage(imagePath, voxels, ImageFormat3D.Obj);
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
