using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LocalLaplacianFilters
{
    public partial class Form4 : Form
    {
        #region Private data
        double brightness = 0;
        double exposure = 0;
        double gamma = 0;
        Bitmap image;
        Space space;
        BrightnessCorrection bc;
        ShiftCorrection sc;
        GammaCorrection gc;
        #endregion

        #region Form voids
        public Form4()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
            trackBar3.MouseUp += new MouseEventHandler(trackBar3_MouseUp);
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            pictureBox1.Image = Apply(image);
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            this.brightness = double.Parse(textBox1.Text) / 100.0;
            this.exposure = double.Parse(textBox2.Text) / 100.0;
            this.gamma = Math.Pow(2, double.Parse(textBox3.Text) / 100.0);

            // applying
            Bitmap dummy = new Bitmap(image);
            this.bc = new BrightnessCorrection(this.brightness, this.space);
            bc.Apply(dummy);

            this.sc = new ShiftCorrection(this.exposure, this.space);
            sc.Apply(dummy);

            this.gc = new GammaCorrection(this.gamma, this.space);
            gc.Apply(dummy);
            return dummy;
        }

        public Space Space
        {
            set
            {
                this.space = value;
            }
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
            textBox2.Text = trackBar2.Value.ToString();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar3.Value.ToString();
        }

        // отжатие trackBar
        void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar3.Value = 0;
                trackBar3_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar2.Value = 0;
                trackBar2_Scroll(sender, e);
            }
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
