using System.Drawing;
using UMapx.Imaging;

namespace LaplacianHDR.Filters
{
    /// <summary>
    /// Defines flip filter.
    /// </summary>
    public class FlipFilter
    {
        #region Private data
        private Flip flip;
        #endregion

        #region Filter components
        /// <summary>
        /// Initializes flip filter.
        /// </summary>
        public FlipFilter()
        {
            this.flip = new Flip();
        }
        /// <summary>
        /// Sets filter params.
        /// </summary>
        /// <param name="x">Flip X</param>
        /// <param name="y">Flip Y</param>
        public void SetParams(bool x, bool y)
        {
            this.flip.X = x;
            this.flip.Y = y;
        }
        /// <summary>
        /// Applies filter to bitmap.
        /// </summary>
        /// <param name="image">Bitmap</param>
        /// <returns>Bitmap</returns>
        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();
            flip.Apply(clone);
            return clone;
        }
        #endregion
    }
}
