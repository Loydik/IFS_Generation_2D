using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.Ifs;
using NUnit.Framework;

namespace IFS_Thesis_Tests.IfsTests
{
    [TestFixture]
    public class IfsFunction3DTests
    {
        [Test, Category("IfsFunction")]
        public void IfsFunctionEqualityTest()
        {
            var firstIfsFunction = new IfsFunction(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.3f);
            var secondIfsFunction = new IfsFunction(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.3f);

            Assert.That(firstIfsFunction, Is.EqualTo(secondIfsFunction));
        }

        [Test, Category("IfsFunction")]
        public void IfsFunctionCreateFromCoefficientsTest()
        {
            var coefficients = new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f };

            var firstIfsFunction = new IfsFunction(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f);

            var secondIfsFunction = new IfsFunction(coefficients);

            Assert.That(firstIfsFunction, Is.EqualTo(secondIfsFunction));
        }

        [Test, Category("IfsFunction")]
        public void IfsFunctionCreateFromCoefficientsCheckEveryVariableTest()
        {
            var coefficients = new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f };

            var ifsFunction = new IfsFunction(coefficients);

            Assert.That(ifsFunction.A11, Is.EqualTo(0.1f));
            Assert.That(ifsFunction.A12, Is.EqualTo(0.2f));
            Assert.That(ifsFunction.A13, Is.EqualTo(0.3f));
            Assert.That(ifsFunction.A21, Is.EqualTo(0.4f));
            Assert.That(ifsFunction.A22, Is.EqualTo(0.5f));
            Assert.That(ifsFunction.A23, Is.EqualTo(0.6f));
            Assert.That(ifsFunction.A31, Is.EqualTo(0.7f));
            Assert.That(ifsFunction.A32, Is.EqualTo(0.8f));
            Assert.That(ifsFunction.A33, Is.EqualTo(0.9f));
            Assert.That(ifsFunction.B1, Is.EqualTo(1f));
            Assert.That(ifsFunction.B2, Is.EqualTo(1.1f));
            Assert.That(ifsFunction.B3, Is.EqualTo(1.2f));
        }

        [Test, Category("Voxels")]
        public void VoxelEqualityTest()
        {
            var voxel1 = new Voxel(1, 2, 3);
            var voxel2 = new Voxel(1, 2, 3);

            Assert.That(voxel1, Is.EqualTo(voxel2));
        }

        [Test, Category("Voxels")]
        public void VoxelInEqualityTest()
        {
            var voxel1 = new Voxel(1, 2, 4);
            var voxel2 = new Voxel(1, 2, 3);

            Assert.That(voxel1, Is.Not.EqualTo(voxel2));
        }


        [Test, Category("Voxels")]
        public void VoxelsHashsetIntersectTest()
        {
            var voxel1 = new Voxel(1, 2, 4);
            var voxel2 = new Voxel(1, 2, 3);
            var voxel3 = new Voxel(1, 2, 5);
            var voxel4 = new Voxel(1, 3, 3);
            var voxel5 = new Voxel(1, 1, 3);

            var voxel6 = new Voxel(9, 2, 3);
            var voxel7 = new Voxel(1, 2, 3);
            var voxel8 = new Voxel(1, 2, 5);
            var voxel9 = new Voxel(10, 2, 3);
            var voxel10 = new Voxel(1, 2, 4);

            var hashset1 = new HashSet<Voxel> {voxel1, voxel2, voxel3, voxel4, voxel5};
            var hashset2 = new HashSet<Voxel> {voxel6, voxel7, voxel8, voxel9, voxel10};

            var intersect = hashset1.Intersect(hashset2).ToList();

            Assert.That(intersect.Count, Is.EqualTo(3));
        }
    }
}
