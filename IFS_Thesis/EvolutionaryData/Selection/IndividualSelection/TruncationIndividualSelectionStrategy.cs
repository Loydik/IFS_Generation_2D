using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public class TruncationIndividualSelectionStrategy : IndividualSelectionStrategy
    {
        public override List<Individual> SelectIndividuals(List<Individual> individualsForSelection, IRankingFitnessFunction rankingFitnessFunction, int count, float selectionPressure, Random randomGen)
        {
            var individualsToSelect = individualsForSelection.Clone().ToList();

            individualsToSelect = rankingFitnessFunction.AssignRankingFitnessToIndividuals(individualsToSelect,
                selectionPressure);

            individualsToSelect = individualsToSelect.OrderByDescending(i => i.RankFitness).ToList();
            
            //taking the best individuals with highest fitness
            return individualsToSelect.Take(count).ToList();
        }
    }
}
