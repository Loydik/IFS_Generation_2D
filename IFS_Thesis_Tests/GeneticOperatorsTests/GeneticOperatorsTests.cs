using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using NUnit.Framework;

namespace IFS_Thesis_Tests.GeneticOperatorsTests
{
    [TestFixture]
    public class GeneticOperatorsTests
    {
        [Test, Repeat(100), Category("GeneticOperators")]
        public void RemoveWeakestSpeciesBasedOnThresholdsOneSpeciesHasAverageDegreeLower()
        {
            var vd = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };
            var random = new Random();
            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, 100, vd, random);
            var testPopulation = new Population();
            testPopulation.AddIndividuals(initialIndividuals);

            foreach (var individual in testPopulation.Individuals)
            {
                individual.ObjectiveFitness = 0.01f;
            }

            var firstSpecies = testPopulation.Species[0];
            var initialSpeciesCount = testPopulation.Species.Count;

            foreach (var individual in testPopulation.Individuals)
            {
                if (individual.Degree == firstSpecies.DegreeOfIndividualsInSpecies)
                {
                    individual.ObjectiveFitness = 0.005f;
                }
            }

           testPopulation = new GeneticOperators().RemoveWeakestSpecies(testPopulation, 0.01f);

            Assert.That(testPopulation.Species, !Does.Contain(firstSpecies));
            Assert.That(testPopulation.Species.Count, Is.EqualTo(initialSpeciesCount - 1));
        }

        [Test, Repeat(100), Category("GeneticOperators")]
        public void RemoveWeakestSpeciesBasedOnThresholdsMultipleSpeciesHasAverageDegreeLowerTest()
        {
            var vd = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };
            var random = new Random();
            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, 100, vd, random);
            var testPopulation = new Population();
            testPopulation.AddIndividuals(initialIndividuals);

            foreach (var individual in testPopulation.Individuals)
            {
                individual.ObjectiveFitness = 0.01f;
            }

            var firstSpecies = testPopulation.Species.First();
            var secondSpecies = testPopulation.Species.Last();
            var initialSpeciesCount = testPopulation.Species.Count;

            foreach (var individual in testPopulation.Individuals)
            {
                if (individual.Degree == firstSpecies.DegreeOfIndividualsInSpecies || individual.Degree == secondSpecies.DegreeOfIndividualsInSpecies)
                {
                    individual.ObjectiveFitness = 0.009f;
                }
            }

            testPopulation = new GeneticOperators().RemoveWeakestSpecies(testPopulation, 0.01f);

            Assert.That(testPopulation.Species, !Does.Contain(firstSpecies));
            Assert.That(testPopulation.Species, !Does.Contain(secondSpecies));
            Assert.That(testPopulation.Species.Count, Is.EqualTo(initialSpeciesCount - 2));
        }

        [Test, Repeat(100), Category("GeneticOperators")]
        public void RemoveWeakestSpeciesNoSpeciesAreRemovedWhenAllAboveAverageTest()
        {
            var vd = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };
            var random = new Random();
            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, 100, vd, random);
            var testPopulation = new Population();
            testPopulation.AddIndividuals(initialIndividuals);

            foreach (var individual in testPopulation.Individuals)
            {
                individual.ObjectiveFitness = 0.01f;
            }

            var initialSpeciesCount = testPopulation.Species.Count;

            testPopulation = new GeneticOperators().RemoveWeakestSpecies(testPopulation, 0.01f);

            Assert.That(testPopulation.Species.Count, Is.EqualTo(initialSpeciesCount));
        }

        [Test, Repeat(100), Category("GeneticOperators")]
        public void RemoveWeakestSpeciesBasedOnThresholdsOneIndividualHasFitnessAboveThreshold()
        {
            var vd = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };
            var random = new Random();
            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, 100, vd, random);
            var testPopulation = new Population();
            testPopulation.AddIndividuals(initialIndividuals);

            foreach (var individual in testPopulation.Individuals)
            {
                individual.ObjectiveFitness = 0.01f;
            }

            var firstSpecies = testPopulation.Species[0];
            var initialSpeciesCount = testPopulation.Species.Count;

            foreach (var individual in testPopulation.Individuals)
            {
                if (individual.Degree == firstSpecies.DegreeOfIndividualsInSpecies)
                {
                    individual.ObjectiveFitness = 0.001f;
                }
            }

            //setting 1 individual to have fitness that of threshold

            testPopulation.Individuals
                .First(x => x.Degree == firstSpecies.DegreeOfIndividualsInSpecies)
                .ObjectiveFitness = 0.01f;

            testPopulation = new GeneticOperators().RemoveWeakestSpecies(testPopulation, 0.01f);

            Assert.That(testPopulation.Species, Does.Contain(firstSpecies));
            Assert.That(testPopulation.Species.Count, Is.EqualTo(initialSpeciesCount));
        }

        [Test, Repeat(100), Category("GeneticOperators")]
        public void RemoveSmallSpeciesBasedOnPercentThreshold()
        {
            var vd = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };
            var random = new Random();
            var populationSize = 100;
            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, populationSize, vd, random);
            var testPopulation = new Population();
            testPopulation.AddIndividuals(initialIndividuals);

            var smallSpecies = testPopulation.Species.Where(x => x.Individuals.Count < 5).ToList();

            testPopulation = new GeneticOperators().RemoveSpeciesWithPopulationBelowTotal(testPopulation, populationSize, 0.05f);
          
            Assert.That(testPopulation.Species, !Does.Contain(smallSpecies));
        }
    }
}
