using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public class LinearRankingFitnessFunction : IRankingFitnessFunction
    {
        public List<Individual> AssignRankingFitnessToIndividuals(List<Individual> individuals, float selectivePressure)
        {
            individuals = individuals.OrderBy(i => i.ObjectiveFitness).ToList();

            int nind = individuals.Count;
            int pos = 1;

            foreach (var individual in individuals)
            {
                individual.RankFitness = 2 - selectivePressure + 2 * (selectivePressure - 1) * (pos - 1) / (nind - 1);
                pos++;
            }

            return individuals;
        }
    }
}
