using LocalLaplacianFilters.Filters;
using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LocalLaplacianFilters
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
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            pictureBox1.Image = Apply(images);
        }

        public Bitmap Apply(Bitmap[] images)
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
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion

        #region TrackBars
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar2.Value / 100.0 + 0.1).ToString();
        }

        void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar2.Value = 45;
                trackBar2_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(images);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = Apply(images);
        }
        #endregion
    }
}
