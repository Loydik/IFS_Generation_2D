using System;
using System.Collections;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using NUnit.Framework;

namespace IFS_Thesis_Tests.FitnessFunctionTests
{
    [TestFixture]
    public class FitnessFunctionTests
    {
        
        [Test, Category("FitnessFunction")]
        [TestCaseSource(typeof(FitnessFunctionCases))]
        public void TestFitnessFunctionRange(int generatedVoxelsCount, int sourceImageVoxelsCount, int matchingVoxelsCount, Tuple<float, float> expectedFitnessRange)
        {
            var fitnessFunction = new WeightedPointsCoverageObjectiveFitnessFunction();
            var fitness = fitnessFunction.CalculateFitness(generatedVoxelsCount, sourceImageVoxelsCount,
                matchingVoxelsCount, 1, 1);

            Assert.That(fitness, Is.InRange(expectedFitnessRange.Item1, expectedFitnessRange.Item2));
        }

    }

    #region Test Cases

    /// <summary>
    /// Test Cases for FitnessFunctionTests
    /// </summary>
    public class FitnessFunctionCases : IEnumerable
    {
        private Tuple<float, float> veryLowFitness = new Tuple<float, float>(0, 0.1f);
        private Tuple<float, float> lowFitness = new Tuple<float, float>(0.1f, 0.3f);
        private Tuple<float, float> lowerThanAverageFitness = new Tuple<float, float>(0.3f, 0.7f);
        private Tuple<float, float> averageFitness = new Tuple<float, float>(0.7f, 1);
        private Tuple<float, float> higherThanAverageFitness = new Tuple<float, float>(1, 1.3f);
        private Tuple<float, float> highFitness = new Tuple<float, float>(1.3f, 1.6f);
        private Tuple<float, float> veryHighFitness = new Tuple<float, float>(1.6f, 2);

        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
               100,//generated voxels count
               100,//source image voxels count
               100,//matching voxels count
               veryHighFitness
            };

            yield return new object[]
            {
               10,//generated voxels count
               1000,//source image voxels count
               10,//matching voxels count
               veryLowFitness
            };

           //Ignored for now - unrealistic scenario
           //yield return new object[]
           //{
           //    1000,//generated voxels count
           //    10,//source image voxels count
           //    10,//matching voxels count
           //    veryLowFitness
           //};

            yield return new object[]
           {
               1000,//generated voxels count
               500,//source image voxels count
               50,//matching voxels count
               lowFitness
           };

            yield return new object[]
          {
               300,//generated voxels count
               1000,//source image voxels count
               150,//matching voxels count
               lowerThanAverageFitness
          };

            yield return new object[]
            {
                1000, //generated voxels count
                1000, //source image voxels count
                500, //matching voxels count
                averageFitness
            };

            //Ignore for now
            //yield return new object[]
            //{
            //    2000, //generated voxels count
            //    1000, //source image voxels count
            //    1000, //matching voxels count
            //    averageFitness
            //};
        }
    }

    #endregion

}
