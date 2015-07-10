using System;
using System.Drawing;

namespace chasegame
{
	public struct Vec
	{
		public double X;
		public double Y;

		public Vec(Point p)
		{
			X = p.X;
			Y = p.Y;
		}

		public Vec(PointF p)
		{
			X = p.X;
			Y = p.Y;
		}

		public Vec(double x, double y)
		{
			X = x;
			Y = y;
		}

		public PointF Point {
			get { return new PointF((float)X, (float)Y); }
		}

		public double Length {
			get { return Math.Sqrt(X * X + Y * Y); }
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

		public override string ToString() {
			return string.Format("({0},{1})", X, Y);
		}
	}
}

