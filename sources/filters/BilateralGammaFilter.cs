using System;
using UMapx.Imaging;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines bilateral gamma correction filter.
    /// </summary>
    public class BilateralGammaCorrection : Correction
    {
        #region Private data
        private float g;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes bilateral gamma correction filter.
        /// </summary>
        /// <param name="g">Gamma</param>
        /// <param name="space">Space</param>
        public BilateralGammaCorrection(float g, Space space)
        {
            Value = g; Space = space;
        }
        /// <summary>
        /// Gets or sets gamma value.
        /// </summary>
        public float Value
        {
            get
            {
                return this.g;
            }
            set
            {
                this.g = value;
                this.rebuild = true;
            }
        }
        /// <summary>
        /// Implements filter rebuilding.
        /// </summary>
        protected override void Rebuild()
        {
            this.values = BilateralGammaCorrection.Gamma(this.g, 256);
        }
        #endregion

        #region Static voids
        /// <summary>
        /// Returns array of bilateral gamma filter.
        /// </summary>
        /// <param name="g">Gamma</param>
        /// <param name="length">Length</param>
        /// <returns>Array</returns>
        public static float[] Gamma(float g, int length)
        {
            float[] table = new float[length];

            for (int x = 0; x < length; x++)
            {
                table[x] = Gamma(x / (float)length, g);
            }
            return table;
        }
        /// <summary>
        /// Returns bilateral gamma function.
        /// </summary>
        /// <param name="x">Argument</param>
        /// <param name="g">Gamma</param>
        /// <returns>Double</returns>
        public static float Gamma(float x, float g)
        {
            double y, z, w;

            // highlights
            if (x >= 0.5)
            {
                z = (1.0 - x) * 2.0;
                w = Math.Pow(z, g);
                y = 1.0 - w / 2.0;
            }
            // shadows
            else
            {
                z = x * 2.0;
                w = Math.Pow(z, g);
                y = w / 2.0;
            }

            // check
            if (double.IsNaN(y))
                return 1.0f;

            return (float)y;
        }
        #endregion
    }
}
