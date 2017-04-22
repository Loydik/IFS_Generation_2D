using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData
{
    /// <summary>
    /// Class which represents the Evolutionary Algorithm
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
        /// Pixels of the source image
        /// </summary>
        private List<Point> _sourceImagePixels;

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
        private List<Individual> BestIndividualsPerDegree => OtherUtils.GetBestIndividualsOfEachDegree(_population, 1);

        /// <summary>
        /// Random number generator for the whole evolutionary run
        /// </summary>
        private Random _randomNumberGenerator;

        /// <summary>
        /// Genetic operators
        /// </summary>
        private GeneticOperators _geneticOperators;

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes to the log the configuration and initial conditions of the EA
        /// </summary>
        private void OutputEvolutinaryAlgorithmParameters()
        {
            Log.Info("WELCOME TO IFS GENERATION ALGORITHM!");
            Log.Info("Initial Parameters are as follows:");
            Log.Info($"Population Size: {Settings.Default.InitialPopulationSize}. Initial Singels Pool Size: {Settings.Default.InitialSingelPoolSize}. Mutation Probability: {Settings.Default.MutationProbability}. Average Fitness Threshold: {Settings.Default.AverageFitnessThreshold}");
            Log.Info(
                $"Individual counts: N1 - {Settings.Default.N1IndividualsCount}; N2 - {Settings.Default.N2IndividualsCount}; N3 = {Settings.Default.N3IndividualsCount}; N4 = {Settings.Default.N4IndividualsCount}");
            Log.Info(
                $"Draw Points Multiplier - {Settings.Default.DrawPointsMultiplier} Prc Fitness - {Settings.Default.PrcFitness} Pro Fitness - {Settings.Default.ProFitness}");
        }

        /// <summary>
        /// Changes configuration of parameters based on the current generation
        /// </summary>
        private void ChangeConfiguration(int currentGeneration)
        {
            //On 1000th generation, decrease the value of pro to refine points coverage
            if (currentGeneration == 1000)
            {
                Settings.Default.ProFitness = 3;
                Log.Info($"Changed the value of Pro fitness to {Settings.Default.ProFitness}");
            }

            if (currentGeneration == 1500)
            {
                Settings.Default.ControlledMutationProbability = 0.7f;
                Settings.Default.RandomMutationProbability = 0.3f;
                Log.Info($"Changed the value of ControlledMutationProbability to {Settings.Default.ControlledMutationProbability}");
            }
        }

        /// <summary>
        /// Generates a report image based on current generation
        /// </summary>
        private void GenerateReportImage(IfsDrawer ifsDrawer, Individual individual, int currentGenerationNumber, string path)
        {
            //every Nth generation, save the highest fit individual as image
            if (currentGenerationNumber % Settings.Default.DrawImageEveryNthGeneration == 0)
            {
                if (individual != null)
                {
                    ifsDrawer.SaveIfsImage(individual.Singels, Settings.Default.ImageX, Settings.Default.ImageY,
                        path +
                        $"/best_{currentGenerationNumber}th_gen_degree_{individual.Degree}_fitness_{individual.ObjectiveFitness:##.#######}.png");
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the Evolution Process
        /// </summary>
        public Individual Start(int maxGenerations, Bitmap sourceImage, IfsDrawer drawer)
        {
            OutputEvolutinaryAlgorithmParameters();

            _geneticOperators = new GeneticOperators();
            _population = new Population();

            //Initial Probability vector, 8 max
            ProbabilityVector = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            Log.Info($"The Probability Vector values are: [{string.Join(",", ProbabilityVector)}]");

            _randomNumberGenerator = new Random();

            var initialIndividuals = new GeneticOperators().CreateIndividuals(Settings.Default.InitialSingelPoolSize, Settings.Default.InitialPopulationSize, ProbabilityVector, _randomNumberGenerator);

            #region Test

            //float[] ifs1 = { 0.0613f, -0.6102f, -0.501f, -0.0842f, 0.3372f, 0.0588f, 0 };
            //float[] ifs2 = { 0.061f, -0.6101f, -0.501f, -0.0844f, 0.337f, 0.0588f, 0 };
            //float[] ifs3 = { 0.0608f, -0.61f, -0.501f, -0.0842f, 0.3372f, 0.0588f, 0 };
            //float[] ifs4 = { 0.0608f, -0.61f, -0.501f, -0.0843f, 0.337f, 0.0588f, 0 };
            //float[] ifs5 = { 0.061f, -0.6101262f, -0.501f, -0.0843f, 0.3371f, 0.0588f, 0 };
            //float[] ifs6 = { 0.0611f, -0.6102f, -0.501f, -0.0842f, 0.3372f, 0.0588f, 0 };

            //var testIndividual = new Individual(new List<IfsFunction> { new IfsFunction(ifs1), new IfsFunction(ifs2), new IfsFunction(ifs3), new IfsFunction(ifs4), new IfsFunction(ifs5), new IfsFunction(ifs6) });

            //var initialIndividuals = new List<Individual> { testIndividual };

            #endregion

            _population.AddIndividuals(initialIndividuals);

            //we get pixels from a source image 
            _sourceImagePixels = new ImageParser().GetMatchingPixels(sourceImage, Color.Black);

            _population.Individuals = new FitnessFunction().CalculateFitnessForIndividuals(_population.Individuals, _sourceImagePixels, sourceImage.Width, sourceImage.Height);

            Individual highestFitnessIndividual =
                _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

            //Iterating for N Generations
            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                Log.Info($"Starting evolving generation {currentGenerationNumber}...");

                ProbabilityVector = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(BestIndividualsPerDegree, ProbabilityVector);

                //Generating New Population (Steps 7 - 11)

                _population = _geneticOperators.GenerateNewPopulation(_population, ProbabilityVector, _randomNumberGenerator);

                _population.Individuals = new FitnessFunction().CalculateFitnessForIndividuals(_population.Individuals, _sourceImagePixels, sourceImage.Width, sourceImage.Height);

                //Step 12
                ProbabilityVector = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(BestIndividualsPerDegree, ProbabilityVector);

                var totalPopulationCount = _population.Count;

                //Removing and adding species
                int speciesCountBefore = _population.Species.Count;

                //Step 13
                _population = new GeneticOperators().RemoveWeakestSpecies(_population,
                    Settings.Default.AverageFitnessThreshold);

                //Step 14
                _population = new GeneticOperators().RemoveSpeciesWithPopulationBelowTotal(_population, totalPopulationCount, 0.05f);

                //Step 15
                if (speciesCountBefore < _population.Species.Count)
                {
                    //TODO - Implement creation of new species
                }

                Log.Info($"Finished evolving generation {currentGenerationNumber}...");

                if (Settings.Default.ExtremeDebugging)
                {
                    Log.Debug($"Best individuals per degree are:\n {string.Join("\n", BestIndividualsPerDegree)} \n\n");
                    Log.Debug($"Current whole population is:\n {string.Join("\n", _population.Individuals)}");
                }

                highestFitnessIndividual =
                    _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

                Log.Info($"Highest fitness individual in the population is {highestFitnessIndividual}.\n");
                Log.Info($"Population size: {_population.Count}");

                ChangeConfiguration(currentGenerationNumber);

                if (Settings.Default.ExtremeDebugging)
                {
                    var folderPath = Settings.Default.WorkingDirectory + $"/best_gen_{currentGenerationNumber}";
                    System.IO.Directory.CreateDirectory(folderPath);

                    foreach (var individual in BestIndividualsPerDegree)
                    {
                        GenerateReportImage(drawer, individual, currentGenerationNumber, folderPath);
                    }
                }

               GenerateReportImage(drawer, highestFitnessIndividual, currentGenerationNumber, Settings.Default.WorkingDirectory); 
            }

            return highestFitnessIndividual;
        }

        #endregion
    }
}
