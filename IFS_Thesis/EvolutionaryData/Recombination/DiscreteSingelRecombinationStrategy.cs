using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    /// <summary>
    /// Discrete Singel Recombination strategy
    /// </summary>
    public class DiscreteSingelRecombinationStrategy : RecombinationStrategy
    {
        #region Overriden Members

        /// <summary>
        /// Produces offspring using discrete singel recombination operator
        /// </summary>
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            var firstChildSingels = new List<IfsFunction>();
            var secondChildSingels = new List<IfsFunction>();

            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                //determining which of the parents will contribute a singel to a child
                firstChildSingels.Add(randomGen.NextDouble() >= 0.5
                    ? firstParentClone.Singels[i]
                    : secondParentClone.Singels[i]);

                secondChildSingels.Add(randomGen.NextDouble() >= 0.5
                    ? firstParentClone.Singels[i]
                    : secondParentClone.Singels[i]);
            }

            return new List<Individual> { new Individual(firstChildSingels), new Individual(secondChildSingels) };
        }

        #endregion
    }
}
