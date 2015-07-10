using System;
using System.Drawing;
using System.Windows.Forms;

namespace chasegame
{
    class Program : Form
    {
        Program()
        {
            Size = new Size(U.WX, U.WY);
            BackColor = Color.Black;

            Controls.Add(new TheInput(this));
            CenterToScreen();
        }

        public static void Main(string[] args)
        {
            U.show("starting");
            Application.Run(new Program());
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
