using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UMapx.Imaging;

namespace LaplacianHDR.Helpers
{
    /// <summary>
    /// Image helper class.
    /// </summary>
    public static class ImageHelper
    {
        #region Static voids
        /// <summary>
        /// Creates a bitmap from the file.
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Open(string filename)
        {
            Bitmap bitmap;
            try
            {
                // try to open image
                Stream stream = new FileStream(filename, FileMode.Open);
                bitmap = new Bitmap(stream);
                stream.Close();
                stream.Dispose();
            }
            catch
            {
                throw new FormatException("Incorrect input image format or file.");
            }

            // check image size
            if (!IsTrueSize(bitmap.Width, bitmap.Height))
            {
                throw new Exception("Input image must be equal or greater than " + 
                    minDimension + " px and equal or lower than " + maxDimension + " px.");
            }

            return bitmap;
        }
        /// <summary>
        /// Returns bitmap array from the files.
        /// </summary>
        /// <param name="files">Files</param>
        /// <returns>Bitmap array</returns>
        public static Bitmap[] Open(string[] files)
        {
            int length = files.Length;
            if (length == 0)
            {
                return null;
            }
            else
            {
                Bitmap[] data = new Bitmap[length];

                for (int i = 0; i < length; i++)
                {
                    data[i] = ImageHelper.Open(files[i]);
                }

                if (!ImageHelper.AreEqualSizes(data))
                    throw new Exception("Input images must be same size.");

                return data;
            }
        }
        /// <summary>
        /// Saves bitmap to the file.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="filename">Filename</param>
        /// <param name="format">ImageFormat</param>
        public static void Save(Bitmap bitmap, string filename, ImageFormat format)
        {
            try
            {
                // try to save image
                Stream stream = new FileStream(filename, FileMode.OpenOrCreate);
                bitmap.Save(stream, format);
                stream.Close();
                stream.Dispose();
            }
            catch
            {
                throw new FormatException("Incorrect output image format or file");
            }
            return;
        }
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
        /// <param name="bitmap">Bitmap</param>
        /// <param name="box">Box size</param>
        /// <returns>Bitmap</returns>
        public static Bitmap Crop(Bitmap bitmap, int box)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int min = Math.Min(width, height);
            double k = min / (double)box;

            return (new Bitmap(bitmap, (int)(width / k + 1), (int)(height / k + 1))).
                Clone(new Rectangle(0, 0, box, box), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        /// <summary>
        /// Checks if bitmaps in array have the same sizes.
        /// </summary>
        /// <param name="bitmaps">Bitmap array</param>
        /// <returns>Boolean value</returns>
        private static bool AreEqualSizes(params Bitmap[] bitmaps)
        {
            int length = bitmaps.Length;
            bool equals = false;

            if (length > 0)
            {
                Size size = bitmaps[0].Size;
                equals = true;

                for (int i = 1; i < length; i++)
                {
                    if (size != bitmaps[i].Size)
                    {
                        equals = false;
                        break;
                    }
                }
            }

            return equals;
        }
        /// <summary>
        /// Checks bitmap sizes.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Bool</returns>
        private static bool IsTrueSize(int width, int height)
        {
            // check this
            return IsRange(width, minDimension, maxDimension) && 
                IsRange(height, minDimension, maxDimension);
        }
        /// <summary>
        /// Checks is value in range.
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="min">Min</param>
        /// <param name="max">Max</param>
        /// <returns>Bool</returns>
        private static bool IsRange(int x, int min, int max)
        {
            return x >= min && x <= max;
        }
        /// <summary>
        /// Minimum dimension.
        /// </summary>
        private const int minDimension = (int)1e2;
        /// <summary>
        /// Maximum dimension.
        /// </summary>
        private const int maxDimension = (int)4e3;
        #endregion
    }
}