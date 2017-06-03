using System;
using System.Collections.Generic;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;
using IFS_Thesis.Properties;
using log4net;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class OnePointCrossoverStrategy : RecombinationStrategy
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, firstParentClone.Degree - 1);

            if (Settings.Default.ExtremeDebugging)
            {
                Log.Debug($"Crossover point in one-point crossover is: {crossoverPoint}");
            }

            var firstSingels = new List<IfsFunction>();
            var secondSingels = new List<IfsFunction>();

            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                if (i >= crossoverPoint)
                {
                    firstSingels.Add(secondParentClone.Singels[i]);
                    secondSingels.Add(firstParentClone.Singels[i]);
                }
                else
                {
                    firstSingels.Add(firstParentClone.Singels[i]);
                    secondSingels.Add(secondParentClone.Singels[i]);
                }
            }

            var individual1 = new Individual(firstSingels);
            var individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
