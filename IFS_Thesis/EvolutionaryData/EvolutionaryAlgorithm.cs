using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.EvolutionaryData.Reinsertion;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Ifs.IFSGenerators;
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
        private void ChangeConfiguration(int currentGeneration)
        {
            //Every 200th generation we turn on extreme debugging
            Settings.Default.ExtremeDebugging = currentGeneration % 200 == 0;
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
        /// Generates a report image based on current generation
        /// </summary>
        private void GenerateReportImage(IfsDrawer3D ifsDrawer, IfsGenerator ifsGenerator, Individual individual, int currentGenerationNumber,
            string path)
        {
            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.DrawImageEveryNthGeneration == 0)
            {
                if (individual != null)
                {
                    var voxels = ifsGenerator.GenerateVoxelsForIfs(individual.Singels, Settings.Default.ImageX,
                        Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);

                    //ifsDrawer.SaveVoxelImage(path +
                    // $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}", voxels, ImageFormat3D.Obj);
                    ifsDrawer.SaveVoxelImage(path +
                                             $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}", voxels, ImageFormat3D.Stl);
                }
            }
        }

        #endregion

        #region Evolutionary Algorithm Methods

        /// <summary>
        /// Generates initial population to seed evolutionary algorithm
        /// </summary>
        /// <param name="sourceImageVoxels">Voxels of the 3D image to evolve towards</param>
        /// <param name="ifsGenerator">IFS generator for 3D</param>
        /// <param name="geneticUniversum">Genetic universum (singels)</param>
        /// <param name="randomGen">Random number generator</param>
        /// <returns></returns>
        private Population GenerateInitialPopulation(HashSet<Voxel> sourceImageVoxels, List<float> probabilityVector, IfsGenerator ifsGenerator, List<Singel> geneticUniversum, Random randomGen)
        {
            var population = new Population();

            List<Individual> initialIndividuals;

            #region Generating 10x random individuals and taking the best

            if (Settings.Default.Generate10xIndividuals)
            {
                Log.Info("Started generation of 10x initial individuals");

                initialIndividuals = new GeneticOperators().CreateIndividualsFromExistingPoolOfSingels(
                    geneticUniversum, Settings.Default.PopulationSize * 10, probabilityVector, randomGen);

                Log.Info("Ended generation of 10x initial individuals. Starting fitness calculation");

                initialIndividuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(initialIndividuals,
                    sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY,
                    Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);

                initialIndividuals = initialIndividuals.OrderByDescending(x => x.ObjectiveFitness).ToList();

                initialIndividuals = initialIndividuals.Take(Settings.Default.PopulationSize).ToList();
            }

            else
            {
                initialIndividuals = new GeneticOperators().CreateIndividualsFromExistingPoolOfSingels(
                    geneticUniversum, Settings.Default.PopulationSize, probabilityVector, randomGen);
                initialIndividuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(initialIndividuals, sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
            }

            Log.Info("Calcualted fitness for generated individuals.");

            #endregion

            Log.Info($"Generated {initialIndividuals.Count} initial individuals from the Universum.");

            population.AddIndividuals(initialIndividuals);

            Log.Info("Added generated individuals to the population.");

            Log.Info($"Highest fitness individual of generated ones is {GetHighestFitIndividual(population)}.\n");

            return population;
        }

        /// <summary>
        /// Evolves one generation
        /// </summary>
        /// <param name="population">Current population to evolve</param>
        /// <param name="geneticUniversum">Genetic universum</param>
        /// <param name="currentGenerationNumber">Current generation number</param>
        /// <param name="probabilityVector">Vector of probabilities</param>
        /// <param name="sourceImageVoxels">Voxels of the 3D image to evolve towards</param>
        /// <param name="ifsGenerator">Ifs generator (generates voxels in 3d space)</param>
        /// <param name="randomGen">Random number generator</param>
        private Population EvolveGeneration(Population population, List<Singel> geneticUniversum,
            int currentGenerationNumber, ref List<float> probabilityVector, HashSet<Voxel> sourceImageVoxels,
             IfsGenerator ifsGenerator, Random randomGen)
        {
            Log.Info($"Starting evolving generation {currentGenerationNumber}...");

            if (currentGenerationNumber > Settings.Default.UpdateProbabilityVectorAfterNGenerations)
            {
                probabilityVector =
                    EaUtils.UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(GetBestIndividualsPerDegree(population),
                        probabilityVector);
            }

            var oldPopulation = population;

            //Generating New Population (Steps 7 - 11)
            var newPopulation = _geneticOperators.GenerateNewPopulation(population, geneticUniversum, probabilityVector,
                randomGen);

            Log.Info("Generated new population");

            newPopulation.Individuals =
                _objectiveFitnessFunction.CalculateFitnessForIndividuals(newPopulation.Individuals, sourceImageVoxels,
                    ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ,
                    Settings.Default.IfsGenerationMultiplier);

            var reinsertionStrategy = new DegreeBasedReinsertionStrategy();

            //Reinsertion
            population = Settings.Default.UseReinsertion
                ? reinsertionStrategy.ReinsertIndividuals(oldPopulation, newPopulation, randomGen)
                : newPopulation;

            if (Settings.Default.RecalculateFitnessAfterReinsertion && Settings.Default.UseReinsertion)
            {
                //recalculating fitness for whole population
                population.Individuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(
                    population.Individuals,
                    sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY,
                    Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
            }

            //var totalPopulationCount = _population.Count;

            ////Removing and adding species
            //int speciesCountBefore = _population.Species.Count;

            ////Step 13
            //_population = new GeneticOperators().RemoveWeakestSpecies(_population,
            //    Settings.Default.AverageFitnessThreshold);

            ////Step 14
            //_population = new GeneticOperators().RemoveSpeciesWithPopulationBelowTotal(_population, totalPopulationCount, 0.04f);

            ////Step 15
            //if (speciesCountBefore < _population.Species.Count)
            //{
            //    //TODO - Implement creation of new species
            //}

            Log.Info($"Finished evolving generation {currentGenerationNumber}...");

            if (Settings.Default.ExtremeDebugging)
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
            //Outputting whole population - removed due to disc space constraints
            //if (currentGenerationNumber % 1000 == 0)
            //{
            //    var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
            //    System.IO.Directory.CreateDirectory(folderPath);

            //    foreach (var individual in population.Individuals)
            //    {
            //        GenerateReportImage(drawer, ifsGenerator, individual, currentGenerationNumber, folderPath);
            //    }
            //}

            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.DrawImageEveryNthGeneration == 0)
            {
                var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
                Directory.CreateDirectory(folderPath);

                if (Settings.Default.ExtremeDebugging)
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
        public Individual StartEvolution(int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            //Initial Probability vector, 8 max
            var probabilityVector = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", probabilityVector)}]");

            var geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            var population = GenerateInitialPopulation(sourceImageVoxels, probabilityVector, ifsGenerator, geneticUniversum,
                randomGen);

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                population = EvolveGeneration(population, geneticUniversum, currentGenerationNumber,
                    ref probabilityVector, sourceImageVoxels, ifsGenerator, randomGen);

                ChangeConfiguration(currentGenerationNumber);

                GenerateReportImages(currentGenerationNumber, population, drawer, ifsGenerator);              
            }

            return GetHighestFitIndividual(population);
        }

        /// <summary>
        /// Start the Evolution Process with given initial population (for testing purposes)
        /// </summary>
        public Individual StartEvolution(Population population, int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            //Initial Probability vector, 8 max
            var probabilityVector = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", probabilityVector)}]");

            var geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                population = EvolveGeneration(population, geneticUniversum, currentGenerationNumber,
                    ref probabilityVector, sourceImageVoxels, ifsGenerator, randomGen);

                GenerateReportImages(currentGenerationNumber, population, drawer, ifsGenerator);

                ChangeConfiguration(currentGenerationNumber);
            }

            return GetHighestFitIndividual(population);
        }

        #endregion
    }
}
