﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        private float CalculateFitness(List<Point> sourceImagePixels, Individual individual, int width, int height)
        {
            var result = new IfsDrawer().GetIfsPixels(individual.Singels, width,
                height);

            var redundantPixels = result.Item1;
            var generatedPixels = result.Item2;

            var matchingPixels = generatedPixels.Intersect(sourceImagePixels).ToList();

            if (generatedPixels.Count == 0)
            {
                //Log.Debug($"Calculated fitness for individual. Fitness - {0}");
                return 0;
            }

            //NA
            var na = generatedPixels.Count;

            //NI
            var ni = sourceImagePixels.Count;

            //NND
            var notDrawnPoints = sourceImagePixels.Except(generatedPixels).Count();

            //NNN
            var pointsNotNeeded = generatedPixels.Except(matchingPixels).Count();

            pointsNotNeeded = pointsNotNeeded + redundantPixels;

            var rc = notDrawnPoints / (float)ni;

            var ro = pointsNotNeeded / (float)na;

            var fitness = (1 - rc) + (1 - ro);

            //Log.Debug($"Calculated fitness for individual. Fitness - {fitness}");

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
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, List<Point> sourceImagePixels, int imageWidth, int imageHeight)
        {
            Log.Debug("Started calculating fitness for all individuals");

            Parallel.ForEach(individuals, individual =>
            {
                individual.ObjectiveFitness = CalculateFitness(sourceImagePixels, individual, imageWidth, imageHeight);

            });

            Log.Debug("Ended calculating fitness for all individuals");

            return individuals;
        }

        #endregion
        
    }
}
