using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.Utils;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class ReasortmentStrategy : RecombinationStrategy
    {
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            List<IfsFunction> allSignels = new List<IfsFunction>();

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            allSignels.AddRange(firstParentClone.Singels);
            allSignels.AddRange(secondParentClone.Singels);

            allSignels.Shuffle(randomGen);

            var firstChildSingels = allSignels.Take(firstParentClone.Degree).ToList();
            var secondChildSingels = allSignels.TakeLast(secondParentClone.Degree).ToList();

            var firstChild = new Individual(firstChildSingels);
            var secondChild = new Individual(secondChildSingels);

            return new List<Individual> {firstChild, secondChild};
        }
    }
}
