using System;
using System.Collections.Generic;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public abstract class IndividualSelectionStrategy
    {
        public abstract List<Individual> SelectIndividuals(List<Individual> individualsForSelection, int count, Random randomGen);
    }
}
