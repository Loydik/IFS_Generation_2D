﻿using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;


namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public class OnePointCrossoverStrategy : RecombinationStrategy
    {
        public override List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen)
        {
            if (firstParent == null || secondParent == null || firstParent.Degree != secondParent.Degree)
            {
                return new List<Individual>();
            }

            //cloning to prevent unexpected behavior
            var firstParentClone = (Individual)firstParent.Clone();
            var secondParentClone = (Individual)secondParent.Clone();

            //we get the crossover point at random
            var crossoverPoint = randomGen.Next(1, firstParentClone.Degree - 1);

            var firstSingels = new List<IfsFunction3D>();
            var secondSingels = new List<IfsFunction3D>();

            for (int i = 0; i < firstParentClone.Degree; i++)
            {
                if (i >= crossoverPoint)
                {
                    firstSingels.Add(secondParentClone.Singels[i]);
                    secondSingels.Add(firstParentClone.Singels[i]);
                }
                else
                {
                    firstSingels.Add(firstParentClone.Singels[i]);
                    secondSingels.Add(secondParentClone.Singels[i]);
                }
            }

            Individual individual1 = new Individual(firstSingels);
            Individual individual2 = new Individual(secondSingels);

            return new List<Individual> { individual1, individual2 };
        }
    }
}
