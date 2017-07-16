﻿using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.EvolutionarySubjects
{
    /// <summary>
    /// Population of individuals
    /// </summary>
    public class Population
    {
        #region Properties

        /// <summary>
        /// List of Species in current population
        /// </summary>
        public List<Species> Species { get; set; }

        /// <summary>
        /// List of individuals in current population
        /// </summary>
        public List<Individual> Individuals
        {
            get { return GetAllIndividuals(); }
            set
            {
                if (value != null)
                {
                    SetAllIndividuals(value);
                }
            }
        }

        /// <summary>
        /// Total count of individuals in population
        /// </summary>
        public int Count => Individuals.Count;

        #endregion

        #region Construction

        /// <summary>
        /// Create new empty population
        /// </summary>
        public Population()
        {
            Species =  new List<Species>();
        }

        /// <summary>
        /// for testing only
        /// </summary>
        public Population(List<Species> species)
        {
            Species = species;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add individual to population
        /// </summary>
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

        /// <summary>
        /// Add a list of individuals to population
        /// </summary>
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

        /// <summary>
        /// Replaces worst individuals in population
        /// </summary>
        public void ReplaceWorstIndividuals(List<Individual> newIndividuals)
        {
            var allIndividuals = GetAllIndividuals();
            var worst = allIndividuals.OrderBy(x => x.ObjectiveFitness).Take(newIndividuals.Count).ToList();
            allIndividuals.RemoveAll(x => worst.Contains(x));
            allIndividuals.AddRange(newIndividuals);
            SetAllIndividuals(allIndividuals);
        }

        /// <summary>
        /// Replaces worst individuals in population of a given degree
        /// </summary>
        public void ReplaceWorstIndividualsOfDegree(int degree, List<Individual> newIndividuals)
        {
            var allIndividuals = GetAllIndividuals();
            var worst = allIndividuals.Where(x => x.Degree == degree).OrderBy(x => x.ObjectiveFitness).Take(newIndividuals.Count).ToList();
            allIndividuals.RemoveAll(x => worst.Contains(x));
            allIndividuals.AddRange(newIndividuals);
            SetAllIndividuals(allIndividuals);
        }

        /// <summary>
        /// Get all individuals which belong to the population
        /// </summary>
        public List<Individual> GetAllIndividuals()
        {
            var allIndividuals = Species.SelectMany(x => x.Individuals).ToList();

            return allIndividuals;
        }

        public void SetAllIndividuals(List<Individual> newIndividuals)
        {
            Species.Clear();

            var allDegrees = EaUtils.GetDegreesOfIndividuals(newIndividuals);

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

        #endregion
    }
}
