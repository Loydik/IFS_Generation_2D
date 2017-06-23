using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

            #endregion

            //Creating the directory if it does not exist
            Directory.CreateDirectory(Settings.Default.WorkingDirectory);

            var randomGen = new Random();
            var ifsGenerator = new RandomIterationIfsGenerator();
            var ifsDrawer = new IfsDrawer3D();

            var voxels = ifsGenerator.GenerateVoxelsForIfs(sierpinskiPyramid, imageSizeX, imageSizeY, imageSizeZ, Settings.Default.IfsGenerationMultiplier);
            //ifsDrawer.SaveVoxelImage(initialImagePath, voxels, ImageFormat3D.Obj);
            ifsDrawer.SaveVoxelImage(initialImagePath, voxels, ImageFormat3D.Stl);


            Log.Info($"Ifs generator is {ifsGenerator.GetType()}");

            var ea = new EvolutionaryAlgorithm();

            var highest = ea.StartEvolution(Settings.Default.NumberOfGenerations, voxels, ifsDrawer, ifsGenerator, randomGen);

            voxels = ifsGenerator.GenerateVoxelsForIfs(highest.Singels, imageSizeX, imageSizeY, imageSizeZ, Settings.Default.IfsGenerationMultiplier);
            ifsDrawer.SaveVoxelImage(finalEvolvedImagePath, voxels, ImageFormat3D.Obj);
           // ifsDrawer.SaveVoxelImage(finalEvolvedImagePath, voxels, ImageFormat3D.Stl);
        }
    }
}
