using System;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.Properties;
using log4net;

namespace IFS_Thesis.EvolutionaryData.Mutation.Individuals
{
    public class StandardMutationRateStrategy : IndividualMutationStrategy
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Private Methods

        /// <summary>
        /// Gets possible range for coefficient at given index
        /// </summary>
        /// <param name="index">index of coefficient</param>
        /// <remarks>(-1, 1) for a variables and (-10,10) for b variables</remarks>
        private Tuple<int, int> GetRangeForIndex(int index)
        {
            if (index >= 0 && index <= 8 )
            {
                return new Tuple<int, int>(-1, 1);
            }
            if (index >= 9 && index <= 11)
            {
                return new Tuple<int, int>(-2, 2);
            }

            return new Tuple<int, int>(0,0);
        }

        /// <summary>
        /// Gets coefficient name at given index
        /// </summary>
        /// <param name="index">index of coefficient</param>
        private string GetCoefficientNameByIndex(int index)
        {
            string name;

            switch (index)
            {
                case 0:
                    name = "a11";
                    break;
                case 1:
                    name = "a12";
                    break;
                case 2:
                    name = "a13";
                    break;
                case 3:
                    name = "a21";
                    break;
                case 4:
                    name = "a22";
                    break;
                case 5:
                    name = "a23";
                    break;
                case 6:
                    name = "a31";
                    break;
                case 7:
                    name = "a32";
                    break;
                case 8:
                    name = "a33";
                    break;
                case 9:
                    name = "b1";
                    break;
                case 10:
                    name = "b2";
                    break;
                case 11:
                    name = "b3";
                    break;
                default:
                    name = "error";
                    break;
            }

            return name;
        }

        #endregion

        /// <summary>
        /// Mutates an Individual using real-valued mutation
        /// </summary>
        public override void Mutate(EaConfiguration configuration, ref Individual individual, RealValueMutationStrategy strategy, Random randomGen)
        {
            //selecting one singel for mutation
            var singelToMutate = randomGen.Next(0, individual.Singels.Count);

            //selecting one variable for mutation from that singel
            var variableToMutate = randomGen.Next(0, 12);

            //getting the allowable range of values for selected variable
            var range = GetRangeForIndex(variableToMutate);

            //performing mutation using specified mutation strategy
            var oldCoefficient = individual.Singels[singelToMutate].Coefficients[variableToMutate];
            var newCoefficient = strategy.Mutate(oldCoefficient, randomGen, range, configuration.MutationRange);

            individual.Singels[singelToMutate][variableToMutate] = newCoefficient;

            if (Settings.Default.ExtremeDebugging)
            {
                Log.Debug(
                    $"Mutated coefficient {GetCoefficientNameByIndex(variableToMutate)} from {oldCoefficient} to {newCoefficient} using {strategy.GetType().Name}");
            }
        }
    }

}
