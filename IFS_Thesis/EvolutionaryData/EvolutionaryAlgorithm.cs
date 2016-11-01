using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace IFS_Thesis.EvolutionaryData
{
    public class EvolutionaryAlgorithm
    { 

        private List<Species> Species;
        private List<float> VD { get; set; }

        private Population _population;

        private float _fitnessThreshold = 0.05f;

        //private List<Singel> PoolOfSingels; 


        public Individual Start(int maxGenerations, Bitmap sourceImage)
        { 
            _population = new Population();

            var initialPopulationSize = Properties.Settings.Default.InitialPopulationSize;

            //8 max
            VD = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            var random = new Random();

            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, initialPopulationSize, VD, random);

            _population.AddIndividuals(initialIndividuals);

            Individual highestFitnessIndividual;

            for (int i = 0; i < maxGenerations; i++)
            {
            
            Console.WriteLine($"Starting evolving generation {i + 1}...");

            var allIndividuals = _population.GetAllIndividuals();

            allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

            var badIndividuals = allIndividuals.Where(x => float.IsNaN(x.ObjectiveFitness)).ToList();

            allIndividuals.RemoveAll(x => badIndividuals.Contains(x));

            _population.SetAllIndividuals(allIndividuals);

            VD = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnFitness(allIndividuals, VD);          

            _population = new GeneticOperators().GenerateNewPopulation(_population, VD, _fitnessThreshold, random);

                //allIndividuals = _population.GetAllIndividuals();

                //allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

                //Step 12
                //VD = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnFitness(allIndividuals, VD);

                Console.WriteLine($"Finished evolving generation {i + 1}...");

                highestFitnessIndividual = _population.Individuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

                new IfsDrawer().SaveIfsImage(highestFitnessIndividual.Singels, 512, 512, "C:/Users/Loydik94/Desktop/IFS Images/new/highest_fitness.png");
            }

            var resultingIndividuals = _population.GetAllIndividuals();

            resultingIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(resultingIndividuals, sourceImage);

            highestFitnessIndividual = resultingIndividuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();

            return highestFitnessIndividual;
        }
    }
}
