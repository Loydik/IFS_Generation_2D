using System.Collections.Generic;

namespace IFS_Thesis.EvolutionaryData.EvolutionarySubjects
{
    /// <summary>
    /// Species of individuals (same degree)
    /// </summary>
    public class Species
    {
        /// <summary>
        /// Degree of Individuals which belong to given species
        /// </summary>
        public int DegreeOfIndividualsInSpecies { get; set; }

        /// <summary>
        /// List of individuals which belong to a given species
        /// </summary>
        public List<Individual> Individuals { get; set; }

        /// <summary>
        /// Create new empty species with a specified degree
        /// </summary>
        public Species(int degreeOfIndividualsInSpecies)
        {
            DegreeOfIndividualsInSpecies = degreeOfIndividualsInSpecies;
            Individuals = new List<Individual>();
        }
    }
}
