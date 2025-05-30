using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mandelbrot
{
    public partial class Form1 : Form
    {
        //declareer alle variabelen bovenin
        double invoerx = 0;
        double invoery = 0;
        int max = 200;
        double schaal = 1;
        double prezoomx = 0;
        double prezoomy = 0;
        public Form1()
        {
            //roep buttonklik aan in constructor

            InitializeComponent();
            this.ClientSize = new Size(600, 600);
            this.panel1.Size = new Size(400, 400);
            this.panel1.Location = new Point(100, 10);
            comboBox1.SelectedIndex = 0;
        }
        // berekenen van mandelgetal

        public int mandelgetal(double x, double y)
        {
            int t = 0; double a = 0; double b = 0; double temp = 0; double getal;
            while (t < max)
            {
                temp = (a * a) - (b * b) + x;
                b = 2 * a * b + y;
                a = temp;
                t++;
                getal = (a * a + b * b);
                if (getal > 4.0) break;
            }
            return t;
        }

        //panel paintevent
        //teken alle pixels (rechthoeken)
        public void tekenPanel(object sender, PaintEventArgs pea)
        {
            double x;
            double y;

            for (x = 0; x < 400; x++)
            {
                for (y = 0; y < 400; y++)
                {
                    double startx = Convert.ToDouble(invoerx);
                    double starty = Convert.ToDouble(invoery);
                    double x1 = ((x / 100 - 2) * schaal) + startx;
                    double y1 = ((2 - y / 100) * schaal) + starty;
                    pea.Graphics.FillRectangle(soort(mandelgetal(x1, y1)), (int)(x), (int)(y), 1, 1);

                }

            }
            textBox1.Text = invoerx.ToString();
            textBox2.Text = invoery.ToString();
            textBox3.Text = schaal.ToString();
            textBox4.Text = max.ToString();
            comboBox1.Text = comboBox1.SelectedItem.ToString();

        }
        //ingevoerde waarden gebruiken als nieuwe waarden bij buttonklik
        public void startknop(object obj, EventArgs e) // start knop
        {
            try
            {
                invoerx = double.Parse(textBox1.Text);
                invoery = double.Parse(textBox2.Text);
                schaal = double.Parse(textBox3.Text);
                max = int.Parse(textBox4.Text);
                panel1.Invalidate();
            }
            catch (FormatException)
            {
                panel1.Invalidate();
            }
            catch (OverflowException)
            {
                panel1.Invalidate();
            }
        }

        //reset waarden elke textbox bij resetbuttonklik

        private void resetknop(object sender, EventArgs e) // reset knop
        {
            try
            {
                invoerx = 0;
                invoery = 0;   // start waarde van de controls
                schaal = 1;
                prezoomx = 0;
                prezoomy = 0;
                max = 200;
                comboBox1.SelectedIndex = 0;
                panel1.Invalidate();
            }
            catch (FormatException)
            {
                panel1.Invalidate();
            }
            catch (OverflowException)
            {
                panel1.Invalidate();
            }
        }
        //zet nieuwe xmid en ymid voor elke muisklik
        //zet nieuwe scale voor linker- en rechtermuisklik
        private void panel1_MouseClick(object sender, MouseEventArgs mea) // inzoom functie
        {
            if (mea.Button == MouseButtons.Left)
            {
                invoerx = mea.X;
                invoery = mea.Y;
                invoerx = (invoerx / 100 - 2) * schaal + prezoomx;
                invoery = (2 - invoery / 100) * schaal + prezoomy;

                prezoomx = invoerx;
                prezoomy = invoery;
                schaal = schaal / 2;
                textBox1.Text = invoerx.ToString();
                textBox2.Text = invoery.ToString();
                textBox3.Text = schaal.ToString();
            }
            if (mea.Button == MouseButtons.Right)
            {
                schaal = schaal * 2;
                invoerx = prezoomx;
                invoery = prezoomy;
            }

            panel1.Invalidate();
        }

        //bepaling van inkleuring aan de hand van keuzes (combobox)
        private Brush soort(int mandelGetal)
        {

            Brush kleur = Brushes.Purple;

            if (comboBox1.SelectedIndex == 0)
            {
                if (mandelGetal >= max)
                {
                    kleur = Brushes.Black;
                }


                else
                {
                    if (mandelGetal % 2 == 0)
                    {
                        kleur = Brushes.White;
                    }
                    else
                    {
                        kleur = Brushes.Black;
                    }
                }

            }

            if (comboBox1.SelectedIndex == 1)
            {
                kleur = new SolidBrush(Color.FromArgb(0, mandelGetal % 255, mandelGetal % 255));



            }
            if (comboBox1.SelectedIndex == 2)
            {
                if (mandelGetal > max)
                {
                    kleur = Brushes.White;
                }

                else if (mandelGetal % 2 == 0)
                {
                    kleur = Brushes.Black;
                }
                else if (mandelGetal % 3 == 0)
                {
                    kleur = Brushes.Blue;
                }
                else
                {
                    kleur = Brushes.Red;
                }
            }
            if (comboBox1.SelectedIndex == 3)
            {
                if (mandelGetal > max)
                {
                    kleur = Brushes.Black;
                }
                else if (mandelGetal % 2 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(0, 0, 0));
                }
                else if (mandelGetal % 3 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(255, 127, 0));
                }
                else if (mandelGetal % 5 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(255, 255, 0));
                }
                else if (mandelGetal % 7 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(0, 255, 0));
                }
                else if (mandelGetal % 11 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(0, 0, 255));
                }
                else if (mandelGetal % 13 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(75, 0, 130));
                }
                else if (mandelGetal % 17 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(148, 0, 211));
                }

            }
            if (comboBox1.SelectedIndex == 4)
            {
                if (mandelGetal > max)
                {
                    kleur = Brushes.Black;
                }


                else if (mandelGetal % 8 == 0)
                {
                    kleur = Brushes.Red;
                }
                else if (mandelGetal % 7 == 0)
                {
                    kleur = Brushes.OrangeRed;
                }
                else if (mandelGetal % 6 == 0)
                {
                    kleur = Brushes.Yellow;
                }
                else if (mandelGetal % 5 == 0)
                {
                    kleur = Brushes.Green;
                }
                else if (mandelGetal % 4 == 0)
                {
                    kleur = Brushes.Blue;
                }
                else if (mandelGetal % 3 == 0)
                {
                    kleur = Brushes.Purple;
                }
                else if (mandelGetal % 2 == 0)
                {
                    kleur = Brushes.Green;
                }

            }
            if (comboBox1.SelectedIndex == 5)
            {
                int rood; int groen; int blauw;

                if (mandelGetal < 400)
                {
                    rood = (mandelGetal % 64) * 4; blauw = (mandelGetal % 16) * 2; groen = (mandelGetal % 32) * 6;
                    kleur = new SolidBrush(Color.FromArgb(rood, groen, blauw));
                }
                if (mandelGetal < 500)
                {
                    rood = (mandelGetal % 64); blauw = (mandelGetal % 16) * 5; groen = (mandelGetal % 32) * 6;
                    kleur = new SolidBrush(Color.FromArgb(rood, groen, blauw));
                }
                if ((mandelGetal % 64) * 4 == 0)
                {
                    kleur = new SolidBrush(Color.FromArgb(0, 5, 5));
                }
                else
                {
                    rood = (mandelGetal % 64) * 4; blauw = (mandelGetal % 16) * 8; groen = (mandelGetal % 32) * 2;
                    kleur = new SolidBrush(Color.FromArgb(rood, groen, blauw));
                }
            }

            return kleur;
        }


    }
}