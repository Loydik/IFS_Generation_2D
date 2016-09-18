using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData
{
    public class EvolutionaryAlgorithm
    { 

        private List<Species> Species;
        private List<float> VD { get; set; }

        private Population _population;

        //private List<Singel> PoolOfSingels; 



        public Individual Start(int maxGenerations, Bitmap sourceImage)
        {
            _population = new Population();

            var initialPopulationSize = 100;

            List<Singel> initialPoolOfSingels;

            //8 max
            VD = new List<float> {0, 0, 0.35f, 0.25f, 0.2f, 0.1f, 0.07f, 0.03f};

            var random = new Random();

            //for (int i = 0; i < maxGenerations; i++)
            //{

            initialPoolOfSingels = GenerateSingels(500, random);

            for (int i = 0; i < initialPopulationSize; i++)
            {
                var individual = new GeneticOperators().CreateIndividual(VD, initialPoolOfSingels, random);

                _population.AddIndividual(individual);
            }

            var allIndividuals = _population.GetAllIndividuals();

            allIndividuals = new FitnessFunction().CalculateFitnessForIndividuals(allIndividuals, sourceImage);

            var badIndividuals = allIndividuals.Where(x => float.IsNaN(x.CurrentFintess)).ToList();

            var goodIndividuals = allIndividuals.Where(x => !float.IsNaN(x.CurrentFintess)).ToList();

            var highestFitnessIndividual = goodIndividuals.OrderByDescending(x => x.CurrentFintess).FirstOrDefault();


            return highestFitnessIndividual;
            //}
        }

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
    }
}
