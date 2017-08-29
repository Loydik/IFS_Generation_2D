using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;
using IFS_Thesis.Properties;
using log4net;

namespace IFS_Thesis.Utils
{
    /// <summary>
    /// Utility methods for evolutionary algorithm
    /// </summary>
    public class EaUtils
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculates average fitness for a given degree
        /// </summary>
        private static float GetAverageFitnessForDegree(List<Individual> individuals, int degree)
        {
            var average = individuals.Where(x => x.Degree == degree).Select(x => x.ObjectiveFitness).Average();

            return average;
        }

        #endregion

        #region Public Methods

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

        /// <summary>
        /// Gets degrees of given individuals
        /// </summary>
        public static List<int> GetDegreesOfIndividuals(List<Individual> individuals)
        {
            var degrees = individuals.Select(x => x.Degree).Distinct().ToList();

            return degrees;
        }

        /// <summary>
        /// Gets best individuals from each degree
        /// </summary>
        public static List<Individual> GetBestIndividualsOfEachDegree(Population population, int numberOfIndividualsPerDegree)
        {
            var bestIndividuals = new List<Individual>();

            var degrees = GetDegreesOfIndividuals(population.Individuals);

            foreach (var degree in degrees)
            {
                var best = population.Individuals.Where(x => x.Degree == degree).OrderByDescending(x => x.ObjectiveFitness).Take(numberOfIndividualsPerDegree).ToList().Clone();
                bestIndividuals.AddRange(best);
            }

            return bestIndividuals;
        }

        /// <summary>
        /// TODO - test/ Needs rework
        /// </summary>
        public static List<float> UpdateVectorOfProbabilitiesBasedOnAverageFitness(List<Individual> individuals, List<float> vector)
        {
            //brute add method

            //foreach (var individual in individuals)
            //{
            //    vector[individual.Degree - 1] = vector[individual.Degree - 1] + individual.ObjectiveFitness;
            //}

            var degrees = GetDegreesOfIndividuals(individuals);

            foreach (var degree in degrees)
            {
                var averageFitnessForDegree = GetAverageFitnessForDegree(individuals, degree);

                vector[degree - 1] = vector[degree - 1] + averageFitnessForDegree;
            }

            vector = NormalizeVectorWithMinimumValue(vector, Settings.Default.ProbabilityVectorMinimum);

            Log.Info($"Updated the probability vector, current values are: [{string.Join(",", vector)}]");

            return vector;
        }

        /// <summary>
        /// Adapts the vector of probability distribution V D proportionally to the fitness value
        ///of the best individual of each degree.
        /// </summary>
        public static List<float> UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(List<Individual> bestIndividuals, List<float> vector)
        {
            var bestFitnessesPerDegree = new Dictionary<int, float>();

            var degrees = GetDegreesOfIndividuals(bestIndividuals);
            degrees.Sort();

            foreach (var degree in degrees)
            {
                var bestFitnessForDegree = bestIndividuals.Single(x => x.Degree == degree).ObjectiveFitness;

                vector[degree - 1] = vector[degree - 1] + bestFitnessForDegree;

                bestFitnessesPerDegree.Add(degree, bestFitnessForDegree);
            }

            vector = NormalizeVectorWithMinimumValue(vector, Settings.Default.ProbabilityVectorMinimum);

            Log.Info($"Best fitnesses for degrees: [{string.Join(";", bestFitnessesPerDegree)}]");
            Log.Info($"Updated the probability vector, current values are: [{string.Join(",", vector)}]");

            return vector;
        }

        /// <summary>
        /// Normalizes a vector
        /// </summary>
        public static List<float> NormalizeVector(List<float> vector)
        {
            //sum of all elements in the vector
            var length = vector.Aggregate(0f, (current, element) => current + element);

            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] = vector[i] / length;
            }

            return vector;
        }

        /// <summary>
        /// Normalizes a vector with minimum value allowed
        /// </summary>
        public static List<float> NormalizeVectorWithMinimumValue(List<float> vector, float minValue)
        {
            var normalized = NormalizeVector(vector);

            while (normalized.Skip(2).Any(x => x < minValue))
            {
                int minIndex = Array.IndexOf(normalized.Skip(2).ToArray(), normalized.Skip(2).Min()) + 2;
                normalized[minIndex] = normalized[minIndex] + 0.001f;
                normalized = NormalizeVector(normalized);
            }

            return normalized;
        }

        /// <summary>
        /// Creates individual from signels string in logs
        /// </summary>
        /// <remarks>string in format [0.2611,0.2358,-0.7685,0.163,-1.5512,-9.0696,0];[-0.9634,0.2324,-0.4512,0.7583,1.7809,-0.0628,0];[-0.6634,0.3488,0.0202,0.3246,4.8702,-2.9171,0];[0.0018,0.5933,-0.9078,0.5663,7.1813,-4.2975,0]</remarks>
        public static Individual CreateIndividualFromSingelsString(string singelsString)
        {
            var individualSingels = new List<IfsFunction>();
            var singels = singelsString.Trim().Split(';');

            foreach (var singel in singels)
            {
                char[] charsToTrim = { '[', ']' };
                var coefficientts = singel.Trim(charsToTrim).Split(',');

                individualSingels.Add(new IfsFunction(float.Parse(coefficientts[0]), float.Parse(coefficientts[1]), float.Parse(coefficientts[2]), float.Parse(coefficientts[3]), float.Parse(coefficientts[4]), float.Parse(coefficientts[5]), float.Parse(coefficientts[6]), float.Parse(coefficientts[7]), float.Parse(coefficientts[8]), float.Parse(coefficientts[9]), float.Parse(coefficientts[10]), float.Parse(coefficientts[11]), float.Parse(coefficientts[12])));
            }

            return new Individual(individualSingels);
        }

        /// <summary>
        /// Creates a list of individuals from text representation of population
        /// </summary>
        public static List<Individual> CreateIndividualsFromPopulationString(string populationString)
        {
            var allIndividuals = new List<Individual>();

            var individualsSingels = Regex.Matches(populationString, @"(?<=Singles:.*)\[.*\]");

            var individualFitnesses = Regex.Matches(populationString, @"(?<=ObjectiveFitness - )[0-9]+(\.[0-9]{1,10})?E?");

            var list = individualsSingels.Cast<Match>().Select(match => match.Value).ToList();
            var fitnessesList = individualFitnesses.Cast<Match>().Select(match => match.Value).ToList();

            for (var i = 0; i < list.Count; i++)
            {
                var singels = list[i];
                var individual = CreateIndividualFromSingelsString(singels);

                individual.ObjectiveFitness = fitnessesList[i].Contains("E") ? 0 : float.Parse(fitnessesList[i]);
                
                allIndividuals.Add(individual);
            }

            return allIndividuals;
        }

        #endregion
    }
}
