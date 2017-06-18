﻿using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class DiscreteRecombinationStrategy : RecombinationStrategy
    {
        private Tuple<Singel, Singel> RecombineSingels(IfsFunction firstSignel, IfsFunction secondSingel, Random randomGen)
        {
            var firstSingelClone = (IfsFunction)firstSignel.Clone();
            var secondSingelClone = (IfsFunction)secondSingel.Clone();

            var result1 = new float[6];
            var result2 = new float[6];

            for (int i = 0; i < 5; i++)
            {
                if (randomGen.NextDouble() >= 0.5)
                    result1[i] = firstSingelClone.Coefficients[i];
                else
                    result1[i] = secondSingelClone.Coefficients[i];

                if (randomGen.NextDouble() >= 0.5)
                    result2[i] = firstSingelClone.Coefficients[i];
                else
                    result2[i] = secondSingelClone.Coefficients[i];
            }

            var offspringSingel1 = new IfsFunction(result1);
            var offspringSingel2 = new IfsFunction(result2);

            return new Tuple<Singel, Singel>(new Singel(offspringSingel1), new Singel(offspringSingel2));
        }

        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstChild = (Individual)firstParent.Clone();
            var secondChild = (Individual)secondParent.Clone();

    
            var singelToExchange = randomGen.Next(firstParent.Degree);

            var childSingels = RecombineSingels(firstChild.Singels[singelToExchange],
                secondChild.Singels[singelToExchange],
                randomGen);

            firstChild.Singels[singelToExchange] = childSingels.Item1;
            secondChild.Singels[singelToExchange] = childSingels.Item2;

            return new List<Individual> { firstChild, secondChild };
        }
    }
}