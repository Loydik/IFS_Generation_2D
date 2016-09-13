using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis
{
    public class IfsFunction
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }
        public float E { get; set; }
        public float F { get; set; }

        public double P { get; set; }

        public IfsFunction(float a, float b, float c, float d, float e, float f, float p )
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            P = p;
        }
    }
}
