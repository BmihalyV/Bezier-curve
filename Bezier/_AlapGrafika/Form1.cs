using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _AlapGrafika
{
    public partial class Form1 : Form
    {

        #region Globális változók
        Graphics g;
        PointF p0, p1, p2, p3;
        int index = -1;
        #endregion

        public Form1()
        {
            InitializeComponent();

            p0 = new PointF(10, 10);
            p1 = new PointF(10, canvas.Height - 10);
            p2 = new PointF(canvas.Width - 10, canvas.Height - 10);
            p3 = new PointF(canvas.Width - 10, 10);
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            DrawBezier3Arc(Color.Red, Color.Blue, p0, p1, p2, p3);
            g.DrawLine(new Pen(Color.Black), p0, p1);
            g.DrawLine(new Pen(Color.Black), p1, p2);
            g.DrawLine(new Pen(Color.Black), p2, p3);
            g.FillRectangle(new SolidBrush(Color.Black), p0.X - 5, p0.Y - 5, 10, 10);
            g.FillRectangle(new SolidBrush(Color.Black), p1.X - 5, p1.Y - 5, 10, 10);
            g.FillRectangle(new SolidBrush(Color.Black), p2.X - 5, p2.Y - 5, 10, 10);
            g.FillRectangle(new SolidBrush(Color.Black), p3.X - 5, p3.Y - 5, 10, 10);


        }

        #region Egérkezelés
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsGrab(p0, 5, e.Location)) index = 0;
            else if (IsGrab(p1, 5, e.Location)) index = 1;
            else if (IsGrab(p2, 5, e.Location)) index = 2;
            else if (IsGrab(p3, 5, e.Location)) index = 3;

        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (index != -1)
            {
                switch (index)
                {
                    //Ha p0, p1-et mozgatjuk, akkor vigyék magukkal a hozzájuk tartozó
                    //érintő végpontokat (t0, t1)
                    case 0: p0 = e.Location; break;
                    case 1: p1 = e.Location; break;
                    case 2: p2 = e.Location; break;
                    case 3: p3 = e.Location; break;
                    default: break;
                }
                canvas.Refresh();
            }

        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            index = -1;
        }
        #endregion

        private bool IsGrab(PointF p, int s, PointF mouseLocation)
        {
            return p.X - s <= mouseLocation.X && mouseLocation.X <= p.X + s &&
                   p.Y - s <= mouseLocation.Y && mouseLocation.Y <= p.Y + s;
        }


        private double B0(double t) { return (1 - t) * (1 - t) * (1 - t); }
        private double B1(double t) { return 3 * t * (1 - t) * (1 - t); }
        private double B2(double t) { return 3 * t * t * (1 - t); }
        private double B3(double t) { return t * t * t; }

        private void DrawBezier3Arc(Color c0, Color c1, PointF p0, PointF p1, PointF p2, PointF p3)
        {
            double a = 0;
            double t = a;
            double h = 1.0 / 500.0;
            PointF d0, d1;

            #region Színátmenet
            double cR = c0.R;
            double cG = c0.G;
            double cB = c0.B;

            int dR = c1.R - c0.R;
            int dG = c1.G - c0.G;
            int dB = c1.B - c0.B;

            double incR = (double)dR / 500;
            double incG = (double)dG / 500;
            double incB = (double)dB / 500;
            #endregion


            d0 = new PointF((float)(B0(t) * p0.X + B1(t) * p1.X + B2(t) * p2.X + B3(t) * p3.X),
                            (float)(B0(t) * p0.Y + B1(t) * p1.Y + B2(t) * p2.Y + B3(t) * p3.Y));


            while (t < 1)
            {
                t += h;
                d1 = new PointF((float)(B0(t) * p0.X + B1(t) * p1.X + B2(t) * p2.X + B3(t) * p3.X),
                                (float)(B0(t) * p0.Y + B1(t) * p1.Y + B2(t) * p2.Y + B3(t) * p3.Y));

                g.DrawLine(new Pen(Color.FromArgb((int)(cR), (int)(cG), (int)(cB)), 2f), d0, d1);

                cR += incR;
                cG += incG;
                cB += incB;

                d0 = d1;
            }
        }
    }
}
