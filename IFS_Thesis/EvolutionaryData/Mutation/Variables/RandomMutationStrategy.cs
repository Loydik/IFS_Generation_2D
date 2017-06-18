using System;

namespace IFS_Thesis.EvolutionaryData.Mutation.Variables
{
    /// <summary>
    /// Random mutation strategy
    /// </summary>
    public class RandomMutationStrategy : RealValueMutationStrategy
    {
        public override float Mutate(float variable, Random randomGen, Tuple<int, int> range)
        {
            var newValue = (float) randomGen.NextDouble() * (range.Item2 - range.Item1) + range.Item1;

            return newValue;
        }
    }
}
