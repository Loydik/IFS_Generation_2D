﻿using IFS_Thesis.Properties;

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
                DiscreteSingelRecombinationProbability = Settings.Default.DiscreteSingelRecombinationProbability,
                EliteFitnessThreshold = Settings.Default.EliteFitnessThreshold,
                ControlledMutationProbability = Settings.Default.ControlledMutationProbability,
                RandomMutationProbability = Settings.Default.RandomMutationProbability,
                EliteIndividualsPerDegree = Settings.Default.EliteIndividualsPerDegree,
                SelectionPressure = Settings.Default.SelectionPressure,
                MutationProbability = Settings.Default.MutationProbability,
                MutationRange = Settings.Default.MutationRange
            };

            return config;
        }
    }
}
