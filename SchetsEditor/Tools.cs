using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;
        public static int lijndikte = 3;
        

        public virtual void MuisVast(SchetsControl s, Point p)
        {   startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {   kwast = new SolidBrush(s.PenKleur);
            
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
    }

    public class TekstTool : StartpuntTool
    {
        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                Graphics gr = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz =
                gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
                gr.DrawString(tekst, font, kwast,
                                              this.startpunt, StringFormat.GenericTypographic);
                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                startpunt.X += (int)sz.Width;
                s.Invalidate();
            }
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                                , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {   Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {   base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {   s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {
            Compleet(s, startpunt, p);
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2);
        
        public virtual void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Element element = new Element(p1, p2, s.PenKleur, this, lijndikte);
            Schets.elementenLijst.Add(element);
            s.Invalidate();
        }
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawRectangle(MaakPen(kwast, lijndikte), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }
    
    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "vlak"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawRectangle(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

    }

    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        { g.DrawEllipse(MaakPen(kwast, lijndikte), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
    }

    public class VolCirkelTool: CirkelTool
    {
        public override string ToString() { return "cirkel "; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawLine(MaakPen(this.kwast, lijndikte), p1, p2);
        }
    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {   this.MuisLos(s, p);
            this.MuisVast(s, p);
        }
    }
    
    public class GumTool : StartpuntTool
    {
        public override string ToString() { return "gum"; }

        public override void MuisVast(SchetsControl s, Point p) { }
        public override void MuisDrag(SchetsControl s, Point p) 
        {
            for(int i = Schets.elementenLijst.Count-1; i >= 0; i--)
            {
                if(Schets.elementenLijst[i].tool is PenTool && p.X == Schets.elementenLijst[i].begin.X && p.Y == Schets.elementenLijst[i].begin.Y)
                {
                    s.Schets.Schoon();
                    Schets.elementenLijst.RemoveAt(i);
                    s.Invalidate();
                    break;
                }
            }
        }
        public override void MuisLos(SchetsControl s, Point p) 
        {
            for(int i = Schets.elementenLijst.Count-1; i >= 0; i--)
            {
                if (p.X > Math.Min(Schets.elementenLijst[i].begin.X, Schets.elementenLijst[i].eind.X) && p.X < Math.Max(Schets.elementenLijst[i].begin.X, Schets.elementenLijst[i].eind.X)
                    && p.Y > Math.Min(Schets.elementenLijst[i].begin.Y, Schets.elementenLijst[i].eind.Y) && p.Y < Math.Max(Schets.elementenLijst[i].begin.Y, Schets.elementenLijst[i].eind.Y) 
                    && Schets.elementenLijst[i].tool is VolRechthoekTool)
                {
                    s.Schets.Schoon();
                    Schets.elementenLijst.RemoveAt(i);
                    s.Invalidate();
                    break;
                }

                // m is het midden punt van de ovaal.
                // a = horizontale diameter
                // b = verticale diameter
                int kleinstex = Math.Min(Schets.elementenLijst[i].begin.X, Schets.elementenLijst[i].eind.X);
                int kleinstey = Math.Min(Schets.elementenLijst[i].begin.Y, Schets.elementenLijst[i].eind.Y);
                int grootstex = Math.Max(Schets.elementenLijst[i].begin.X, Schets.elementenLijst[i].eind.X);
                int grootstey = Math.Max(Schets.elementenLijst[i].begin.Y, Schets.elementenLijst[i].eind.Y);
                Point m = new Point(kleinstex + (Math.Abs(Schets.elementenLijst[i].eind.X - Schets.elementenLijst[i].begin.X)) / 2
                                  , kleinstey + (Math.Abs(Schets.elementenLijst[i].eind.Y - Schets.elementenLijst[i].begin.Y)) / 2);
                double a = Math.Abs(Schets.elementenLijst[i].eind.X - Schets.elementenLijst[i].begin.X);
                double b = Math.Abs(Schets.elementenLijst[i].eind.Y - Schets.elementenLijst[i].begin.Y);

                if (Schets.elementenLijst[i].tool is VolCirkelTool && (((Math.Pow(p.X - m.X, 2)) / (Math.Pow(a, 2))) + ((Math.Pow(p.Y - m.Y, 2)) / (Math.Pow(b, 2)))) <= 0.27)
                {
                    s.Schets.Schoon();
                    Schets.elementenLijst.RemoveAt(i);
                    s.Invalidate();
                    break;
                }

                if (Schets.elementenLijst[i].tool is CirkelTool && (((Math.Pow(p.X - m.X, 2)) / (Math.Pow(a, 2))) + ((Math.Pow(p.Y - m.Y, 2)) / (Math.Pow(b, 2)))) < 0.27 &&
                     (((Math.Pow(p.X - m.X, 2)) / (Math.Pow(a, 2))) + ((Math.Pow(p.Y - m.Y, 2)) / (Math.Pow(b, 2)))) > 0.23)
                {
                    s.Schets.Schoon();
                    Schets.elementenLijst.RemoveAt(i);
                    s.Invalidate();
                    break;
                }

                // rechthoektool
                if (p.X > kleinstex - Schets.elementenLijst[i].dikte && p.X < kleinstex + Schets.elementenLijst[i].dikte && p.Y >= kleinstey && p.Y <= grootstey
                    || p.X > grootstex - Schets.elementenLijst[i].dikte && p.X < grootstex + Schets.elementenLijst[i].dikte && p.Y >= kleinstey && p.Y <= grootstey
                    || p.Y > kleinstey - Schets.elementenLijst[i].dikte && p.Y < kleinstey + Schets.elementenLijst[i].dikte && p.X >= kleinstex && p.X <= grootstex
                    || p.Y > grootstey - Schets.elementenLijst[i].dikte && p.Y < grootstey + Schets.elementenLijst[i].dikte && p.X >= kleinstex && p.X <= grootstex
                    && Schets.elementenLijst[i].tool is RechthoekTool)
                {
                    Schets.elementenLijst.RemoveAt(i);
                    s.Schets.Schoon();
                    s.Invalidate();
                    break;
                }
                /*
                // lijntool
                if (Schets.elementenLijst[i].tool is LijnTool)
                {
                    double a2 = ((Schets.elementenLijst[i].eind.Y - Schets.elementenLijst[i].begin.Y) / (Schets.elementenLijst[i].eind.X - Schets.elementenLijst[i].begin.X));
                    double b2 = Schets.elementenLijst[i].begin.Y;

                    if( (Math.Abs(a2*p.X + b2*p.Y -1 )) / (Math.Sqrt(a2 * a2 + b2 * b2)) <= 15)
                    {
                        s.Schets.Schoon();
                        Schets.elementenLijst.RemoveAt(i);
                        s.Invalidate();
                        break;
                    }
                }
                */
            }
        }
        public override void Letter(SchetsControl s, char c) { }
        
       
    }

    public class Element : Schets
    {
        public Point begin, eind;
        public Color kleur;
        public ISchetsTool tool;
        public int dikte;

        public Element(Point b, Point e, Color co, ISchetsTool t, int PenDikte)
        {
            this.begin = b;
            this.eind = e;
            this.kleur = co;
            this.tool = t;
            this.dikte = PenDikte;
        }
        
        public string stringmaken(Element elem)
        {
            string elementstring;
            elementstring = elem.begin.X.ToString() + "/" + elem.begin.Y.ToString() + "/" + elem.eind.X.ToString() + "/" + elem.eind.Y.ToString() + "/" + elem.kleur.ToArgb().ToString() + "/" + 
                      elem.tool.ToString()  + "/" + elem.dikte.ToString();
            return elementstring;
        }
    }
    public class Uitstring
    {
        public void UitString(string s)
        {
            Point begin, eind;
            Color kleur;
            ISchetsTool tool;
            int dikte;
            

            String sub = s;
            string[] splits = sub.Split('/');
            

            begin = new Point(int.Parse(splits[0]), int.Parse(splits[1]));
            eind = new Point(int.Parse(splits[2]), int.Parse(splits[3]));
            kleur = Color.FromArgb(int.Parse(splits[4]));
            dikte = int.Parse(splits[6]);

            tool = null;

            if (splits[5] == "pen")
            {
                PenTool pent = new PenTool();
                tool = pent;
            }
            if (splits[5] == "lijn")
            {
                LijnTool lijnt = new LijnTool();
                tool = lijnt;
            }
            if (splits[5] == "vlak")
            {
                VolRechthoekTool volrechtt = new VolRechthoekTool();
                tool = volrechtt;
            }

            if (splits[5] == "kader")
            {
                RechthoekTool rechtt = new RechthoekTool();
                tool = rechtt;
            }

            if (splits[5] == "cirkel ")
            {
                VolCirkelTool volcirt = new VolCirkelTool();
                tool = volcirt;
            }

            if (splits[5] == "cirkel")
            {
                CirkelTool cir = new CirkelTool();
                tool = cir;
            }
              
            Element element = new Element(begin, eind, kleur, t: tool, dikte);
            Schets.elementenLijst.Add(element);
        }
    }
        

        
}

