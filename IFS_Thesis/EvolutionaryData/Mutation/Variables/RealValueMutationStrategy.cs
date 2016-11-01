using System;

namespace IFS_Thesis.EvolutionaryData.Mutation.Variables
{
    public abstract class RealValueMutationStrategy
    {
        public abstract float Mutate(float variable, Random randomGen);
    }
}
