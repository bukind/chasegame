using System;
using System.Drawing;

namespace chasegame
{
	public struct Rot
	{
		private Vec rot;

		public Rot(double alpha) {
			rot = new Vec(Math.Cos(alpha), Math.Sin(alpha));
		}

		public Vec Rotate(Vec a) {
			return Rotate(a.X, a.Y);
		}

		public Vec Rotate(double x, double y) {
			return new Vec(x * rot.X + y * rot.Y, -x * rot.Y + y * rot.X);
		}
	}

	public struct Vec
	{
		public double X;
		public double Y;
	
		public Vec() {
			X = Y = 0.0;
		}

		public Vec(Point p) {
			X = p.X;
			Y = p.Y;
		}

		public Vec(Size p) {
			X = p.Width;
			Y = p.Height;
		}

		public Vec(PointF p) {
			X = p.X;
			Y = p.Y;
		}

		public Vec(double x, double y) {
			X = x;
			Y = y;
		}

		public static Vec Dir(double alpha) {
			return new Vec(Math.Cos(alpha),Math.Sin(alpha));
		}

		public PointF Point {
			get { return new PointF((float)X, (float)Y); }
		}

		public double Length {
			get { return Math.Sqrt(X * X + Y * Y); }
		}

		public double Length2 {
			get { return X * X + Y * Y; }
		}

		public Vec Add(Vec x) {
			var tmp = this;
			tmp.X += x.X;
			tmp.Y += x.Y;
			return tmp;
		}

		public Vec Subtract(Vec x) {
			var tmp = this;
			tmp.X -= x.X;
			tmp.Y -= x.Y;
			return tmp;
		}

		public Vec Scale(double x) {
			var tmp = this;
			tmp.X *= x;
			tmp.Y *= x;
			return tmp;
		}

		public double Scalar(Vec v) {
			return X * v.X + Y * v.Y;
		}

		public override string ToString() {
			return string.Format("({0},{1})", X, Y);
		}
	}
}

