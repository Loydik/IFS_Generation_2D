using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public class RouletteWheelIndividualSelectionStrategy : IndividualSelectionStrategy
    {
        #region Private Methods

        /// <summary>
        /// Selects individual based on roulette selection method
        /// </summary>
        private Individual RouletteSelect(List<Individual> selectionPool, Random randomGen)
        {
            //total sum of fitnesses
            double weightSum = selectionPool.Aggregate(0f, (current, element) => current + element.RankFitness);

            // get a random value
            double randomValue = randomGen.NextDouble()* weightSum;

            double partialSum = 0;

            foreach (var individual in selectionPool)
            {
                partialSum += individual.RankFitness;

                if (partialSum >= randomValue)
                {
                    return individual;
                }
            }

            return null;
        }

        #endregion

        #region Overriden Members

        /// <summary>
        /// Private selection using Roulette Select
        /// </summary>
        public override List<Individual> SelectIndividuals(List<Individual> individualsForSelection, int numberOfIndividualsToSelect, Random randomGen)
        {
            List<Individual> selectedIndividuals = new List<Individual>();

            var selectionPool = individualsForSelection.Clone().ToList();

            selectionPool = selectionPool.OrderByDescending(i => i.RankFitness).ToList();

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

        #endregion
    }
}
