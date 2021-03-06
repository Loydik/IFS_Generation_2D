﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.EvolutionaryData.Mutation.Individuals;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.EvolutionaryData.Selection.IndividualSelection;
using IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection;
using IFS_Thesis.IFS;
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
        /// Gets Recombination strategy (either one-point crossover, arithmetic crossover or discrete recombination) based on probabilities
        /// </summary>
        private RecombinationStrategy GetRecombinationStrategy(EaConfiguration configuration, Random randomGen)
        {
            var result = randomGen.NextDouble();

            if (result <= configuration.ArithmeticCrossoverProbability)
            {
                return new ArithmeticCrossoverStrategy();
            }

            if (result <= configuration.ArithmeticCrossoverProbability + configuration.OnePointCrossoverProbability)
            {
                return new OnePointCrossoverStrategy();
            }

            return new DiscreteSingelRecombinationStrategy();
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
            var range2 = new Tuple<int, int>(-2, 2);

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
        public List<Singel> GeneratePoolOfSingels(int amount, Random random)
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

            var initialPoolOfSingels = GeneratePoolOfSingels(singelsPoolSize, randomGen);

            for (int i = 0; i < populationSize; i++)
            {
                var individual = new GeneticOperators().CreateIndividual(probablityVectors, initialPoolOfSingels, randomGen);

                generatedIndividuals.Add(individual);
            }

            return generatedIndividuals;
        }

        /// <summary>
        /// Creates individuals based on probability vector
        /// </summary>
        public List<Individual> CreateIndividualsFromRandomPoolOfSingels(int singelsPoolSize, int populationSize, List<float> probablityVectors, Random randomGen)
        {
            var generatedIndividuals = new List<Individual>();

            var initialPoolOfSingels = GeneratePoolOfSingels(singelsPoolSize, randomGen);

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
        public Population GenerateNewPopulation(EaConfiguration configuration, Population population, List<float> probabilityVectors, Random randomGen)
        {
            IndividualSelectionStrategy individualSelectionStrategy = new StochasticUniversalSamplingIndividualSelectionStrategy();
            SpeciesSelectionStrategy speciesSelectionStrategy = new ProbabilityVectorSpeciesSelectionStrategy();
            RecombinationStrategy recombinationStrategy;
            IndividualMutationStrategy individualMutationStrategy = new StandardMutationRateStrategy();
            IRankingFitnessFunction rankingFitnessFunction = new LinearRankingFitnessFunction();

            var newPopulation = new Population();

            #region Adding Elite Individuals

            // (Step 6.) Adding best individuals of each new degree 
            var bestIndividuals = EaUtils.GetBestIndividualsOfEachDegree(population,
                configuration.EliteIndividualsPerDegree);

            //bestIndividuals.ForEach(i => i.Elite = true);

            foreach (var individual in bestIndividuals)
            {
                if (individual.ObjectiveFitness >= configuration.EliteFitnessThreshold)
                {
                    //we set best individual as elite
                    individual.Elite = true;
                    newPopulation.AddIndividual(individual);
                }
            }

            Log.Info("Added elite individuals to new population");

            #endregion

            #region N1 Individuals

            //Step 7
            var n1Count = configuration.N1IndividualsPercentage * configuration.PopulationSize;

            var n1Individuals = new List<Individual>();

            var selectedDegrees = new List<int>();

            //Creating a list of selected species
            for (int i = 0; i < n1Count / 2; i++)
            {
                var selectedSpecies = speciesSelectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);
                if (selectedSpecies != null)
                {
                    selectedDegrees.Add(selectedSpecies.DegreeOfIndividualsInSpecies);
                }
            }

            foreach (var degree in selectedDegrees.Distinct().ToList())
            {
                var individualsForDegree = selectedDegrees.Count(x => x == degree) * 2;

                var selectedIndividuals =
                    individualSelectionStrategy.SelectIndividuals(
                        population.Individuals
                            .Where(x => x.Degree.Equals(degree))
                            .ToList(), rankingFitnessFunction, individualsForDegree, configuration.SelectionPressure, randomGen);

                selectedIndividuals.Shuffle(randomGen);

                for (int i = 0; i < selectedIndividuals.Count; i++)
                {
                    //selecting recombination strategy
                    recombinationStrategy = GetRecombinationStrategy(configuration, randomGen);

                    var firstParent = selectedIndividuals[i];
                    var secondParent = selectedIndividuals[i+1];

                    if (Settings.Default.ExtremeLogging)
                    {
                        Log.Debug(
                            $"Selected N1 individuals for recombination: \n {firstParent} \n {secondParent}  \n Recombination strategy: {recombinationStrategy.GetType()}");
                    }

                    var offspring = recombinationStrategy.ProduceOffsprings(firstParent,
                    secondParent,
                    randomGen);
                    n1Individuals.AddRange(offspring);
                    i++;

                    if (Settings.Default.ExtremeLogging)
                    {
                        Log.Debug(
                            $"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", offspring)}");
                    }
                }              
            }

            newPopulation.AddIndividuals(n1Individuals);
            Log.Debug($"Added {n1Individuals.Count} N1 individuals to new population");

            #endregion

            #region N2 Individuals

            //Step 8
            var n2Count = (int)(configuration.N2IndividualsPercentage * configuration.PopulationSize);

            var n2Individuals = CreateIndividualsFromExistingPoolOfSingels(population.GetAllSingels(), n2Count,
                probabilityVectors, randomGen);

            newPopulation.AddIndividuals(n2Individuals);
            Log.Debug($"Added {n2Individuals.Count} N2 individuals to new population");

            if (Settings.Default.ExtremeLogging)
            {
                Log.Debug($"Added N2 individuals into population: : \n {string.Join("\n", n2Individuals)}");
            }

            #endregion

            #region N3 Individuals

            //Step 9
            var n3Count = configuration.N3IndividualsPercentage * configuration.PopulationSize;

            var n3Individuals = new List<Individual>();

            if (n3Count > 0)
            {
                for (int i = 0; i <= n3Count / 2; i++)
                {
                    var firstSpecies = speciesSelectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);

                    if (firstSpecies != null)
                    {
                        var secondSpecies = speciesSelectionStrategy.SelectSecondSpecies(population, firstSpecies, 1,
                            randomGen);

                        if (secondSpecies != null)
                        {
                            var firstIndividual =
                                individualSelectionStrategy.SelectIndividuals(firstSpecies.Individuals,
                                    rankingFitnessFunction, 1, configuration.SelectionPressure, randomGen).First();
                            var secondIndividual =
                                individualSelectionStrategy.SelectIndividuals(secondSpecies.Individuals,
                                    rankingFitnessFunction, 1, configuration.SelectionPressure, randomGen).First();

                            if (Settings.Default.ExtremeLogging)
                            {
                                Log.Debug(
                                    $"Selected N3 individuals for inter-species crossover: \n {firstIndividual} \n {secondIndividual} \n");
                            }

                            recombinationStrategy = new InterSpeciesCrossoverStrategy();

                            var children = recombinationStrategy.ProduceOffsprings(firstIndividual, secondIndividual,
                                randomGen);
                            n3Individuals.AddRange(children);

                            if (Settings.Default.ExtremeLogging)
                            {
                                Log.Debug(
                                    $"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", children)}");
                            }

                        }
                    }
                }
            }

            newPopulation.AddIndividuals(n3Individuals);
            Log.Debug($"Added {n3Individuals.Count} N3 individuals to new population");

            #endregion

            #region N4 Individuals

            //Step 10
            var n4Count = (int) (configuration.N4IndividualsPercentage * configuration.PopulationSize);

            //in case if n4 count is not divisible by 2
            if (n4Count % 2 != 0)
            {
                n4Count = n4Count - 1;
            }

            var n4Individuals = new List<Individual>();

            recombinationStrategy = new ReasortmentStrategy();
            var individualsForReasortment = individualSelectionStrategy.SelectIndividuals(population.Individuals, rankingFitnessFunction, n4Count, configuration.SelectionPressure, randomGen);
            individualsForReasortment.Shuffle(randomGen);

            for (int i = 0; i < individualsForReasortment.Count; i++)
            {
                var firstParent = individualsForReasortment[i];
                var secondParent = individualsForReasortment[i + 1];

                if (Settings.Default.ExtremeLogging)
                {
                    Log.Debug($"Selected N4 individuals for reasortment: \n {firstParent} \n {secondParent}  \n");
                }     

                var offspring = recombinationStrategy.ProduceOffsprings(firstParent,
                secondParent,
                randomGen);
                n4Individuals.AddRange(offspring);
                i++;

                if (Settings.Default.ExtremeLogging)
                {
                    Log.Debug(
                        $"Produced 2 offspring using {recombinationStrategy.GetType()}: \n {string.Join("\n", offspring)}");
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
                if (EaUtils.DeterminePercentProbability(randomGen, configuration.MutationProbability))
                {
                    var mutationStrategy = GetMutationStrategy(configuration.RandomMutationProbability, randomGen);

                    if (Settings.Default.ExtremeLogging)
                    {
                        Log.Debug($"Selected individual for mutation: {individual} using {mutationStrategy.GetType()}");
                    }

                    var currentIndividual = individual;

                    individualMutationStrategy.Mutate(configuration, ref currentIndividual, mutationStrategy, randomGen);

                    if (Settings.Default.ExtremeLogging)
                    {
                        Log.Debug($"Individual after mutation: {currentIndividual}");
                    }
                }
            }

            #endregion

            return newPopulation;
        }

        /// <summary>
        /// Migrates individuals between subpopulations
        /// </summary>
        public List<Population> MigrateIndividualsBetweenSubpopulations(List<Population> subpopulations, int migrationRatePerDegree, Random randomGen)
        {
            for (var i = 0; i < subpopulations.Count; i++)
            {
                var currentSubpopulation = subpopulations[i];

                var otherSubpopulations = subpopulations.Where((c, index) => index != i).ToList();

                var migrationPool = new List<Individual>();
                
                //constructing a migration pool of best individuals from other subpopulations
                foreach (var pop in otherSubpopulations)
                {
                    migrationPool.AddRange(EaUtils.GetBestIndividualsOfEachDegree(pop, migrationRatePerDegree).Clone());
                }

                migrationPool.Shuffle(randomGen);

                var degreesOfIndividualsInCurrentSubpopulation = EaUtils.GetDegreesOfIndividuals(currentSubpopulation.Individuals);
                
                //For each degree in current subpopulation, we replace worst individuals with migrated ones
                foreach (var degree in degreesOfIndividualsInCurrentSubpopulation)
                {
                    if (currentSubpopulation.Individuals.Count(x => x.Degree == degree) >= migrationRatePerDegree && migrationPool.Count(x => x.Degree == degree) >= migrationRatePerDegree)
                    {
                        currentSubpopulation.ReplaceWorstIndividualsOfDegree(degree, migrationPool.Where(x => x.Degree == degree).Take(migrationRatePerDegree).ToList());
                    }
                }
            }

            Log.Info("Finished Migration process");

            return subpopulations;
        }

        #endregion
    }
}
