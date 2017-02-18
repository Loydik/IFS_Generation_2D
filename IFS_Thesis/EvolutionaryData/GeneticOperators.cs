using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.Mutation.Individuals;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.EvolutionaryData.Selection;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData
{
    public class GeneticOperators
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Singel CreateRandomSingel(Random random)
        {
            var a = GetRandomCoefficient(random);
            var b = GetRandomCoefficient(random);
            var c = GetRandomCoefficient(random);
            var d = GetRandomCoefficient(random);
            var e = GetRandomCoefficient(random);
            var f = GetRandomCoefficient(random);

            var coefficients = new IfsFunction(a,b,c,d,e,f,0f);

            var singel = new Singel(coefficients);

            return singel;
        }

        public Individual CreateIndividual(List<float> probabilityVectors, List<Singel> poolOfSingels, Random randomGen)
        {
            var degreeOfIndividual = GetIndividualsDegreeBasedOnVectors(probabilityVectors, randomGen);

            poolOfSingels.Shuffle();

            var individual = new Individual(poolOfSingels.Select(x => x.Coefficients).Take(degreeOfIndividual).ToList())
            {
                Degree = degreeOfIndividual
            };


            return individual;
        }

        private float GetRandomCoefficient(Random random)
        {
            //var randomGen = new Random();

            var value = random.NextDouble();

            float coefficient = (float) Math.Round(value, 3);

            return coefficient;

        }

        /// <summary>
        /// This function calculates cumulative probability and returns an index of the vector
        /// </summary>
        private int GetIndividualsDegreeBasedOnVectors(List<float> probablityVectors, Random randomGen)
        {
            double diceRoll = randomGen.NextDouble();

            double cumulative = 0.0;

            int degree = 0;

            for (int i = 0; i < probablityVectors.Count; i++)
            {
                cumulative += probablityVectors[i];

                if (diceRoll < cumulative)
                {
                    degree = i + 1;
                    break;
                }
            }

            return degree;
        }

        private List<Singel> GenerateSingels(int amount, Random random)
        {
            var result = new List<Singel>();

            for (int j = 0; j < amount; j++)
            {
                var singel = new GeneticOperators().CreateRandomSingel(random);

                result.Add(singel);
            }

            return result;
        }

        public List<Individual> CreateIndividuals(int singelsPoolSize, int populationSize, List<float> probablityVectors, Random randomGen)
        {
            var generatedIndividuals = new List<Individual>();

            var initialPoolOfSingels = GenerateSingels(singelsPoolSize, randomGen);

            for (int i = 0; i < populationSize; i++)
            {
                var individual = new GeneticOperators().CreateIndividual(probablityVectors, initialPoolOfSingels, randomGen);

                generatedIndividuals.Add(individual);
            }

            return generatedIndividuals;
        }

        public List<Individual> CreateIndividualsFromExistingPoolOfSingels(List<Singel> poolOfSingels, int populationSize, List<float> probablityVectors, Random randomGen)
        {
            var generatedIndividuals = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                var individual = new GeneticOperators().CreateIndividual(probablityVectors, poolOfSingels, randomGen);

                generatedIndividuals.Add(individual);
            }

            return generatedIndividuals;
        }

        /// <summary>
        /// Generates new population
        /// </summary>
        public Population GenerateNewPopulation(Population population, List<float> probabilityVectors, Random randomGen)
        {
            SelectionStrategy selectionStrategy = new RouletteWheelSelectionStrategy();
            RecombinationStrategy recombinationStrategy = new OnePointCrossoverStrategy();
            IndividualMutationStrategy mutationStrategy = new StandardMutationRateStrategy();

            var newPopulation = new Population();

            // (Step 6.) Adding best individuals of each new degree 
            var degrees = OtherUtils.GetDegreesOfIndividuals(population.GetAllIndividuals());

            foreach (var degree in degrees)
            {
                var best = population.GetAllIndividuals().Where(x => x.Degree == degree).MaxBy(x => x.ObjectiveFitness);
                newPopulation.AddIndividual(best);
                //leave individual??
                //population.RemoveIndividual(best);
            }

            //Step 7
            var count = Properties.Settings.Default.N1IndividualsCount;

            for (int i = 0; i <= count/2; i++)
            {
                var selectedSpecies = selectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);

                if (selectedSpecies != null)
                {
                    var selectedIndividuals =
                        selectionStrategy.SelectIndividuals(
                            population.GetAllIndividuals()
                                .Where(x => x.Degree.Equals(selectedSpecies.DegreeOfIndividualsInSpecies))
                                .ToList(), 2, randomGen);

                    //one-point crossover for now
                    var offspring = recombinationStrategy.ProduceOffsprings(selectedIndividuals[0],
                        selectedIndividuals[1],
                        randomGen);
                    newPopulation.AddIndividuals(offspring);
                }
            }

            //Step 8
            count = Properties.Settings.Default.N2IndividualsCount;
            var allSingles = population.GetAllSingels();
            var generatedIndividuals = CreateIndividualsFromExistingPoolOfSingels(allSingles, count, probabilityVectors, randomGen);
            newPopulation.AddIndividuals(generatedIndividuals);

            //Step 9
            count = Settings.Default.N3IndividualsCount;

            for (int i = 0; i <= count/2; i++)
            {
                var firstSpecies = selectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);

                if (firstSpecies != null)
                {
                    var secondSpecies = selectionStrategy.SelectSecondSpecies(population, firstSpecies, 1, randomGen);

                    if (secondSpecies != null)
                    {
                        var firstIndividual =
                            selectionStrategy.SelectIndividuals(firstSpecies.Individuals, 1, randomGen).First();
                        var secondIndividual =
                            selectionStrategy.SelectIndividuals(secondSpecies.Individuals, 1, randomGen).First();

                        recombinationStrategy = new InterSpeciesCrossoverStrategy();

                        var children = recombinationStrategy.ProduceOffsprings(firstIndividual, secondIndividual,
                            randomGen);
                        newPopulation.AddIndividuals(children);
                    }
                }

            }


            //Step 10
            count = Properties.Settings.Default.N4IndividualsCount;

            for (int i = 0; i <= count / 2; i++)
            {
                var parents = selectionStrategy.SelectIndividuals(population.GetAllIndividuals(), 2, randomGen);

                recombinationStrategy = new ReasortmentStrategy();

                if (parents[0] != null && parents[1] != null)
                {
                    var children = recombinationStrategy.ProduceOffsprings(parents[0], parents[1], randomGen);
                    newPopulation.AddIndividuals(children);
                }
            }

            //Step 11
            foreach (Individual individual in newPopulation.Individuals)
            {
                //Mutates individual with frequency of defined probability
                if (OtherUtils.DeterminePercentProbability(randomGen, Settings.Default.MutationProbability))
                {
                    var currentIndividual = individual;

                    mutationStrategy.Mutate(ref currentIndividual, new CoefficientsMutationStrategy(), randomGen);
                }
            }
            
            return newPopulation;
        }

        /// <summary>
        /// Removes weakest species from a population if average fitness is below threshold, and no element has higher fitness
        /// </summary>
        public Population RemoveWeakestSpecies(Population population, float averageFitnessThreshold)
        {
            var weakestSpecies =
                population.Species.Where(x => x.Individuals.Average(individual => individual.ObjectiveFitness) < averageFitnessThreshold && !x.Individuals.Any(f => f.ObjectiveFitness >= averageFitnessThreshold)).ToList();

            foreach (var species in weakestSpecies)
            {
                population.RemoveSpecies(species);
                Log.Info($"Removed species with degree {species.DegreeOfIndividualsInSpecies} because average fitness was below threshold. Average Fitness: {species.Individuals.Average(individual => individual.ObjectiveFitness)}");
            }

            return population;
        }


        /// <summary>
        /// Removes weakest species from a population if its population is below %of total
        /// </summary>
        public Population RemoveSpeciesWithPopulationBelowTotal(Population population, int totalPopulationCount, float percentThreshold)
        {
            var threshold = (int)(totalPopulationCount * percentThreshold);

            var smallSpecies =
                population.Species.Where(x => x.Individuals.Count < threshold).ToList();

            foreach (var species in smallSpecies)
            {
                population.RemoveSpecies(species);
                Log.Info($"Removed species with degree: {species.DegreeOfIndividualsInSpecies} due to their population count below 5% total");
            }

            return population;
        }
    }
}
