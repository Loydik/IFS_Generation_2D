using IFS_Thesis.Properties;

namespace IFS_Thesis.Configuration
{
    /// <summary>
    /// Class which creates configuration
    /// </summary>
    public class EaConfigurator
    {
        /// <summary>
        /// Gets default (base) configuration
        /// </summary>
        public static EaConfiguration GetDefaultConfiguration()
        {
            var config = new EaConfiguration
            {
                PopulationSize = Settings.Default.PopulationSize,
                N1IndividualsPercentage = Settings.Default.N1IndividualsPercentage,
                N2IndividualsPercentage = Settings.Default.N2IndividualsPercentage,
                N3IndividualsPercentage = Settings.Default.N3IndividualsPercentage,
                N4IndividualsPercentage = Settings.Default.N4IndividualsPercentage,
                ArithmeticCrossoverProbability = Settings.Default.ArithmeticCrossoverProbability,
                OnePointCrossoverProbability = Settings.Default.OnePointCrossoverProbability,
                DiscreteRecombinationProbability = Settings.Default.DiscreteRecombinationProbability,
                AverageFitnessThreshold = Settings.Default.AverageFitnessThreshold,
                ControlledMutationProbability = Settings.Default.ControlledMutationProbability,
                RandomMutationProbability = Settings.Default.RandomMutationProbability,
                EliteIndividualsPerDegree = Settings.Default.EliteIndividualsPerDegree,
                Generate5XIndividualsInBeginning = Settings.Default.Generate5xIndividuals,
                GeneticUniversumAtRandom = Settings.Default.GeneticUniversumAtRandom,
                SelectionPressure = Settings.Default.SelectionPressure,
                UseReinsertion = Settings.Default.UseReinsertion,
                OffspringReinserted = Settings.Default.OffspringReinserted,
                ParentsReinserted = Settings.Default.ParentsReinserted,
                MutationProbability = Settings.Default.MutationProbability,
                MutationRange = Settings.Default.MutationRange,
                UpdateProbabilityVectorAfterNGenerations = Settings.Default.UpdateProbabilityVectorAfterNGenerations,
                RecalculateFitnessAfterReinsertion = Settings.Default.RecalculateFitnessAfterReinsertion
            };

            return config;
        }
    }
}
