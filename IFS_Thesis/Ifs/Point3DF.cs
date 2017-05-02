namespace IFS_Thesis.Ifs
{
    /// <summary>
    /// Represents 3d floating point 
    /// </summary>
    public class Point3Df
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Create a 3d floating point
        /// </summary>
        public Point3Df(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
