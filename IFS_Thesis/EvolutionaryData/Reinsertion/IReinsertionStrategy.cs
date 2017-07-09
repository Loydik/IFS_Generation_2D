using System;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.Reinsertion
{
    public interface IReinsertionStrategy
    {
        /// <summary>
        /// Reinsert individuals into population after recombination
        /// </summary>
        /// <param name="previousGeneration">Old population of individuals</param>
        /// <param name="newGeneration">New population of individuals</param>
        /// <param name="offspringReinsertedPercentage">Percentage of offspring reinserted into population</param>
        /// <param name="randomGen">Random number generator</param>
        /// <param name="parentsReinsertedPercentage">Percentage of parents to be reinserted into population</param>
        /// <returns></returns>
        Population ReinsertIndividuals(Population previousGeneration, Population newGeneration, float parentsReinsertedPercentage, float offspringReinsertedPercentage, Random randomGen);
    }
}
