﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.IFS;
using IFS_Thesis.IFS.IFSDrawers;
using IFS_Thesis.IFS.IFSGenerators;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData
{
    /// <summary>
    /// Class which represents the Evolutionary Algorithm for 3D IFS
    /// </summary>
    public class EvolutionaryAlgorithm
    {
        #region Private Properties

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Genetic operators
        /// </summary>
        private readonly GeneticOperators _geneticOperators;

        /// <summary>
        /// Fitness function
        /// </summary>
        private readonly IObjectiveFitnessFunction _objectiveFitnessFunction;


        #endregion

        #region Private Methods

        #region Helper Methods

        /// <summary>
        /// Writes to the log the configuration and initial conditions of the EA
        /// </summary>
        private void OutputEvolutinaryAlgorithmParameters()
        {
            Log.Info("WELCOME TO IFS GENERATION ALGORITHM IN 3D!");
            Log.Info("Initial settings are as follows:");

            foreach (SettingsProperty currentProperty in Settings.Default.Properties)
            {
                Log.Info($"{currentProperty.Name} : {currentProperty.DefaultValue}");
            }
        }

        /// <summary>
        /// Changes configuration of parameters based on the current generation
        /// </summary>
        private void ChangeConfiguration(int currentGeneration, EaConfiguration configuration)
        {
            //Every 200th generation we turn on extreme debugging
            Settings.Default.ExtremeLogging = currentGeneration % 200 == 0;

            if (currentGeneration == 500)
            {
                configuration.RandomMutationProbability = 0.4f;
                configuration.ControlledMutationProbability = 0.6f;

                Log.Info($"Changed configuration:\n {configuration}");
            }

            if (currentGeneration == 1000)
            {
                configuration.RandomMutationProbability = 0.3f;
                configuration.ControlledMutationProbability = 0.7f;

                Log.Info($"Changed configuration:\n {configuration}");
            }

            if (currentGeneration == 2000)
            {
                configuration.RandomMutationProbability = 0.2f;
                configuration.ControlledMutationProbability = 0.8f;

                Log.Info($"Changed configuration:\n {configuration}");
            }

            if (currentGeneration == 3000)
            {
                configuration.RandomMutationProbability = 0.1f;
                configuration.ControlledMutationProbability = 0.9f;

                Log.Info($"Changed configuration:\n {configuration}");
            }

            if (currentGeneration == 4000)
            {
                configuration.RandomMutationProbability = 0.05f;
                configuration.ControlledMutationProbability = 0.95f;

                Log.Info($"Changed configuration:\n {configuration}");
            }
        }

        /// <summary>
        /// Gets best individuals per degree for population
        /// </summary>
        private List<Individual> GetBestIndividualsPerDegree(Population population)
        {
            var bestIndividualsPerDegree = EaUtils.GetBestIndividualsOfEachDegree(population, 1);
            return bestIndividualsPerDegree;
        }

        /// <summary>
        /// Gets individual with the highest fitness from population
        /// </summary>
        private Individual GetHighestFitIndividual(Population population)
        {
            var highestFitnessIndividual = population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();
            return highestFitnessIndividual;
        }

        /// <summary>
        /// Gets individual with the highest fitness from several populations
        /// </summary>
        private Individual GetHighestFitIndividual(List<Population> populations)
        {
            var highestFitInPopulations = new List<Individual>();

            foreach (var population in populations)
            {
                var highestFitIndividual =
                    population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();
                highestFitInPopulations.Add(highestFitIndividual);
            }

            return highestFitInPopulations.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();
        }

        /// <summary>
        /// Generates a report image based on current generation
        /// </summary>
        private void GenerateReportImage(IfsDrawer3D ifsDrawer, IfsGenerator ifsGenerator, Individual individual, int currentGenerationNumber,
            string path)
        {
            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.ReportImageEveryNthGeneration == 0)
            {
                if (individual != null)
                {
                    var voxels = ifsGenerator.GenerateVoxelsForIfs(individual.Singels, Settings.Default.ImageX,
                        Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);

                    ifsDrawer.SaveVoxelsTo3DImage(path +
                                             $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}", voxels, ImageFormat3D.Stl);
                }
            }
        }

        #endregion

        #region Evolutionary Algorithm Methods

        /// <summary>
        /// Generates initial population to seed evolutionary algorithm
        /// </summary>
        /// <param name="configuration">Configuration of Evolutionary algorithm</param>
        /// <param name="sourceImageVoxels">Voxels of the 3D image to evolve towards</param>
        /// <param name="probabilityVector">Vector of probabilities</param>
        /// <param name="ifsGenerator">IFS generator for 3D</param>
        /// <param name="geneticUniversum">Genetic universum (singels)</param>
        /// <param name="randomGen">Random number generator</param>
        private Population GenerateInitialPopulation(EaConfiguration configuration, HashSet<Voxel> sourceImageVoxels,
            List<float> probabilityVector, IfsGenerator ifsGenerator, List<Singel> geneticUniversum, Random randomGen)
        {
            var population = new Population();

            Log.Info($"Started generation of {Settings.Default.InitialPopulationGenerationMultiplier}x initial individuals");

            //Creating more initial individuals than needed
            var initialIndividuals = new GeneticOperators().CreateIndividualsFromExistingPoolOfSingels(
                geneticUniversum, configuration.PopulationSize * Settings.Default.InitialPopulationGenerationMultiplier,
                probabilityVector, randomGen);

            Log.Info(
                $"Ended generation of {Settings.Default.InitialPopulationGenerationMultiplier}x initial individuals. Starting fitness calculation");

            //Calculating fitness for created individuals and taking N best
            initialIndividuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(initialIndividuals,
                sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY,
                Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier).OrderByDescending(x => x.ObjectiveFitness).Take(configuration.PopulationSize).ToList();

            Log.Info("Calcualted fitness for generated individuals.");

            //Adding generated individuals to the population
            population.AddIndividuals(initialIndividuals);

            Log.Info($"Highest fitness individual of generated ones is {GetHighestFitIndividual(population)}.\n");

            return population;
        }

        /// <summary>
        /// Evolves one generation
        /// </summary>
        /// <param name="configuration">Configuration of Evolutionary Algorithm</param>
        /// <param name="population">Current population to evolve</param>
        /// <param name="geneticUniversum">Genetic universum</param>
        /// <param name="currentGenerationNumber">Current generation number</param>
        /// <param name="probabilityVector">Vector of probabilities</param>
        /// <param name="sourceImageVoxels">Voxels of the 3D image to evolve towards</param>
        /// <param name="ifsGenerator">Ifs generator (generates voxels in 3d space)</param>
        /// <param name="randomGen">Random number generator</param>
        private Population EvolveGeneration(EaConfiguration configuration, Population population, List<Singel> geneticUniversum,
            int currentGenerationNumber, ref List<float> probabilityVector, HashSet<Voxel> sourceImageVoxels,
             IfsGenerator ifsGenerator, Random randomGen)
        {
            Log.Info($"Starting evolving generation {currentGenerationNumber}...");

            //Updating Probability Vector
            probabilityVector =
                EaUtils.UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(
                    GetBestIndividualsPerDegree(population),
                    probabilityVector);

            //Generating New Population (Steps 7 - 11)
            var newPopulation = _geneticOperators.GenerateNewPopulation(configuration, population, probabilityVector,
                randomGen);

            Log.Info("Generated new population");

            newPopulation.Individuals =
                _objectiveFitnessFunction.CalculateFitnessForIndividuals(newPopulation.Individuals, sourceImageVoxels,
                    ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ,
                    Settings.Default.IfsGenerationMultiplier);

            population = newPopulation;

            Log.Info($"Finished evolving generation {currentGenerationNumber}...");

            if (Settings.Default.ExtremeLogging)
            {
                Log.Debug($"Best individuals per degree are:\n {string.Join("\n", GetBestIndividualsPerDegree(population))} \n\n");
                Log.Debug($"Current whole population is:\n {string.Join("\n", population.Individuals)}");
            }

            Log.Info($"Highest fitness individual in the population is {GetHighestFitIndividual(population)}.\n");
            Log.Info($"Average population fitness: {population.Individuals.Average(x => x.ObjectiveFitness):#.#####}");
            Log.Info($"Population size: {population.Count}");

            return population;
        }

        /// <summary>
        /// Generates Report images to track progress
        /// </summary>
        /// <param name="currentGenerationNumber">Current generation number</param>
        /// <param name="population">Current population</param>
        /// <param name="drawer">Ifs Drawer (creates an image)</param>
        /// <param name="ifsGenerator">Ifs generator (creates voxels in 3D space)</param>
        private void GenerateReportImages(int currentGenerationNumber, Population population, IfsDrawer3D drawer, IfsGenerator ifsGenerator)
        {
            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.ReportImageEveryNthGeneration == 0)
            {
                var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
                Directory.CreateDirectory(folderPath);

                if (Settings.Default.ExtremeLogging)
                {
                    foreach (var individual in GetBestIndividualsPerDegree(population))
                    {
                        GenerateReportImage(drawer, ifsGenerator, individual, currentGenerationNumber, folderPath);
                    }
                }

                else
                {
                    GenerateReportImage(drawer, ifsGenerator, GetHighestFitIndividual(population), currentGenerationNumber, folderPath);
                }
            }
        }

        /// <summary>
        /// Generates Report images from multiple populations to track progress
        /// </summary>
        /// <param name="currentGenerationNumber">Current generation number</param>
        /// <param name="populations">Current populations</param>
        /// <param name="drawer">Ifs Drawer (creates an image)</param>
        /// <param name="ifsGenerator">Ifs generator (creates voxels in 3D space)</param>
        private void GenerateReportImages(int currentGenerationNumber, List<Population> populations, IfsDrawer3D drawer, IfsGenerator ifsGenerator)
        {
            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.ReportImageEveryNthGeneration == 0)
            {
                var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
                Directory.CreateDirectory(folderPath);

                for (var i = 0; i < populations.Count; i++)
                {
                    var population = populations[i];

                    var populationFolderPath = folderPath + $"/population_{i+1}";
                    Directory.CreateDirectory(populationFolderPath);

                    if (Settings.Default.ExtremeLogging)
                    {
                        foreach (var individual in GetBestIndividualsPerDegree(population))
                        {
                            GenerateReportImage(drawer, ifsGenerator, individual, currentGenerationNumber, populationFolderPath);
                        }
                    }

                    else
                    {
                        GenerateReportImage(drawer, ifsGenerator, GetHighestFitIndividual(population),
                            currentGenerationNumber, populationFolderPath);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Construction

        /// <summary>
        /// Create an instance of Evolutionary Algorithm
        /// </summary>
        public EvolutionaryAlgorithm()
        {
            _geneticOperators = new GeneticOperators();
            _objectiveFitnessFunction = new WeightedPointsCoverageObjectiveFitnessFunction();
            Log.Info($"Objective fitness function is {_objectiveFitnessFunction.GetType()}");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the Evolution Process
        /// </summary>
        public Individual StartEvolution(EaConfiguration configuration, int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            //Initial Probability vector, 8 max
            var probabilityVector = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.05f, 0.05f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", probabilityVector)}]");

            var geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            var population = GenerateInitialPopulation(configuration, sourceImageVoxels, probabilityVector, ifsGenerator, geneticUniversum,
                randomGen);

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                population = EvolveGeneration(configuration, population, geneticUniversum, currentGenerationNumber,
                    ref probabilityVector, sourceImageVoxels, ifsGenerator, randomGen);

                ChangeConfiguration(currentGenerationNumber, configuration);

                GenerateReportImages(currentGenerationNumber, population, drawer, ifsGenerator);              
            }

            return GetHighestFitIndividual(population);
        }

        /// <summary>
        /// Start the Evolution Process with given initial population (for testing purposes)
        /// </summary>
        public Individual StartEvolution(EaConfiguration configuration, Population population, int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            //Initial Probability vector, 8 max
            var probabilityVector = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.05f, 0.05f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", probabilityVector)}]");

            Log.Info($"Evolutionary run with defined initial population \n {string.Join("\n", population.Individuals)}");

            var geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            population.Individuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(population.Individuals,
                sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY,
                Settings.Default.ImageZ,
                Settings.Default.IfsGenerationMultiplier);

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                population = EvolveGeneration(configuration, population, geneticUniversum, currentGenerationNumber,
                    ref probabilityVector, sourceImageVoxels, ifsGenerator, randomGen);

                ChangeConfiguration(currentGenerationNumber, configuration);

                GenerateReportImages(currentGenerationNumber, population, drawer, ifsGenerator);
            }

            return GetHighestFitIndividual(population);
        }

        /// <summary>
        /// Starts evolutionary process with multiple parallel populations
        /// </summary>
        public Individual StartEvolutionWithMultiplePopulations(List<EaConfiguration> populationConfigurations, int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            foreach (var configuration in populationConfigurations)
            {
                Log.Info($"Custom configuration is as follows: \n {configuration}");
            }

            //Initial Probability vector, 8 max
            var probabilityVector = new List<float> { 0, 0, 0.2f, 0.2f, 0.2f, 0.15f, 0.15f, 0.1f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", probabilityVector)}]");

            var geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            var populations = new List<Population>();

            foreach (var configuration in populationConfigurations)
            {
                var population = GenerateInitialPopulation(configuration, sourceImageVoxels, probabilityVector, ifsGenerator, geneticUniversum,
            randomGen);
                populations.Add(population);
            }

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                for (var index = 0; index < populationConfigurations.Count; index++)
                {
                    var configuration = populationConfigurations[index];
                    populations[index] = EvolveGeneration(configuration, populations[index], geneticUniversum, currentGenerationNumber,
                        ref probabilityVector, sourceImageVoxels, ifsGenerator, randomGen);
                    ChangeConfiguration(currentGenerationNumber, configuration);
                }

                if (currentGenerationNumber % Settings.Default.MigrationFrequency == 0)
                {
                    populations = _geneticOperators.MigrateIndividualsBetweenSubpopulations(populations, Settings.Default.MigrationRatePerDegree,
                        randomGen);
                }
                
                GenerateReportImages(currentGenerationNumber, populations, drawer, ifsGenerator);
                
            }

            return GetHighestFitIndividual(populations);
        }

        #endregion
    }
}
