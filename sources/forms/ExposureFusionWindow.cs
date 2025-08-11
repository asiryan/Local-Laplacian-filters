using LaplacianHDR.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LaplacianHDR
{
    public partial class ExposureFusionWindow : Form
    {
        #region Private data

        private readonly ExposureFusion fusion = new ExposureFusion(int.MaxValue);
        private Bitmap[] images;

        #endregion

        #region Form voids

        public ExposureFusionWindow()
        {
            InitializeComponent();
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            UpdatePictureBox(Apply(images));
        }

        public Bitmap Apply(params Bitmap[] images)
        {
            this.fusion.Sigma = float.Parse(textBox2.Text);
            return this.fusion.Apply(images);
        }

        private void UpdatePictureBox(Bitmap image)
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = image;
        }

        public Bitmap[] Images
        {
            set
            {
                int length = value.Length;
                
                images?.Dispose();
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
            this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region TrackBars

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar1.Value / 100.0 + 0.1).ToString();
        }

        void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trackBar1.Value = 45;
                trackBar1_Scroll(sender, e);
            }
            UpdatePictureBox(Apply(images));
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            images?.Dispose();
            pictureBox1.Dispose();
            trackBar1.Dispose();
            base.Dispose();
        }

        #endregion
    }
}
