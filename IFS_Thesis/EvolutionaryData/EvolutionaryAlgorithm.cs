using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        private List<float> VD { get; set; }
        private Population _population;


        public Individual Start(int maxGenerations, Bitmap sourceImage, IfsDrawer drawer)
        {
            _population = new Population();

            var initialPopulationSize = Properties.Settings.Default.InitialPopulationSize;

            //8 max
            VD = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            var random = new Random();

            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, initialPopulationSize, VD, random);

            _population.AddIndividuals(initialIndividuals);

            var allIndividuals = _population.GetAllIndividuals();

            allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

            //Maybe remove this?
            var badIndividuals = allIndividuals.Where(x => float.IsNaN(x.ObjectiveFitness)).ToList();

            allIndividuals.RemoveAll(x => badIndividuals.Contains(x));

            _population.SetAllIndividuals(allIndividuals);

            Individual highestFitnessIndividual =
                _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

            for (int i = 0; i < maxGenerations; i++)
            {
                var currentGenerationNumber = i + 1;

                Log.Info($"Starting evolving generation {currentGenerationNumber}...");

                VD = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnFitness(allIndividuals, VD);

                //Generating New Population (Steps 7 - 11)

                _population = new GeneticOperators().GenerateNewPopulation(_population, VD, random);

                allIndividuals = _population.GetAllIndividuals();

                allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

                badIndividuals = allIndividuals.Where(x => float.IsNaN(x.ObjectiveFitness)).ToList();

                allIndividuals.RemoveAll(x => badIndividuals.Contains(x));

                _population.SetAllIndividuals(allIndividuals);

                //Step 12
                VD = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnFitness(allIndividuals, VD);
                
                //Step 13
                //_population = new GeneticOperators().RemoveWeakestSpecies(_population,
                   // Properties.Settings.Default.AverageFitnessThreshold);

                //Step 14
                //_population = new GeneticOperators().RemoveSpeciesWithPopulationBelowTotal(_population, 0.05f);

                Log.Info($"Finished evolving generation {currentGenerationNumber}...");

                highestFitnessIndividual =
                    _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

                Log.Info($"Highest fitness individual in the population is {highestFitnessIndividual}.\n");
                Log.Info($"Population size: {_population.Count}");

                //every 50th generation, save the highest fit individual as image
                if (currentGenerationNumber%50 == 0)
                {
                    if (highestFitnessIndividual != null)
                    {
                        drawer.SaveIfsImage(highestFitnessIndividual.Singels, 512, 512,
                            Properties.Settings.Default.WorkingDirectory +
                            $"/highest_fit_{currentGenerationNumber}_generation.png");
                    }
                }
            }

            return highestFitnessIndividual;
        }
    }
}
