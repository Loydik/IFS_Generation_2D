using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData.Selection
{
    public class RouletteWheelSelectionStrategy : SelectionStrategy
    {
        /// <summary>
        /// Selects individual based on roulette selection method
        /// </summary>
        private Individual RouletteSelect(List<Individual> selectionPool, Random randomGen)
        {
            //total sum of fitnesses
            double weight_sum = selectionPool.Aggregate(0f, (current, element) => current + element.ObjectiveFitness);

            // get a random value
            double randomValue = randomGen.NextDouble()* weight_sum;

            double partialSum = 0;

            foreach (var individual in selectionPool)
            {
                partialSum += individual.ObjectiveFitness;

                if (partialSum >= randomValue)
                {
                    return individual;
                }
            }

            return null;
        }

        /// <summary>
        /// Private selection using Roulette Select
        /// </summary>
        public override List<Individual> SelectIndividuals(List<Individual> selectionPool, int numberOfIndividualsToSelect, Random randomGen)
        {
            List<Individual> selectedIndividuals = new List<Individual>();

            for (int i = 0; i < numberOfIndividualsToSelect; i++)
            {
                var selectedIndividual = RouletteSelect(selectionPool, randomGen);

                selectedIndividuals.Add(selectedIndividual);

                //TODO - Add a preference
                if(selectionPool.Count > 1)
                { 
                selectionPool.Remove(selectedIndividual);
                }
            }

            return selectedIndividuals;
        }

        /// <summary>
        /// Selects species based on probability vector
        /// </summary>
        /// <returns></returns>
        public override Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen)
        {
            double randomValue = randomGen.NextDouble();

            double partialSum = 0;

            for (int i = 0; i < probabilityVector.Count; i++)
            {
                partialSum += probabilityVector[i];

                if (partialSum >= randomValue)
                {
                    var degree = i + 1;

                    var selectedSpecies = population.Species.Single(x => x.DegreeOfIndividualsInSpecies.Equals(degree));

                    return selectedSpecies;
                }
            }

            return null;
        }

        /// <summary>
        /// Selects two species according to VD and maximum distance between species 
        /// </summary>
        public override Species SelectSecondSpecies(Population population, Species firstSpecies, int maximumDistance, Random randomGen)
        {
            var firstSpeciesDegree = firstSpecies.DegreeOfIndividualsInSpecies;

            var possibleMatches =
                population.Species.Where(
                    x =>
                        x.DegreeOfIndividualsInSpecies < firstSpeciesDegree &&
                        x.DegreeOfIndividualsInSpecies >= firstSpeciesDegree - maximumDistance ||
                        x.DegreeOfIndividualsInSpecies <= firstSpeciesDegree + maximumDistance &&
                        x.DegreeOfIndividualsInSpecies > firstSpeciesDegree).ToList();

            var randomIndex = randomGen.Next(0, possibleMatches.Count - 1);

            var secondSpecies = possibleMatches[randomIndex];

            return secondSpecies;
        }
    }
}
