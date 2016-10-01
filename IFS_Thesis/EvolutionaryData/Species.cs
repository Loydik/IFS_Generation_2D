using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData
{
    public class Species
    {
        public int DegreeOfIndividualsInSpecies { get; set; }

        public List<Individual> Individuals { get; set; }

        public Species(int degreeOfIndividualsInSpecies)
        {
            DegreeOfIndividualsInSpecies = degreeOfIndividualsInSpecies;
            Individuals = new List<Individual>();
        }
    }
}
