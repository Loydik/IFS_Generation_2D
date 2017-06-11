using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    /// <summary>
    /// Fitness function which uses ranking instead of objective fitness
    /// </summary>
    public interface IRankingFitnessFunction
    {
        /// <summary>
        /// Assign ranking to given individuals
        /// </summary>
        /// <returns></returns>
        List<Individual> AssignRankingFitnessToIndividuals(List<Individual> individuals, float selectivePressure);
    }
}
