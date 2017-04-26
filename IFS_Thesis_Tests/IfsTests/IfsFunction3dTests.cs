using IFS_Thesis.EvolutionaryData.Ifs;
using NUnit.Framework;

namespace IFS_Thesis_Tests.IfsTests
{
    [TestFixture]
    public class IfsFunction3DTests
    {
        [Test, Category("IfsFunction")]
        public void IfsFunctionEqualityTest()
        {
            var firstIfsFunction = new IfsFunction3D(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.3f);
            var secondIfsFunction = new IfsFunction3D(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f, 1.3f);

            Assert.That(firstIfsFunction, Is.EqualTo(secondIfsFunction));
        }

        [Test, Category("IfsFunction")]
        public void IfsFunctionCreateFromCoefficientsTest()
        {
            var coefficients = new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f };

            var firstIfsFunction = new IfsFunction3D(0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f);

            var secondIfsFunction = new IfsFunction3D(coefficients);

            Assert.That(firstIfsFunction, Is.EqualTo(secondIfsFunction));
        }

        [Test, Category("IfsFunction")]
        public void IfsFunctionCreateFromCoefficientsCheckEveryVariableTest()
        {
            var coefficients = new[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f, 1.1f, 1.2f };

            var ifsFunction = new IfsFunction3D(coefficients);

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
    }
}
