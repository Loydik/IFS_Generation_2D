using System;

namespace IFS_Thesis.Utils
{
    public class IfsFunction : IEquatable<IfsFunction>
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }
        public float E { get; set; }
        public float F { get; set; }

        public double P { get; set; }

        /// <summary>
        /// Coefficients expressed in array form
        /// </summary>
        public float[] Coefficients
        {
            get { return new[] { A, B, C, D, E, F }; }
            set
            {
                A = value[0];
                B = value[1];
                C = value[2];
                D = value[3];
                E = value[4];
                F = value[5];
            }
        }

        public IfsFunction(float a, float b, float c, float d, float e, float f, float p)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            P = p;
        }

        /// <summary>
        /// Overriding GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 13;

            hash = (hash * 7) + A.GetHashCode();
            hash = (hash * 7) + B.GetHashCode();
            hash = (hash * 7) + C.GetHashCode();
            hash = (hash * 7) + D.GetHashCode();
            hash = (hash * 7) + E.GetHashCode();
            hash = (hash * 7) + F.GetHashCode();
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
            return other != null && A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C) && D.Equals(other.D) &&
                   E.Equals(other.E) && F.Equals(other.F) && P.Equals(other.P);
        }
    }
}
