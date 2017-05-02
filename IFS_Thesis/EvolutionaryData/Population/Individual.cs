using System;
using System.Collections.Generic;
using IFS_Thesis.Ifs;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Population
{
    public class Individual : ICloneable
    {
        #region Properties

        /// <summary>
        /// The degree of an individual
        /// </summary>
        public int Degree { get; set; }

        /// <summary>
        /// Signels
        /// </summary>
        public List<IfsFunction> Singels { get; set; }

        /// <summary>
        /// The objective fitness of an individual
        /// </summary>
        public float ObjectiveFitness { get; set; }

        /// <summary>
        /// Whether an endividual is elitist (highest fitness and untouched)
        /// </summary>
        public bool Elite { get; set; }

        #endregion

        #region Construction

        public Individual(List<IfsFunction> singels)
        {
            Singels = singels;
            Degree = singels.Count;
        }

        #endregion

        #region Public Methods 

        public override string ToString()
        {
            return $"Degree - {Degree}, Fitness - {ObjectiveFitness}\n Singles:{string.Join(";", Singels)}";
        }

        /// <summary>
        /// Create a deep clone of individual
        /// </summary>
        public object Clone()
        {
            //first we create a shallow copy of the object
            var clonedIndividual = (Individual) MemberwiseClone();

            //then we clone the singels of individual and assign them to cloned individual
            clonedIndividual.Singels = (List<IfsFunction>)Singels.Clone();

            return clonedIndividual;
        }

        #endregion
    }
}
