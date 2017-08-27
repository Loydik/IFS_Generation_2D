using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;
using IFS_Thesis.Utils;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class ReasortmentStrategy : RecombinationStrategy
    {
        /// <summary>
        /// Produces offspring using reasortment operator
        /// </summary>
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null)
            {
                return new List<Individual>();
            }

            var allSignels = new List<IfsFunction>();

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            allSignels.AddRange(firstParentClone.Singels);
            allSignels.AddRange(secondParentClone.Singels);

            allSignels.Shuffle(randomGen);

            var firstChildSingels = allSignels.Take(firstParentClone.Degree).ToList();
            var secondChildSingels = allSignels.TakeLast(secondParentClone.Degree).ToList();

            return new List<Individual> { new Individual(firstChildSingels), new Individual(secondChildSingels) };
        }
    }
}
