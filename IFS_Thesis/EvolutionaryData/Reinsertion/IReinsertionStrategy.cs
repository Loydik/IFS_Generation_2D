using System;

namespace IFS_Thesis.EvolutionaryData.Reinsertion
{
    public interface IReinsertionStrategy
    {
        /// <summary>
        /// Reinsert individuals into population after recombination
        /// </summary>
        /// <param name="oldPopulation">Old population of individuals</param>
        /// <param name="newPopulation">New population of individuals</param>
        /// <param name="randomGen">Random number generator</param>
        /// <returns></returns>
        Population ReinsertIndividuals(Population oldPopulation, Population newPopulation, Random randomGen);
    }
}
