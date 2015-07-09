using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace chasegame
{
	class TheInput : UserControl
	{
		private Point mouseDownLocation;
		private Point mouseLastLocation;
		private Timer timer;
		private DateTime t0;
		private Bitmap imageCross;
		private List<Bitmap> images;
		private List<Sprite> sprites;
		private int curimage;
		private Random random;

		public TheInput(Form parent)
		{
			this.DoubleBuffered = true;

			Parent = parent;
			Dock = DockStyle.Fill;

			try {
				imageCross = new Bitmap("../../cross.png");
				images = new List<Bitmap>();
				images.Add(new Bitmap("../../spark.png"));
				images.Add(new Bitmap("../../katya-head.png"));
				images.Add(new Bitmap("../../nastya-head.png"));
			} catch (Exception e) {
				Console.WriteLine("cannot load images:" + e.Message);
				Environment.Exit(1);
			}
			t0 = DateTime.Now;
			sprites = new List<Sprite>();
			sprites.Add(new Sprite(images[0], 0.0F));
			curimage = 0;
			random = new Random();

			KeyUp += new KeyEventHandler(this.OnKeyUp);
			MouseDown += new MouseEventHandler(this.OnMouseDown);
			MouseUp += new MouseEventHandler(this.OnMouseUp);
			MouseMove += new MouseEventHandler(this.OnMouseMove);

			timer = new Timer(new Container());
			timer.Interval = 50;
			timer.Enabled = true;
			timer.Tick += new EventHandler(this.OnTick);

			Paint += new PaintEventHandler(this.OnPaint);
		}

		void OnKeyUp(object sender, KeyEventArgs a)
		{
			U.show("OnKeyUp");
			if (a.KeyCode == Keys.Escape) {
				U.show("Ended");
				Environment.Exit(0);
			} else if (a.KeyCode == Keys.Left) {
				curimage -= 1;
				if (curimage < 0) {
					curimage = images.Count - 1;
				}
				sprites[0] = new Sprite(images[curimage], 0.0F);
			} else if (a.KeyCode == Keys.Right) {
				curimage += 1;
				if (curimage >= images.Count) {
					curimage = 0;
				}
				sprites[0] = new Sprite(images[curimage], 0.0F);
			} else if (a.KeyCode == Keys.Back) {
				sprites.RemoveRange(1, sprites.Count - 1);
			}
		}

		void OnMouseDown(object sender, MouseEventArgs a)
		{
			if (a.Button == MouseButtons.Left) {
				U.show("OnMouseDown");
				mouseDownLocation = a.Location;
				mouseLastLocation = a.Location;
			}
		}

		void OnMouseMove(object sender, MouseEventArgs a)
		{
			if (a.Button == MouseButtons.Left) {
				U.show("OnMouseMove");
				mouseLastLocation = a.Location;
			}
		}

		void OnMouseUp(object sender, MouseEventArgs a)
		{
			if (a.Button == MouseButtons.Left && !mouseDownLocation.IsEmpty) {
				U.show("OnMouseUp");
				PointF pos = new PointF(mouseDownLocation.X, mouseDownLocation.Y);
				PointF spd = U.SubtractScale(a.Location, mouseDownLocation, 0.4F);
				float speedlen = U.Hypot(spd);
				if (speedlen > U.maxspeed) {
					spd = U.Scale(spd,U.maxspeed/speedlen);
				} else if (speedlen < U.minspeed) {
					// make random speed
					double alpha = Math.PI * 2.0 * random.NextDouble();
					speedlen = (float)(random.NextDouble() * (U.maxspeed-U.minspeed) + U.minspeed);
					spd = U.Scale(U.Dir(alpha), speedlen);
				}
				var s = new Sprite(sprites[0].image, 0.0F);
				s.Position = pos;
				s.Speed = spd;
				s.Rotation = (float)(random.NextDouble() - 0.5) * 10.0F;
				sprites.Add(s);
				mouseDownLocation = new Point();
				mouseLastLocation = mouseDownLocation;
			} else if (a.Button == MouseButtons.Right) {
				curimage += 1;
				if (curimage >= images.Count) {
					curimage = 0;
				}
				sprites[0] = new Sprite(images[curimage], 0.0F);
			}
		}

		void OnTick(object sender, EventArgs a)
		{
			U.show("OnTick");
			move();
			this.Refresh();
		}

		private void move()
		{
			var now = DateTime.Now;
			var dt = now.Subtract(t0).TotalMilliseconds / 1000.0;
			foreach (Sprite s in sprites) {
				s.Move(dt);
			}
			t0 = now;
		}

		private void OnPaint(object sender, PaintEventArgs a)
		{
			U.show("OnPaint");
			Graphics g = a.Graphics;

			// draw all sprites we have
			foreach (Sprite s in sprites) {
				s.Draw(g);
			}

			if (!mouseDownLocation.IsEmpty) {
				g.DrawImage(imageCross, mouseDownLocation);
				if (!mouseLastLocation.IsEmpty) {
					Pen pen = new Pen(new SolidBrush(Color.White));
					PointF start = new PointF((float)(mouseDownLocation.X + imageCross.Size.Width / 2.0),
						(float)(mouseDownLocation.Y + imageCross.Size.Height / 2.0));
					g.DrawLine(pen, start, mouseLastLocation);
				}
			}
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
