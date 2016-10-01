using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData
{
    public class SelectionAlgorithms
    {


        #region Older Methods

        ///// <summary>
        ///// Selects an item using roulette selection
        ///// </summary>
        //private int RouletteSelect(List<float> weights, Random randomGen)
        //{
        //    // calculate the total weight
        //    double weight_sum = 0;

        //    for (int i = 0; i < weights.Count; i++)
        //    {
        //        weight_sum += weights[i];
        //    }

        //    // get a random value
        //    double value = randomGen.NextDouble();

        //    // locate the random value based on the weights
        //    for (int i = 0; i < weights.Count; i++)
        //    {
        //        value -= weights[i];
        //        if (value <= 0) return i;
        //    }

        //    return weights.Count - 1;
        //}

        ///// <summary>
        ///// Private selection using Roulette Select
        ///// </summary>
        //private List<Individual> SelectIndividuals(List<Individual> selectionPool, int numberOfIndividualsToSelect, Random randomGen)
        //{
        //    List<Individual> selectedIndividuals = new List<Individual>();

        //    //selective pressure - hard coding for now
        //    float sp = 1.5f;

        //    //linear ranking

        //    var numberOfIndividuals = selectionPool.Count;

        //    selectionPool = selectionPool.OrderBy(x => x.ObjectiveFitness).ToList();

        //    int position = 1;

        //    foreach (var individual in selectionPool)
        //    {
        //        individual.LinearRank = 2 - sp + 2 * (sp - 1) * (position - 1) / (numberOfIndividuals - 1);
        //        position++;
        //    }

        //    selectionPool = selectionPool.OrderByDescending(x => x.LinearRank).ToList();

        //    var totalSumOfFitnesses = selectionPool.Aggregate(0f, (current, element) => current + element.LinearRank);

        //    List<float> individualWeights = selectionPool.Select(individual => individual.LinearRank / totalSumOfFitnesses).ToList();

        //    for (int i = 0; i < numberOfIndividualsToSelect; i++)
        //    {
        //        selectedIndividuals.Add(selectionPool.ElementAt(RouletteSelect(individualWeights, randomGen)));
        //    }

        //    return selectedIndividuals;
        //}

        #endregion

    }
}
