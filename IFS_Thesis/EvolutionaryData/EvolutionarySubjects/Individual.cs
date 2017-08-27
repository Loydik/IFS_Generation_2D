using System;
using System.Collections.Generic;
using IFS_Thesis.IFS;
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
        /// The degree of given individual (number of IFS mappings)
        /// </summary>
        public int Degree { get; set; }

        /// <summary>
        /// Signels of individual
        /// </summary>
        public List<IfsFunction> Singels { get; set; }

        /// <summary>
        /// The objective fitness of an individual
        /// </summary>
        public float ObjectiveFitness { get; set; }

        /// <summary>
        /// The ranking fitness of an individual
        /// </summary>
        public float RankFitness { get; set; }

        /// <summary>
        /// Whether an individual is elite (will not be mutated)
        /// </summary>
        public bool Elite { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public Individual()
        {

        }

        /// <summary>
        /// Create individual from a list of singels
        /// </summary>
        public Individual(List<IfsFunction> singels)
        {
            Singels = singels;
            Degree = singels.Count;
        }

        /// <summary>
        /// Create individual from a string representation of singels
        /// </summary>
        /// <param name="singelsString">Singels as a string in format [-0.3342,-0.4563,0.2808,-0.25,0.7359,-0.3295,0.1859,0.0776,0.3743,-1.3749,2.327,1.4122,0];[-0.3346,-0.4565,0.281,-0.2656,0.7463,-0.3348,0.1857,0.0778,0.3742,-1.3727,2.3189,1.412,0];</param>
        public Individual(string singelsString)
        {
            var individualSingels = new List<IfsFunction>();
            var singels = singelsString.Trim().Split(';');

            foreach (var singel in singels)
            {
                char[] charsToTrim = { '[', ']' };
                var coefficientts = singel.Trim(charsToTrim).Split(',');

                individualSingels.Add(new IfsFunction(float.Parse(coefficientts[0]), float.Parse(coefficientts[1]), float.Parse(coefficientts[2]), float.Parse(coefficientts[3]), float.Parse(coefficientts[4]), float.Parse(coefficientts[5]), float.Parse(coefficientts[6]), float.Parse(coefficientts[7]), float.Parse(coefficientts[8]), float.Parse(coefficientts[9]), float.Parse(coefficientts[10]), float.Parse(coefficientts[11]), float.Parse(coefficientts[12])));
            }

            Singels = individualSingels;
            Degree = individualSingels.Count;
        }

        #endregion

        #region Public Methods 

        public override string ToString()
        {
            return $"Degree - {Degree}, ObjectiveFitness - {ObjectiveFitness}, RankFitness - {RankFitness}, Elite - {Elite} Singles:{string.Join(";", Singels)} \n";
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
