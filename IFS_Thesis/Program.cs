using System.Collections.Generic;
using System.Drawing;
using IFS_Thesis.EvolutionaryData;
using IFS_Thesis.Utils;
using Image = System.Drawing.Image;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IFS_Thesis
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageSavePath = Properties.Settings.Default.WorkingDirectory + "/fern.png";
            var imageSavePath2 = Properties.Settings.Default.WorkingDirectory + @"/highest_fitness_final.png";

            int sizeX = 512;
            int sizeY = 512;

            List<double[]> fern = new List<double[]> {new[] { 0.1, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01 }, new[] { 0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.85 }, new[] { 0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07 }, new[] { -0.15, 0.29, 0.25, 0.24, 0.0, 0.41, 0.07 }};


            List<double[]> fernTemp = new List<double[]> { new[] { 0.1, 0.0, 0.0, 0.16, 0.0, 0.0, 0.25 }, new[] { 0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.25 }, new[] { 0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.25 }, new[] { -0.15, 0.29, 0.25, 0.24, 0.0, 0.41, 0.25 } };

            List<double[]> pentagon = new List<double[]> { new[] { 0.382, 0.0, 0.0, 0.382, 0.0, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.618, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.809, 0.588, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.309, 0.951, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, -0.191, 0.588, 0.2 } };

            List<IfsFunction> fernMine = new List<IfsFunction> { new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.01f), new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.85f), new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.07f), new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.07f) };

            List<IfsFunction> fernMine2 = new List<IfsFunction> { new IfsFunction(0.1f, 0.0f, 0.0f, 0.16f, 0.0f, 0.0f, 0.25f), new IfsFunction(0.85f, 0.04f, -0.04f, 0.85f, 0.0f, 1.6f, 0.25f), new IfsFunction(0.2f, -0.26f, 0.23f, 0.22f, 0.0f, 1.6f, 0.25f), new IfsFunction(-0.15f, 0.29f, 0.25f, 0.24f, 0.0f, 0.41f, 0.25f) };

            List<IfsFunction> pentagonMine = new List<IfsFunction> { new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.0f, 0.0f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.618f, 0.0f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.809f, 0.588f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.951f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, -0.191f, 0.588f, 0.2f) };

            List<IfsFunction> pentagonMine2 = new List<IfsFunction> { new IfsFunction(0.311f, 0.0f, 0.2f, 0.312f, 0.0f, 0.0f, 0.1f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.3822f, 0.5f, 0.0f, 0.25f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.6f, 0.809f, 0.1f, 0.2f), new IfsFunction(0.382f, 0.0f, 0.0f, 0.382f, 0.309f, 0.1f, 0.6f), new IfsFunction(0.1f, 0.1f, 0.0f, 0.382f, -0.191f, 0.588f, 0.2f) };


            var drawer = new IfsDrawer();
            drawer.SaveIfsImage(pentagonMine, 512, 512, imageSavePath);

            Bitmap image = (Bitmap)Image.FromFile(imageSavePath, true);

            var ea = new EvolutionaryAlgorithm();

            var highest = ea.Start(2000, image, drawer);

            drawer.SaveIfsImage(highest.Singels, 512, 512, imageSavePath2);
        }
    }
}
