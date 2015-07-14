using System;
using System.Drawing;
using System.Collections.Generic;

namespace chasegame
{
	public class Rib
	{
		private Vec speed;
		private Vec pos0;
		private Vec pos1;
		private double time0; // relative time
		private double time1;
		private Sprite sprite;
		private Collision collision;

		public Vec StartPos { get { return pos0; } }
		public Vec EndPos { get { return pos1; } }
		public Vec Speed { get { return speed; } }
		public double DeltaTime { get { return time1 - time0; } }
		public double StartTime { get { return time0; } }
		public double EndTime { get { return time1; } }
		public Sprite Sprite { get { return sprite; } }
		public Collision Collision { get { return collision == null ? new Collision(this) : collision; } }

		public Rib(Sprite spr, double deltat)
		{
			sprite = spr;
			speed = sprite.Speed;
			pos0 = sprite.Position;
			pos1 = pos0.Add(speed.Scale(deltat));
			time0 = 0.0;
			time1 = deltat;
			collision = null;
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

		private void setSpeed(Vec spd) {
			speed = spd;
			pos1 = pos0.Add(speed.Scale(DeltaTime));
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
				next.setSpeed(new Vec(-speed.X,speed.Y));
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
				next.setSpeed(new Vec(speed.X, -speed.Y));
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

		public static int CompareByTime(Rib a, Rib b) {
			return a.time0.CompareTo(b.time0);
		}

		public bool CollidesWith(Rib rib, out double hittime)
		{
			// we are working in the reference frame of this.pos0, moving with the speed this.speed
			Vec ribpos = rib.pos0.Subtract(pos0);
			Vec ribspd = rib.speed.Subtract(speed);
			double time = time0;
			if (time0 < rib.time0) {
				// correct the position
				time = rib.time0;
				ribpos = ribpos.Subtract(speed.Scale(time - time0));
			} else if (time0 > rib.time0) {
				ribpos = ribpos.Add(rib.speed.Scale(time - rib.time0));
			}
			var spd2 = ribspd.Length2;
			if (spd2 <= 0.00001) {
				hittime = 0;
				return false;
			}
			// Let's only find the closest position
			// Xclose = X - (X*V)*V / (V*V)
			// Tclose = -(X*V) / (V*V)
			hittime = -ribpos.Scalar(ribspd) / spd2;
			if (hittime < time ||
				hittime > time1 ||
				hittime > rib.time1) {
				return false;
			}
			return true;
		}
	}
}