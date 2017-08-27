using System;
using System.Collections.Generic;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;
using log4net;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    /// <summary>
    /// Arithmetic crossover strategy
    /// </summary>
    public class ArithmeticCrossoverStrategy : RecombinationStrategy
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        /// <summary>
        /// Produces offspring using arithmetic crossover operator
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

            var firstChildSingels = new List<IfsFunction>();
            var secondChildSingels = new List<IfsFunction>();

            //For each IFS function in parents
            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                var firstParentCoefficients = firstParentClone.Singels[i];
                var secondParentCoefficients = secondParentClone.Singels[i];

                var firstChildCoefficients = new float[12];
                var secondChildCoefficients = new float[12];

                //For each coefficient we perform arithmetic crossover
                for (int j = 0; j < 12; j++)
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

                firstChildSingels.Add(new IfsFunction(firstChildCoefficients));
                secondChildSingels.Add(new IfsFunction(secondChildCoefficients));
            }

            return new List<Individual> { new Individual(firstChildSingels), new Individual(secondChildSingels) };
        }
    }
}
