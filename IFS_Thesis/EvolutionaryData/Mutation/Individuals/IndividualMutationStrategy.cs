using System;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;

namespace IFS_Thesis.EvolutionaryData.Mutation.Individuals
{
    public abstract class IndividualMutationStrategy
    {
        public abstract void Mutate(ref Individual individual, RealValueMutationStrategy strategy, Random randomGen, float? mutationRate = null);
    }
}
