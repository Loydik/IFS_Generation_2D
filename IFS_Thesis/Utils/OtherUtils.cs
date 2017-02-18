using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFS_Thesis.EvolutionaryData;

namespace IFS_Thesis.Utils
{
    public class OtherUtils
    {
        /// <summary>
        /// Determines whether probability happened
        /// </summary>
        public static bool DeterminePercentProbability(Random random, float probability)
        {
            var randomResult = random.NextDouble();

            if (randomResult < probability)
            {
                return true;
            }

            return false;
        }

        public static List<float> NormalizeVector(List<float> vector)
        {
            //sum of all elements in the vector
            var length = vector.Aggregate(0f, (current, element) => current + element);

            for(int i = 0; i < vector.Count; i++)
            {
                vector[i] = vector[i]/length;
            }

            return vector;
        }

        public static List<int> GetDegreesOfIndividuals(List<Individual> individuals )
        {
            var degrees = individuals.Select(x => x.Degree).Distinct().ToList();

            return degrees;
        }
    }
}
