using System;
using System.Collections.Generic;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class OnePointCrossoverStrategy : RecombinationStrategy
    {
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, firstParent.Degree - 1);

            var firstSingels = new List<IfsFunction>();
            var secondSingels = new List<IfsFunction>();

            for (int i = 0; i < firstParent.Degree; i++)
            {
                if (i >= crossoverPoint)
                {
                    firstSingels.Add(secondParent.Singels[i]);
                    secondSingels.Add(firstParent.Singels[i]);
                }
                else
                {
                    firstSingels.Add(firstParent.Singels[i]);
                    secondSingels.Add(secondParent.Singels[i]);
                }
            }

            Individual individual1 = new Individual(firstSingels);
            Individual individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
