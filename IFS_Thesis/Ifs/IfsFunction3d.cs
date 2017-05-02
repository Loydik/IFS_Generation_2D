using System;

namespace IFS_Thesis.Ifs
{
    /// <summary>
    /// IFS function 3d representation
    /// </summary>
    public class IfsFunction3D : IEquatable<IfsFunction3D>, ICloneable
    {
        #region Properties

        public float A11 { get; set; }
        public float A12 { get; set; }
        public float A13 { get; set; }
        public float A21 { get; set; }
        public float A22 { get; set; }
        public float A23 { get; set; }
        public float A31 { get; set; }
        public float A32 { get; set; }
        public float A33 { get; set; }

        public float B1 { get; set; }
        public float B2 { get; set; }
        public float B3 { get; set; }

        public double P { get; set; }

        /// <summary>
        /// Coefficients expressed in array form
        /// </summary>
        public float[] Coefficients
        {
            get { return new[] { A11, A12, A13, A21, A22, A23, A31, A32, A33, B1, B2, B3 }; }
            set
            {
                A11 = value[0];
                A12 = value[1];
                A13 = value[2];
                A21 = value[3];
                A22 = value[4];
                A23 = value[5];
                A31 = value[6];
                A32 = value[7];
                A33 = value[8];
                B1 = value[9];
                B2 = value[10];
                B3 = value[11];
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Create IFS Function without probabilities
        /// </summary>
        public IfsFunction3D(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33, float b1, float b2, float b3)
        {
            A11 = a11;
            A12 = a12;
            A13 = a13;
            A21 = a21;
            A22 = a22;
            A23 = a23;
            A31 = a31;
            A32 = a32;
            A33 = a33;
            B1 = b1;
            B2 = b2;
            B3 = b3;
        }

        /// <summary>
        /// Create IFS Function with probabilities
        /// </summary>
        public IfsFunction3D(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33, float b1, float b2, float b3, float p)
        {
            A11 = a11;
            A12 = a12;
            A13 = a13;
            A21 = a21;
            A22 = a22;
            A23 = a23;
            A31 = a31;
            A32 = a32;
            A33 = a33;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            P = p;
        }

        /// <summary>
        /// Create IFS Function from array of coefficients
        /// </summary>
        public IfsFunction3D(float[] coefficients)
        {
            Coefficients = coefficients;
        }

        #endregion

        #region Overriding Members

        /// <summary>
        /// Overriding GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + A11.GetHashCode();
            hash = (hash * 7) + A12.GetHashCode();
            hash = (hash * 7) + A13.GetHashCode();
            hash = (hash * 7) + A21.GetHashCode();
            hash = (hash * 7) + A22.GetHashCode();
            hash = (hash * 7) + A23.GetHashCode();
            hash = (hash * 7) + A23.GetHashCode();
            hash = (hash * 7) + A31.GetHashCode();
            hash = (hash * 7) + A32.GetHashCode();
            hash = (hash * 7) + A33.GetHashCode();
            hash = (hash * 7) + B1.GetHashCode();
            hash = (hash * 7) + B2.GetHashCode();
            hash = (hash * 7) + B3.GetHashCode();
            hash = (hash * 7) + P.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Overriding Object Equals methods
        /// </summary>
        public override bool Equals(object obj)
        {
            var ifsFunction = obj as IfsFunction3D;

            if (ifsFunction != null)
            {
                return Equals(ifsFunction);
            }

            return false;
        }

        /// <summary>
        /// Implementing IEquatable Equals
        /// </summary>
        public bool Equals(IfsFunction3D other)
        {
            return other != null && A11.Equals(other.A11) && A12.Equals(other.A12) && A13.Equals(other.A13) && A21.Equals(other.A21) &&
                   A22.Equals(other.A22) && A23.Equals(other.A23) && A31.Equals(other.A31) && A32.Equals(other.A32) && A33.Equals(other.A33) && B1.Equals(other.B1) && B2.Equals(other.B2) && B3.Equals(other.B3) && P.Equals(other.P);
        }

        /// <summary>
        /// Returns a string representation of 3D IFS
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{A11},{A12},{A13},{A21},{A22},{A23},{A31},{A32},{A33},{B1},{B2},{B3},{P}]";
        }

        /// <summary>
        /// Creates a clone of IfsFunction
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
