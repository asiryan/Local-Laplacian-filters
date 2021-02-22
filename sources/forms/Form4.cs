using LaplacianHDR.Filters;
using LaplacianHDR.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaplacianHDR
{
    public partial class Form4 : Form
    {
        #region Private data
        HueSaturationLightnessFilter hsl = new HueSaturationLightnessFilter();
        Bitmap image;
        #endregion

        #region Form voids
        public Form4()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
            trackBar3.MouseUp += new MouseEventHandler(trackBar3_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar2.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar3.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar2.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar3.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Apply(image);
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            float h = float.Parse(textBox1.Text);
            float s = float.Parse(textBox2.Text) / 100.0f;
            float l = float.Parse(textBox3.Text) / 100.0f;

            // applying filter
            hsl.SetParams(h, s, l);
            return hsl.Apply(image);
        }

        public Bitmap Image
        {
            set
            {
                image = ImageHelper.Crop(value, pictureBox1.Width);
                pictureBox1.Image = image;
            }
            get
            {
                return image;
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

        void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar1.Value = 0;
                trackBar1_Scroll(sender, e);
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
        void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar3.Value = 0;
                trackBar3_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        #endregion
    }
}
