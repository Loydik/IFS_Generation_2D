using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public class TruncationIndividualSelectionStrategy : IndividualSelectionStrategy
    {
        public override List<Individual> SelectIndividuals(List<Individual> individualsForSelection, int count, Random randomGen)
        {
            var individualsToSelect = individualsForSelection.Clone();

            individualsToSelect = individualsToSelect.OrderByDescending(i => i.ObjectiveFitness).ToList();
            
            //taking the best individuals with highest fitness
            return individualsToSelect.Take(count).ToList();
        }
    }
}
