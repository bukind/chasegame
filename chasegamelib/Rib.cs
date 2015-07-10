using System;
using System.Drawing;
using System.Collections.Generic;

namespace chasegame
{
	public struct Rib : IComparable< Rib >
	{
		public PointF speed;
		public PointF pos0;
		public PointF pos1;
		public float  time0; // relative time
		public float  time1;
		public Sprite sprite;

		public Rib(Sprite spr, double dt)
		{
			sprite = spr;
			speed = sprite.Speed;
			pos0 = sprite.Position;
			pos1 = pos0 + new SizeF(U.Scale(speed, dt));
			time0 = 0.0f;
			time1 = (float)dt;
		}

		public void Clip(List<Rib> ribs)
		{
			var radius = sprite.Radius;
			bool hit = false;
			float poshit = 0.0F;
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
				float thit = (poshit - pos0.X) / speed.X;
				Rib next = new Rib();
				next.speed = speed;
				next.speed.X = -next.speed.X;
				next.pos0 = pos0 + new SizeF(U.Scale(speed, thit));
				next.pos1 = pos1;
				next.pos1.X = 2 * poshit - pos1.X;
				next.time0 = time0 + thit;
				next.time1 = time1;
				next.sprite = sprite;
				// clip self
				pos1 = next.pos0;
				time1 = next.time0;
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
				float thit = (poshit - pos0.Y) / speed.Y;
				Rib next = new Rib();
				next.speed = speed;
				next.speed.Y = -next.speed.Y;
				next.pos0 = pos0 + new SizeF(U.Scale(speed, thit));
				next.pos1 = pos1;
				next.pos1.Y = 2 * poshit - pos1.Y;
				next.time0 = time0 + thit;
				next.time1 = time1;
				next.sprite = sprite;
				// clip self
				pos1 = next.pos0;
				time1 = next.time0;
				next.Clip(ribs);
				ribs.Add(next);
				hit = false;
			}
		}

		public int CompareTo( Rib r)
		{
			return pos0.X < r.pos0.X ? -1 : (pos0.X > r.pos0.X ? 1 : 0);
		}
	}
}

