using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace chasegame
{
	struct U
	{
		public const int DX = 16;
		public const int DY = 16;
		public const int NX = 100;
		public const int NY = 60;
		public const int WX = DX * NX;
		public const int WY = DY * NY;

		public static void show(string arg)
		{
			DateTime n = DateTime.Now;
			Console.WriteLine(string.Format("{0:d4}-{1:d2}-{2:d2}_{3:d2}:{4:d2}:{5:d2}.{6:d3} {7}", n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second, n.Millisecond, arg));
		}
	}


	class Sprite
	{
		public float x; // position of the image center
		public float y;
		public float dx; // speed
		public float dy;
		public float alpha; // current angle (radians)
		public float omega; // rotation speed
		public float mass;  // the mass
		public float scale; // scaling factor (recalculated from the mass)
		public float radius; // current radius (recalculated from image size and scaling factor)
		public Bitmap image;

		public Sprite(Bitmap theimg, float themass)
		{
			image = theimg;
			dx = dy = alpha = omega = 0.0F;
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

		public void Move(double dt)
		{
			x += (float)(dx * dt);
			y += (float)(dy * dt);
			alpha += (float)(omega * dt);
			if (dx < 0) {
				if (x < radius) {
					x = radius * 2 - x;
					dx = -dx;
				}
			} else if (x >= U.WX - radius) {
				x = (U.WX - radius) * 2 - x;
				dx = -dx;
			}
			if (dy < 0) {
				if (y < radius) {
					y = radius * 2 - y;
					dy = -dy;
				}
			} else if (y >= U.WY - radius) {
				y = (U.WY - radius) * 2 - y;
				dy = -dy;
			}
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
				var s = new Sprite(sprites[0].image, 0.0F);
				s.x = (float)mouseDownLocation.X;
				s.y = (float)mouseDownLocation.Y;
				s.dx = (float)((a.Location.X - s.x) / 5.0);
				s.dy = (float)((a.Location.Y - s.y) / 5.0);
				if (Math.Sqrt(s.dx * s.dx + s.dy * s.dy) < 0.1) {
					double alpha = Math.PI * 2.0 * random.NextDouble();
					var cosa = Math.Cos(alpha);
					var sina = Math.Sin(alpha);
					double speed = random.NextDouble()*100.0 + 10.0;
					s.dx = (float)(cosa * speed);
					s.dy = (float)(sina * speed);
				}
				s.omega = (float)(random.NextDouble() - 0.5) * 10.0F;
				sprites.Add(s);
				mouseDownLocation = new Point();
				mouseLastLocation = mouseDownLocation;
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


	class TheGame : Form
	{
		TheGame()
		{
			Size = new Size(U.WX, U.WY);
			BackColor = Color.Black;

			Controls.Add(new TheInput(this));
			CenterToScreen();
		}

		public static void Main(string[] args)
		{
			U.show("starting");
			Application.Run(new TheGame());
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
