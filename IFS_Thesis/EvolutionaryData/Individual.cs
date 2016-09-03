using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData
{
    public class Individual
    {
        public int Degree { get; set; }
        public List<Singel> Singels { get; set; }

        public Individual(List<Singel> singels )
        {
            Singels = singels;
        }

        public static implicit operator List<double[]>(Individual x)
        {
            return x.Singels.Select(singel => singel.Coefficients).ToList();
        }

        public static explicit operator Individual(List<double[]> x)
        {
            var singels = x.Select(array => new Singel(array)).ToList();

            Individual individual = new Individual(singels);

            return individual;
        }
    }
}
