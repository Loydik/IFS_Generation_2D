using System;
using System.Collections.Generic;

namespace IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection
{
    public abstract class SpeciesSelectionStrategy
    {
        public abstract Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen);

        public abstract Species SelectSecondSpecies(Population population, Species firstSpecies, int maximumDistance, Random randomGen);
    }
}
