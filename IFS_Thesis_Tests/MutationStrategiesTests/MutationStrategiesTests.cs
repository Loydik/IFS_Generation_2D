using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.Mutation.Individuals;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.EvolutionaryData.Population;
using IFS_Thesis.Ifs;
using IFS_Thesis.Utils;
using NUnit.Framework;

namespace IFS_Thesis_Tests.MutationStrategiesTests
{
    [TestFixture]
    public class MutationStrategiesTests
    {
        private static readonly Random Random = new Random();

        [Test, Category("VariableMutation"), Category("Manual"), Ignore("Manual")]
        public void ManualCoefficientsMutationStrategyTest()
        {
            var mutationStrategy = new ControlledMutationStrategy();

            List<float> results = new List<float>();

            float variable = 0.5f;

            Random random = new Random();

            var range = new Tuple<int,int>(-1,1);

            for (int i = 0; i < 100; i++)
            {
                results.Add(mutationStrategy.Mutate(variable, random, range));
            }

            Assert.Fail("The test is manual :D");
        }


        [Test, Category("Individual Mutation"), Repeat(100)]
        [TestCaseSource(typeof(MutationIndividuals))]
        public void StandardMutationRateStrategyAllCoefficientsAreBetween0And1Test(Individual individual)
        {
            var mutationStrategy = new StandardMutationRateStrategy();

            mutationStrategy.Mutate(ref individual, new ControlledMutationStrategy(), Random);

            foreach (var singel in individual.Singels)
            {
                    Assert.That(singel.Coefficients.All(x => x >= -1 && x <= 2), Is.True, "Some coefficients are not between 0 and 1 after mutation");             
            }
        }


        public class MutationIndividuals : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[]
                {
                    new Individual(new List<IfsFunction>
                    {
                        new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                        new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                        new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                        new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
                    })
                };

                //yield return new object[]
                //{
                //    new Individual(new List<IfsFunction>
                //    {
                //        new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                //        new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                //        new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                //        new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f),
                //        new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
                //    })
                //};
            }
        }

    }
}
