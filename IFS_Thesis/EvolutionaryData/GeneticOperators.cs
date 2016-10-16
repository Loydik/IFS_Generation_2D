using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFS_Thesis.EvolutionaryData.Recombination;
using IFS_Thesis.EvolutionaryData.Selection;
using IFS_Thesis.Utils;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData
{
    public class GeneticOperators
    {
        public Singel CreateRandomSingel(Random random)
        {
            var a = GetRandomCoefficient(random);
            var b = GetRandomCoefficient(random);
            var c = GetRandomCoefficient(random);
            var d = GetRandomCoefficient(random);
            var e = GetRandomCoefficient(random);
            var f = GetRandomCoefficient(random);

            var coefficients = new IfsFunction(a,b,c,d,e,f,0f);

            var singel = new Singel(coefficients);

            return singel;
        }


        public Individual CreateIndividual(List<float> probabilityVectors, List<Singel> poolOfSingels, Random randomGen)
        {
            var degreeOfIndividual = GetIndividualsDegreeBasedOnVectors(probabilityVectors, randomGen);

            poolOfSingels.Shuffle();

            var individual = new Individual(poolOfSingels.Select(x => x.Coefficients).Take(degreeOfIndividual).ToList())
            {
                Degree = degreeOfIndividual
            };


            return individual;
        }

        private float GetRandomCoefficient(Random random)
        {
            //var randomGen = new Random();

            var value = random.NextDouble();

            float coefficient = (float) Math.Round(value, 3);

            return coefficient;

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

        public List<Individual> CreateIndividuals(int singelsPoolSize, int populationSize, List<float> probablityVectors, Random randomGen)
        {
            var generatedIndividuals = new List<Individual>();

            var initialPoolOfSingels = GenerateSingels(singelsPoolSize, randomGen);

            for (int i = 0; i < populationSize; i++)
            {
                var individual = new GeneticOperators().CreateIndividual(probablityVectors, initialPoolOfSingels, randomGen);

                generatedIndividuals.Add(individual);
            }

            return generatedIndividuals;
        }

        public Population GenerateNewPopulation(Population population, List<float> probabilityVectors,  float fitnessThreshold, Random randomGen)
        {
            SelectionStrategy selectionStrategy = new RouletteWheelSelectionStrategy();
            RecombinationStrategy recombinationStrategy = new OnePointCrossoverStrategy();

            var newPopulation = new Population();

            var degrees = OtherUtils.GetDegreesOfIndividuals(population.GetAllIndividuals());

            //Adding best individuals of each new degree (Step 6.)
            foreach (var degree in degrees)
            {
                var best = population.GetAllIndividuals().Where(x => x.Degree == degree).MaxBy(x => x.ObjectiveFitness);
                newPopulation.AddIndividual(best);
                population.RemoveIndividual(best);
            }

            //Step 7
            var selectedSpecies = selectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);
            var selectedIndividuals = selectionStrategy.SelectIndividuals(population.GetAllIndividuals().Where(x => x.Degree.Equals(selectedSpecies.DegreeOfIndividualsInSpecies)).ToList(), 2, randomGen);
            
            //one-point crossover for now
            var offspring = recombinationStrategy.ProduceOffsprings(selectedIndividuals[0], selectedIndividuals[1],
                randomGen);
            newPopulation.AddIndividuals(offspring);

            //Step 8
            var count = Properties.Settings.Default.N2IndividualsCount;
            var generatedIndividuals = CreateIndividuals(100, count, probabilityVectors, randomGen);
            newPopulation.AddIndividuals(generatedIndividuals);

            //Step 9
            count = Properties.Settings.Default.N3IndividualsCount;

            for (int i = 0; i <= count/2; i++)
            {
                var firstSpecies = selectionStrategy.SelectSpecies(population, probabilityVectors, randomGen);
                var secondSpecies = selectionStrategy.SelectSecondSpecies(population, firstSpecies, 1, randomGen);
                var firstIndividual = selectionStrategy.SelectIndividuals(firstSpecies.Individuals, 1, randomGen).First();
                var secondIndividual = selectionStrategy.SelectIndividuals(secondSpecies.Individuals, 1, randomGen).First();

                recombinationStrategy = new InterSpeciesCrossoverStrategy();

                var children = recombinationStrategy.ProduceOffsprings(firstIndividual, secondIndividual, randomGen);
                newPopulation.AddIndividuals(children);
            }


            //Step 10
            count = Properties.Settings.Default.N4IndividualsCount;

            for (int i = 0; i <= count / 2; i++)
            {
                var parents = selectionStrategy.SelectIndividuals(population.GetAllIndividuals(), 2, randomGen);

                recombinationStrategy = new ReasortmentStrategy();

                var children = recombinationStrategy.ProduceOffsprings(parents[0], parents[1], randomGen);
                newPopulation.AddIndividuals(children);
            }

            return newPopulation;
        }
    }
}
