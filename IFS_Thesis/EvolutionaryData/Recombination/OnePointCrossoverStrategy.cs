using System;
using System.Collections.Generic;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;
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

        /// <summary>
        /// Produces offsprings using one-point crossover operator
        /// </summary>
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            //individuals must be of the same degree
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, firstParentClone.Degree - 1);

            var firstChildSingels = new List<IfsFunction>();
            var secondChildSingels = new List<IfsFunction>();

            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                if (i >= crossoverPoint)
                {
                    firstChildSingels.Add(secondParentClone.Singels[i]);
                    secondChildSingels.Add(firstParentClone.Singels[i]);
                }
                else
                {
                    firstChildSingels.Add(firstParentClone.Singels[i]);
                    secondChildSingels.Add(secondParentClone.Singels[i]);
                }
            }

            return new List<Individual> { new Individual(firstChildSingels), new Individual(secondChildSingels) };
        }
    }
}
