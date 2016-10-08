using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData
{
    public class Singel
    {
        public IfsFunction Coefficients { get; set; }

        public Singel(IfsFunction coefficients)
        {
            Coefficients = coefficients;
        }

        public static implicit operator IfsFunction (Singel x)
        {
            return x.Coefficients;
        }

        public static explicit operator Singel(IfsFunction x)
        {
            Singel singel = new Singel(x);

            return singel;
        }
    }
}