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
		public float x;
		public float y;
		public float dx;
		public float dy;
		public Bitmap image;
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
			var s = new Sprite();
			s.x = 0;
			s.y = 0;
			s.dx = 0;
			s.dy = 0;
			s.image = images[0];
			curimage = 0;
			sprites.Add(s);

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
					curimage = images.Count;
				}
				sprites[0].image = images[curimage];
			} else if (a.KeyCode == Keys.Right) {
				curimage += 1;
				if (curimage >= images.Count) {
					curimage = 0;
				}
				sprites[0].image = images[curimage];
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
				var s = new Sprite();
				s.x = (float)(mouseDownLocation.X + imageCross.Size.Width / 2.0);
				s.y = (float)(mouseDownLocation.Y + imageCross.Size.Height / 2.0);
				s.dx = (float)((a.Location.X - s.x) / 100.0);
				s.dy = (float)((a.Location.Y - s.y) / 100.0);
				s.image = sprites[0].image;
				s.x -= (float)(s.image.Size.Width / 2.0);
				s.y -= (float)(s.image.Size.Height / 2.0);
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
			var dt = now.Subtract(t0).TotalMilliseconds;
			foreach (Sprite s in sprites) {
				var x = s.x + s.dx * dt;
				var y = s.y + s.dy * dt;
				if (s.dx < 0) {
					if (x < 0) {
						s.dx = -s.dx;
						x = -x;
					}
				} else if (x >= U.WX - s.image.Size.Width) {
					x = (U.WX - s.image.Size.Width) * 2 - x;
					s.dx = -s.dx;
				}
				if (s.dy < 0) {
					if (y < 0) {
						s.dy = -s.dy;
						y = -y;
					}
				} else if (y >= U.WY - s.image.Size.Height) {
					y = (U.WY - s.image.Size.Height) * 2 - y;
					s.dy = -s.dy;
				}
				s.x = (float)x;
				s.y = (float)y;
			}
			t0 = now;
		}

		private void OnPaint(object sender, PaintEventArgs a)
		{
			U.show("OnPaint");
			Graphics g = a.Graphics;

			// draw all sprites we have
			foreach (Sprite s in sprites) {
				g.DrawImage(s.image, s.x, s.y);
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
	*/

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
}
