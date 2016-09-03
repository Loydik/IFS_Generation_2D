using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using IFS_Thesis.EvolutionaryData;
using Image = System.Drawing.Image;

namespace IFS_Thesis
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageSavePath = "C:/Users/Loydik94/Desktop/IFS Images/ifs4.bmp";
            var imageSavePath2 = @"C:/Users/Loydik94/Desktop/IFS Images/Fitness Experiment/ifs_temp.png";

            var imageReadPath = @"C:/Users/Loydik94/Desktop/IFS Images/Fitness Experiment/ifs_result.png";
            var image2ReadPath = @"C:/Users/Loydik94/Desktop/IFS Images/Fitness Experiment/ifs_result2.png";

            int sizeX = 512;
            int sizeY = 512;

            List<double[]> fern = new List<double[]> {new[] { 0.1, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01 }, new[] { 0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.85 }, new[] { 0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07 }, new[] { -0.15, 0.29, 0.25, 0.24, 0.0, 0.41, 0.07 }};

            List<double[]> pentagon = new List<double[]> { new[] { 0.382, 0.0, 0.0, 0.382, 0.0, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.618, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.809, 0.588, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.309, 0.951, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, -0.191, 0.588, 0.2 } };

            List<double[]> fern2 = new List<double[]> { new[] { 0.1, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01 }, new[] { 0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.85 }, new[] { 0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07 }, new[] { -0.15, 0.29, 0.25, 0.24, 0.0, 0.41, 0.07 } };

            Bitmap image = (Bitmap)Image.FromFile(imageReadPath, true);

            ////Bitmap image2 = (Bitmap)Image.FromFile(image2ReadPath, true);


            //var drawer = new IfsDrawer();
            //var image = drawer.DrawIfs(pentagon, sizeX, sizeY);
            //image.Save(imageSavePath, ImageFormat.Bmp);

            Individual individual = (Individual) fern2;

            var fitness = FitnessFunction.CalculateFitness(image, individual);

            image.Save(imageSavePath2);
        }
    }
}
