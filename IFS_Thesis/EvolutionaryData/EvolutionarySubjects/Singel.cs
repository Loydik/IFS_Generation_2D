using IFS_Thesis.Utils;

namespace IFS_Thesis.EvolutionaryData.EvolutionarySubjects
{
    /// <summary>
    /// Single genotypic element (single Ifs function)
    /// </summary>
    public class Singel
    {
        /// <summary>
        /// Coefficients of a singel
        /// </summary>
        public IfsFunction Coefficients { get; set; }

        public Singel(IfsFunction coefficients)
        {
            Coefficients = coefficients;
        }

        #region Converting to and from IfsFunction

        public static implicit operator IfsFunction(Singel x)
        {
            return x.Coefficients;
        }

        public static explicit operator Singel(IfsFunction x)
        {
            Singel singel = new Singel(x);

            return singel;
        }

        #endregion
    }
}