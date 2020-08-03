using LaplacianHDR.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LaplacianHDR
{
    public partial class Form5 : Form
    {
        #region Private data
        ExposureFusion fusion;
        Bitmap[] images;
        #endregion

        #region Form voids
        public Form5()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Apply(images);
        }

        public Bitmap Apply(params Bitmap[] images)
        {
            this.fusion = new ExposureFusion(int.MaxValue, double.Parse(textBox2.Text));
            return this.fusion.Apply(images);
        }

        public Bitmap[] Images
        {
            set
            {
                int length = value.Length;
                images = new Bitmap[length];

                for (int i = 0; i < length; i++)
                {
                    images[i] = ImageHelper.Crop(value[i], pictureBox1.Width);
                }
            }
            get
            {
                return images;
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
            textBox2.Text = (trackBar1.Value / 100.0 + 0.1).ToString();
        }

        void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar1.Value = 45;
                trackBar1_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(images);
        }
        #endregion
    }
}
