﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using static System.Single;

namespace IFS_Thesis.Ifs.IFSGenerators
{
    /// <summary>
    /// Class used for generating 3D IFS-es
    /// </summary>
    public abstract class IfsGenerator3D
    {
        #region Protected Members

        /// <summary>
        /// Applies 3D IFS transformation to a given point
        /// </summary>
        /// <param name="ifsFunction">IFS Function to make transformation with</param>
        /// <param name="currentPoint">Current point</param>
        /// <returns></returns>
        protected Point3Df Apply3DIfsTransformation(IfsFunction3D ifsFunction, Point3Df currentPoint)
        {
            var x0 = currentPoint.X;
            var y0 = currentPoint.Y;
            var z0 = currentPoint.Z;

            var x = ifsFunction.A11 * x0 + ifsFunction.A12 * y0 + ifsFunction.A13 * z0 + ifsFunction.B1;
            var y = ifsFunction.A21 * x0 + ifsFunction.A22 * y0 + ifsFunction.A23 * z0 + ifsFunction.B2;
            var z = ifsFunction.A31 * x0 + ifsFunction.A32 * y0 + ifsFunction.A33 * z0 + ifsFunction.B3;

            return new Point3Df(x, y, z);
        }

        /// <summary>
        /// Converts generated 3Df Points to Voxels
        /// </summary>
        /// <param name="points">3D floating-point points generated by IFS</param>
        /// <param name="imgx">Image X dimension</param>
        /// <param name="imgy">Image Y dimension</param>
        /// <param name="imgz">Image Z dimension</param>
        protected HashSet<Voxel> Convert3DfPointsToVoxels(HashSet<Point3Df> points, int imgx, int imgy, int imgz)
        {
            var voxels = new HashSet<Voxel>();

            var xMin = points.Min(x => x.X);
            var xMax = points.Max(x => x.X);
            var yMin = points.Min(y => y.Y);
            var yMax = points.Max(y => y.Y);
            var zMin = points.Min(z => z.Z);
            var zMax = points.Max(z => z.Z);

            if (IsInfinity(xMax) || IsInfinity(yMax) || IsInfinity(zMax) || IsInfinity(xMin) || IsInfinity(yMin) || IsInfinity(zMin) || IsNaN(xMax) || IsNaN(yMax) || IsNaN(zMax) || IsNaN(xMin) || IsNaN(yMin) || IsNaN(zMin))
            {
                //invalid IFS in this case
                return new HashSet<Voxel>();
            }

            var xDelta = xMax - xMin;
            var yDelta = yMax - yMin;
            var zDelta = zMax - zMin;

            var scaleX = (imgx - 1) / xDelta;
            var scaleY = (imgy - 1) / yDelta;
            var scaleZ = (imgz - 1) / zDelta;

            try
            {
                Convert.ToInt32(xDelta * scaleX);
                Convert.ToInt32(xDelta * scaleY);
                Convert.ToInt32(xDelta * scaleZ);
            }

            catch (OverflowException)
            {
                //invalid IFS in this case
                return new HashSet<Voxel>();
            }

            foreach (var point in points)
            {
                var jx = Convert.ToInt32((point.X - xMin) * scaleX);
                var jy = Convert.ToInt32((point.Y - yMin) * scaleY);
                var jz = Convert.ToInt32((point.Z - zMin) * scaleZ);

                voxels.Add(new Voxel(jx, jy, jz));                
            }

            //Remove duplicated voxels
            voxels = voxels.Distinct().ToHashSet();

            return voxels;
        }

        #endregion

        /// <summary>
        /// Generate Voxels for given IFS
        /// </summary>
        public abstract HashSet<Voxel> GenerateVoxelsForIfs(List<IfsFunction3D> ifsMappings, int imageX, int imageY,
            int imageZ);
    }
}
