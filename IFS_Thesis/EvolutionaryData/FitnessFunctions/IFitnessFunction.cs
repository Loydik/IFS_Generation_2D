using System;
using System.Collections.Generic;
using IFS_Thesis.EvolutionaryData.EvolutionarySubjects;
using IFS_Thesis.Ifs;
using IFS_Thesis.Ifs.IFSGenerators;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public interface IFitnessFunction
    {
        /// <summary>
        /// Calculate fitness for a given individual
        /// </summary>
        float CalculateFitnessForIndividual(HashSet<Voxel> sourceImageVoxels, Individual individual,
            IfsGenerator ifsGenerator, int imageX, int imageY, int imageZ, int multiplier);

        /// <summary>
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals, HashSet<Voxel> sourceImageVoxels, IfsGenerator ifsGenerator, int imageX, int imageY, int imageZ, int multiplier);
    }
}
