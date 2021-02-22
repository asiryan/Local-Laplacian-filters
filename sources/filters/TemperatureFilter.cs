using System.Drawing;
using UMapx.Imaging;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines temperature filter.
    /// </summary>
    public class TemperatureFilter
    {
        #region Private data
        TemperatureCorrection temp;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes temperature filter.
        /// </summary>
        public TemperatureFilter()
        {
            temp = new TemperatureCorrection();
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="temperature">Temperature</param>
        /// <param name="strenght">Strength</param>
        public void SetParams(float temperature, float strenght)
        {
            this.temp.Temperature = temperature;
            this.temp.Strength = strenght;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();

            if (this.temp.Strength != 0)
            {
                temp.Apply(clone);
            }

            return clone;
        }
        #endregion
    }
}
