using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IFS_Thesis.EvolutionaryData.Ifs;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using log4net;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public class WeightedPointsCoverageFitnessFunction : IFitnessFunction
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate fitness for a given individual
        /// </summary>
        public float CalculateFitnessForIndividual(List<Point> sourceImagePixels, Individual individual, int width, int height)
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
