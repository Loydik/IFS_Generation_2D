using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.FitnessFunctions;

namespace IFS_Thesis.EvolutionaryData.Selection.IndividualSelection
{
    public abstract class IndividualSelectionStrategy
    {
        public abstract List<Individual> SelectIndividuals(List<Individual> individualsForSelection, IRankingFitnessFunction rankingFitnessFunction, int count, Random randomGen);
    }
}
