using System.Collections.Generic;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData
{
    public class Individual
    {
        public int Degree { get; set; }

        public List<IfsFunction> Singels { get; set; }

        public float ObjectiveFitness { get; set; }


        public Individual(List<IfsFunction> singels)
        {
            Singels = singels;
            Degree = singels.Count;
        }

        public override string ToString()
        {
            return $"Degree - {Degree}, Fitness - {ObjectiveFitness}\n Singles:{string.Join(";", Singels)}";
        }

        //public static implicit operator List<Singel>(Individual x)
        //{
        //    return x.Singels;
        //}

        //public static explicit operator Individual(List<Singel> x)
        //{
        //    var singels = x.Select(array => new Singel(array)).ToList();

        //    Individual individual = new Individual(singels);

        //    return individual;
        //}
    }
}
