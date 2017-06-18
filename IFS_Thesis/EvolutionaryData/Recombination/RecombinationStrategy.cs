using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public abstract class RecombinationStrategy
    {
        public abstract List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen);
    }
}
