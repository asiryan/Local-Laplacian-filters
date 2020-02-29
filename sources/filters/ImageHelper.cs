using System;
using System.Drawing;
using System.Drawing.Imaging;
using UMapx.Imaging;

namespace LocalLaplacianFilters.Filters
{
    /// <summary>
    /// Image helper class.
    /// </summary>
    public static class ImageHelper
    {
        #region Static voids
        /// <summary>
        /// Returns image format.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Image format</returns>
        public static ImageFormat GetImageFormat(int index)
        {
            switch (index)
            {
                case 1:
                    return ImageFormat.Bmp;
                case 2:
                    return ImageFormat.Jpeg;
                case 3:
                    return ImageFormat.Png;
                case 4:
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Tiff;
            }
        }
        /// <summary>
        /// Returns colorspace.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Colorspace</returns>
        public static Space GetSpace(int index)
        {
            switch (index)
            {
                case 0:
                    return Space.YCbCr;
                case 1:
                    return Space.HSB;
                case 2:
                    return Space.HSL;
                default:
                    return Space.Grayscale;
            }
        }
        /// <summary>
        /// Returns cropped bitmap.
        /// </summary>
        /// <param name="value">Bitmap</param>
        /// <param name="box">Box size</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Crop(Bitmap value, int box)
        {
            int width = value.Width;
            int height = value.Height;
            int min = Math.Min(width, height);
            double k = min / (double)box;

            return (new Bitmap(value, (int)(width / k + 1), (int)(height / k + 1))).
                Clone(new Rectangle(0, 0, box, box), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        #endregion
    }
}
