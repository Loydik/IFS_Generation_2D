using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IFS_Thesis.EvolutionaryData.Mutation.Variables;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Mutation.Individuals
{
    public class StandardMutationRateStrategy : IndividualMutationStrategy
    {
        /// <summary>
        /// Mutates an Individual using real-valued mutation
        /// </summary>
        public override void Mutate(ref Individual individual, RealValueMutationStrategy strategy, Random randomGen, float? mutationRate = null)
        {
            if(mutationRate == null)
            { 
                //by default, mutation rate is 1/n (n -> number of variables/coefficients)
                mutationRate = 1f / (individual.Singels.Count*6f);
            }

            var newSingels = new List<IfsFunction>();

            //var mutatedIndividual = new Individual(individualSingels);

            //can be optimized
            foreach (var singel in individual.Singels)
            {
                var tempCoefficients = new List<float>();

                foreach (var coefficient in singel.Coefficients)
                {
                    var tempCoefficient = coefficient;

                    bool mutate = mutationRate >= randomGen.NextDouble();

                    if (mutate)
                    {
                        tempCoefficient = strategy.Mutate(tempCoefficient, randomGen);
                    }

                    tempCoefficients.Add(tempCoefficient);
                }

                newSingels.Add(new IfsFunction(tempCoefficients.ToArray()));
            }

            individual.Singels = newSingels;

            //mutatedIndividual.Singels = individualSingels;

            //individual = mutatedIndividual;
        }
    }

}
