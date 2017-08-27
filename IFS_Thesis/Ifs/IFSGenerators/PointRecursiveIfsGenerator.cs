using System.Collections.Generic;

namespace IFS_Thesis.IFS.IFSGenerators
{
    /// <summary>
    /// IFS Generator using Point-Recursive algorithm
    /// </summary>
    public class PointRecursiveIfsGenerator : IfsGenerator
    {
        /// <summary>
        /// Applies Point-Recursive procedure
        /// </summary>
        private void ApplyPointRecursiveAlgortithm(HashSet<Point3Df> resultPoints, Point3Df q0, int l, List<IfsFunction> ifsMappings)
        {
            foreach (var t in ifsMappings)
            {
                var q1Point = ApplyIfsTransformationTo3DPoint(t, q0);

                if (l == 0)
                {
                    resultPoints.Add(q1Point);
                }
                else
                {
                    ApplyPointRecursiveAlgortithm(resultPoints, q1Point, l - 1, ifsMappings);
                }
            }
        }

        public override HashSet<Voxel> GenerateVoxelsForIfs(List<IfsFunction> ifsMappings, int imageX, int imageY,
            int imageZ, int multiplier)
        {
            var resultPoints = new HashSet<Point3Df>();

            //we start at B1, B2, B3
            var q0 = new Point3Df(ifsMappings[0].B1, ifsMappings[0].B2, ifsMappings[0].B3);

            var l = multiplier;

            ApplyPointRecursiveAlgortithm(resultPoints, q0, l, ifsMappings);

            var result = Convert3DfPointsToVoxels(resultPoints, imageX, imageY, imageZ);

            return result;
        }
    }
}
