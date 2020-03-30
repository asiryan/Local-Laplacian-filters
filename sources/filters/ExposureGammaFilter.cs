using System.Drawing;
using UMapx.Imaging;

namespace LocalLaplacianFilters.Filters
{
    /// <summary>
    /// Defines exposure/gamma filter.
    /// </summary>
    public class ExposureGammaFilter
    {
        #region Private data
        private BrightnessCorrection bc;
        private ShiftCorrection sc;
        private GammaCorrection gc;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes exposure/gamma filter.
        /// </summary>
        public ExposureGammaFilter()
        {
            this.bc = new BrightnessCorrection(0, Space.YCbCr);
            this.sc = new ShiftCorrection(0, Space.YCbCr);
            this.gc = new GammaCorrection(0, Space.YCbCr);
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="brightness">Brightness</param>
        /// <param name="exposure">Exposure</param>
        /// <param name="gamma">Gamma</param>
        /// <param name="space">Colorspace</param>
        public void SetParams(double brightness, double exposure, double gamma, Space space)
        {
            this.bc.Brightness = brightness;
            this.bc.Space = space;

            this.sc.Offset = exposure;
            this.sc.Space = space;

            this.gc.Gamma = gamma;
            this.gc.Space = space;
            return;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();

            if (this.bc.Brightness != 0)
                bc.Apply(clone);

            if (this.sc.Offset != 0)
                sc.Apply(clone);

            if (this.gc.Gamma != 1.0)
                gc.Apply(clone);

            return clone;
        }
        #endregion
    }
}
