using LocalLaplacianFilters.Filters;
using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LocalLaplacianFilters
{
    public partial class Form4 : Form
    {
        #region Private data
        ExposureGammaFilter egf = new ExposureGammaFilter();
        Bitmap image;
        Space space;
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
            double brightness = double.Parse(textBox1.Text) / 100.0;
            double exposure = double.Parse(textBox2.Text) / 100.0;
            double gamma = Math.Pow(2, double.Parse(textBox3.Text) / 100.0);

            // applying filter
            egf.SetParams(brightness, exposure, gamma, space);
            return egf.Apply(image);
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
                image = ImageHelper.Crop(value, pictureBox1.Width);
                pictureBox1.Image = image;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region TrackBars
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
