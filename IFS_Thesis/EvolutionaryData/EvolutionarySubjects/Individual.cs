using System;
using System.Collections.Generic;
using IFS_Thesis.Ifs;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.EvolutionarySubjects
{
    /// <summary>
    /// Individual in population
    /// </summary>
    public class Individual : ICloneable
    {
        #region Properties

        /// <summary>
        /// The degree of an individual
        /// </summary>
        public int Degree { get; set; }

        /// <summary>
        /// Signels of individual
        /// </summary>
        public List<IfsFunction3D> Singels { get; set; }

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

        /// <summary>
        /// Create individual from a list of singels
        /// </summary>
        public Individual(List<IfsFunction3D> singels)
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
            clonedIndividual.Singels = (List<IfsFunction3D>)Singels.Clone();

            return clonedIndividual;
        }

        #endregion
    }
}
