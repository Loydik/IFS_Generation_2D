using System;
using System.Collections;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.Ifs;
using Moq;
using NUnit.Framework;

namespace IFS_Thesis_Tests.RecombinationStrategiesTests
{
    [TestFixture]
    public class InterSpeciesCrossoverTests
    {
        [Test, Category("InterSpeciesCrossover")]
        [TestCaseSource(typeof(InterSpeciesCrossoverCases))]
        public void InterSpeciesCrossoverAtDifferentPointsTest(Individual parent1, Individual parent2, Individual child1, Individual child2, int expectedRandomNumber)
        {
            var strategy = new InterSpeciesCrossoverStrategy();

            var randomMock = new Mock<Random>();

            randomMock.Setup(random => random.Next(1, 11)).Returns(expectedRandomNumber);

            var producedIndividuals = strategy.ProduceOffsprings(parent1, parent2, randomMock.Object);

            Assert.That(producedIndividuals[0].Singels, Is.EqualTo(child1.Singels));
            Assert.That(producedIndividuals[1].Singels, Is.EqualTo(child2.Singels));
        }

        #region Test Case Data
   
        public class InterSpeciesCrossoverCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                //crossover at point 1 
                yield return new object[]
                {
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 0f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f,-0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                        new IfsFunction3D(0.85f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                        new IfsFunction3D(0.2f,  0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                    }),
                    1
                };

                //crossover at point 1 reverse order
                yield return new object[]
                {
                   new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 0f)
                    }),
                     new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                        new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f,-0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                      new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                        new IfsFunction3D(0.85f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                        new IfsFunction3D(0.2f,  0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                    }),
                    1
                };

                ////crossover at point 11
                yield return new object[]
              {
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 2f),
                        new IfsFunction3D(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 1.5f),
                        new IfsFunction3D(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 2f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 2f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 1.5f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 2f),
                        new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
                    }),
                    new Individual(new List<IfsFunction3D>
                    {
                        new IfsFunction3D(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f, -0.5f, 0.4f, 1f, -1f, 0f),
                        new IfsFunction3D(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f, -0.5f, 0.4f, 1f, -1f, 0f)
                    }),
                    11
              };
            }
        }

        #endregion
    }
}
