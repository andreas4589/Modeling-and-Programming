using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    /// <summary>
    /// Als je niet weet hoe te beginnen: teken eerst een (flexibel ingevuld) bord, voeg dan de 
    /// interactie toe maar nog zonder controle van legaliteit en veranderen van kleur van ingesloten stenen, 
    /// maak dan de help-functie (daarvoor heb je een methode voor controle op legaliteit nodig, die je daarna 
    /// ook mooi kunt gebruiken voor het controleren van de zetten), enz.
    /// 
    /// 5. En nu: afwisselend blauwe en rode stenen neerzetten
    /// 6. Een begin van spelregels: verbied het zetten op een bezette plaats
    /// 7. Nu kun je voorzichtig over de echte spelregels gaan nadenken
    /// </summary>

    public partial class Form1 : Form
    {
        // membervariabelen
        int x;
        int y;
        int hoogte = 500;
        int breedte = 500;
        Steem[,] stenen;
        int bordx = 50;
        int bordy = 75;
        int beurt = 0;

        // belanrijke variabelen
        int kolommen = 6;
        int rijen = 6;



        public Form1()
        {
            //constructor
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.Text = "Reversi";
            this.Paint += this.teken;
            this.MouseClick += this.Klik;
            DoubleBuffered = true;

            stenen = new int[kolommen, rijen];
            Beginstand();
        }

        public void teken(object obj, PaintEventArgs pea) //tekenmethode
        {
            Pen pen = new Pen(Brushes.Black, 2);

            for (y = 0; y <= kolommen; y++)
            {
                for (x = 0; x <= rijen; x++)
                {
                    pea.Graphics.DrawLine(pen, bordx , hoogte / rijen * x + bordy, breedte + bordx, hoogte / rijen * x + bordy); // horizontale lijnen
                    pea.Graphics.DrawLine(pen, breedte / kolommen * y + bordx, bordy, breedte / kolommen * y + bordx, hoogte + bordy); // verticale lijnen
                }
            }
            for (int n = 0; n < kolommen; n++)
            {
                for (int t = 0; t < rijen; t++)
                {
                    if (vraagBol(n, t) == true)
                        pea.Graphics.FillEllipse(x, breedte / kolommen * n + bordx, hoogte / rijen * t + bordy, breedte / kolommen, hoogte / rijen);
                    
                }
            }
        }

        public bool vraagBol(int x, int y)
        {
            return stenen[x,y];
        }

        public void veranderBol(int x, int y, bool b)
        {
            stenen[x, y] = b;
        }

        //BeginStand tekent de vier stenen die al op het bord liggen aan het begin van het spel.
        private void Beginstand()
        {
            int centerX = kolommen / 2;
            int centerY = rijen / 2;
            stenen[centerX - 1, centerY - 1] = new int(centerX - 1, centerY - 1, true);
            stenen[centerX, centerY] = new VeranderBol(centerX, centerY, true);
            stenen[centerX - 1, centerY] = new VeranderBol(centerX - 1, centerY, false);
            stenen[centerX, centerY - 1] = new VeranderBol(centerX, centerY - 1, false);
        }
        public void Klik(object obj, MouseEventArgs mea)
        {
            beurt++;
            x = (mea.X - bordx) / (breedte / kolommen);
            y = (mea.Y - bordy) / (hoogte / rijen);

            if (mea.X >= bordx && x < kolommen && mea.Y >= bordy && y < rijen)
                veranderBol(x, y, mea.Button == MouseButtons.Left);

            this.Invalidate();
        }

        private void tbkolommen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbkolommen.Text.Length != 0 && int.Parse(tbkolommen.Text) > 2)
                    kolommen = int.Parse(tbkolommen.Text);
                stenen = new bool[kolommen, rijen];
                this.Invalidate();
            }
            catch (Exception)
            {
                this.Invalidate();
            }
        }

        private void tbrijen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbrijen.Text.Length != 0 && int.Parse(tbrijen.Text) > 2)
                    rijen = int.Parse(tbrijen.Text);
                stenen = new bool[kolommen, rijen];
                this.Invalidate();
            }
            catch (Exception)
            {
                this.Invalidate();
            }
        }
    }
}
