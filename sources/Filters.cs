using System;
using System.Drawing;
using UMapx.Imaging;
using UMapx.Transform;

namespace LocalLaplacianFilters
{
    public class BilateralGammaCorrection : Correction
    {
        #region Private data
        private double g;
        #endregion

        #region Filter components
        public BilateralGammaCorrection(double g, Space space)
        {
            Value = g; Space = space;
        }
        public double Value
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
        public static double[] Gamma(double g, int length)
        {
            double[] table = new double[length];

            for (int x = 0; x < length; x++)
            {
                table[x] = Gamma(x / (double)length, g);
            }
            return table;
        }
        public static double Gamma(double x, double g)
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
                return 1.0;

            return y;
        }
        #endregion
    }

    public class GeneralizedLocalLaplacianFilter
    {
        #region Private data
        BilateralGammaCorrection bgc;
        LocalLaplacianFilter llf;
        BitmapFilter filter;
        #endregion

        #region Filter components
        public GeneralizedLocalLaplacianFilter()
        {
            this.bgc = new BilateralGammaCorrection(0, Space.YCbCr);
            this.llf = new LocalLaplacianFilter();
            this.filter = new BitmapFilter(this.llf, Space.YCbCr);
        }

        public void SetParams(double lightshadows, double sigma, int discrets, int levels, double factor, Space space)
        {
            this.bgc.Value = lightshadows;
            this.bgc.Space = space;

            this.llf.Sigma = sigma;
            this.llf.N = discrets;
            this.llf.Levels = levels;
            this.llf.Factor = factor;

            this.filter.Filter = this.llf;
            this.filter.Space = space;
        }

        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();
            this.bgc.Apply(clone);
            this.filter.Apply(clone);
            return clone;
        }
        #endregion
    }

    public class ExposureGammaFilter
    {
        #region Private data
        BrightnessCorrection bc;
        ShiftCorrection sc;
        GammaCorrection gc;
        #endregion

        #region Filter components
        public ExposureGammaFilter()
        {
            this.bc = new BrightnessCorrection(0, Space.YCbCr);
            this.sc = new ShiftCorrection(0, Space.YCbCr);
            this.gc = new GammaCorrection(0, Space.YCbCr);
        }

        public void SetParams(double brightness, double exposure, double gamma, Space space)
        {
            this.bc.Brightness = brightness;
            this.bc.Space = space;

            this.sc.Offset = exposure;
            this.sc.Space = space;

            this.gc.Gamma = gamma;
            this.gc.Space = space;
        }

        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();
            bc.Apply(clone); sc.Apply(clone); gc.Apply(clone);
            return clone;
        }
        #endregion
    }

    public class SaturationContrastFilter
    {
        #region Private data
        SaturationCorrection sc;
        ContrastEnhancement ce;
        #endregion

        #region Filter components
        public SaturationContrastFilter()
        {
            this.ce = new ContrastEnhancement(0, Space.YCbCr);
            this.sc = new SaturationCorrection(0);
        }

        public void SetParams(double saturation, double contrast, Space space)
        {
            this.sc.Saturation = saturation;

            this.ce.Contrast = contrast;
            this.ce.Space = space;
        }

        public Bitmap Apply(Bitmap image)
        {
            Bitmap clone = (Bitmap)image.Clone();
            sc.Apply(clone); ce.Apply(clone);
            return clone;
        }
        #endregion
    }

    public static class ImageHelper
    {
        #region Static voids
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
