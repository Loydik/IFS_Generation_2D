using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSGenerators;
using IFS_Thesis.Properties;
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
        public float CalculateFitnessForIndividual(HashSet<Voxel> sourceImageVoxels, Individual individual, IfsGenerator ifsGenerator, int imageX, int imageY, int imageZ)
        {
            var generatedVoxels = ifsGenerator.GenerateVoxelsForIfs(individual.Singels, imageX, imageY, imageZ);

            //var redundantPixels = result.Item1;
            //var generatedPixels = result.Item2;

            if (generatedVoxels.Count == 0)
            {
                return 0;
            }

            // new IfsDrawer().CreateImageFromPixels(generatedPixels).Save($"{Settings.Default.WorkingDirectory}/generatedPixels.png");

            var matchingVoxelsCount = generatedVoxels.Intersect(sourceImageVoxels).Count();


            //new IfsDrawer().CreateImageFromPixels(matchingPixels).Save($"{Settings.Default.WorkingDirectory}/matchingPixels.png");

            //var pixelsDrawnOutside = generatedPixels.Except(matchingPixels).ToList();

            //new IfsDrawer().CreateImageFromPixels(pixelsDrawnOutside).Save($"{Settings.Default.WorkingDirectory}/pixelsDrawnOutsidePixels.png");

            //NA
            var na = generatedVoxels.Count;

            //NI
            var ni = sourceImageVoxels.Count;

            //NND
            var notDrawnPoints = ni - matchingVoxelsCount;

            //NNN
            var pointsNotNeeded = na - matchingVoxelsCount;

            //pointsNotNeeded = pointsNotNeeded + redundantPixels;

            var rc = notDrawnPoints / (float)ni;

            var ro = pointsNotNeeded / (float)na;

            var fitness = Settings.Default.PrcFitness * (1 - rc) + Settings.Default.ProFitness * (1 - ro);


            return fitness;
        }

        /// <summary>
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        public List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, HashSet<Voxel> sourceImageVoxels, IfsGenerator ifsGenerator, int imageX, int imageY, int imageZ)
        {
            Log.Debug("Started calculating fitness for all individuals");

            //For each individual which is not elite we calculate fitness
            Parallel.ForEach(individuals, individual =>
            {
                individual.ObjectiveFitness = CalculateFitnessForIndividual(sourceImageVoxels, individual, ifsGenerator, imageX, imageY, imageZ);

            });

            ////For debugging
            //foreach (var individual in individuals)
            //{
            //    individual.ObjectiveFitness = CalculateFitnessForIndividual(sourceImageVoxels, individual, ifsGenerator, imageX, imageY, imageZ, randomGen);
            //}

            Log.Debug("Ended calculating fitness for all individuals");

            return individuals;
        }

        #endregion        
    }
}
