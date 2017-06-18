using System;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;

namespace IFS_Thesis.EvolutionaryData.Selection.SpeciesSelection
{
    public class ProbabilityVectorSpeciesSelectionStrategy : SpeciesSelectionStrategy
    {
        #region Private Methods

        private Species SelectNearestSpecies(Population population, int degree)
        {
            var closestSpecies = population.Species.Aggregate((x, y) => Math.Abs(x.DegreeOfIndividualsInSpecies - degree) < Math.Abs(y.DegreeOfIndividualsInSpecies - degree) ? x : y);

            return closestSpecies;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Selects species based on probability vector
        /// </summary>
        /// <returns></returns>
        public override Species SelectSpecies(Population population, List<float> probabilityVector, Random randomGen)
        {
            double randomValue = randomGen.NextDouble();

            double partialSum = 0;

            for (int i = 0; i < probabilityVector.Count; i++)
            {
                partialSum += probabilityVector[i];

                if (partialSum >= randomValue)
                {
                    var degree = i + 1;

                    Species selectedSpecies;

                    //If the population with a given degree is empty, select the nearest one
                    if (population.Species.All(x => x.DegreeOfIndividualsInSpecies != degree))
                    {
                        selectedSpecies = SelectNearestSpecies(population, degree);
                    }
                    else
                    {
                        selectedSpecies =
                            population.Species.Single(x => x.DegreeOfIndividualsInSpecies.Equals(degree));
                    }

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

            var possibleMatches =
                population.Species.Where(
                    x =>
                        x.DegreeOfIndividualsInSpecies < firstSpeciesDegree &&
                        x.DegreeOfIndividualsInSpecies >= firstSpeciesDegree - maximumDistance ||
                        x.DegreeOfIndividualsInSpecies <= firstSpeciesDegree + maximumDistance &&
                        x.DegreeOfIndividualsInSpecies > firstSpeciesDegree).ToList();

            if (possibleMatches.Count != 0)
            {
                var randomIndex = randomGen.Next(0, possibleMatches.Count - 1);
                var secondSpecies = possibleMatches[randomIndex];

                return secondSpecies;
            }

            return null;
        }

        #endregion
    }
}
