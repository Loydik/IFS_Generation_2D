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
        public HashSet<Individual> Individuals { get; set; }
    }
}
