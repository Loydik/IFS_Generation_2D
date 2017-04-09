using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.Utils;
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
        /// <remarks>(-1, 1) for a,b,c,d and (-10,10) for e,f</remarks>
        private Tuple<int, int> GetRangeForIndex(int index)
        {
            if (index >= 0 && index <= 3 )
            {
                return new Tuple<int, int>(-1, 1);
            }
            if (index >= 4 && index <= 5)
            {
                return new Tuple<int, int>(-10, 10);
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
                    name = "a";
                    break;
                case 1:
                    name = "b";
                    break;
                case 2:
                    name = "c";
                    break;
                case 3:
                    name = "d";
                    break;
                case 4:
                    name = "e";
                    break;
                case 5:
                    name = "f";
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
        public override void Mutate(ref Individual individual, RealValueMutationStrategy strategy, Random randomGen, float? mutationRate = null)
        {
            if(mutationRate == null)
            { 
                //by default, mutation rate is 1/n (n -> number of variables/coefficients)
                mutationRate = 1f / (individual.Singels.Count*6f);
            }

            var newSingels = new List<IfsFunction>();

            //can be optimized
            foreach (var singel in individual.Singels)
            {
                var tempCoefficients = new List<float>();

                for (var index = 0; index < singel.Coefficients.Length; index++)
                {
                    var coefficient = singel.Coefficients[index];
                    var tempCoefficient = coefficient;

                    var mutate = mutationRate >= randomGen.NextDouble();

                    if (mutate)
                    {
                        var range = GetRangeForIndex(index);
                        tempCoefficient = strategy.Mutate(tempCoefficient, randomGen, range);
                        Log.Debug($"Mutated coefficient {GetCoefficientNameByIndex(index)} from {coefficient} to {tempCoefficient} using {strategy.GetType().Name}");
                    }

                    tempCoefficients.Add(tempCoefficient);
                }

                newSingels.Add(new IfsFunction(tempCoefficients.ToArray()));
            }

            individual.Singels = newSingels;
        }
    }

}
