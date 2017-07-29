using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    /// <summary>
    /// Discrete Recombination strategy
    /// </summary>
    public class DiscreteVariableRecombinationStrategy : RecombinationStrategy
    {
        #region Private Methods

        /// <summary>
        /// Recombine two given singels using Discrete Recombination strategy
        /// </summary>
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

        #endregion

        #region Overriden Members

        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstChild = (Individual)firstParent.Clone();
            var secondChild = (Individual)secondParent.Clone();


            var crossoverPoint = randomGen.Next(firstParent.Degree);

            for (int i = crossoverPoint; i < firstParent.Degree; i++)
            {
                var childSingels = RecombineSingels(firstChild.Singels[i],
                    secondChild.Singels[i],
                    randomGen);

                firstChild.Singels[i] = childSingels.Item1;
                secondChild.Singels[i] = childSingels.Item2;
            }

            return new List<Individual> { firstChild, secondChild };
        }

        #endregion
    }
}
