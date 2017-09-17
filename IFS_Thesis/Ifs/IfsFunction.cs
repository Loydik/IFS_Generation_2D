using System;

namespace IFS_Thesis.IFS
{
    /// <summary>
    /// IFS function in 3D
    /// </summary>
    public class IfsFunction : IEquatable<IfsFunction>, ICloneable
    {
        #region Properties

        //Coefficients of affine functions
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

        /// <summary>
        /// Get or set coefficient by index
        /// </summary>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return A11;
                    case 1:
                        return A12;
                    case 2:
                        return A13;
                    case 3:
                        return A21;
                    case 4:
                        return A22;
                    case 5:
                        return A23;
                    case 6:
                        return A31;
                    case 7:
                        return A32;
                    case 8:
                        return A33;
                    case 9:
                        return B1;
                    case 10:
                        return B2;
                    case 11:
                        return B3;
                }

                return 0;
            }
            set
            {
                if (index >= 0 && index <= 11)
                {
                    switch (index)
                    {
                        case 0:
                             A11 = value;
                            break;
                        case 1:
                            A12 = value;
                            break;
                        case 2:
                            A13 = value;
                            break;
                        case 3:
                            A21 = value;
                            break;
                        case 4:
                            A22 = value;
                            break;
                        case 5:
                            A23 = value;
                            break;
                        case 6:
                            A31 = value;
                            break;
                        case 7:
                            A32 = value;
                            break;
                        case 8:
                            A33 = value;
                            break;
                        case 9:
                            B1 = value;
                            break;
                        case 10:
                            B2 = value;
                            break;
                        case 11:
                            B3 = value;
                            break;
                    }
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Create IFS Function without probabilities
        /// </summary>
        public IfsFunction(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33, float b1, float b2, float b3)
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
        public IfsFunction(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33, float b1, float b2, float b3, float p)
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
        public IfsFunction(float[] coefficients)
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
            var ifsFunction = obj as IfsFunction;

            if (ifsFunction != null)
            {
                return Equals(ifsFunction);
            }

            return false;
        }

        /// <summary>
        /// Implementing IEquatable Equals
        /// </summary>
        public bool Equals(IfsFunction other)
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
