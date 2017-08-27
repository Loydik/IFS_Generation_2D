using System;
using System.Collections.Generic;

namespace IFS_Thesis.IFS.IFSGenerators
{
    /// <summary>
    /// Generates IFS 3D using Random Iteration Algorithm
    /// </summary>
    public class RandomIterationIfsGenerator : IfsGenerator
    {
        /// <summary>
        /// Generates voxels for given Ifs mappings
        /// </summary>
        public override HashSet<Voxel> GenerateVoxelsForIfs(List<IfsFunction> ifsMappings, int imageX, int imageY, int imageZ, int multiplier)
        {
            var resultPoints = new HashSet<Point3Df>();
            var randomGen = new Random();

            var length = ifsMappings.Count;

            //we start at B1, B2, B3 point
            var currentPoint = new Point3Df(ifsMappings[0].B1, ifsMappings[0].B2, ifsMappings[0].B3);

            //we ignore first 100 iterations
            var minIterations = 100;
            var maxIterations = imageX * imageY * multiplier;

            for (int k = 0; k < maxIterations; k++)
            {
                var i = randomGen.Next(0, length);

                currentPoint = ApplyIfsTransformationTo3DPoint(ifsMappings[i], currentPoint);

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
