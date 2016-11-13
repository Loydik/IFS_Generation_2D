﻿using System;
using System.Collections;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.Utils;
using Moq;
using NUnit.Framework;

namespace IFS_Thesis_Tests.RecombinationStrategiesTests
{
    [TestFixture]
    public class OnePointCrossoverTests
    {
        [Test, Category("OnePointCrossover")]
        [TestCaseSource(typeof(OnePointCrossoverCases))]
        public void OnePointCrossoverAtDifferentPointsTest(Individual parent1, Individual parent2, Individual child1, Individual child2, int expectedRandomNumber)
        {
            var strategy = new OnePointCrossoverStrategy();

            var randomMock = new Mock<Random>();

            randomMock.Setup(random => random.Next(1, parent1.Singels.Count - 1)).Returns(expectedRandomNumber);

            var producedIndividuals = strategy.ProduceOffsprings(parent1, parent2, randomMock.Object);

            Assert.That(producedIndividuals[0].Singels, Is.EqualTo(child1.Singels));
            Assert.That(producedIndividuals[1].Singels, Is.EqualTo(child2.Singels));
        }

        [Test, Category("OnePointCrossover")]
        public void OnePointCrossoverDifferentDegreesReturnsEmptyListTest()
        {
            #region Test Case Data

            var firstParent = new Individual(new List<IfsFunction>
            {
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
            });


            var secondParent = new Individual(new List<IfsFunction>
            {
                new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f),
                new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
            });

            #endregion

            var strategy = new OnePointCrossoverStrategy();

            var producedIndividuals = strategy.ProduceOffsprings(firstParent, secondParent, new Random());

            Assert.That(producedIndividuals, Is.Empty);
        }
    }


    public class OnePointCrossoverCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            //crossover at point 1 
            yield return new object[]
            {
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                    new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                    new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                    new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                    new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                    new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                    new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
                }),
                1
            };

            //crossover at point 3
            yield return new object[]
            {
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                    new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                    new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                    new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f),
                    new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f)
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                    new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f),
                    new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f),
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f)
                }),
                3
            };

            //crossover on point 1 with parent degree 1
            yield return new object[]
           {
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f),
                }),
                new Individual(new List<IfsFunction>
                {
                    new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f),
                }),
                1
           };
        }
    }
}