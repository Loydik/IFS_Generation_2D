using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    /// <summary>
    /// Discrete Singel Recombination strategy
    /// </summary>
    public class DiscreteSingelRecombinationStrategy : RecombinationStrategy
    {
        #region Overriden Members

        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            var firstSingels = new List<IfsFunction>();
            var secondSingels = new List<IfsFunction>();

            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                firstSingels.Add(randomGen.NextDouble() >= 0.5
                    ? firstParentClone.Singels[i]
                    : secondParentClone.Singels[i]);

                secondSingels.Add(randomGen.NextDouble() >= 0.5
                    ? firstParentClone.Singels[i]
                    : secondParentClone.Singels[i]);
            }

            var individual1 = new Individual(firstSingels);
            var individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }

        #endregion
    }
}
