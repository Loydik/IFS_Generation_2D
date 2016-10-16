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

            allSignels.AddRange(firstParent.Singels);
            allSignels.AddRange(secondParent.Singels);

            allSignels.Shuffle();

            var firstChildSingels = allSignels.Take(firstParent.Degree).ToList();
            var secondChildSingels = allSignels.TakeLast(secondParent.Degree).ToList();

            var firstChild = new Individual(firstChildSingels);
            var secondChild = new Individual(secondChildSingels);

            return new List<Individual> {firstChild, secondChild};
        }
    }
}
