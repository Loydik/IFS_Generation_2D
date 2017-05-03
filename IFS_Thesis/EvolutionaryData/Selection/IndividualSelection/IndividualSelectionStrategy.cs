using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public abstract class IndividualSelectionStrategy
    {
        public abstract List<Individual> SelectIndividuals(List<Individual> individualsForSelection, int count, Random randomGen);
    }
}
