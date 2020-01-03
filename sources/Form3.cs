using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LocalLaplacianFilters
{
    public partial class Form3 : Form
    {
        #region Private data
        SaturationContrastFilter scf = new SaturationContrastFilter();
        Bitmap image;
        Space space;
        #endregion

        #region Form voids
        public Form3()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            pictureBox1.Image = Apply(image);
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            double saturation = int.Parse(textBox1.Text);
            double contrast = double.Parse(textBox2.Text) / 100.0;

            // applying
            scf.SetParams(saturation, contrast, space);
            return scf.Apply(image);
        }

        public Space Space
        {
            set
            {
                this.space = value;
                trackBar1.Enabled = (this.space != UMapx.Imaging.Space.Grayscale);
            }
        }

        public Bitmap Image
        {
            set
            {
                int width = value.Width;
                int height = value.Height;

                int min = Math.Min(width, height);
                int box = pictureBox1.Width;
                double k = min / (double)box;

                image = (new Bitmap(value, (int)(width / k + 1), (int)(height / k + 1))).
                    Clone(new Rectangle(0, 0, box, box), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

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
