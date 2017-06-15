using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
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
        /// List of probabilites to generate IFS of a given degree
        /// </summary>
        private List<float> ProbabilityVector { get; set; }

        /// <summary>
        /// The current Population
        /// </summary>
        private Population _population;

        /// <summary>
        /// Best individuals per degree in a current population
        /// </summary>
        private List<Individual> BestIndividualsPerDegree => EaUtils.GetBestIndividualsOfEachDegree(_population, 1);

        /// <summary>
        /// Genetic operators
        /// </summary>
        private GeneticOperators _geneticOperators;

        /// <summary>
        /// Fitness function
        /// </summary>
        private IObjectiveFitnessFunction _objectiveFitnessFunction;

        /// <summary>
        /// Rank-based fitness function
        /// </summary>
        private IRankingFitnessFunction _rankBasedFitnessFunction;

        /// <summary>
        /// Reinsertion strategy
        /// </summary>
        private IReinsertionStrategy _reinsertionStrategy;

        /// <summary>
        /// Genetic Universum - initial set of singels
        /// </summary>
        private List<Singel> _geneticUniversum;

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes to the log the configuration and initial conditions of the EA
        /// </summary>
        private void OutputEvolutinaryAlgorithmParameters()
        {
            Log.Info("WELCOME TO IFS GENERATION ALGORITHM!");
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
            //Every 100th generation we turn on extreme debugging
            Settings.Default.ExtremeDebugging = currentGeneration % 100 == 0;

            if (currentGeneration == 2000)
            {
                Settings.Default.N3IndividualsPercentage = Settings.Default.N3IndividualsPercentage - 0.2f;
                Settings.Default.N1IndividualsPercentage = Settings.Default.N1IndividualsPercentage + 0.1f;
                Settings.Default.N4IndividualsPercentage = Settings.Default.N4IndividualsPercentage + 0.1f;
                Log.Info($"Changed the proportions of N individuals. Now they are N1:{Settings.Default.N1IndividualsPercentage} N2:{Settings.Default.N2IndividualsPercentage} N3:{Settings.Default.N3IndividualsPercentage} N4:{Settings.Default.N4IndividualsPercentage}");
            }

            //if (currentGeneration == 2000)
            //{
            //    Settings.Default.ControlledMutationProbability = 0.8f;
            //    Settings.Default.RandomMutationProbability = 0.2f;
            //    Log.Info($"Changed the value of ControlledMutationProbability to {Settings.Default.ControlledMutationProbability}");
            //}
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

                    ifsDrawer.SaveVoxelImage(path +
                        $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}", voxels, ImageFormat3D.Obj);
                    ifsDrawer.SaveVoxelImage(path +
                        $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}", voxels, ImageFormat3D.Stl);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the Evolution Process
        /// </summary>
        public Individual StartEvolution(int maxGenerations, HashSet<Voxel> sourceImageVoxels, IfsDrawer3D drawer, IfsGenerator ifsGenerator, Random randomGen)
        {
            OutputEvolutinaryAlgorithmParameters();

            _geneticOperators = new GeneticOperators();
            _population = new Population();

            _objectiveFitnessFunction = new WeightedPointsCoverageObjectiveFitnessFunction();
            Log.Info($"Objective fitness function is {_objectiveFitnessFunction.GetType()}");

            _rankBasedFitnessFunction = new LinearRankingFitnessFunction();
            Log.Info($"Rank based fitness function is {_rankBasedFitnessFunction.GetType()}");

            _reinsertionStrategy = new DegreeBasedReinsertionStrategy();
            Log.Info($"Reinsertion strategy is {_reinsertionStrategy.GetType()}");

            //Initial Probability vector, 8 max
            ProbabilityVector = new List<float> { 0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f };

            Log.Info($"The Probability Vector values are: [{string.Join(",", ProbabilityVector)}]");

            _geneticUniversum = _geneticOperators.GeneratePoolOfSingels(Settings.Default.InitialSingelPoolSize,
                randomGen);

            List<Individual> initialIndividuals;

            #region Generating 10x random individuals and taking the best

            if (Settings.Default.Generate10xIndividuals)
            {
                Log.Info("Started generation of 10x initial individuals");

                initialIndividuals = new GeneticOperators().CreateIndividualsFromExistingPoolOfSingels(
                    _geneticUniversum, Settings.Default.PopulationSize*10, ProbabilityVector, randomGen);

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
                   _geneticUniversum, Settings.Default.PopulationSize, ProbabilityVector, randomGen);
                initialIndividuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(initialIndividuals, sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
            }

            Log.Info("Calcualted fitness for generated individuals.");

            #endregion

            Log.Info($"Generated {initialIndividuals.Count} initial individuals from the Universum.");

            initialIndividuals = _rankBasedFitnessFunction.AssignRankingFitnessToIndividuals(initialIndividuals,
                Settings.Default.SelectionPressure);

            _population.AddIndividuals(initialIndividuals);

            Log.Info("Added generated individuals to the population.");

            var highestFitnessIndividual =
                _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

            Log.Info($"Highest fitness individual of generated ones is {highestFitnessIndividual}.\n");

            //Iterating for N Generations
            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                Log.Info($"Starting evolving generation {currentGenerationNumber}...");

                if (currentGenerationNumber > Settings.Default.UpdateProbabilityVectorAfterNGenerations)
                {
                    ProbabilityVector =
                        EaUtils.UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(BestIndividualsPerDegree,
                            ProbabilityVector);
                }

                //Generating New Population (Steps 7 - 11)

                var oldPopulation = _population;

                var newPopulation = _geneticOperators.GenerateNewPopulation(_population, _geneticUniversum, ProbabilityVector, randomGen);

                Log.Info("Generated new population");

                newPopulation.Individuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(newPopulation.Individuals, sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
                newPopulation.Individuals = _rankBasedFitnessFunction.AssignRankingFitnessToIndividuals(newPopulation.Individuals,
                Settings.Default.SelectionPressure);

                if (Settings.Default.UseReinsertion)
                {
                    //Reinserting individuals to population
                    _population = _reinsertionStrategy.ReinsertIndividuals(oldPopulation, newPopulation, randomGen);
                    _population.Individuals = _rankBasedFitnessFunction.AssignRankingFitnessToIndividuals(_population.Individuals,
                       Settings.Default.SelectionPressure);
                }

                else
                {
                    _population = newPopulation;
                }

                if (Settings.Default.RecalculateFitnessAfterReinsertion && Settings.Default.UseReinsertion)
                {
                    //recalculating fitness for whole population
                    _population.Individuals = _objectiveFitnessFunction.CalculateFitnessForIndividuals(_population.Individuals,
                        sourceImageVoxels, ifsGenerator, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
                    _population.Individuals = _rankBasedFitnessFunction.AssignRankingFitnessToIndividuals(_population.Individuals,
                       Settings.Default.SelectionPressure);
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
                    Log.Debug($"Best individuals per degree are:\n {string.Join("\n", BestIndividualsPerDegree)} \n\n");
                    Log.Debug($"Current whole population is:\n {string.Join("\n", _population.Individuals)}");
                }

                highestFitnessIndividual =
                    _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

                Log.Info($"Highest fitness individual in the population is {highestFitnessIndividual}.\n");
                Log.Info($"Average population fitness: {_population.Individuals.Average(x => x.ObjectiveFitness):#.#####}");
                Log.Info($"Population size: {_population.Count}");

                ChangeConfiguration(currentGenerationNumber);

                //every Nth generation, save the highest fit individual as image
                if (currentGenerationNumber % Settings.Default.DrawImageEveryNthGeneration == 0)
                {
                    var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
                    System.IO.Directory.CreateDirectory(folderPath);

                    if (Settings.Default.ExtremeDebugging)
                    {
                        foreach (var individual in BestIndividualsPerDegree)
                        {
                            GenerateReportImage(drawer, ifsGenerator, individual, currentGenerationNumber, folderPath);
                        }
                    }

                    else
                    {
                        GenerateReportImage(drawer, ifsGenerator, highestFitnessIndividual, currentGenerationNumber, folderPath);
                    }
                }
                
            }

            return highestFitnessIndividual;
        }

        #endregion
    }
}
