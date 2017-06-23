using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    /// <summary>
    /// Stochastic universal sampling
    /// </summary>
    public class StochasticUniversalSamplingIndividualSelectionStrategy : IndividualSelectionStrategy
    {
        /// <summary>
        /// Gets individual based on selection value
        /// </summary>
        private Individual GetIndividualBasedOnSelectionValue(List<Individual> selectionPool, float selectionValue)
        {
            double partialSum = 0;

            foreach (var individual in selectionPool)
            {
                partialSum += individual.RankFitness;

                if (partialSum >= selectionValue)
                {
                    return individual;
                }
            }

            return null;
        }

        public override List<Individual> SelectIndividuals(List<Individual> individualsForSelection, IRankingFitnessFunction rankingFitnessFunction, int count, Random randomGen)
        {
            var selectedIndividuals = new List<Individual>();

            var selectionPool = individualsForSelection.Clone().ToList();

            selectionPool = rankingFitnessFunction.AssignRankingFitnessToIndividuals(selectionPool,
                Settings.Default.SelectionPressure);

            selectionPool = selectionPool.OrderByDescending(i => i.RankFitness).ToList();

            var totalRank = selectionPool.Sum(i => i.RankFitness);

            var distance = totalRank / count;

            var selectionValue = (float)randomGen.NextDouble() * distance;

            for (int i = 0; i < count; i++)
            {
                selectedIndividuals.Add(GetIndividualBasedOnSelectionValue(selectionPool, selectionValue));
                selectionValue = selectionValue + distance;
            }

            return selectedIndividuals;
        }
    }
}
