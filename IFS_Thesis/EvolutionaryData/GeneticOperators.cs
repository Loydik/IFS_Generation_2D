using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Mutation.Individuals;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.EvolutionaryData.Selection.IndividualSelection;
using IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection;
using IFS_Thesis.Ifs;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData
{
    public class GeneticOperators
    {
        #region Private Properties

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets Random coefficient for IFS function
        /// </summary>
        private float GetRandomCoefficient(Random random, Tuple<int, int> range)
        {
            //random.NextDouble() * (maximum - minimum) + minimum
            var value = random.NextDouble() * (range.Item2 - range.Item1) + range.Item1;

            var coefficient = (float)Math.Round(value, 4);

            return coefficient;
        }

        /// <summary>
        /// Gets Recombination strategy (either one-point crossover or arithmetic crossover) based on probabilities
        /// </summary>
        private RecombinationStrategy GetRecombinationStrategy(float onePointCrossoverProbability, Random randomGen)
        {
            var result = randomGen.NextDouble();

            if (result <= onePointCrossoverProbability)
            {
                return new OnePointCrossoverStrategy();
            }

            return new ArithmeticCrossoverStrategy();
        }

        /// <summary>
        /// Gets Mutation strategy based on probabilities
        /// </summary>
        private RealValueMutationStrategy GetMutationStrategy(float randomMutationProbability, Random randomGen)
        {
            var result = randomGen.NextDouble();

            if (result <= randomMutationProbability)
            {
                return new RandomMutationStrategy();
            }

            return new ControlledMutationStrategy();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create Random Singel
        /// </summary>
        public Singel CreateRandomSingel(Random random)
        {
            //range for a,b,c,d coefficients
            var range1 = new Tuple<int, int>(-1, 1);

            //range for e,f coefficients
            var range2 = new Tuple<int, int>(-5, 5);

            var a11 = GetRandomCoefficient(random, range1);
            var a12 = GetRandomCoefficient(random, range1);
            var a13 = GetRandomCoefficient(random, range1);
            var a21 = GetRandomCoefficient(random, range1);
            var a22 = GetRandomCoefficient(random, range1);
            var a23 = GetRandomCoefficient(random, range1);
            var a31 = GetRandomCoefficient(random, range1);
            var a32 = GetRandomCoefficient(random, range1);
            var a33 = GetRandomCoefficient(random, range1);

            var b1 = GetRandomCoefficient(random, range2);
            var b2 = GetRandomCoefficient(random, range2);
            var b3 = GetRandomCoefficient(random, range2);

            var coefficients = new IfsFunction(a11,a12,a13,a21,a22,a23,a31,a32,a33,b1,b2,b3);

            var singel = new Singel(coefficients);

            return singel;
        }

        /// <summary>
        /// Create individual based on probabilities vector from a given pool of singels
        /// </summary>
        public Individual CreateIndividual(List<float> probabilityVectors, List<Singel> poolOfSingels, Random randomGen)
        {
            var degreeOfIndividual = GetIndividualsDegreeBasedOnVectors(probabilityVectors, randomGen);

            poolOfSingels.Shuffle(randomGen);

            var individual = new Individual(poolOfSingels.Take(degreeOfIndividual).Select(x => x.Coefficients).ToList())
            {
                Degree = degreeOfIndividual
            };

            return individual;
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

        /// <summary>
        /// Generates a specified amount of singels
        /// </summary>
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

        /// <summary>
        /// Creates individuals based on probability vector
        /// </summary>
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

        /// <summary>
        /// Creates individuals from existing pool of singels
        /// </summary>
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
            IndividualSelectionStrategy individualSelectionStrategy = new RouletteWheelIndividualSelectionStrategy();
            SpeciesSelectionStrategy speciesSelectionStrategy = new ProbabilityVectorSpeciesSelectionStrategy();
            RecombinationStrategy recombinationStrategy;
            IndividualMutationStrategy individualMutationStrategy = new StandardMutationRateStrategy();

            var newPopulation = new Population();

            if (Settings.Default.UseReinsertion == false)
            {
                // (Step 6.) Adding best individuals of each new degree 
                var bestIndividuals = EaUtils.GetBestIndividualsOfEachDegree(population, Settings.Default.EliteIndividualsPerDegree);

                //we set those individuals as elite
                bestIndividuals.ForEach(i => i.Elite = true);

                newPopulation.AddIndividuals(bestIndividuals);

                Log.Info("Added elite individuals to new population");
            }

            #region N1 Individuals

            //Step 7
            var n1Count = Settings.Default.N1IndividualsPercentage * Settings.Default.PopulationSize;

            var n1Individuals = new List<Individual>();

            for (int i = 0; i <= n1Count / 2; i++)
            {
                //Selecting Species
                var selectedSpecies = speciesSelectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);

                //selecting recombination strategy
                recombinationStrategy = GetRecombinationStrategy(Settings.Default.OnePointCrossoverProbability,
                    randomGen);

                if (selectedSpecies != null)
                {
                    var selectedIndividuals =
                        individualSelectionStrategy.SelectIndividuals(
                            population.Individuals
                                .Where(x => x.Degree.Equals(selectedSpecies.DegreeOfIndividualsInSpecies))
                                .ToList(), 2, randomGen);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        Log.Debug($"Selected N1 individuals for recombination: \n {string.Join("\n", selectedIndividuals)} \n Recombination strategy: {recombinationStrategy.GetType()}");
                    }

                    var offspring = recombinationStrategy.ProduceOffsprings(selectedIndividuals[0],
                        selectedIndividuals[1],
                        randomGen);
                    n1Individuals.AddRange(offspring);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        Log.Debug($"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", offspring)}");
                    }
                }
                else
                {
                    Log.Warn($"There was a problem with selecting species (Step 7). Population count - {population.Count}. \n Probability vectors - {probabilityVectors}");
                }
            }

            newPopulation.AddIndividuals(n1Individuals);
            Log.Debug($"Added {n1Individuals.Count} N1 individuals to new population");

            #endregion

            #region N2 Individuals

            //Step 8
            var n2Count = (int)(Settings.Default.N2IndividualsPercentage * Settings.Default.PopulationSize);
            var n2Individuals = CreateIndividuals(1000, n2Count, probabilityVectors, randomGen);
            newPopulation.AddIndividuals(n2Individuals);
            Log.Debug($"Added {n2Individuals.Count} N2 individuals to new population");

            if (Settings.Default.ExtremeDebugging)
            {
                Log.Debug($"Added N2 individuals into population: : \n {string.Join("\n", n2Individuals)}");
            }

            #endregion

            #region N3 Individuals

            //Step 9
            var n3Count = Settings.Default.N3IndividualsPercentage * Settings.Default.PopulationSize;

            var n3Individuals = new List<Individual>();

            for (int i = 0; i <= n3Count / 2; i++)
            {
                var firstSpecies = speciesSelectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);

                if (firstSpecies != null)
                {
                    var secondSpecies = speciesSelectionStrategy.SelectSecondSpecies(population, firstSpecies, 1, randomGen);

                    if (secondSpecies != null)
                    {
                        var firstIndividual =
                            individualSelectionStrategy.SelectIndividuals(firstSpecies.Individuals, 1, randomGen).First();
                        var secondIndividual =
                            individualSelectionStrategy.SelectIndividuals(secondSpecies.Individuals, 1, randomGen).First();

                        if (Settings.Default.ExtremeDebugging)
                        {
                            Log.Debug($"Selected N3 individuals for inter-species crossover: \n {firstIndividual} \n {secondIndividual} \n");
                        }

                        recombinationStrategy = new InterSpeciesCrossoverStrategy();

                        var children = recombinationStrategy.ProduceOffsprings(firstIndividual, secondIndividual,
                            randomGen);
                        n3Individuals.AddRange(children);

                        if (Settings.Default.ExtremeDebugging)
                        {
                            Log.Debug($"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", children)}");
                        }

                    }
                }
            }

            newPopulation.AddIndividuals(n3Individuals);
            Log.Debug($"Added {n3Individuals.Count} N3 individuals to new population");

            #endregion

            #region N4 Individuals

            //Step 10
            var n4Count = Settings.Default.N4IndividualsPercentage * Settings.Default.PopulationSize;
            var n4Individuals = new List<Individual>();

            for (int i = 0; i <= n4Count / 2; i++)
            {
                var parents = individualSelectionStrategy.SelectIndividuals(population.Individuals, 2, randomGen);

                if (Settings.Default.ExtremeDebugging)
                {
                    Log.Debug($"Selected N4 individuals for reasortment: \n {string.Join("\n", parents)} \n");
                }

                recombinationStrategy = new ReasortmentStrategy();

                if (parents[0] != null && parents[1] != null)
                {
                    var children = recombinationStrategy.ProduceOffsprings(parents[0], parents[1], randomGen);
                    n4Individuals.AddRange(children);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        Log.Debug($"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", children)}");
                    }
                }
            }

            newPopulation.AddIndividuals(n4Individuals);
            Log.Debug($"Added {n4Individuals.Count} N4 individuals to new population");

            #endregion

            #region Mutation

            //Step 11
            //Mutate all individuals except elite ones
            foreach (Individual individual in newPopulation.Individuals.Where(i => i.Elite == false))
            {
                //Mutates individual with frequency of defined probability
                if (EaUtils.DeterminePercentProbability(randomGen, Settings.Default.MutationProbability))
                {
                    var mutationStrategy = GetMutationStrategy(Settings.Default.RandomMutationProbability, randomGen);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        Log.Debug($"Selected individual for mutation: {individual} using {mutationStrategy.GetType()}");
                    }

                    var currentIndividual = individual;

                    individualMutationStrategy.Mutate(ref currentIndividual, mutationStrategy, randomGen);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        Log.Debug($"Individual after mutation: {currentIndividual}");
                    }
                }
            }

            #endregion

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

        #endregion
    }
}
