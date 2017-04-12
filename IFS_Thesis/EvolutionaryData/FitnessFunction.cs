using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;
using log4net.Repository.Hierarchy;

namespace IFS_Thesis.EvolutionaryData
{
    public class FitnessFunction
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
        private float GetAverageFitnessForDegree(List<Individual> individuals, int degree)
        {
            var average = individuals.Where(x => x.Degree == degree).Select(x => x.ObjectiveFitness).Average();

            return average;
        }

        /// <summary>
        /// Calculate fitness for a given individual
        /// </summary>
        /// //TODO Refactor to simpler calculations
        private float CalculateFitness(List<Point> sourceImagePixels, Individual individual, int width, int height)
        {
            var result = new IfsDrawer().GetIfsPixels(individual.Singels, width,
                height);

            var redundantPixels = result.Item1;
            var generatedPixels = result.Item2;

            if (generatedPixels.Count == 0)
            {
                return 0;
            }

           // new IfsDrawer().CreateImageFromPixels(generatedPixels).Save($"{Settings.Default.WorkingDirectory}/generatedPixels.png");

            var matchingPixels = generatedPixels.Intersect(sourceImagePixels).ToList();

            //new IfsDrawer().CreateImageFromPixels(matchingPixels).Save($"{Settings.Default.WorkingDirectory}/matchingPixels.png");

            //var pixelsDrawnOutside = generatedPixels.Except(matchingPixels).ToList();

            //new IfsDrawer().CreateImageFromPixels(pixelsDrawnOutside).Save($"{Settings.Default.WorkingDirectory}/pixelsDrawnOutsidePixels.png");

            //NA
            var na = generatedPixels.Count;

            //NI
            var ni = sourceImagePixels.Count;

            //NND
            var notDrawnPoints = ni - matchingPixels.Count;

            //NNN
            var pointsNotNeeded = na - matchingPixels.Count;

            pointsNotNeeded = pointsNotNeeded + redundantPixels;

            var rc = notDrawnPoints / (float)ni;

            var ro = pointsNotNeeded / (float)na;

            var fitness = Settings.Default.PrcFitness * (1 - rc) + Settings.Default.ProFitness * (1 - ro);

            return fitness;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// TODO - test/ Needs rework
        /// </summary>
        public List<float> UpdateVectorOfProbabilitiesBasedOnFitness(List<Individual> individuals, List<float> vector )
        {
            //brute add method

            //foreach (var individual in individuals)
            //{
            //    vector[individual.Degree - 1] = vector[individual.Degree - 1] + individual.ObjectiveFitness;
            //}

            var degrees = OtherUtils.GetDegreesOfIndividuals(individuals);

            foreach (var degree in degrees)
            {
                var averageFitnessForDegree = GetAverageFitnessForDegree(individuals, degree);

                vector[degree - 1] = vector[degree - 1] + averageFitnessForDegree;
            }

            vector = OtherUtils.NormalizeVector(vector);

            Log.Info($"Updated the probability vector, current values are: [{string.Join(",", vector)}]");

            return vector;
        }

        /// <summary>
        /// Adapts the vector of probability distribution V D proportionally to the fitness value
        ///of the best individual of each degree.
        /// </summary>
        public List<float> UpdateVectorOfProbabilitiesBasedOnBestIndividualsFromDegree(List<Individual> individuals, List<float> vector)
        {
            Dictionary<int, float> bestFitnessesPerDegree = new Dictionary<int, float>();

            var degrees = OtherUtils.GetDegreesOfIndividuals(individuals);
            
            degrees.Sort();

            foreach (var degree in degrees)
            {
                var bestFitnessForDegree = individuals.Where(x => x.Degree == degree).Max(x => x.ObjectiveFitness);

                vector[degree - 1] = vector[degree - 1] + bestFitnessForDegree;

                bestFitnessesPerDegree.Add(degree, bestFitnessForDegree);
            }

            vector = OtherUtils.NormalizeVector(vector);

            for (var index = 0; index < vector.Count; index++)
            {
                var probability = vector[index];
                
                //Setting to zero
                if (Math.Abs(probability) > 0.00000001 && probability < 0.00000001)
                {
                    vector[index] = 0;
                }
            }

            Log.Info($"Best fitnesses for degrees: [{string.Join(";", bestFitnessesPerDegree)}]");
            Log.Info($"Updated the probability vector, current values are: [{string.Join(",", vector)}]");

            return vector;
        }

        /// <summary>
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, List<Point> sourceImagePixels, int imageWidth, int imageHeight)
        {
            Log.Debug("Started calculating fitness for all individuals");

            Parallel.ForEach(individuals, individual =>
            {
                individual.ObjectiveFitness = CalculateFitness(sourceImagePixels, individual, imageWidth, imageHeight);

            });

            //For debugging
            //foreach (var individual in individuals)
            //{
            //    individual.ObjectiveFitness = CalculateFitness(sourceImagePixels, individual, imageWidth, imageHeight);
            //}

            Log.Debug("Ended calculating fitness for all individuals");

            return individuals;
        }

        #endregion
        
    }
}
