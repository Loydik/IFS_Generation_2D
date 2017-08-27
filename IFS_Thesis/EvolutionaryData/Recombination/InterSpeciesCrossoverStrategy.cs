using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using log4net;
using MoreLinq;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class InterSpeciesCrossoverStrategy : RecombinationStrategy
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        /// <summary>
        /// Produces offspring using inter-species crossover operator
        /// </summary>
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree == secondParent.Degree)
            {
                return new List<Individual>();
            }

            var parentWithBiggerDegree = (Individual)firstParent.Clone();
            var parentWithLesserDegree = (Individual)secondParent.Clone();

            if (firstParent.Degree < secondParent.Degree)
            {
                parentWithBiggerDegree = (Individual)secondParent.Clone();
                parentWithLesserDegree = (Individual)firstParent.Clone();
            }

            var firstChildSingels = parentWithBiggerDegree.Singels;
            var secondChildSingels = parentWithLesserDegree.Singels;

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, 11);

            for (int i = 0; i < parentWithLesserDegree.Degree; i++)
            {
                var firstIfsFunction = new List<float>();
                firstIfsFunction.AddRange(firstChildSingels[i].Coefficients.Take(crossoverPoint));
                firstIfsFunction.AddRange(secondChildSingels[i].Coefficients.TakeLast(secondChildSingels[i].Coefficients.Length - crossoverPoint));

                var secondIfsFunction = new List<float>();
                secondIfsFunction.AddRange(secondChildSingels[i].Coefficients.Take(crossoverPoint));
                secondIfsFunction.AddRange(firstChildSingels[i].Coefficients.TakeLast(firstChildSingels[i].Coefficients.Length - crossoverPoint));

                firstChildSingels[i].Coefficients = firstIfsFunction.ToArray();
                secondChildSingels[i].Coefficients = secondIfsFunction.ToArray();
            }

            return new List<Individual> { new Individual(firstChildSingels), new Individual(secondChildSingels) };
        }
    }
}
