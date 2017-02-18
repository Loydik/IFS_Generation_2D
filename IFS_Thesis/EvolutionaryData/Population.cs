﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData
{
    public class Population
    {
        public List<Species> Species { get; set; }

        public List<Individual> Individuals => GetAllIndividuals();

        public int Count => Individuals.Count;

        public Population()
        {
            Species =  new List<Species>();
        }

        //for testing only
        public Population(List<Species> species)
        {
            Species = species;
        }

        public void AddIndividual(Individual individual)
        {
            bool existingSpecies = Species.Any(x => x.DegreeOfIndividualsInSpecies == individual.Degree);

            if (existingSpecies)
            {
                Species.Single(x => x.DegreeOfIndividualsInSpecies == individual.Degree).Individuals.Add(individual);
            }

            else
            {
              var newSpecies = new Species(individual.Degree);

              newSpecies.Individuals.Add(individual);

              Species.Add(newSpecies);

            }
        }

        public void AddIndividuals(List<Individual> individuals)
        {
            foreach (var individual in individuals)
            {
                AddIndividual(individual);
            }
        }

        public void RemoveIndividual(Individual individual)
        {
             Species.Single(x => x.DegreeOfIndividualsInSpecies == individual.Degree).Individuals.Remove(individual);
        }

        public List<Individual> GetAllIndividuals()
        {
            var allIndividuals = new List<Individual>();

            foreach (var species in Species)
            {
                allIndividuals.AddRange(species.Individuals);
            }

            return allIndividuals;
        }

        public void SetAllIndividuals(List<Individual> newIndividuals)
        {
            Species.Clear();

            var allDegrees = OtherUtils.GetDegreesOfIndividuals(newIndividuals);

            foreach (var degree in allDegrees)
            {
                var newSpecies = new Species(degree)
                {
                    Individuals = newIndividuals.Where(x => x.Degree.Equals(degree)).ToList()
                };

                Species.Add(newSpecies);
            }
        }

        public void RemoveSpecies(Species species)
        {
            Species.Remove(species);
        }

        /// <summary>
        /// Gets all singles in population (gene pool)
        /// </summary>
        public List<Singel> GetAllSingels()
        {
            List<Singel> singels = new List<Singel>();

            foreach (var individual in Individuals)
            {
                singels.AddRange(individual.Singels.Select(singel => new Singel(singel)));
            }

            return singels;
        }
    }
}
