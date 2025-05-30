using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace SchetsEditor
{
    public class SchetsControl : UserControl
    {
        private Schets schets;
        private Color penkleur;


        public Color PenKleur
        {
            get { return penkleur; }
        }
        public Schets Schets
        {
            get { return schets; }
        }
        public SchetsControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        public void opslaan()
        {
            Bitmap b = schets.bitmap;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Schets|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bestand.SaveActies(sfd.FileName, this);
            }
        }

        public bool openen()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Graphics g = schets.BitmapGraphics;
                
                foreach (string line in File.ReadLines(ofd.FileName))
                {
                    Uitstring txt = new Uitstring();
                    txt.UitString(line);
                }
                
                this.Invalidate();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void teken(object o, PaintEventArgs pea)
        {
            Graphics g = MaakBitmapGraphics();

            foreach (Element elem in Schets.elementenLijst)
            {
                Brush brush = new SolidBrush(elem.kleur);

                if (elem.tool is RechthoekTool)
                {
                    g.DrawRectangle(TweepuntTool.MaakPen(brush, elem.dikte), TweepuntTool.Punten2Rechthoek(elem.begin, elem.eind));
                }

                if (elem.tool is VolRechthoekTool)
                {
                    g.FillRectangle(brush, TweepuntTool.Punten2Rechthoek(elem.begin, elem.eind));
                }

                if (elem.tool is CirkelTool)
                {
                    
                    g.DrawEllipse(TweepuntTool.MaakPen(brush, elem.dikte), TweepuntTool.Punten2Rechthoek(elem.begin, elem.eind));
                }

                if (elem.tool is VolCirkelTool)
                {
                    
                    g.FillEllipse(brush, TweepuntTool.Punten2Rechthoek(elem.begin, elem.eind));
                }

                if (elem.tool is LijnTool)
                {
                    
                    g.DrawLine(TweepuntTool.MaakPen(brush, elem.dikte), elem.begin, elem.eind);
                }

                
            }
            schets.Teken(pea.Graphics);
        }

        private void veranderAfmeting(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {
            Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public void Schoon(object o, EventArgs ea)
        {
            Schets.Schoon();
            Schets.elementenLijst.Clear();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {
            schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }

        public void VeranderKleur(object obj, EventArgs ea)
        {
            string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {
            string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }

        public void VergrootDikte(object obj, EventArgs ea)
        {
            StartpuntTool.lijndikte++;

        }

        public void VerkleinDikte(object obj, EventArgs ea)
        {
            while (StartpuntTool.lijndikte > 0)
                StartpuntTool.lijndikte--;
        }

        public void KleurKiezer(object o, EventArgs ea)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                penkleur = cd.Color;
            }
        }
    }
    public static class Bestand
    {
        public static void SaveActies(string filename, SchetsControl sc)
        {
            using (StreamWriter s = new StreamWriter(filename))
            {
                
                foreach(Element elem in Schets.elementenLijst)
                {

                    s.WriteLine(elem.stringmaken(elem));

                }
            }
        }
    }
}

