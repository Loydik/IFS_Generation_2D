using IFS_Thesis.Ifs;

namespace IFS_Thesis.EvolutionaryData.Population
{
    /// <summary>
    /// Single genotypic element (single Ifs function)
    /// </summary>
    public class Singel
    {
        /// <summary>
        /// Coefficients of a singel
        /// </summary>
        public IfsFunction3D Coefficients { get; set; }

        public Singel(IfsFunction3D coefficients)
        {
            Coefficients = coefficients;
        }

        #region Converting to and from IfsFunction3D

        public static implicit operator IfsFunction3D(Singel x)
        {
            return x.Coefficients;
        }

        public static explicit operator Singel(IfsFunction3D x)
        {
            Singel singel = new Singel(x);

            return singel;
        }

        #endregion
    }
}