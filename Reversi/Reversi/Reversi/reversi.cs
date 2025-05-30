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
    public partial class Form1 : Form
    {
        // membervariabelen
        int x;
        int y;
        int hoogte = 500;
        int breedte = 500;
        int[,] stenen;
        int bordx = 50;
        int bordy = 75;

        int kleur = -1;
        int tblauw = 0;
        int trood = 0;
        int helpknop = 0;
        bool help = false;
        bool legaal = false;

        // belanrijke variabelen
        int kolommen = 6;
        int rijen = 6;
        
        public Form1()
        {
            //constructor
            InitializeComponent();
            this.ClientSize = new Size(800, 600);
            this.Text = "Reversi";
            this.button1.Location = new Point(575, 240);
            this.button2.Location = new Point(575, 288);
            this.label3.Text = "Blauw aan zet";
            this.Paint += this.teken;
            this.Paint += Legenda;
            this.MouseClick += this.Klik;
            DoubleBuffered = true;

            stenen = new int[kolommen, rijen];
            Beginstand();
            Telmethode();
        }

        // de teken-methode tekent het veld, de stenen en de help-stenen.
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
                    legaal = false;

                    if (stenen[n, t] == 1)
                        pea.Graphics.FillEllipse(Brushes.Red, breedte / kolommen * n + bordx, hoogte / rijen * t + bordy, breedte / kolommen, hoogte / rijen); // rode stenen

                    if (stenen[n, t] == -1)
                        pea.Graphics.FillEllipse(Brushes.Blue, breedte / kolommen * n + bordx, hoogte / rijen * t + bordy, breedte / kolommen, hoogte / rijen); // blauwe stenen
                        
                    if (help == true && stenen[n,t] == 0 && legaliteit(n, t) == true)
                        pea.Graphics.DrawEllipse(Pens.Black, breedte / kolommen * n + bordx + 30, hoogte / rijen * t + bordy + 30, breedte / kolommen - 60, hoogte / rijen - 60); // help stenen

                }
            }
        }

        // legaliteit bepaald voor een punt ingesloten kan worden door een andere kleur voor alle 8 richtingen. De bool legaal tekent een help-cirkel zodra het punt legaal is.
        public bool legaliteit(int x, int y)
        {
            if (x < kolommen - 1 && stenen[x + 1, y] == -1 * kleur)
                for (int i = x; i < kolommen; i++)
                    if (stenen[i, y] == kleur)
                    {
                        legaal = true;
                        break;
                    }

            if (x > 0 && stenen[x - 1, y] == -1 * kleur)
                for (int i = x; i >= 0; i--)
                    if (stenen[i, y] == kleur)
                        legaal = true;

            if (y < rijen - 1 && stenen[x, y + 1] == -1 * kleur)
                for (int i = y; i < rijen; i++)
                    if (stenen[x, i] == kleur)
                        legaal = true;

            if (y > 0 && stenen[x, y - 1] == -1 * kleur)
                for (int i = y; i >= 0; i--)
                    if (stenen[x, i] == kleur)
                        legaal = true;

            if (x > 0 && y < rijen - 1 && stenen[x - 1, y + 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x - i >= 0 && y + i <= rijen - 1)
                    {
                        if (stenen[x - i, y + i] == kleur)
                            if (x > 0 && y < rijen - 1 && stenen[x - 1, y + 1] == -1 * kleur)
                                legaal = true;
                    }
                }

            if (x < kolommen - 1 && y > 0 && stenen[x + 1, y - 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x + i <= kolommen - 1 && y - i >= 0)
                    {
                        if (stenen[x + i, y - i] == kleur)
                            if (x < kolommen - 1 && y > 0 && stenen[x + 1, y - 1] == -1 * kleur)
                                legaal = true;
                    }
                }

            if (x < kolommen - 1 && y < rijen - 1 && stenen[x + 1, y + 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x + i <= kolommen - 1 && y + i <= rijen - 1)
                    {
                        if (stenen[x + i, y + i] == kleur)
                            if (x < kolommen - 1 && y < rijen - 1 && stenen[x + 1, y + 1] == -1 * kleur)
                                legaal = true;
                    }
                }

            if (x > 0 && y > 0 && stenen[x - 1, y - 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x - i >= 0 && y - i >= 0)
                    {
                        if (stenen[x - i, y - i] == kleur)
                            if (x > 0 && y > 0 && stenen[x - 1, y - 1] == -1 * kleur)
                                legaal = true;  
                    }
                }

            return legaal;
        }

        // "Ingesloten" verandert de waarde van de stenen tussen de nieuwe geplaatste steen en de steen van dezelfde kleur. 
        private void Ingesloten(int x, int y)
        {
            if (x < kolommen - 1 && stenen[x + 1, y] == -1 * kleur)
                for (int i = x; i < kolommen; i++)
                    if (stenen[i, y] == kleur)
                    {
                        int k = i;
                        while (k > x)
                        {
                            k--;
                            stenen[k, y] = kleur;
                        }
                    }

            if (x > 0 && stenen[x - 1, y] == -1 * kleur)
                for (int i = x; i >= 0; i--)
                    if (stenen[i, y] == kleur)
                    {
                        int k = i;
                        while (k < x)
                        {
                            k++;
                            stenen[k, y] = kleur;
                        }
                    }

            if (y < rijen - 1 && stenen[x, y + 1] == -1 * kleur)
                for (int i = y; i < rijen; i++)
                    if (stenen[x, i] == kleur)
                    {
                        int k = i;
                        while (k > y)
                        {
                            k--;
                            stenen[x, k] = kleur;
                        }
                    }


            if (y > 0 && stenen[x, y - 1] == -1 * kleur)
                for (int i = y; i >= 0; i--)
                    if (stenen[x, i] == kleur)
                    {
                        int k = i;
                        while (k < y)
                        {
                            k++;
                            stenen[x, k] = kleur;
                        }
                    }
                        
            if (x > 0 && y < rijen - 1 && stenen[x - 1, y + 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x - i >= 0 && y + i <= rijen - 1)
                    {
                        if (stenen[x - i, y + i] == kleur)
                            if (x > 0 && y < rijen - 1 && stenen[x - 1, y + 1] == -1 * kleur)
                            {
                                int r = i;
                                int k = 0;
                                while (k < r)
                                {
                                    k++;
                                    stenen[x - k, y + k] = kleur;
                                }
                            }      
                    }
                }

            if (x < kolommen - 1 && y > 0 && stenen[x + 1, y - 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x + i <= kolommen - 1 && y - i >= 0)
                    {
                        if (stenen[x + i, y - i] == kleur)
                            if (x < kolommen - 1 && y > 0 && stenen[x + 1, y - 1] == -1 * kleur)
                            {
                                int r = i;
                                int k = 0;
                                while (k < r)
                                {
                                    k++;
                                    stenen[x + k, y - k] = kleur;
                                }
                            }
                    }
                }

            if (x < kolommen - 1 && y < rijen - 1 && stenen[x + 1, y + 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x + i <= kolommen - 1 && y + i <= rijen - 1)
                    {
                        if (stenen[x + i, y + i] == kleur)
                            if (x < kolommen - 1 && y < rijen - 1 && stenen[x + 1, y + 1] == -1 * kleur)
                            {
                                int r = i;
                                int k = 0;
                                while (k < r)
                                {
                                    k++;
                                    stenen[x + k, y + k] = kleur;
                                }
                            }   
                    }
                }

            if (x > 0 && y > 0 && stenen[x - 1, y - 1] == -1 * kleur)
                for (int i = 1; i < Math.Max(kolommen, rijen); i++)
                {
                    if (x - i >= 0 && y - i >= 0)
                    {
                        if (stenen[x - i, y - i] == kleur)
                            if (x > 0 && y > 0 && stenen[x - 1, y - 1] == -1 * kleur)
                            {
                                int r = i;
                                int k = 0;
                                while (k < r)
                                {
                                    k++;
                                    stenen[x - k, y - k] = kleur;
                                }
                            }        
                    }
                }
        }

        // Telmethode zet de tellers voor blauwe en rode stenen op nul en kijkt voor elk vak in het bord of het rood of blauw is.
        private void Telmethode()
        {
            trood = 0; tblauw = 0;
            for (int i = 0; i < kolommen; i++)
                for (int j = 0; j < rijen; j++)
                {
                    if (stenen[i, j] == 1)
                        trood++;
                    if (stenen[i, j] == -1)
                        tblauw++;
                }
        }

        //BeginStand tekent de vier stenen die al op het bord liggen aan het begin van het spel.
        private void Beginstand()
        {
            int centerX = kolommen / 2;
            int centerY = rijen / 2;
            stenen[centerX - 1, centerY - 1] = -1;
            stenen[centerX, centerY] = -1;
            stenen[centerX - 1, centerY] = 1;
            stenen[centerX, centerY - 1] = 1;
        }

        // Klik-event dat bij een klik, een steen tekent (om de beurt rood / blauw) op de geklikte locatie in het bord.
        public void Klik(object obj, MouseEventArgs mea)
        {
            x = (mea.X - bordx) / (breedte / kolommen);
            y = (mea.Y - bordy) / (hoogte / rijen);
            
            if (mea.X >= bordx && x < kolommen && mea.Y >= bordy && y < rijen && legaliteit(x,y) == true)
            {
                if (stenen[x, y] == 0)
                {
                    stenen[x, y] = kleur;
                    Ingesloten(x, y);
                    kleur *= -1;

                    if (stenen[x, y] == -1)
                    {
                        label3.Text = "Rood aan zet";
                        Telmethode();
                    }
                    if (stenen[x, y] == 1)
                    {
                        label3.Text = "Blauw aan zet";
                        Telmethode();
                        
                    }
                    // Hier wordt de status van het spel bepaald.
                    if (trood == kolommen * rijen)
                    label3.Text = "Rood is winnaar!";
                    if (tblauw == kolommen * rijen)
                    label3.Text = "Blauw is winnaar!";
                    
                    for (int i = 0; i < kolommen; i++)
                        for (int j = 0; j < rijen; j++)
                            if (stenen[i, j] == 0)
                                break;
                            else if (trood > tblauw)
                                    label3.Text = "Rood is winnaar!";
                            else if (tblauw > trood)
                                    label3.Text = "Blauw is winnaar!";
                            else
                                    label3.Text = "Gelijkspel!";

                }
                this.Invalidate();
            }
        }

        // kolommen-textbox textchanged-event verandert het aantal kolommen naar de ingevulde waarde.
        private void tbkolommen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbkolommen.Text.Length != 0 && int.Parse(tbkolommen.Text) > 2 && trood == 2)
                {
                    kolommen = int.Parse(tbkolommen.Text);
                    stenen = new int[kolommen, rijen];
                    Beginstand();
                }
                this.Invalidate();
            }
            catch (Exception)
            {
                this.Invalidate();
            }
        }

        // rijen-textbox textchanged-event verandert het aantal rijen naar de ingevulde waarde.
        private void tbrijen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbrijen.Text.Length != 0 && int.Parse(tbrijen.Text) > 2 && trood == 2)
                {
                    rijen = int.Parse(tbrijen.Text);
                    stenen = new int[kolommen, rijen];
                    Beginstand();
                }
                this.Invalidate();
            }
            catch (Exception)
            {
                this.Invalidate();
            }
        }

        //De methode Legenda tekent de rode en blauwe steen die in de GUI staan en vult de labels in.
        private void Legenda(object o, PaintEventArgs pea)
        {
            int diameter = 40;

            pea.Graphics.FillEllipse(Brushes.Red, breedte + 75, 75, diameter, diameter);
            pea.Graphics.DrawEllipse(Pens.Black, breedte + 75, 75, diameter, diameter);
            pea.Graphics.FillEllipse(Brushes.Blue, breedte + 75, 120, diameter, diameter);
            pea.Graphics.DrawEllipse(Pens.Black, breedte + 75, 120, diameter, diameter);

            this.label3.Location = new Point(575, 200);
            this.label4.Location = new Point(625, 85);
            this.label5.Location = new Point(625, 130);
            this.label4.Text = trood.ToString() + " / " + (kolommen*rijen).ToString() + " stenen";
            this.label5.Text = tblauw.ToString() + " / " + (kolommen * rijen).ToString() + " stenen";
        }
        // "Nieuw Spel" knop
        private void button1_Click(object sender, EventArgs e)
        {
            kolommen = 6;
            rijen = 6;
            stenen = new int[kolommen, rijen];
            trood = 0;
            tblauw = 0;
            help = false;
            tbkolommen.Text = "";
            tbrijen.Text = "";
            kleur = -1;
            helpknop = 0;
            this.label3.Text = "Blauw aan zet";
            Beginstand();
            Telmethode();
            
            this.Invalidate();
        }
        // "Help" knop
        private void button2_Click(object sender, EventArgs e)
        {
            helpknop++;
            if(helpknop % 2 == 0)
            help = false;
            else
            help = true;

            this.Invalidate();
        }
    }
}
