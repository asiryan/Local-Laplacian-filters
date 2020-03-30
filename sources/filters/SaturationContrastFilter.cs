using System.Drawing;
using UMapx.Imaging;

namespace LocalLaplacianFilters.Filters
{
    /// <summary>
    /// Defines saturation/contrast filter.
    /// </summary>
    public class SaturationContrastFilter
    {
        #region Private data
        private SaturationCorrection sc;
        private ContrastEnhancement ce;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes saturation/contrast filter.
        /// </summary>
        public SaturationContrastFilter()
        {
            this.ce = new ContrastEnhancement(0, Space.YCbCr);
            this.sc = new SaturationCorrection(0);
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="saturation">Saturation</param>
        /// <param name="contrast">Contrast</param>
        /// <param name="space">Colorspace</param>
        public void SetParams(double saturation, double contrast, Space space)
        {
            this.sc.Saturation = saturation;
            this.ce.Contrast = contrast;
            this.ce.Space = space;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();

            if (sc.Saturation != 0)
                sc.Apply(clone);

            if (ce.Contrast != 0)
                ce.Apply(clone);

            return clone;
        }
        #endregion
    }
}
