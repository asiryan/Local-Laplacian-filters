﻿using LaplacianHDR.Filters;
using LaplacianHDR.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LaplacianHDR
{
    public partial class Form2 : Form
    {
        #region Private data
        GeneralizedLocalLaplacianFilter gllf = new GeneralizedLocalLaplacianFilter();
        Space space = Space.YCbCr;
        Bitmap image;
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
            trackBar6.MouseUp += new MouseEventHandler(trackBar6_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar2.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar3.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar4.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar5.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar6.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar2.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar3.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar4.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar5.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar6.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Apply(image);
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

        public Space Space
        {
            set
            {
                space = value;
            }
            get
            {
                return space;
            }
        }

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            float lightshadows = (float)Math.Pow(2, float.Parse(textBox1.Text) / 100.0);
            float sigma = float.Parse(textBox2.Text);
            int discrets = int.Parse(textBox3.Text);
            int levels = int.Parse(textBox4.Text);
            float factor = float.Parse(textBox5.Text);
            int radius = 2 * (int.Parse(textBox6.Text) + 1);

            // applying filter
            gllf.SetParams(radius, lightshadows, sigma, discrets, levels, factor, space);
            return gllf.Apply(image);
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
            textBox2.Text = (trackBar2.Value / 1000.0 + 0.001).ToString();
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
            int v = trackBar5.Value;

            if (v < 0)
            {
                textBox5.Text = (v / 100.0).ToString();
            }
            else
            {
                textBox5.Text = (v / 10.0).ToString();
            }
        }
        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            textBox6.Text = trackBar6.Value.ToString();
        }

        void trackBar6_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar6.Value = 0;
                trackBar6_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar5_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar5.Value = 0;
                trackBar5_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar4.Value = 20;
                trackBar4_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar3.Value = 20;
                trackBar3_Scroll(sender, e);
            }
            pictureBox1.Image = Apply(image);
        }
        void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar2.Value = 49;
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
