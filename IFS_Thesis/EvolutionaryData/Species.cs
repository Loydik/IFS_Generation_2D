using System.Collections.Generic;

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
