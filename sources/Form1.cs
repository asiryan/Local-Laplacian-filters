using LocalLaplacianFilters.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        Form5 form5 = new Form5();
        OpenFileDialog openFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        Bitmap image, redo, dummy;
        Space space;
        string[] file;
        string text = "Local Laplacian filters";
        #endregion

        #region Form voids
        public Form1()
        {
            InitializeComponent();

            // main
            DoubleBuffered = true;
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyPreview = true;

            // owner
            form2.Owner = this;
            form3.Owner = this;
            form4.Owner = this;
            form5.Owner = this;
            form5.TopMost = true;

            // elements
            pictureBox1.AllowDrop = true;
            pictureBox1.DragDrop += new DragEventHandler(pictureBox1_DragDrop);
            pictureBox1.DragEnter += new DragEventHandler(pictureBox1_DragEnter);
            pictureBox1.MouseDoubleClick += pictureBox1_MouseDoubleClick;
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // openfile
            openFile.Filter = "BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tiff)|*.tiff";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            openFile.Multiselect = true;

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
            return;
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
                TryOpen(openFile.FileNames);
            }
            return;
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryOpen(file);
            return;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryOpen();
            return;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TrySave(saveFile.FileName, saveFile.FilterIndex);
            }
            return;
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            TryOpen((string[])(e.Data.GetData(DataFormats.FileDrop, true)));
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, " Originals " +
                "\n Tom Mertens, Jan Kautz, Frank Van Reeth," +
                "\n Sylvain Paris, Samuel W. Hasinoff, Mathieu Aubry" +
                "\n 2007-2014 \n" +
                "\n Developed by Valery Asiryan" +
                "\n Powered by UMapx.NET" +
                "\n 2019-2020", 
                text + ": About",
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void enhancementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form2.Image = image;
            form2.Space = space;

            if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form2.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
            return;
        }

        private void saturationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form3.Image = image;
            form3.Space = space;

            if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form3.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
            return;
        }

        private void exposureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form4.Image = image;
            form4.Space = space;

            if (form4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                redo = (Bitmap)image.Clone();
                undoToolStripMenuItem1.Enabled = true;
                image = form4.Apply(image);
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
            return;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            space = ImageHelper.GetSpace(comboBox1.SelectedIndex);
        }
        #endregion

        #region Private voids
        private void TryOpen(params string[] filenames)
        {
            int length = filenames.Length;

            // try to open
            try
            {
                if (length == 0)
                {
                    // null
                    pictureBox1.Image = redo = image = null;
                    file = null;
                    Text = text;
                    FormOptions(false);
                }
                else
                {
                    // collect input data
                    List<Bitmap> images = new List<Bitmap>();
                    Bitmap current = new Bitmap(filenames[0]);
                    int width = current.Width;
                    int height = current.Height;
                    images.Add(current);

                    // do job
                    for (int i = 1; i < length; i++)
                    {
                        current = new Bitmap(filenames[i]);
                        if (current.Width != width ||
                            current.Height != height)
                        {
                            continue;
                        }
                        else
                        {
                            images.Add(current);
                        }
                    }

                    // get images array
                    Bitmap[] array = images.ToArray();
                    length = array.Length;

                    // exposure fusion
                    if (length > 1)
                    {
                        form5.Images = array;
                        this.BringToFront();

                        if (form5.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            Cursor = Cursors.WaitCursor;
                            image = form5.Apply(array);
                            pictureBox1.Image = image;
                            file = filenames;
                            Text = text + ": Exposure Fusion (" + file.Length + " images)";
                            Cursor = Cursors.Arrow;
                            FormOptions(true);
                        }
                    }
                    // single image
                    else
                    {
                        image = (Bitmap)Bitmap.FromFile(filenames[0]);
                        pictureBox1.Image = image;
                        file = new string[] { filenames[0] };
                        Text = text + ": " + System.IO.Path.GetFileName(file[0]);
                        FormOptions(true);
                    }
                    
                    // clear data
                    images.Clear();
                    images = null;
                    array = null;
                }
            }
            catch
            {
                MessageBox.Show("Incorrect input image data", text + ": Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return;
        }

        private void TrySave(string filename, int index)
        {
            // try to save
            try
            {
                image.Save(filename, ImageHelper.GetImageFormat(index));
                file = new string[] { filename };
                Text = text + ": " + System.IO.Path.GetFileName(filename);
            }
            catch
            {
                MessageBox.Show("Incorrect output image data", text + ": Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return;
        }

        private void FormOptions(bool enabled)
        {
            if (enabled)
            {
                // file
                reloadToolStripMenuItem.Enabled = 
                    closeToolStripMenuItem.Enabled =
                    saveToolStripMenuItem.Enabled = true;

                // filters
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
        #endregion
    }
}
