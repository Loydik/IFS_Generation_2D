using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageSavePath = "C:/Users/Loydik94/Desktop/IFS Images/ifs.png";

            int sizeX = 512;
            int sizeY = 512;

            List<double[]> fern = new List<double[]> {new[] { 0.0, 0.0, 0.0, 0.16, 0.0, 0.0, 0.01 }, new[] { 0.85, 0.04, -0.04, 0.85, 0.0, 1.6, 0.85 }, new[] { 0.2, -0.26, 0.23, 0.22, 0.0, 1.6, 0.07 }, new[] { -0.15, 0.28, 0.26, 0.24, 0.0, 0.44, 0.07 }};

            List<double[]> pentagon = new List<double[]> { new[] { 0.382, 0.0, 0.0, 0.382, 0.0, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.618, 0.0, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.809, 0.588, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, 0.309, 0.951, 0.2 }, new[] { 0.382, 0.0, 0.0, 0.382, -0.191, 0.588, 0.2 } };

            var drawer = new IfsDrawer();
            drawer.SaveIfsImage(fern, sizeX, sizeY, imageSavePath);


        }
    }
}
