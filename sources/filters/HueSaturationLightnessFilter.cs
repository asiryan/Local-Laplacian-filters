using System.Drawing;
using UMapx.Imaging;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines colors filter filter.
    /// </summary>
    public class HueSaturationLightnessFilter
    {
        #region Private data
        private HSLFilter hsl;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes colors filter.
        /// </summary>
        public HueSaturationLightnessFilter()
        {
            this.hsl = new HSLFilter(0, 0, 0);
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="h">Hue</param>
        /// <param name="s">Saturation</param>
        /// <param name="l">Lightness</param>
        public void SetParams(double h, double s, double l)
        {
            this.hsl.Hue = h;
            this.hsl.Saturation = s;
            this.hsl.Lightness = l;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();
            hsl.Apply(clone);
            return clone;
        }
        #endregion
    }
}
