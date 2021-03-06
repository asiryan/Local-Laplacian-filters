﻿using System.Drawing;
using UMapx.Imaging;
using UMapx.Transform;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines generalized local Laplacian filter.
    /// </summary>
    public class GeneralizedLocalLaplacianFilter
    {
        #region Private data
        private BilateralGammaCorrection bgc;
        private LocalLaplacianFilter llf;
        private BitmapFilter filter;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes generalized local Laplacian filter.
        /// </summary>
        public GeneralizedLocalLaplacianFilter()
        {
            this.bgc = new BilateralGammaCorrection(0, Space.YCbCr);
            this.llf = new LocalLaplacianFilter();
            this.filter = new BitmapFilter(this.llf, Space.YCbCr);
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="radius">Radius</param>
        /// <param name="lightshadows">Lights and shadows</param>
        /// <param name="sigma">Sigma</param>
        /// <param name="discrets">Number of samples</param>
        /// <param name="levels">Number of levels</param>
        /// <param name="factor">Factor</param>
        /// <param name="space">Colorspace</param>
        public void SetParams(int radius, float lightshadows, float sigma, int discrets, int levels, float factor, Space space)
        {
            this.bgc.Value = lightshadows;
            this.bgc.Space = space;

            this.llf.Radius = radius;
            this.llf.Sigma = sigma;
            this.llf.N = discrets;
            this.llf.Levels = levels;
            this.llf.Factor = factor;

            this.filter.Filter = this.llf;
            this.filter.Space = space;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();

            if (this.bgc.Value != 1)
                this.bgc.Apply(clone);
            
            if (this.llf.Factor != 0)
                this.filter.Apply(clone);

            return clone;
        }
        #endregion
    }
}
