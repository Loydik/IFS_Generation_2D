using System;
using System.Collections.Generic;
using IFS_Thesis.Properties;

namespace IFS_Thesis.Ifs.IFSGenerators
{
    /// <summary>
    /// Generates IFS 3D using Random Iteration Algorithm
    /// </summary>
    public class RandomIterationIfs3DGenerator : IfsGenerator3D
    {
        /// <summary>
        /// Generates voxels for a given Ifs function
        /// </summary>
        public override HashSet<Voxel> GenerateVoxelsForIfs(List<IfsFunction3D> ifsMappings, int imageX, int imageY, int imageZ, Random randomGen)
        {
            var resultPoints = new HashSet<Point3Df>();

            var length = ifsMappings.Count;

            //we start at B1, B2, B3
            var currentPoint = new Point3Df(ifsMappings[0].B1, ifsMappings[0].B2, ifsMappings[0].B3);

            //we ignore first 100 iterations
            var minIterations = 100;
            var maxIterations = imageX * imageY * Settings.Default.DrawPointsMultiplier;

            for (int k = 0; k < maxIterations; k++)
            {
                var i = randomGen.Next(0, length);

                currentPoint = Apply3DIfsTransformation(ifsMappings[i], currentPoint);

                if (k > minIterations)
                {
                    resultPoints.Add(currentPoint);
                }
            }

            var result = Convert3DfPointsToVoxels(resultPoints, imageX, imageY, imageZ);

            return result;
        }
    }
}
