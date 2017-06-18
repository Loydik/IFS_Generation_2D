using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;

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

    }
}
