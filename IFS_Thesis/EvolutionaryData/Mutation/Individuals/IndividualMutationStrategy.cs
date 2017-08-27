using System;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;

namespace IFS_Thesis.EvolutionaryData.Mutation.Individuals
{
    public abstract class IndividualMutationStrategy
    {
        public abstract void Mutate(EaConfiguration configuration, ref Individual individual, RealValueMutationStrategy strategy, Random randomGen);
    }
}
