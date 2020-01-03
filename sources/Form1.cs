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
        string text = "Local Laplacian filters";
        #endregion

        #region Form voids
        public Form1()
        {
            InitializeComponent();

            // owner
            form2.Owner = this;
            form3.Owner = this;
            form4.Owner = this;

            // handlers
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
            comboBox1.Items.Add(Space.Grayscale);
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
            else if (e.Control && e.KeyCode == Keys.S && saveToolStripMenuItem.Enabled)
            {
                e.SuppressKeyPress = true;
                saveToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.X && closeToolStripMenuItem.Enabled)
            {
                e.SuppressKeyPress = true;
                closeToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.R && reloadToolStripMenuItem.Enabled)
            {
                e.SuppressKeyPress = true;
                reloadToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Z && undoToolStripMenuItem1.Enabled)
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
                Open(openFile.FileName);
            }
        }

        void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string parse = ((string[])(e.Data.GetData(DataFormats.FileDrop, true)))[0];
            try
            {
                Open(parse);
            }
            catch { MessageBox.Show("File is not an image", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
        }
        void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(file);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open(null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Save(saveFile.FileName, saveFile.FilterIndex);
                Cursor = Cursors.Arrow;
            }
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, " Originals: S. Paris, S.W. Hasinoff, J. Kautz, M. Aubry, \n 2011-2014 \n Developed by V. Asiryan, 2019 \n Powered by UMapx.NET", "Local Laplacian filters", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form2.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
        }

        private void saturationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form3.Image = image;
            form3.Space = GetSpace(comboBox1.SelectedIndex);

            if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form3.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
        }

        private void exposureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form4.Image = image;
            form4.Space = GetSpace(comboBox1.SelectedIndex);

            if (form4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form4.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (undoToolStripMenuItem1.Enabled)
            {
                // swap images
                dummy = redo; redo = image; image = dummy;
                undoToolStripMenuItem1.Text = (undoToolStripMenuItem1.Text == "Undo") ? "Redo" : "Undo";
                pictureBox1.Image = image;
            }
            return;
        }
        #endregion

        #region Private voids
        private void Open(string path)
        {
            Cursor = Cursors.WaitCursor;
            bool flag = path != null;

            if (flag)
            {
                // open image
                image = (Bitmap)Bitmap.FromFile(path);
                pictureBox1.Image = image;
                file = path;
                Text = text + ": " + System.IO.Path.GetFileName(file);
            }
            else
            {
                // close image
                pictureBox1.Image = redo = image = null;
                file = null;
                Text = text;
            }

            Options(flag);
            Cursor = Cursors.Arrow;
            return;
        }

        private void Save(string path, int index)
        {
            Cursor = Cursors.WaitCursor;

            switch (index)
            {
                case 1:
                    image.Save(saveFile.FileName, ImageFormat.Bmp);
                    break;
                case 2:
                    image.Save(saveFile.FileName, ImageFormat.Jpeg);
                    break;
                case 3:
                    image.Save(saveFile.FileName, ImageFormat.Png);
                    break;
                case 4:
                    image.Save(saveFile.FileName, ImageFormat.Gif);
                    break;
                default:
                    image.Save(saveFile.FileName, ImageFormat.Tiff);
                    break;
            }

            file = saveFile.FileName;
            Text = text + ": " + System.IO.Path.GetFileName(file);
            Cursor = Cursors.Arrow;
            return;
        }

        private void Options(bool flag)
        {
            if (flag)
            {
                // file
                reloadToolStripMenuItem.Enabled = closeToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = true;

                // filtes
                localLaplacianToolStripMenuItem.Enabled =
                    saturationToolStripMenuItem.Enabled = 
                    exposureToolStripMenuItem.Enabled = true;
                undoToolStripMenuItem1.Enabled = false;
                return;
            }
            else
            {
                // file
                reloadToolStripMenuItem.Enabled = 
                    closeToolStripMenuItem.Enabled = 
                    saveToolStripMenuItem.Enabled = false;

                // filters
                localLaplacianToolStripMenuItem.Enabled = 
                    saturationToolStripMenuItem.Enabled = 
                    exposureToolStripMenuItem.Enabled = 
                    undoToolStripMenuItem1.Enabled = false;
                return;
            }
        }

        private Space GetSpace(int index)
        {
            switch (index)
            {
                case 0:
                    return Space.YCbCr;
                case 1:
                    return Space.HSB;
                case 2:
                    return Space.HSL;
                default:
                    return Space.Grayscale;
            }
        }
        #endregion
    }
}
