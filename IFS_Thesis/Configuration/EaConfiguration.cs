using System.ComponentModel;
using System.Text;

namespace IFS_Thesis.Configuration
{
    /// <summary>
    /// Configuration class for Evolutionary algorithms
    /// </summary>
    public class EaConfiguration
    {
        #region Properties

        public int PopulationSize { get; set; }

        public float N1IndividualsPercentage { get; set; }

        public float N2IndividualsPercentage { get; set; }

        public float N3IndividualsPercentage { get; set; }

        public float N4IndividualsPercentage { get; set; }

        public float MutationProbability { get; set; }

        public float MutationRange { get; set; }

        public float AverageFitnessThreshold { get; set; }

        public float ArithmeticCrossoverProbability { get; set; }

        public float OnePointCrossoverProbability { get; set; }

        public float DiscreteSingelRecombinationProbability { get; set; }

        public float RandomMutationProbability { get; set; }

        public float ControlledMutationProbability { get; set; }

        public int EliteIndividualsPerDegree { get; set; }

        public bool UseReinsertion { get; set; }

        public float ParentsReinserted { get; set; }

        public float OffspringReinserted { get; set; }

        public int UpdateProbabilityVectorAfterNGenerations { get; set; }

        public bool Generate5XIndividualsInBeginning { get; set; }

        public float SelectionPressure { get; set; }

        public bool N2IndividualsFromExistingPoolOfSingels { get; set; }

        public bool GeneticUniversumAtRandom { get; set; }

        public bool RecalculateFitnessAfterReinsertion { get; set; }

        #endregion

        /// <summary>
        /// Gets a string representation of EaConfiguration class
        /// </summary>
        public override string ToString()
        {
            var outputString = new StringBuilder();

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                var name = descriptor.Name;
                var value = descriptor.GetValue(this);
                outputString.Append($"{name} - {value}; \n");
            }

            return outputString.ToString();
        }
    }
}
