using System;
using System.Collections;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection;
using Moq;
using NUnit.Framework;

namespace IFS_Thesis_Tests.SelectionStrategiesTests
{
    [TestFixture]
    public class SpeciesSelectionStrategiesTests
    {

        //REWRITE THIS STUPID TEST
        [Test, Category("SpeciesSelection")]
        [TestCaseSource(typeof(SpeciesSelectionStrategiesCases))]
        public void RouletteWheelTwoSpeciesSelectionTests(Population population, Species firstSpecies, int maximumDistance, int expectedRandomNumber, Species expectedSecondSpecies)
        {
            var strategy = new ProbabilityVectorSpeciesSelectionStrategy();

            var randomMock = new Mock<Random>();

            randomMock.Setup(random => random.Next(0,3)).Returns(expectedRandomNumber);

            var selectedSpecies = strategy.SelectSecondSpecies(population, firstSpecies, maximumDistance, randomMock.Object);

            Assert.That(selectedSpecies.DegreeOfIndividualsInSpecies, Is.EqualTo(expectedSecondSpecies.DegreeOfIndividualsInSpecies));
        }


        public class SpeciesSelectionStrategiesCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                
                //middle species
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(5),
                    2,
                    0,
                    new Species(3)  
                };

                //middle species last index
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(5),
                    2,
                    3,
                    new Species(7)
                };

                //first species selected
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(1),
                    2,
                    0,
                    new Species(2)
                };

                //last species selected
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(7),
                    2,
                    0,
                    new Species(5)
                };

                //last species selected second random
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(7),
                    2,
                    1,
                    new Species(5)
                };

                //middle species selected, range of 1
                yield return new object[]
                {
                    new Population(new List<Species>
                    {
                        new Species(1),
                        new Species(2),
                        new Species(3),
                        new Species(4),
                        new Species(5),
                        new Species(6),
                        new Species(7),
                    }),
                    new Species(4),
                    1,
                    0,
                    new Species(3)
                };
            }
        }

    }
}
