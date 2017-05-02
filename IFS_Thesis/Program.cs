using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.Ifs;
using IFS_Thesis.EvolutionaryData.Ifs.IFSDrawers;
using IFS_Thesis.EvolutionaryData.Ifs.IFSGenerators;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using MoreLinq;
using Image = System.Drawing.Image;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IFS_Thesis
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            #region Image Parameters

            var initialImagePath = Settings.Default.WorkingDirectory + "/tested_ifs";

            var finalGeneratedImagePath = Settings.Default.WorkingDirectory + @"/highest_fitness_final.png";

            int imageSizeX = Settings.Default.ImageX;
            int imageSizeY = Settings.Default.ImageY;
            int imageSizeZ = Settings.Default.ImageZ;

            #endregion

            #region IFS Definitions 3D

            var sierpinskiPyramid = new List<IfsFunction3D>()
            {
                new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f),
                new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0.5f, 0f, 0f),
                new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0.5f, 0f),
                new IfsFunction3D(0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0f, 0.5f, 0f, 0f, 0.5f)
            };

            #endregion

            //Creating the directory if it does not exist
            Directory.CreateDirectory(Settings.Default.WorkingDirectory);

            var randomGen = new Random();

            var ifsGenerator = new RandomIterationIfs3DGenerator();
            var voxels = ifsGenerator.GenerateVoxelsForIfs(sierpinskiPyramid, imageSizeX, imageSizeY, imageSizeZ, randomGen);
            new IfsDrawer3D().SaveImage(initialImagePath, voxels, ImageFormat3D.Obj);
            new IfsDrawer3D().SaveImage(initialImagePath, voxels, ImageFormat3D.Stl);

            //var drawer = new IfsDrawer();
            //drawer.SaveIfsImage(sierpinskiPentagon, imageSizeX, imageSizeY, initialImagePath);

            //Bitmap image = (Bitmap)Image.FromFile(initialImagePath, true);

            //var ea = new EvolutionaryAlgorithm();

            //var highest = ea.Start(Settings.Default.NumberOfGenerations, image, drawer);

            //drawer.SaveIfsImage(highest.Singels, image.Width, image.Height, finalGeneratedImagePath);
        }
    }
}
