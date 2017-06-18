using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Properties;
using log4net;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public class WeightedPointsCoverageObjectiveFitnessFunction : IObjectiveFitnessFunction
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
        /// Gets fitness adjusment for fitness function
        /// </summary>
        private float GetFitnessAdjustment(int generatedVoxelsCount, int sourceImageVoxelsCount, float currentFitness)
        {
            float percentToSubtract = 0;

            if (generatedVoxelsCount < sourceImageVoxelsCount)
            {
                //calculating percentage error
                var pError = (float)(sourceImageVoxelsCount - generatedVoxelsCount) / sourceImageVoxelsCount;
                pError = Math.Abs(pError);

                if (pError > 0.8)
                {
                    percentToSubtract = pError;
                }
            }

            if (generatedVoxelsCount > sourceImageVoxelsCount)
            {
                //calculating percentage difference
                var pDiff = (generatedVoxelsCount - sourceImageVoxelsCount) /
                            ((generatedVoxelsCount + sourceImageVoxelsCount) / 2.0f);

                pDiff = Math.Abs(pDiff);

                if (pDiff > 1.5)
                {
                    percentToSubtract = pDiff / 2;
                }
            }

            var fitnessAdjustment = currentFitness * percentToSubtract;

            return fitnessAdjustment;
        }
        
        #endregion
        
        #region Public Methods

        /// <summary>
        /// Calculates fitness based on parameters
        /// </summary>
        public float CalculateFitness(int generatedVoxelsCount, int sourceImageVoxelsCount, int matchingVoxelsCount, int prcFitness, int proFitness)
        {
            //NA
            var na = generatedVoxelsCount;

            //NI
            var ni = sourceImageVoxelsCount;

            //NND
            var notDrawnPoints = ni - matchingVoxelsCount;

            //NNN
            var pointsNotNeeded = na - matchingVoxelsCount;

            var rc = notDrawnPoints / (float)ni;
            var ro = pointsNotNeeded / (float)na;

            var fitness = prcFitness * (1 - rc) + proFitness * (1 - ro);

            if (Settings.Default.UseFitnessAdjustment)
            {
                var fitnessAdjustment = GetFitnessAdjustment(generatedVoxelsCount, sourceImageVoxelsCount, fitness);

                fitness = fitness - fitnessAdjustment;

                if (fitness < 0)
                {
                    return 0;
                }
            }

            return fitness;
        }


        /// <summary>
        /// Calculate fitness for a given individual
        /// </summary>
        public float CalculateFitnessForIndividual(List<Point> sourceImagePixels, Individual individual, int width, int height)
        {
            var result = new IfsDrawer().GetIfsPixels(individual.Singels, width,
                height);

            //TODO - check redundant as well
            var redundantPixels = result.Item1;
            var generatedPixels = result.Item2;

            if (generatedPixels.Count == 0)
            {
                return 0;
            }

            var matchingPixels = generatedPixels.Intersect(sourceImagePixels).ToList();


            var fitness = CalculateFitness(generatedPixels.Count, sourceImagePixels.Count, matchingPixels.Count,
                 Settings.Default.PrcFitness, Settings.Default.ProFitness);

            return fitness;
        }

        /// <summary>
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, List<Point> sourceImagePixels, int imageWidth, int imageHeight)
        {
            Log.Debug("Started calculating fitness for all individuals");

            //For each individual which is not elite we calculate fitness
            Parallel.ForEach(individuals, individual =>
            {
                individual.ObjectiveFitness = CalculateFitnessForIndividual(sourceImagePixels, individual, imageWidth, imageHeight);

            });

            //For debugging
            //foreach (var individual in individuals)
            //{
            //    individual.ObjectiveFitness = CalculateFitnessForIndividual(sourceImagePixels, individual, imageWidth, imageHeight);
            //}

            Log.Debug("Ended calculating fitness for all individuals");

            return individuals;
        }

        #endregion        
    }
}
