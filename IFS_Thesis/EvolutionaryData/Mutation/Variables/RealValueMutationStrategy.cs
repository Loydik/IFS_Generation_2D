using System;

namespace IFS_Thesis.EvolutionaryData.Mutation.Variables
{
    /// <summary>
    /// Mutation strategy for mutation real-valued variables
    /// </summary>
    public abstract class RealValueMutationStrategy
    {
        public abstract float Mutate(float variable, Random randomGen, Tuple<int, int> range);
    }
}
