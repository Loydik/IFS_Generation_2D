﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData
{
    public class Individual
    {
        public int Degree { get; set; }

        public List<IfsFunction> Singels { get; set; }

        public Individual(List<IfsFunction> singels )
        {
            Singels = singels;
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
