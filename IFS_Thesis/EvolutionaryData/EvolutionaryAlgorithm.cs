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

            List<Singel> initialPoolOfSingels;

            //8 max
            VD = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            var random = new Random();

            //for (int i = 0; i < maxGenerations; i++)
            //{

            var initialIndividuals = new GeneticOperators().CreateIndividuals(500, initialPopulationSize, VD, random);

            _population.AddIndividuals(initialIndividuals);

            //initialPoolOfSingels = GenerateSingels(500, random);

            //for (int i = 0; i < initialPopulationSize; i++)
            //{
            //    var individual = new GeneticOperators().CreateIndividual(VD, initialPoolOfSingels, random);

            //    _population.AddIndividual(individual);
            //}

            var allIndividuals = _population.GetAllIndividuals();

            allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

            var badIndividuals = allIndividuals.Where(x => float.IsNaN(x.ObjectiveFitness)).ToList();

            allIndividuals.RemoveAll(x => badIndividuals.Contains(x));

            _population.SetAllIndividuals(allIndividuals);

            VD = new FitnessFunction().UpdateVectorOfProbabilitiesBasedOnFitness(allIndividuals, VD);

            var highestFitnessIndividual = allIndividuals.OrderByDescending(x => x.ObjectiveFitness).FirstOrDefault();


            _population = new GeneticOperators().GenerateNewPopulation(_population, VD, _fitnessThreshold, random);

            var members = _population.GetAllIndividuals();

            return highestFitnessIndividual;
            //}
        }
    }
}
