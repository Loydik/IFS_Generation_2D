using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSDrawers;
using IFS_Thesis.Ifs.IFSGenerators;
using IFS_Thesis.Properties;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IFS_Thesis
{
    public class Program
    {
        #region Logger

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

         #endregion

        [STAThread]
        static void Main(string[] args)
        {
            #region Image Parameters

            var initialImagePath = Settings.Default.WorkingDirectory + "/tested_ifs";

            var finalEvolvedImagePath = Settings.Default.WorkingDirectory + "/final_evolved_ifs";

            int imageSizeX = Settings.Default.ImageX;
            int imageSizeY = Settings.Default.ImageY;
            int imageSizeZ = Settings.Default.ImageZ;

            #endregion

            #region IFS Definitions 3D

            var sierpinskiPyramid = new List<IfsFunction>
            {
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
            };

            var barnsleyFern = new List<IfsFunction>
            {
                new IfsFunction(0f, 0f, 0f, 0f, 0.18f, 0f, 0f, 0f, 0f, 0f, 0f, 0f),
                new IfsFunction(0.85f, 0f, 0f, 0f, 0.85f, 0.1f, 0f, -0.1f, 0.85f, 0f, 1.6f, 0f),
                new IfsFunction(0.2f, -0.2f, 0f, 0.2f, 0.2f, 0f, 0f, 0f, 0.3f, 0f, 0.8f, 0f),
                new IfsFunction(-0.2f, 0.2f, 0f, 0.2f, 0.2f, 0f, 0f, 0f, 0.3f, 0f, 0.8f, 0f)
            };

            var bridge = new List<IfsFunction>
            {
                new IfsFunction(0.5f, 0f, 0f, 0.25f, 0.25f, 0f, 0f, 0f, 0.5f, -0.1f, 0.1f, 0f),
                new IfsFunction(0.5f, 0f, 0f, -0.25f, 0.25f, 0f, 0f, 0f, 0.5f, 0.1f, 0.1f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0.25f, 0.25f, 0f, 0f, 0f, 0.5f, -0.1f, 0.1f, -1f),
                new IfsFunction(0.5f, 0f, 0f, -0.25f, 0.25f, 0f, 0f, 0f, 0.5f, 0.1f, 0.1f, -1f),
            };

            //NOT WORKING
            var cantorSet = new List<IfsFunction>
            {
                new IfsFunction(0.3333f, 0f, 0f, 0f, 0f, 0f, 0.3333f, 0f, 0f, 0f, 0.3333f, 0f),
                new IfsFunction(0.3333f, 0f, 0f, 0.6666f, 0f, 0.3333f, 0f, 0f, 0f, 0f, 0.3333f, 0f),
                new IfsFunction(0.3333f, 0f, 0f, 0.6666f, 0f, 0.3333f, 0f, 0.6666f, 0f, 0f, 0.3333f, 0f),
                new IfsFunction(0.3333f, 0f, 0f, 0f, 0f, 0f, 0.3333f, 0f, 0.6666f, 0f, 0.3333f, 0f),
                new IfsFunction(0.3333f, 0f, 0f, 0f, 0f, 0f, 0.3333f, 0f, 0f, 0f, 0.3333f, 0.6666f),
                new IfsFunction(0.3333f, 0f, 0f, 0.6666f, 0f, 0.3333f, 0f, 0f, 0f, 0f, 0.3333f, 0.6666f),
                new IfsFunction(0.3333f, 0f, 0f, 0.6666f, 0f, 0.3333f, 0f, 0.6666f, 0f, 0f, 0.3333f, 0.6666f),
                new IfsFunction(0.3333f, 0f, 0f, 0f, 0f, 0f, 0.3333f, 0f, 0.6666f, 0f, 0.3333f, 0.6666f),
            };

            #endregion

            #region Initializing Default Image

            //Creating the directory if it does not exist
            Directory.CreateDirectory(Settings.Default.WorkingDirectory);

            var randomGen = new Random();
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();

            var voxels = ifsGenerator.GenerateVoxelsForIfs(sierpinskiPyramid, imageSizeX, imageSizeY, imageSizeZ, Settings.Default.IfsGenerationMultiplier);
            //ifsDrawer.SaveVoxelImage(initialImagePath, voxels, ImageFormat3D.Obj);
            ifsDrawer.SaveVoxelImage(initialImagePath, voxels, ImageFormat3D.Stl);

            Log.Info($"Ifs generator is {ifsGenerator.GetType()}");

            #endregion

            var ea = new EvolutionaryAlgorithm();

            #region Initializing Different Configurations

            var configuration1 = EaConfigurator.GetDefaultConfiguration();
            configuration1.PopulationSize = Settings.Default.PopulationSize / 4;
            configuration1.RandomMutationProbability = 0.25f;
            configuration1.ControlledMutationProbability = 0.75f;
            configuration1.MutationRange = 1f;

            var configuration2 = EaConfigurator.GetDefaultConfiguration();
            configuration2.PopulationSize = Settings.Default.PopulationSize / 4;
            configuration2.MutationRange = 0.5f;

            var configuration3 = EaConfigurator.GetDefaultConfiguration();
            configuration3.PopulationSize = Settings.Default.PopulationSize / 4;
            configuration3.MutationRange = 0.25f;
            configuration3.N1IndividualsPercentage = 0.3f;
            configuration3.N3IndividualsPercentage = 0.2f;
            configuration3.N4IndividualsPercentage = 0.5f;

            var configuration4 = EaConfigurator.GetDefaultConfiguration();
            configuration4.PopulationSize = Settings.Default.PopulationSize / 4;
            configuration4.MutationRange = 0.05f;
            configuration4.RandomMutationProbability = 0;
            configuration4.ControlledMutationProbability = 1;

            var configurations = new List<EaConfiguration> { configuration1, configuration2, configuration3, configuration4 };

            #endregion

            //Multiple Populations
            //var highest = ea.StartEvolutionWithMultiplePopulations(configurations, Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen);

            //Single population
            var highest = ea.StartEvolution(Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen);

            voxels = ifsGenerator.GenerateVoxelsForIfs(highest.Singels, imageSizeX, imageSizeY, imageSizeZ, Settings.Default.IfsGenerationMultiplier);
            ifsDrawer.SaveVoxelImage(finalEvolvedImagePath, voxels, ImageFormat3D.Obj);
           // ifsDrawer.SaveVoxelImage(finalEvolvedImagePath, voxels, ImageFormat3D.Stl);
        }
    }
}
