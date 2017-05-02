using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.Population;

namespace IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection
{
    public abstract class SpeciesSelectionStrategy
    {
        public abstract Species SelectSpecies(Population.Population population, List<float> probabilityVector, Random randomGen);

        public abstract Species SelectSecondSpecies(Population.Population population, Species firstSpecies, int maximumDistance, Random randomGen);
    }
}
