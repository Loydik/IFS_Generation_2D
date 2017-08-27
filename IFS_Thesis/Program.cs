using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IFS_Thesis.Configuration;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.IFS;
using IFS_Thesis.IFS.IFSDrawers;
using IFS_Thesis.IFS.IFSGenerators;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
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

            #endregion

            #region IFS Definitions 3D

            var sierpinskiPyramid = new List<IfsFunction>
            {
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                new IfsFunction(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
            };

            #endregion

            #region Initializing Default Image

            //Creating the directory if it does not exist
            Directory.CreateDirectory(Settings.Default.WorkingDirectory);

            var randomGen = new Random();
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();

            var voxels = ifsGenerator.GenerateVoxelsForIfs(sierpinskiPyramid, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
            ifsDrawer.SaveVoxelsTo3DImage(initialImagePath, voxels, ImageFormat3D.Stl);

            Log.Info($"Ifs generator is {ifsGenerator.GetType()}");

            #endregion

            var ea = new EvolutionaryAlgorithm();

            Population initialPopulation = null;

            #region Initializing Different Configurations

            var configuration1 = EaConfigurator.GetDefaultConfiguration();
            configuration1.PopulationSize = Settings.Default.PopulationSize / 3;
            configuration1.MutationRange = 0.6f;

            var configuration2 = EaConfigurator.GetDefaultConfiguration();
            configuration2.PopulationSize = Settings.Default.PopulationSize / 3;
            configuration2.MutationRange = 0.3f;

            var configuration3 = EaConfigurator.GetDefaultConfiguration();
            configuration3.PopulationSize = Settings.Default.PopulationSize / 3;
            configuration3.MutationRange = 0.15f;

            var configurations = new List<EaConfiguration> { configuration1, configuration2, configuration3};

            #endregion

            #region Population from text file

            if (Settings.Default.InitialPopulationFromTextFile)
            {
                //Population from text file
                var txtFilepath = Settings.Default.WorkingDirectory + "/population.txt";

                var populationString = File.ReadAllText(txtFilepath);

                var allIndividuals = EaUtils.CreateIndividualsFromPopulationString(populationString);

                initialPopulation = new Population();
                initialPopulation.SetAllIndividuals(allIndividuals);
            }

            #endregion

            //Multiple Populations
            var highest = ea.StartEvolutionWithMultiplePopulations(configurations, Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen);

            //var highest = Settings.Default.InitialPopulationFromTextFile ? ea.StartEvolution(initialPopulation, Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen) : ea.StartEvolution(Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen);
                    
            voxels = ifsGenerator.GenerateVoxelsForIfs(highest.Singels, Settings.Default.ImageX, Settings.Default.ImageY, Settings.Default.ImageZ, Settings.Default.IfsGenerationMultiplier);
            ifsDrawer.SaveVoxelsTo3DImage(finalEvolvedImagePath, voxels, ImageFormat3D.Obj);
        }
    }
}
