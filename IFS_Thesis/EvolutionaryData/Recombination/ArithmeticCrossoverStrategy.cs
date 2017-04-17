using System;
using System.Collections.Generic;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    /// <summary>
    /// Arithmetic crossover strategy
    /// </summary>
    public class ArithmeticCrossoverStrategy : RecombinationStrategy
    {
        /// <summary>
        /// Produces offsprings using arithmetic crossover operator
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

            // a ∈ (0,1)
            var a = randomGen.NextDouble();

            var firstSingels = new List<IfsFunction>();
            var secondSingels = new List<IfsFunction>();

            //For each IFS function
            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                var firstParentCoefficients = firstParentClone.Singels[i];
                var secondParentCoefficients = secondParentClone.Singels[i];

                var firstChildCoefficients = new float[6];
                var secondChildCoefficients = new float[6];

                //For each coefficient
                for (int j = 0; j < 6; j++)
                {
                   var x1 =
                        a * firstParentCoefficients.Coefficients[j] +
                        (1 - a) * secondParentCoefficients.Coefficients[j];

                    var x2 = 
                        (1 - a) * firstParentCoefficients.Coefficients[j] +
                        a * secondParentCoefficients.Coefficients[j];

                    firstChildCoefficients[j] = (float) Math.Round(x1, 4, MidpointRounding.AwayFromZero);
                    secondChildCoefficients[j] = (float) Math.Round(x2, 4, MidpointRounding.AwayFromZero);
                }

                firstSingels.Add(new IfsFunction(firstChildCoefficients));
                secondSingels.Add(new IfsFunction(secondChildCoefficients));
            }

            var individual1 = new Individual(firstSingels);
            var individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
