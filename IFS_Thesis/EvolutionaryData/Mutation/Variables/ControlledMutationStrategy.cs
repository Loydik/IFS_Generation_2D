using System;

namespace IFS_Thesis.EvolutionaryData.Mutation.Variables
{
    public class ControlledMutationStrategy : RealValueMutationStrategy
    {
        /// <summary>
        /// Mutation operator of the Breeder Genetic Algorithm
        /// </summary>
        /// <remarks>produces small step-sizes with a high probability and large step-sizes with a low probability.</remarks>>
        public override float MutateVariable(float variable, Random randomGen, Tuple<int, int> range, float mutationRange)
        {
            //General formula (taken from GEATbx) -> Var = Var + s * r * a

            //s = {-1; +1} uniform at random
            var sRange = new[] { -1, 1 };
            var s = sRange[randomGen.Next(0, 2)];

            //r -> mutation range, a standard of 10% was chosen
            var r = mutationRange;

            //u ∈ [0,1] uniform at random
            var u =  randomGen.NextDouble();

            //mutation precision, which defines minimum step size, k ∈ {4,5,...20}
            var k = 8;

            //a = 2^(-u*k)
            var a = (float)Math.Pow(2, (-1) * u * k);

            var tempVariable =  variable + s * r * a;

            //Clipping allowed range overflows
            if (tempVariable > range.Item2)
            {
                tempVariable = range.Item2;
            }
            else if (tempVariable < range.Item1)
            {
                tempVariable = range.Item1;
            }

            return tempVariable;
        }
    }
}
