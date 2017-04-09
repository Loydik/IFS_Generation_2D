using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData
{
    public class EvolutionaryAlgorithm
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// List of Probability Vectors
        /// </summary>
        private List<float> ProbabilityVectors { get; set; }

        /// <summary>
        /// The current Population
        /// </summary>
        private Population _population;

        /// <summary>
        /// Writes to the log the configuration and initial conditions of the EA
        /// </summary>
        private void OutputEvolutinaryAlgorithmParameters()
        {
            Log.Info("WELCOME TO IFS GENERATION ALGORITHM!");
            Log.Info("Initial Parameters are as follows:");
            Log.Info($"Population Size: {Settings.Default.InitialPopulationSize}. Mutation Probability: {Settings.Default.MutationProbability}. Average Fitness Threshold: {Settings.Default.AverageFitnessThreshold}");
            Log.Info(
                $"Individual counts: N1 - {Settings.Default.N1IndividualsCount}; N2 - {Settings.Default.N2IndividualsCount}; N3 = {Settings.Default.N3IndividualsCount}; N4 = {Settings.Default.N4IndividualsCount}");
        }

        /// <summary>
        /// Start the Evolution Process
        /// </summary>
        public Individual Start(int maxGenerations, Bitmap sourceImage, IfsDrawer drawer)
        {
            OutputEvolutinaryAlgorithmParameters();

            _population = new Population();

            var initialPopulationSize = Properties.Settings.Default.InitialPopulationSize;

            //8 max
            ProbabilityVectors = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            Log.Info($"The Probability Vector values are: [{string.Join(",", ProbabilityVectors)}]");

            var random = new Random();

            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, initialPopulationSize, ProbabilityVectors, random);

            _population.AddIndividuals(initialIndividuals);

            var allIndividuals = _population.GetAllIndividuals();

            var sourceImagePixels = new ImageParser().GetMatchingPixels(sourceImage, Color.Black);

            allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImagePixels, sourceImage.Width, sourceImage.Height);

            _population.SetAllIndividuals(allIndividuals);

            Individual highestFitnessIndividual =
                _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                Log.Info($"Starting evolving generation {currentGenerationNumber}...");

                ProbabilityVectors = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(allIndividuals, ProbabilityVectors);

                //Generating New Population (Steps 7 - 11)

                _population = new GeneticOperators().GenerateNewPopulation(_population, ProbabilityVectors, random);

                allIndividuals = _population.GetAllIndividuals();

                allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImagePixels, sourceImage.Width, sourceImage.Height);

                _population.SetAllIndividuals(allIndividuals);

                //Step 12
                ProbabilityVectors = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(allIndividuals, ProbabilityVectors);

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


                highestFitnessIndividual =
                    _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

                Log.Info($"Highest fitness individual in the population is {highestFitnessIndividual}.\n");
                Log.Info($"Population size: {_population.Count}");

                //every 20th generation, save the highest fit individual as image
                if (currentGenerationNumber % 20 == 0)
                {
                    if (highestFitnessIndividual != null)
                    {
                        drawer.SaveIfsImage(highestFitnessIndividual.Singels, 512, 512,
                            Settings.Default.WorkingDirectory +
                            $"/highest_fit_{currentGenerationNumber}_generation.png");
                    }
                }
            }

            return highestFitnessIndividual;
        }
    }
}
