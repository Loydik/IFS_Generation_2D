using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.Ifs;
using MoreLinq;
using NUnit.Framework;

namespace IFS_Thesis_Tests.RecombinationStrategiesTests
{
    [TestFixture]
    public class ReasortmentOperatorTests
    {
        [Test, Category("ReasortmentOperator")]
        public void ReasortmentOperatorChildrenHaveTheSameDegreeAsParentsTest()
        {
            var strategy = new ReasortmentStrategy();

            var parent1 = new Individual(new List<IfsFunction>
            {
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f)
            });
            var parent2 = new Individual(new List<IfsFunction>
            {
                new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
            });

            var producedIndividuals = strategy.ProduceOffsprings(parent1, parent2, new Random());

            Assert.That(producedIndividuals[0].Degree, Is.EqualTo(parent1.Degree));
            Assert.That(producedIndividuals[1].Degree, Is.EqualTo(parent2.Degree));
        }


        [Test, Category("ReasortmentOperator")]
        public void ReasortmentOperatorSameSingelsArePreservedTest()
        {
            var strategy = new ReasortmentStrategy();

            var allSingels = new List<IfsFunction>
            {
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f, -0.5f, 0.4f, 1f, -1f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
            };

            var parent1 = new Individual(allSingels.Take(4).ToList());

            var parent2 = new Individual(allSingels.TakeLast(3).ToList());

            var producedIndividuals = strategy.ProduceOffsprings(parent1, parent2, new Random());

            var allProducedSingels = producedIndividuals.SelectMany(x => x.Singels).ToList();
            allSingels = allSingels.ToList();

            bool equal = allProducedSingels.All(allSingels.Contains);

            Assert.That(equal, Is.True);
        }
    }
}
