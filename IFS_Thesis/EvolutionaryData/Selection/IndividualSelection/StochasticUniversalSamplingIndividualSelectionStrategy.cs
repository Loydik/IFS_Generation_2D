using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    /// <summary>
    /// Stochastic universal sampling
    /// </summary>
    public class StochasticUniversalSamplingIndividualSelectionStrategy : IndividualSelectionStrategy
    {
        /// <summary>
        /// Gets individual based on pointer value
        /// </summary>
        private Individual GetIndividualBasedOnPointerValue(List<Individual> selectionPool, float pointerValue)
        {
            double partialSum = 0;

            foreach (var individual in selectionPool)
            {
                partialSum += individual.RankFitness;

                if (partialSum >= pointerValue)
                {
                    return individual;
                }
            }

            return null;
        }

        /// <summary>
        /// Select individuals using Stochastic Universal Sampling selection method
        /// </summary>
        public override List<Individual> SelectIndividuals(List<Individual> individualsForSelection, IRankingFitnessFunction rankingFitnessFunction, int count, float selectionPressure, Random randomGen)
        {
            var selectedIndividuals = new List<Individual>();

            var selectionPool = individualsForSelection.Clone().ToList();

            //First we assign a ranking fitness to individuals in the selection pool
            selectionPool = rankingFitnessFunction.AssignRankingFitnessToIndividuals(selectionPool,
                selectionPressure);
            
            //Then we order the selection pool, with highest fit individuals at the beginning
            selectionPool = selectionPool.OrderByDescending(i => i.RankFitness).ToList();
            
            //We calculate the total sum of fitnesses in the pool
            var totalFitnessesSum = selectionPool.Sum(i => i.RankFitness);

            //Size of the pointer
            var pointerDistance = totalFitnessesSum / count;

            //First pointer location at random
            var pointerValue = (float)randomGen.NextDouble() * pointerDistance;

            //Placing equally spaced pointers and selecting individuals
            for (int i = 0; i < count; i++)
            {
                selectedIndividuals.Add(GetIndividualBasedOnPointerValue(selectionPool, pointerValue));
                pointerValue = pointerValue + pointerDistance;
            }

            return selectedIndividuals;
        }
    }
}
