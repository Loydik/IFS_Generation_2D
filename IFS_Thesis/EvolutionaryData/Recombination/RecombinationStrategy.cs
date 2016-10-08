using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFS_Thesis.EvolutionaryData.Recombination
{
    public abstract class RecombinationStrategy
    {
        public abstract List<Individual> ProduceOffsprings(Individual firstParent, Individual secondParent, Random randomGen);
    }
}
