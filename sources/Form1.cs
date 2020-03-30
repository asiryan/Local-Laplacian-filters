using LocalLaplacianFilters.Filters;
using LocalLaplacianFilters.Helpers;
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
        const string text = "Local Laplacian filters";
        Form2 form2 = new Form2();
        Form3 form3 = new Form3();
        Form4 form4 = new Form4();
        Form5 form5 = new Form5();
        OpenFileDialog openFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        Bitmap image;
        Space space;
        Stack<Bitmap> undo = new Stack<Bitmap>();
        Stack<Bitmap> redo = new Stack<Bitmap>();
        string[] file;
        int[] hist;
        bool mouse;
        #endregion

        #region Form voids
        public Form1()
        {
            InitializeComponent();

            // main
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyPreview = true;
            Text = text;
            Size = new Size(1280, 800);

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

            // histograms
            histogram1.Color = Color.DarkGray;
            histogram1.AllowSelection = true;
            histogram1.SelectionChanged += histogram1_SelectionChanged;
            histogram1.MouseDown += histogram1_MouseDown;
            histogram1.MouseUp += histogram1_MouseUp;
            histogram2.Color = Color.IndianRed;
            histogram2.AllowSelection = false;
            histogram3.Color = Color.LightGreen;
            histogram3.AllowSelection = false;
            histogram4.Color = Color.CornflowerBlue;
            histogram4.AllowSelection = false;

            // labels
            label4.Text = label8.Text = null;

            // trackbars
            trackBar1.MouseUp += new MouseEventHandler(trackBar1_MouseUp);
            trackBar2.MouseUp += new MouseEventHandler(trackBar2_MouseUp);
            trackBar3.MouseUp += new MouseEventHandler(trackBar3_MouseUp);
            trackBar4.MouseUp += new MouseEventHandler(trackBar4_MouseUp);
            trackBar5.MouseUp += new MouseEventHandler(trackBar5_MouseUp);
            trackBar1.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar2.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar3.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar4.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar5.MouseWheel += (sender, e) => ((HandledMouseEventArgs)e).Handled = true;
            trackBar1.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar2.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar3.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar4.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
            trackBar5.KeyDown += (sender, e) => ((KeyEventArgs)e).Handled = true;
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

            // channels
            comboBox2.Items.Add("Average");
            comboBox2.Items.Add(RGBA.Red);
            comboBox2.Items.Add(RGBA.Green);
            comboBox2.Items.Add(RGBA.Blue);
            comboBox2.SelectedIndex = 0;
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
            else if (e.Control && e.KeyCode == Keys.Z && undoToolStripMenuItem.Enabled)
            {
                e.SuppressKeyPress = true;
                undoToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Y && redoToolStripMenuItem.Enabled)
            {
                e.SuppressKeyPress = true;
                redoToolStripMenuItem_Click(sender, e);
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

        private void exposureFusionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
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
                Processor(image, form2.Apply);
            }
            return;
        }

        private void temperatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form3.Image = image;

            if (form3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Processor(image, form3.Apply);
            }
            return;
        }

        private void hslToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form4.Image = image;

            if (form4.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Processor(image, form4.Apply);
            }
            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            space = ImageHelper.GetSpace(comboBox1.SelectedIndex);
        }
        #endregion

        #region Histogram
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetHistogram(image, false);
            return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            histogram1.IsLogarithmicView =
                histogram2.IsLogarithmicView =
                histogram3.IsLogarithmicView =
                histogram4.IsLogarithmicView = checkBox1.Checked;
            return;
        }

        private void histogram1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse = false;
            label8.Text = null;
        }
        private void histogram1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = true;
        }
        private void histogram1_SelectionChanged(object sender, controls.HistogramEventArgs e)
        {
            if (mouse)
            {
                int min = e.Min;
                int max = e.Max;
                int count = 0;

                // count pixels
                for (int i = min; i <= max; i++)
                {
                    count += hist[i];
                }

                // print
                label8.Text =
                    min.ToString() + "..." + max.ToString() + "\n" +
                    count.ToString() + "\n" +
                    ((float)count * 100 / Statistics.Sum(hist)).ToString("F2");
            }
        }
        #endregion

        #region Private voids
        private void TryOpen(params string[] filenames)
        {
            // length
            int length = filenames.Length;

            // try to open
            try
            {
                if (length == 0)
                {
                    // dispose and clear
                    DisposeControls();
                    ClearStacks();
                    ActivateControls(false);
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
                            Processor(array, form5.Apply);
                            file = filenames;
                            Text = text + ": exposure fusion (" + file.Length + " images)";
                            ActivateControls(true);
                        }
                    }
                    // single image
                    else
                    {
                        Processor((Bitmap)Bitmap.FromFile(filenames[0]), null, false);
                        file = new string[] { filenames[0] };
                        Text = text + ": " + System.IO.Path.GetFileName(file[0]);
                        ActivateControls(true);
                    }

                    // clear data
                    ClearStacks();
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

        private void ActivateControls(bool enabled)
        {
            // file
            reloadToolStripMenuItem.Enabled =
                closeToolStripMenuItem.Enabled =
                saveToolStripMenuItem.Enabled = enabled;

            // filters
            localLaplacianToolStripMenuItem.Enabled =
                temperatureToolStripMenuItem.Enabled =
                exposureToolStripMenuItem.Enabled =
                flipVerticalToolStripMenuItem.Enabled =
                flipHorizontalToolStripMenuItem.Enabled = enabled;

            // scrolls
            trackBar1.Enabled =
                trackBar2.Enabled =
                trackBar3.Enabled =
                trackBar4.Enabled =
                trackBar5.Enabled =
                button1.Enabled =
                button2.Enabled = enabled;

            // stacks
            undoToolStripMenuItem.Enabled =
                redoToolStripMenuItem.Enabled = false;

            return;
        }

        private void GetHistogram(Bitmap image, bool update = true)
        {
            // check null
            if (image != null)
            {
                // histograms: r, g, b
                if (update)
                {
                    histogram2.Values = Statistics.Histogram(image, RGBA.Red);
                    histogram3.Values = Statistics.Histogram(image, RGBA.Green);
                    histogram4.Values = Statistics.Histogram(image, RGBA.Blue);
                }

                // comboBox2
                int index = comboBox2.SelectedIndex;

                // switch
                switch (index)
                {
                    case 1:
                        hist = histogram2.Values;
                        break;
                    case 2:
                        hist = histogram3.Values;
                        break;
                    case 3:
                        hist = histogram4.Values;
                        break;
                    default:
                        hist = Statistics.Histogram(image);
                        break;
                }

                // statistics
                double pixels = Statistics.Sum(hist);
                double mean = Statistics.Mean(hist);
                int median = Statistics.Median(hist);
                double std = Statistics.StdDev(hist);

                // set main histogram
                histogram1.Values = hist;

                // label
                label4.Text =
                    mean.ToString("F2") + "\n" +
                    std.ToString("F2") + "\n" +
                    median + "\n" +
                    pixels;
            }

            return;
        }

        private void DisposeControls()
        {
            file = null;
            image = null;
            Text = text;
            label4.Text = null;
            label8.Text = null;
            pictureBox1.Image = null;
            histogram1.Values = null;
            histogram2.Values = null;
            histogram3.Values = null;
            histogram4.Values = null;
            return;
        }

        private void ClearStacks()
        {
            undo.Clear();
            redo.Clear();
            return;
        }

        private void ResetAdjustments()
        {
            trackBar1.Value =
                trackBar2.Value =
                trackBar3.Value =
                trackBar4.Value =
                trackBar5.Value = 0;

            textBox1.Text =
                textBox2.Text =
                textBox3.Text =
                textBox4.Text =
                textBox5.Text = "0";

            pictureBox1.Image = image;
            return;
        }

        private void Processor(Bitmap bitmap, SingleFilter filter, bool cache = true)
        {
            // check if null
            if (bitmap != null)
            {
                Cursor = Cursors.WaitCursor;

                // cache to stack or not?
                if (cache)
                {
                    undoToolStripMenuItem.Enabled = true;
                    undo.Push(image);
                    redo.Clear();
                }
                // apply filter or not?
                image = (filter != null) ? filter(bitmap) : bitmap;

                // settings
                GetHistogram(image);
                ResetAdjustments();
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
            return;
        }

        private void Processor(Bitmap[] bitmap, ComplexFilter filter)
        {
            // check if null
            if (bitmap != null)
            {
                Cursor = Cursors.WaitCursor;
                image = (filter != null) ? filter(bitmap) : null; // not implemented
                GetHistogram(image);
                ResetAdjustments();
                pictureBox1.Image = image;
                Cursor = Cursors.Arrow;
            }
            return;
        }
        #endregion

        #region Adjustments
        SaturationContrastFilter scf = new SaturationContrastFilter();
        ExposureGammaFilter egf = new ExposureGammaFilter();

        public Bitmap Apply(Bitmap image)
        {
            // parsing
            double saturation = int.Parse(textBox1.Text);
            double contrast = double.Parse(textBox2.Text) / 100.0;
            double brightness = double.Parse(textBox5.Text) / 100.0;
            double exposure = double.Parse(textBox4.Text) / 100.0;
            double gamma = Math.Pow(2, -3 * double.Parse(textBox3.Text) / 100.0);

            // parameters
            scf.SetParams(saturation, contrast, space);
            egf.SetParams(brightness, exposure, gamma, space);

            // applying filter
            Bitmap filter = egf.Apply(scf.Apply(image));
            GetHistogram(filter);
            return filter;
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
        void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                trackBar4.Value = 0;
                trackBar4_Scroll(sender, e);
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
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = trackBar4.Value.ToString();
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = trackBar5.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Processor((Bitmap)pictureBox1.Image, null);
            return;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ResetAdjustments();
            return;
        }
        #endregion

        #region Edit
        FlipFilter flip = new FlipFilter();

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoToolStripMenuItem.Enabled && undo.Count > 0)
            {
                redo.Push(image);
                image = undo.Pop();
                redoToolStripMenuItem.Enabled = redo.Count > 0;
                undoToolStripMenuItem.Enabled = undo.Count > 0;
                GetHistogram(image);
                pictureBox1.Image = image;
            }
            return;
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoToolStripMenuItem.Enabled && redo.Count > 0)
            {
                undo.Push(image);
                image = redo.Pop();
                redoToolStripMenuItem.Enabled = redo.Count > 0;
                undoToolStripMenuItem.Enabled = undo.Count > 0;
                GetHistogram(image);
                pictureBox1.Image = image;
            }
            return;
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flip.SetParams(false, true);
            Processor(image, flip.Apply);
        }
        private void flipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flip.SetParams(true, false);
            Processor(image, flip.Apply);
        }
        #endregion
    }
}
