using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Utils;
using NUnit.Framework;

namespace IFS_Thesis_Tests.GeneticOperatorsTests
{
    [TestFixture]
    public class GeneticOperatorsTests
    {
        [Test, Category("VectorNormalization")]
        [TestCaseSource(typeof(VectorNormalizationCases))]
        public void VectorNormalizationTest(List<float> probabilityVector)
        {
            float minValue = 0.05f;
            float maxValue = 0.75f;

            var vdNormalized = EaUtils.NormalizeVectorWithMinimumValue(probabilityVector, minValue);

            bool boolEqualToOne = Math.Abs(vdNormalized.Sum() - 1) < 0.00001;

            Assert.That(boolEqualToOne, Is.True, "Sum of elements in VD is not equal to 1");
            Assert.That(vdNormalized.Skip(2).Any(element => element < minValue), Is.False, "There are elements in VD smaller that min value");
            Assert.That(vdNormalized.Skip(2).Any(element => element > maxValue), Is.False, "There are elements in VD greater than max value");
        }
    }

    #region Test Cases

    /// <summary>
    /// Test cases for vector normalization
    /// </summary>
    public class VectorNormalizationCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
                new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f}
            };

            yield return new object[]
            {
                new List<float> {0, 0, 1.7f, 2.25f, 1.2f, 4.1f, 1.07f, 0.03f}
            };

            yield return new object[]
            {
                new List<float> {0, 0, 25.7f, 13.25f, 2.2f, 4.1f, 1.07f, 0.001f}
            };

            yield return new object[]
            {
                new List<float> {0, 0, 0.36f, 13.25f, 1.2f, 0.1f, 1.07f, 0.001f}
            };
        }
    }

    #endregion
}
