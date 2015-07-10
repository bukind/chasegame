using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

// The axis are like this:
// *-------> X
// |
// |
// V Y
//
// And the angle alpha is calculated from axis X in the counter-clockwise direction.

namespace chasegame
{
    public class Sprite
    {
		private float x; // position of the image center
        private float y;
        private float vx; // speed
        private float vy;
        private float alpha; // current angle (radians)
        private float omega; // rotation speed
        private float mass;  // the mass
        private float scale; // scaling factor (recalculated from the mass)
        private float radius; // current radius (recalculated from image size and scaling factor)
        private Bitmap image;

        public Sprite(Bitmap theimg, float themass)
        {
            image = theimg;
            vx = vy = alpha = omega = 0.0F;
            // init is the initial radius
            float init = Math.Min(image.Size.Width, image.Size.Height) * 0.5F;
            if (themass <= 0.001F) {
                themass = init * init;
            }
            mass = themass;
            scale = (float)(Math.Sqrt(mass) / init) * 0.5F;
            radius = init * 2 * scale;
            x = image.Size.Width * scale;
            y = image.Size.Height * scale;
        }

        public PointF Position {
            get { return new PointF(x, y); }
            set {
                x = value.X;
                y = value.Y;
            }
        }

        public PointF Speed {
            get { return new PointF(vx, vy); }
            set {
                vx = value.X;
                vy = value.Y;
            }
        }
        public float Rotation {
            get { return omega; }
            set { omega = value; }
        }

		public float Radius {
			get { return radius; }
		}

        public void Move(double dt)
        {
            x += (float)(vx * dt);
            y += (float)(vy * dt);
            alpha += (float)(omega * dt);
            if (vx < 0) {
                if (x < radius) {
                    x = radius * 2 - x;
                    vx = -vx;
                }
            } else if (x >= U.WX - radius) {
                x = (U.WX - radius) * 2 - x;
                vx = -vx;
            }
            if (vy < 0) {
                if (y < radius) {
                    y = radius * 2 - y;
                    vy = -vy;
                }
            } else if (y >= U.WY - radius) {
                y = (U.WY - radius) * 2 - y;
                vy = -vy;
            }
        }

		public void MakeMove(double dt, List<Rib> ribs)
		{
			Rib r = new Rib(this, dt);
			r.Clip(ribs);
			ribs.Add(r);
		}

        public void Draw(Graphics g)
        {
            PointF[] dest = new PointF[3];
            float cosa = (float)Math.Cos(alpha);
            float sina = (float)Math.Sin(alpha);
            var wx = image.Size.Width * scale;
            var wy = image.Size.Height * scale;
            // x + px*cosa+py*sina, y - px*sina + py*cosa 
            dest[0] = new PointF(x - wx * cosa - wy * sina, y + wx * sina - wy * cosa);
            dest[1] = new PointF(x + wx * cosa - wy * sina, y - wx * sina - wy * cosa);
            dest[2] = new PointF(x - wx * cosa + wy * sina, y + wx * sina + wy * cosa);
            g.DrawImage(image, dest);
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
