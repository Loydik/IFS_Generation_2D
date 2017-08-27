using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using log4net;

namespace IFS_Thesis.IFS.IFSDrawers
{
    /// <summary>
    /// Class which will convert a list of voxels to an image of a given format
    /// </summary>
    public class IfsDrawer3D
    {
        #region Private Fields

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Exporter
        /// </summary>
        private IExporter _exporter;

        /// <summary>
        /// File extension to be used
        /// </summary>
        private string _fileExtension;

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts voxels to HelixToolkitWPF PointsVisual3D point cloud
        /// </summary>
        private PointsVisual3D ConvertVoxelsTo3DPointCloud(HashSet<Voxel> voxels)
        {
            var dataList = new Point3DCollection();

            foreach (var voxel in voxels)
            {
                dataList.Add(new Point3D(voxel.X, voxel.Y, voxel.Z));
            }

            var cloudPoints = new PointsVisual3D
            {
                Size = 1,
                Points = dataList
            };

            return cloudPoints;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves 3D Ifs image generated from voxels to a given path
        /// </summary>
        public void SaveVoxelsTo3DImage(string path, HashSet<Voxel> voxels, ImageFormat3D imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat3D.Obj:
                    _fileExtension = ".obj";
                    _exporter = new ObjExporter();
                    var exporter = (ObjExporter) _exporter;
                    exporter.MaterialsFile = path + ".mtl";
                    break;

                case ImageFormat3D.Stl:
                    _fileExtension = ".stl";
                    _exporter = new StlExporter();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(imageFormat), imageFormat, null);
            }

            if (voxels.Count != 0)
            {
                var cloudPoints = ConvertVoxelsTo3DPointCloud(voxels);
                cloudPoints.Color = Colors.Blue;
                
                path = path + _fileExtension;

                using (Stream filestream = File.Create(path))
                {
                    _exporter.Export(cloudPoints, filestream);
                }
            }

            else
            {
                Log.Error("Voxels count was 0, could not generate an image");
            }
        }

        /// <summary>
        /// Create 3D Ifs overlay image generated from two sets of voxels to a given path
        /// </summary>
        public void SaveVoxelOverlayImage(string path, HashSet<Voxel> sourceVoxels, HashSet<Voxel> generatedVoxels, ImageFormat3D imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat3D.Obj:
                    _fileExtension = ".obj";
                    _exporter = new ObjExporter();
                    var exporter = (ObjExporter)_exporter;
                    exporter.MaterialsFile = path + ".mtl";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(imageFormat), imageFormat, null);
            }

            if (sourceVoxels.Count != 0 && generatedVoxels.Count != 0)
            {
                var cloudPoints1 = ConvertVoxelsTo3DPointCloud(sourceVoxels);
                var cloudPoints2 = ConvertVoxelsTo3DPointCloud(generatedVoxels);

                cloudPoints1.Color = Colors.Blue;
                cloudPoints2.Color = Colors.Red;

                path = path + _fileExtension;

                using (Stream filestream = File.Create(path))
                {
                    var viewport = new Viewport3D();
                    viewport.Children.Add(cloudPoints1);
                    viewport.Children.Add(cloudPoints2);
                    _exporter.Export(viewport, filestream);
                }
            }

            else
            {
                Log.Error("Voxels count was 0, could not generate an image");
            }
        }

        #endregion
    }
}
