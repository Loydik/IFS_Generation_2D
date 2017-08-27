using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public class LinearRankingFitnessFunction : IRankingFitnessFunction
    {
        /// <summary>
        /// Assign Rank fitness to individuals
        /// </summary>
        public List<Individual> AssignRankingFitnessToIndividuals(List<Individual> individuals, float selectivePressure)
        {
            //we first order individuals according to their objective fitness
            individuals = individuals.OrderBy(i => i.ObjectiveFitness).ToList();

            var nind = individuals.Count;
            var pos = 1;

            foreach (var individual in individuals)
            {
                individual.RankFitness = 2 - selectivePressure + 2 * (selectivePressure - 1) * (pos - 1) / (nind - 1);
                pos++;
            }

            return individuals;
        }
    }
}
