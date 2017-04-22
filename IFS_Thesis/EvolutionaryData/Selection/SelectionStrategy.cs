using System;
using System.Collections.Generic;

namespace IFS_Thesis.EvolutionaryData.Selection
{
    public abstract class SelectionStrategy
    {
        public abstract List<Individual> SelectIndividuals(List<Individual> individualsForSelection, int count, Random randomGen);

        public abstract Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen);

        public abstract Species SelectSecondSpecies(Population population, Species firstSpecies, int maximumDistance, Random randomGen);
    }
}
