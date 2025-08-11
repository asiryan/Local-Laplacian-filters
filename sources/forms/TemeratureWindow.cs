using LaplacianHDR.Filters;
using LaplacianHDR.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaplacianHDR
{
    public partial class TemeratureWindow : Form
    {
        #region Private data

        private readonly TemperatureFilter temp = new TemperatureFilter();
        private Bitmap image;

        #endregion

        #region Form voids

        public TemeratureWindow()
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
            UpdatePictureBox(Apply(image));
        }

        private void UpdatePictureBox(Bitmap image)
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = image;
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            float saturation = float.Parse(textBox1.Text);
            float contrast = float.Parse(textBox2.Text);

            // applying
            temp.SetParams(saturation, contrast);
            return temp.Apply(image);
        }

        public Bitmap Image
        {
            set
            {
                image?.Dispose();
                image = ImageHelper.Crop(value, pictureBox1.Width);
            }
            get
            {
                return image;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
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
            if (e.Button == MouseButtons.Right)
            {
                trackBar2.Value = 0;
                trackBar2_Scroll(sender, e);
            }
            UpdatePictureBox(Apply(image));
        }

        void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trackBar1.Value = 0;
                trackBar1_Scroll(sender, e);
            }
            UpdatePictureBox(Apply(image));
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            image?.Dispose();
            pictureBox1.Dispose();
            trackBar1.Dispose();
            trackBar2.Dispose();
            base.Dispose();
        }

        #endregion
    }
}
