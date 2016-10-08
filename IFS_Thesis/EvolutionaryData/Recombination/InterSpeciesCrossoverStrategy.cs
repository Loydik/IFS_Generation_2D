using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class InterSpeciesCrossoverStrategy : RecombinationStrategy
    {
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            Individual parentWithBiggerDegree = firstParent;
            Individual parentWithLesserDegree = secondParent;

            if (firstParent == null || secondParent == null || firstParent.Degree == secondParent.Degree)
            {
                return new List<Individual>();
            }

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, 5);

            if (firstParent.Degree < secondParent.Degree)
            {
                parentWithBiggerDegree = secondParent;
                parentWithLesserDegree = firstParent;
            }

            var firstSingels = parentWithBiggerDegree.Singels;
            var secondSingels = parentWithLesserDegree.Singels;

            for (int i = 0; i < parentWithLesserDegree.Degree; i++)
            {
                List<float> firstIfsFunction = new List<float>();
                firstIfsFunction.AddRange(firstSingels[i].Coefficients.Take(crossoverPoint));
                firstIfsFunction.AddRange(secondSingels[i].Coefficients.TakeLast(secondSingels[i].Coefficients.Length - crossoverPoint));

                List<float> secondIfsFunction = new List<float>();
                secondIfsFunction.AddRange(secondSingels[i].Coefficients.Take(crossoverPoint));
                secondIfsFunction.AddRange(firstSingels[i].Coefficients.TakeLast(firstSingels[i].Coefficients.Length - crossoverPoint));

                firstSingels[i].Coefficients = firstIfsFunction.ToArray();
                secondSingels[i].Coefficients = secondIfsFunction.ToArray();
            }

            Individual individual1 = new Individual(firstSingels);
            Individual individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
