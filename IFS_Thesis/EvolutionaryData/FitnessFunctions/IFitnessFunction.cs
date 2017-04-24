﻿using System.Collections.Generic;
using System.Drawing;

namespace IFS_Thesis.EvolutionaryData.FitnessFunctions
{
    public interface IFitnessFunction
    {
        /// <summary>
        /// Calculate fitness for a given individual
        /// </summary>
        float CalculateFitnessForIndividual(List<Point> sourceImagePixels, Individual individual, int width,
            int height);

        /// <summary>
        /// Calculates fintess for given individuals using source Image pixels
        /// </summary>
        List<Individual> CalculateFitnessForIndividuals(List<Individual> individuals,
            List<Point> sourceImagePixels, int imageWidth, int imageHeight);
    }
}