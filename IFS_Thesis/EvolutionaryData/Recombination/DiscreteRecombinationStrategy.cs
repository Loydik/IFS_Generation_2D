using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class DiscreteRecombinationStrategy : RecombinationStrategy
    {
        private Tuple<Singel, Singel> RecombineSingels(IfsFunction firstSignel, IfsFunction secondSingel, Random randomGen)
        {
            var firstSingelClone = (IfsFunction)firstSignel.Clone();
            var secondSingelClone = (IfsFunction)secondSingel.Clone();

            var result1 = new float[12];
            var result2 = new float[12];

            for (int i = 0; i < 11; i++)
            {
                if (randomGen.NextDouble() >= 0.5)
                    result1[i] = firstSingelClone.Coefficients[i];
                else
                    result1[i] = secondSingelClone.Coefficients[i];

                if (randomGen.NextDouble() >= 0.5)
                    result2[i] = firstSingelClone.Coefficients[i];
                else
                    result2[i] = secondSingelClone.Coefficients[i];
            }

            var offspringSingel1 = new IfsFunction(result1);
            var offspringSingel2 = new IfsFunction(result2);

            return new Tuple<Singel, Singel>(new Singel(offspringSingel1), new Singel(offspringSingel2));
        }

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

            for (int i = 0; i < firstParent.Degree; i++)
            {
                var childSingels = RecombineSingels(firstParentClone.Singels[i], secondParentClone.Singels[i],
                    randomGen);
                firstSingels.Add(childSingels.Item1);
                secondSingels.Add(childSingels.Item2);
            }

            var individual1 = new Individual(firstSingels);
            var individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
