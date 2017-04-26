using System.Collections.Generic;
using System.Drawing;
using System.IO;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.EvolutionaryData.Ifs;
using IFS_Thesis.Properties;
using IFS_Thesis.Utils;
using Image = System.Drawing.Image;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IFS_Thesis
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Image Parameters

            var initialImagePath = Settings.Default.WorkingDirectory + "/tested_ifs.png";
            var finalGeneratedImagePath = Settings.Default.WorkingDirectory + @"/highest_fitness_final.png";

            int imageSizeX = Settings.Default.ImageX;
            int imageSizeY = Settings.Default.ImageY;

            #endregion

            #region IFS Definitions
            
            //Sierpinski Triangle - degree 3 (Equal probabilities)
            var sierpinskiTriangle = new List<IfsFunction> { new IfsFunction(0.5f, 0.0f, 0.0f, 0.5f, 0.0f, 0.0f, 0.333f), new IfsFunction(0.5f, 0.0f, 0.0f, 0.5f, 0.5f, 0.0f, 0.333f), new IfsFunction(0.5f, 0.0f, 0.0f, 0.5f, 0.25f, 0.433f, 0.333f) };

            //Koch Curve - degree 4 (Equal probabilities)
            var kochCurve = new List<IfsFunction> { new IfsFunction(0.333f, 0.0f, 0.0f, 0.333f, 0.0f, 0.0f, 0.25f), new IfsFunction(0.167f, -0.289f, 0.289f, 0.167f, 0.333f, 0f, 0.25f), new IfsFunction(0.167f, 0.289f, -0.289f, 0.167f, 0.5f, 0.289f, 0.25f), new IfsFunction(0.333f, 0f, 0f, 0.333f, 0.667f, 0f, 0.25f) };

            //Barnsley's fern - degree 4
            var barnsleysFern = new List<IfsFunction> { new IfsFunction(0.0f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f), new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f), new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f), new IfsFunction(-0.15f, 0.28f, 0.26f, 0.24f, 0.0f, 0.44f, 0.07f) };

            //Sierpinski pentagon - degree 5 (Equal probabilities)
            var sierpinskiPentagon = new List<IfsFunction> { new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, -0.191f, 0.588f, 0.2f) };

            #endregion

            //Creating the directory if it does not exist
            Directory.CreateDirectory(Settings.Default.WorkingDirectory);

            var drawer = new IfsDrawer();
            drawer.SaveIfsImage(sierpinskiPentagon, imageSizeX, imageSizeY, initialImagePath);

            Bitmap image = (Bitmap)Image.FromFile(initialImagePath, true);

            var ea = new EvolutionaryAlgorithm();

            var highest = ea.Start(Settings.Default.NumberOfGenerations, image, drawer);

            drawer.SaveIfsImage(highest.Singels, image.Width, image.Height, finalGeneratedImagePath);
        }
    }
}
