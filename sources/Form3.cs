using LocalLaplacianFilters.Filters;
using LocalLaplacianFilters.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LocalLaplacianFilters
{
    public partial class Form3 : Form
    {
        #region Private data
        TemperatureFilter temp = new TemperatureFilter();
        Bitmap image;
        #endregion

        #region Form voids
        public Form3()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar2.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar2.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Apply(image);
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            double saturation = double.Parse(textBox1.Text);
            double contrast = double.Parse(textBox2.Text);

            // applying
            temp.SetParams(saturation, contrast);
            return temp.Apply(image);
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
            textBox1.Text = (trackBar1.Value * 100.0).ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar2.Value / 100.0).ToString();
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
