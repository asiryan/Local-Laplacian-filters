using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;
using UMapx.Transform;
using UMapx.Core;

namespace LocalLaplacianFilters
{
    public partial class Form2 : Form
    {
        #region Private data
        int lightshadows;
        double sigma;
        int discrets;
        int levels;
        double factor;
        Space space = Space.YCbCr;
        Bitmap image;
        GammaCorrection gc;
        LocalLaplacianFilter llf;
        BitmapBlender blend;
        #endregion

        #region Form voids
        public Form2()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
            trackBar3.MouseUp += new MouseEventHandler(trackBar3_MouseUp);
            trackBar4.MouseUp += new MouseEventHandler(trackBar4_MouseUp);
            trackBar5.MouseUp += new MouseEventHandler(trackBar5_MouseUp);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // culture
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            pictureBox1.Image = Apply(image);
        }
        
        public Bitmap Image
        {
            set
            {
                int width = value.Width;
                int height = value.Height;

                int min = Math.Min(width, height);
                int mmm = pictureBox1.Width;
                double k = min / (double)mmm;

                image = new Bitmap(value, (int)(width / k + 1), (int)(height / k + 1));
                image = image.Clone(new Rectangle(0, 0, mmm, mmm), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                pictureBox1.Image = image;
            }
        }

        public Space Space
        {
            set
            {
                this.space = value;
            }
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            this.lightshadows = int.Parse(textBox1.Text);
            this.sigma = double.Parse(textBox2.Text);
            this.discrets = int.Parse(textBox3.Text);
            this.levels = int.Parse(textBox4.Text);
            this.factor = double.Parse(textBox5.Text);

            // lights and shadows
            this.gc = new GammaCorrection(0, this.space);
            int r = 2 * lightshadows + 1;
            int v = r >> 1, i;
            Bitmap[] images = new Bitmap[r];
            Bitmap dummy;

            for (i = 0; i < r; i++)
            {
                dummy = new Bitmap(image);
                gc.Gamma = Math.Pow(2.0, i - v);
                gc.Apply(dummy);
                images[i] = dummy;
            }

            // blending
            this.llf = new LocalLaplacianFilter(this.sigma, this.discrets, this.levels, this.factor);
            this.blend = new BitmapBlender(this.llf, this.space);
            return blend.Apply(images);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region TrackBars
        // движение по trackBar
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar2.Value / 100.0 + 0.01).ToString();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar3.Value.ToString();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = trackBar4.Value.ToString();
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = (trackBar5.Value / 10.0 - 1.0).ToString();
        }

        // отжатие trackBar
        void trackBar5_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar5.Value = 10;
                trackBar5_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Apply(image);
        }
        void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Apply(image);
        }
        void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = Apply(image);
        }
        void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar1.Value = 0;
                trackBar1_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        #endregion
    }
}
