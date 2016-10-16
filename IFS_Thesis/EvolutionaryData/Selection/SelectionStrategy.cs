using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData.Selection
{
    public abstract class SelectionStrategy
    {
        public abstract List<Individual> SelectIndividuals(List<Individual> selectionPool, int count, Random randomGen);

        public abstract Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen);

        public abstract Species SelectSecondSpecies(Population population, Species firstSpecies, int maximumDistance, Random randomGen);
    }
}
