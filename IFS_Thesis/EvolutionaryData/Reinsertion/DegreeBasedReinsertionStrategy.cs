using System;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Selection.IndividualSelection;
using IFS_Thesis.Properties;
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
        public Population ReinsertIndividuals(Population previousGeneration, Population newGeneration, Random randomGen)
        {
            IndividualSelectionStrategy strategy = new TruncationIndividualSelectionStrategy();

            var finalPopulation = new Population();

            var degreesOfIndividuals = EaUtils.GetDegreesOfIndividuals(newGeneration.Individuals);

            Log.Debug("Started Reinsertion process");

            //Each subpopulation
            foreach (var degree in degreesOfIndividuals)
            {
                var individualsCount = newGeneration.Individuals.Count(x => x.Degree == degree);

                var oldSubpopulation = previousGeneration.Individuals.Where(x => x.Degree == degree).ToList();
                var newSubpopulation = newGeneration.Individuals.Where(x => x.Degree == degree).ToList();

                var bestOffspring = strategy.SelectIndividuals(newSubpopulation, individualsCount / 2, randomGen);
                var bestParents = strategy.SelectIndividuals(oldSubpopulation, individualsCount / 2, randomGen);

                finalPopulation.AddIndividuals(bestParents);
                finalPopulation.AddIndividuals(bestOffspring);

                if (Settings.Default.ExtremeDebugging)
                {
                    Log.Debug($"Best parents for degree {degree} are: \n {string.Join("\n", bestParents)} ");
                    Log.Debug($"Best offspring for degree {degree} are: \n {string.Join("\n", bestOffspring)} ");
                }

                Log.Debug($"Reinserted {individualsCount} individuals of degree {degree} to population");
            }

            return finalPopulation;
        }
    }
}
