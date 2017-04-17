using System;
using System.Collections.Generic;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public abstract class RecombinationStrategy
    {
        public abstract List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen);
    }
}
