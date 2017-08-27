using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection
{
    /// <summary>
    /// Species selection strategy based on vector of probabilities
    /// </summary>
    public class ProbabilityVectorSpeciesSelectionStrategy : SpeciesSelectionStrategy
    {
        /// <summary>
        /// Selects species based on probability vector
        /// </summary>
        public override Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen)
        {
            //Generating a random value between 0.001 and 0.999
            var randomValue = randomGen.NextDouble() * (0.999 - 0.001) + 0.001;

            double partialSum = 0;

            for (int i = 0; i < probabilityVector.Count; i++)
            {
                partialSum += probabilityVector[i];

                if (partialSum >= randomValue)
                {
                    var degree = i + 1;

                        var selectedSpecies =
                            population.Species.Single(x => x.DegreeOfIndividualsInSpecies.Equals(degree));
                    
                    return selectedSpecies;
                }
            }

            return null;
        }

        /// <summary>
        /// Selects two species according to VD and maximum distance between species 
        /// </summary>
        public override Species SelectSecondSpecies(Population population, Species firstSpecies, int maximumDistance, Random randomGen)
        {
            var firstSpeciesDegree = firstSpecies.DegreeOfIndividualsInSpecies;

            //Get possible matches for selection (select all available species within a given distance )
            var possibleMatches =
                population.Species.Where(
                    x =>
                        x.DegreeOfIndividualsInSpecies < firstSpeciesDegree &&
                        x.DegreeOfIndividualsInSpecies >= firstSpeciesDegree - maximumDistance ||
                        x.DegreeOfIndividualsInSpecies <= firstSpeciesDegree + maximumDistance &&
                        x.DegreeOfIndividualsInSpecies > firstSpeciesDegree).ToList();

            if (possibleMatches.Count != 0)
            {
                //select species randomly out of possible matches
                var randomIndex = randomGen.Next(0, possibleMatches.Count - 1);
                var secondSpecies = possibleMatches[randomIndex];

                return secondSpecies;
            }

            return null;
        }
    }
}
