using System;

namespace IFS_Thesis.EvolutionaryData.Mutation.Variables
{
    /// <summary>
    /// Random mutation strategy
    /// </summary>
    public class RandomMutationStrategy : RealValueMutationStrategy
    {
        /// <summary>
        /// Mutate a variable using Random Mutation strategy
        /// </summary>
        public override float MutateVariable(float variable, Random randomGen, Tuple<int, int> range, float mutationPrecision)
        {
            var newValue = (float) randomGen.NextDouble() * (range.Item2 - range.Item1) + range.Item1;

            return newValue;
        }
    }
}
