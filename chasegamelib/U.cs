using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace chasegame
{
    public struct U
    {
        public const int DX = 16;
        public const int DY = 16;
        public const int NX = 100;
        public const int NY = 60;
        public const int WX = DX * NX;
        public const int WY = DY * NY;
        public const float minspeed = 30.0F;
        public const float maxspeed = 120.0F;

        public static void show(string arg)
        {
            DateTime n = DateTime.Now;
            Console.WriteLine(string.Format("{0:d4}-{1:d2}-{2:d2}_{3:d2}:{4:d2}:{5:d2}.{6:d3} {7}", n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second, n.Millisecond, arg));
        }

		public static void show(string fmt, params object[] objs)
		{
			show(string.Format(fmt, objs));
		}

        public static float Hypot(float x, float y)
        {
            return (float)Hypot((double)x, (double)y);
        }
        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
        public static float Hypot(PointF a)
        {
            return Hypot(a.X, a.Y);
        }

        public static PointF Subtract(Point a, Point b)
        {
            return new PointF(a.X-b.X, a.Y-b.Y);
        }
        public static PointF Scale(PointF a, float scale) {
            return new PointF(a.X * scale, a.Y * scale);
        }
		public static PointF Scale(PointF a, double scale) {
			return new PointF((float)(a.X * scale), (float)(a.Y * scale));
		}

        public static PointF Dir(double alpha)
        {
            return new PointF((float)Math.Cos(alpha), (float)Math.Sin(alpha));
        }
    }

    /*
    class ATest1 : Form
    {
        private Bitmap image;
        private Timer timer;
        private double alpha;
        private DateTime t0;

        ATest1() {
            image = new Bitmap("../../nastya-head.png");
            alpha = 0.0;
            t0 = DateTime.Now;

            Size = new Size(500, 500);
            BackColor = Color.Black;
            this.DoubleBuffered = true;

            timer = new Timer(new Container());
            timer.Interval = 50;
            timer.Enabled = true;
            timer.Tick += new EventHandler(this.OnTick);

            Paint += new PaintEventHandler(this.OnPaint);
        }

        void OnTick(object sender, EventArgs a)
        {
            DateTime now = DateTime.Now;
            var dt = now - t0;
            alpha += dt.TotalMilliseconds * 0.0005;
            t0 = now;
            this.Refresh();
        }

        void OnPaint(object sender, PaintEventArgs a)
        {
            var g = a.Graphics;
            // g.DrawImage(image, 0, 0);
            PointF[] dest = new PointF[3];
            float x = image.Size.Width;
            float y = image.Size.Height;
            float x0 = 50.0F;
            float y0 = 50.0F;
            float cosa = (float)Math.Cos(alpha);
            float sina = (float)Math.Sin(alpha);
            dest[0] = new PointF(x0,y0);
            dest[1] = new PointF(x0 + x * cosa, y0 - x * sina);
            dest[2] = new PointF(x0 + y * sina, y0 + y * cosa);
            g.DrawImage(image, dest[0]);
            g.DrawImage(image, dest); 
        }

        public static void Main(string[] args)
        {
            U.show("main");
            Application.Run(new ATest1());
        }
    }
    */
}
