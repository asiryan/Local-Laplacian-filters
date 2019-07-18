using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using UMapx.Imaging;

namespace LocalLaplacianFilters
{
    public partial class Form1 : Form
    {
        #region Private data
        Form2 form2 = new Form2();
        Form3 form3 = new Form3();
        Form4 form4 = new Form4();
        OpenFileDialog openFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        Bitmap image, redo, dummy;
        string file;
        #endregion

        #region Form voids
        public Form1()
        {
            InitializeComponent();
            pictureBox1.AllowDrop = true;
            pictureBox1.DragDrop += new DragEventHandler(pictureBox1_DragDrop);
            pictureBox1.DragEnter += new DragEventHandler(pictureBox1_DragEnter);
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // openfile
            openFile.Filter = "BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tiff)|*.tiff";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;

            // savefile
            saveFile.Filter = "BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tiff)|*.tiff";
            saveFile.FilterIndex = 2;
            saveFile.RestoreDirectory = true;

            // spaces
            comboBox1.Items.Add(Space.YCbCr);
            comboBox1.Items.Add(Space.HSB);
            comboBox1.Items.Add(Space.HSL);
            comboBox1.SelectedIndex = 0;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // hot keys
            if (e.Control && e.KeyCode == Keys.O)
            {
                // Stops other controls on the form receiving event.
                e.SuppressKeyPress = true;
                openToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                saveToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                e.SuppressKeyPress = true;
                closeToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
                reloadToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                e.SuppressKeyPress = true;
                undoToolStripMenuItem1_Click(sender, e);
                return;
            }
            return;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = openFile.FileName;
                image = (Bitmap)Bitmap.FromFile(file);
                pictureBox1.Image = image;
                Booleans(true);
            }
        }

        void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string parse = ((string[])(e.Data.GetData(DataFormats.FileDrop, true)))[0];
            try
            {
                image = (Bitmap)Bitmap.FromFile(parse);
                pictureBox1.Image = image;
                file = parse;
                Booleans(true);
            }
            catch { MessageBox.Show("File is not an image", "Error"); }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = (Bitmap)Bitmap.FromFile(file);
            pictureBox1.Image = image;
            Booleans(true);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redo = image = null;
            pictureBox1.Image = null;
            Booleans(false);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFile.FilterIndex == 1)
                {
                    image.Save(saveFile.FileName, ImageFormat.Bmp);
                }
                if (saveFile.FilterIndex == 2)
                {
                    image.Save(saveFile.FileName, ImageFormat.Jpeg);
                }
                if (saveFile.FilterIndex == 3)
                {
                    image.Save(saveFile.FileName, ImageFormat.Png);
                }
                if (saveFile.FilterIndex == 4)
                {
                    image.Save(saveFile.FileName, ImageFormat.Gif);
                }
                if (saveFile.FilterIndex == 5)
                {
                    image.Save(saveFile.FileName, ImageFormat.Tiff);
                }
            }
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Local Laplacian filters \n Developed by Asiryan Valeriy, 2019 \n Originals: S. Paris, S.W. Hasinoff, J. Kautz, M. Aubry, 2011-2014 \n Powered by UMapx.NET", "About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void localLaplacianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2.Image = image;
            form2.Space = GetSpace(comboBox1.SelectedIndex);

            if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form2.Apply(image);
                pictureBox1.Image = image;
            }
        }

        private void saturationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form3.Image = image;
            form3.Space = GetSpace(comboBox1.SelectedIndex);

            if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form3.Apply(image);
                pictureBox1.Image = image;
            }
        }

        private void exposureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form4.Image = image;
            form4.Space = GetSpace(comboBox1.SelectedIndex);

            if (form4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form4.Apply(image);
                pictureBox1.Image = image;
            }
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (undoToolStripMenuItem1.Enabled)
            {
                if (undoToolStripMenuItem1.Text == "Undo")
                {
                    dummy = redo; redo = image; image = dummy;
                    undoToolStripMenuItem1.Text = "Redo";
                    pictureBox1.Image = image;
                }
                else
                {
                    dummy = redo; redo = image; image = dummy;
                    undoToolStripMenuItem1.Text = "Undo";
                    pictureBox1.Image = image;
                }
            }
            return;
        }
        #endregion

        #region Private voids
        private void Booleans(bool fix)
        {
            if (fix)
            {
                // file
                reloadToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                
                // filtes
                localLaplacianToolStripMenuItem.Enabled = true;
                saturationToolStripMenuItem.Enabled = true;
                exposureToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem1.Enabled = false;
                return;
            }
            else
            {
                // file
                closeToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;

                // filters
                localLaplacianToolStripMenuItem.Enabled = false;
                saturationToolStripMenuItem.Enabled = false;
                exposureToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem1.Enabled = false;
                return;
            }
        }
        private Space GetSpace(int index)
        {
            if (index == 0)
                return Space.YCbCr;
            else if (index == 1)
                return Space.HSB;
            return Space.HSL;
        }
        #endregion
    }
}
