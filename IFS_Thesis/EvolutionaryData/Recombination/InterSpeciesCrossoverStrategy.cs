using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Properties;
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

        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree == secondParent.Degree)
            {
                return new List<Individual>();
            }

            var parentWithBiggerDegree = (Individual)firstParent.Clone();
            var parentWithLesserDegree = (Individual)secondParent.Clone();

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, 11);

            if (Settings.Default.ExtremeDebugging)
            {
                Log.Debug($"Crossover point in inter-species crossover is: {crossoverPoint}");
            }

            if (firstParent.Degree < secondParent.Degree)
            {
                parentWithBiggerDegree = (Individual)secondParent.Clone();
                parentWithLesserDegree = (Individual)firstParent.Clone();
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

            var individual1 = new Individual(firstSingels);
            var individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
