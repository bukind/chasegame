using System;
using System.Drawing;
using System.Collections.Generic;

namespace chasegame
{
	public class Rib
	{
		public Vec speed;
		public Vec pos0;
		public Vec pos1;
		public double time0; // relative time
		public double time1;
		public Sprite sprite;

		public Rib(Sprite spr, double deltat)
		{
			sprite = spr;
			speed = sprite.Speed;
			pos0 = sprite.Position;
			pos1 = pos0.Add(speed.Scale(deltat));
			time0 = 0.0;
			time1 = deltat;
		}

		public Rib(Rib other)
		{
			speed = other.speed;
			pos0 = other.pos0;
			pos1 = other.pos1;
			time0 = other.time0;
			time1 = other.time1;
			sprite = other.sprite;
		}

		public RibRect Rect {
			get { return new RibRect(this); }
		}

		private void SetSpeed(Vec spd) {
			speed = spd;
			pos1 = pos0.Add(speed.Scale(time1 - time0));
		}

		public void Clip(List<Rib> ribs)
		{
			var radius = sprite.Radius;
			bool hit = false;
			double poshit = 0.0;
			if (speed.X < 0) {
				if (pos1.X < radius) {
					hit = true;
					poshit = radius;
				}
			} else if (pos1.X > U.WX - radius) {
				hit = true;
				poshit = U.WX - radius;
			}
			if (hit) {
				// clipped by X
				double thit = (poshit - pos0.X) / speed.X;
				Rib next = Split(thit);
				next.SetSpeed(new Vec(-speed.X,speed.Y));
				next.Clip(ribs);
				ribs.Add(next);
				hit = false;
			}

			if (speed.Y < 0) {
				if (pos1.Y < radius) {
					hit = true;
					poshit = radius;
				}
			} else if (pos1.Y > U.WY - radius) {
				hit = true;
				poshit = U.WY - radius;
			}
			if (hit) {
				double thit = (poshit - pos0.Y) / speed.Y;
				Rib next = Split(thit);
				next.SetSpeed(new Vec(speed.X, -speed.Y));
				next.Clip(ribs);
				ribs.Add(next);
				hit = false;
			}
		}

		public Rib Split(double deltat)
		{
			Rib next = new Rib(this);
			next.pos0 = pos0.Add(speed.Scale(deltat));
			next.pos1 = pos1;
			pos1 = next.pos0;
			next.time0 = time0 + deltat;
			next.time1 = time1;
			time1 = next.time0;
			return next;
		}

		public int CompareTo(Rib r)
		{
			return pos0.X < r.pos0.X ? -1 : (pos0.X > r.pos0.X ? 1 : 0);
		}
	}
}

