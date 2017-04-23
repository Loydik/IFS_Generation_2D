using System;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.Selection.IndividualSelection;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData.Reinsertion
{
    /// <summary>
    /// Reinsertion strategy based on elitist and fitness-based reinsertion
    /// </summary>
    public class DegreeBasedReinsertionStrategy : IReinsertionStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Reinserts individuals to a population
        /// </summary>
        public Population ReinsertIndividuals(Population oldPopulation, Population newPopulation, Random randomGen)
        {
            IndividualSelectionStrategy strategy = new TruncationIndividualSelectionStrategy();

            var finalPopulation = new Population();

            var degreesOfIndividuals = EaUtils.GetDegreesOfIndividuals(newPopulation.Individuals);

            //Each subpopulation
            foreach (var degree in degreesOfIndividuals)
            {
                var individualsCount = newPopulation.Individuals.Count(x => x.Degree == degree);

                var oldSubpopulation = oldPopulation.Individuals.Where(x => x.Degree == degree).ToList();
                var newSubpopulation = newPopulation.Individuals.Where(x => x.Degree == degree).ToList();

                var bestOffspring = strategy.SelectIndividuals(newSubpopulation, individualsCount / 2, randomGen);
                var bestParents = strategy.SelectIndividuals(oldSubpopulation, individualsCount / 2, randomGen);

                finalPopulation.AddIndividuals(bestParents);
                finalPopulation.AddIndividuals(bestOffspring);

                Log.Debug($"Reinserted {individualsCount} individuals of degree {degree} to population");
            }

            return finalPopulation;
        }
    }
}
