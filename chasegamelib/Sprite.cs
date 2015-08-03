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
		public Vec Position;
		public Vec Speed;
        private double alpha; // current angle (radians)
        private double omega; // rotation speed
		private double radius;
        private double mass;  // the mass
        private double scale; // scaling factor (recalculated from the mass)
        public Bitmap image;
		private int identity;

		private static int globid = 0;
		private static int getIdentity() {
			return ++globid;
		}

		public Sprite()
		{
			identity = getIdentity();
		}

		public Sprite(Bitmap theimg, double themass) : this()
        {
            image = theimg;
			Speed = new Vec();
            alpha = omega = 0.0;
            // init is the initial radius
            var init = Math.Min(image.Size.Width, image.Size.Height) * 0.5;
            if (themass <= 0.001) {
                themass = init * init;
            }
            mass = themass;
			radius = Math.Sqrt(mass);
            scale = radius / init * 0.5;
			Position = new Vec(image.Size).Scale(scale);
        }

		public Sprite(Bitmap theimg, Sprite one, Sprite two) : this(theimg, one.mass + two.mass)
		{
			// now calculate the position, speed and rotation
			double invertmass = 1.0/mass;
			Position = one.Position.Scale(one.Mass).Add(two.Position.Scale(two.Mass)).Scale(invertmass);
			Speed = one.Speed.Scale(one.Mass).Add(two.Speed.Scale(two.mass)).Scale(invertmass);
			alpha = (Math.IEEERemainder(one.alpha,Math.PI*2) * one.AngularMass + Math.IEEERemainder(two.alpha,Math.PI*2) * two.AngularMass) / AngularMass;
			// sum up all moments of rotation
			double totalRotationMomentum = 
				one.Mass * one.Position.Subtract(Position).Normal(one.Speed).Length +
				two.Mass * two.Position.Subtract(Position).Normal(two.Speed).Length +
				one.AngularMass * one.omega + two.AngularMass * two.omega;
			omega = totalRotationMomentum / AngularMass;
		}

		public bool IsValid() {
			return image != null;
		}

        public double Rotation {
            get { return omega; }
            set { omega = value; }
        }
		public double Radius { get { return radius; } }
		public int Id { get { return identity; } }
		public double Mass { get { return mass; } }
		public double AngularMass { get { return mass * radius * radius / 2.0; } }

		public void Update(Rib rib) {
			Position = rib.EndPos;
			Speed = rib.Speed;
			alpha += omega * rib.DeltaTime;
		}

		/*
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
        */

		public void MakeMove(double starttime, double endtime, List<Rib> ribs)
		{
			Rib r = new Rib(this, endtime, starttime);
			r.Clip(ribs);
			ribs.Add(r);
		}

        public void Draw(Graphics g)
        {
            PointF[] dest = new PointF[3];
			Rot rotation = new Rot(alpha);
			Vec sz = new Vec(image.Size).Scale(scale);
            // x + px*cosa+py*sina, y - px*sina + py*cosa 
			dest[0] = Position.Add(rotation.Rotate(-sz.X, -sz.Y)).Point;
			dest[1] = Position.Add(rotation.Rotate(sz.X, -sz.Y)).Point;
			dest[2] = Position.Add(rotation.Rotate(-sz.X, sz.Y)).Point;
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
