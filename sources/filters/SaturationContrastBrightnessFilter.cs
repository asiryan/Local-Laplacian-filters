using System.Drawing;
using UMapx.Imaging;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines saturation/contrast/exposure/gamma/brightess filter.
    /// </summary>
    public class SaturationContrastBrightnessFilter
    {
        #region Private data
        private SaturationCorrection sc;
        private ContrastEnhancement ce;
        private BrightnessCorrection bc;
        private ShiftCorrection ec;
        private GammaCorrection gc;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes saturation/contrast/exposure/gamma/brightess filter.
        /// </summary>
        public SaturationContrastBrightnessFilter()
        {
            this.ce = new ContrastEnhancement(0, Space.YCbCr);
            this.sc = new SaturationCorrection(0);
            this.bc = new BrightnessCorrection(0, Space.YCbCr);
            this.ec = new ShiftCorrection(0, Space.YCbCr);
            this.gc = new GammaCorrection(0, Space.YCbCr);
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="saturation">Saturation</param>
        /// <param name="contrast">Contrast</param>
        /// <param name="brightness">Brightness</param>
        /// <param name="exposure">Exposure</param>
        /// <param name="gamma">Gamma</param>
        /// <param name="space">Colorspace</param>
        public void SetParams(double saturation, double contrast, double brightness, double exposure, double gamma, Space space)
        {
            this.sc.Saturation = saturation;

            this.ce.Contrast = contrast;
            this.ce.Space = space;

            this.bc.Brightness = brightness;
            this.bc.Space = space;

            this.ec.Offset = exposure;
            this.ec.Space = space;

            this.gc.Gamma = gamma;
            this.gc.Space = space;
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

            if (this.bc.Brightness != 0)
                bc.Apply(clone);

            if (this.ec.Offset != 0)
                ec.Apply(clone);

            if (this.gc.Gamma != 1.0)
                gc.Apply(clone);

            return clone;
        }
        #endregion
    }
}
