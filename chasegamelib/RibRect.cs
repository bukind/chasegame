using System;
using System.Drawing;
using System.Collections.Generic;

namespace chasegame
{
	public struct RibRect
	{
		public Vec min;
		public Vec max;

		public RibRect(Rib r) {
			var radius = r.Sprite.Radius;
			min = r.StartPos;
			max = r.EndPos;
			if (max.X < min.X) {
				var t = max.X;
				max.X = min.X;
				min.X = t;
			}
			if (max.Y < min.Y) {
				var t = max.Y;
				max.Y = min.Y;
				min.Y = t;
			}
			min.X -= radius;
			min.Y -= radius;
			max.X += radius;
			max.Y += radius;
		}

		/*
		public void FindCollision(List<RibRect> rects, Dictionary<Rib,Collision> coldict)
		{
			if (rib == null || rib.sprite == null) {
				return;
			}
			List<RibRect> possible = rects.FindAll(this.possiblyCollide);
			foreach (RibRect next in possible) {
				double t = collide(next);
				// FIXME
			}
		}

		private bool possiblyCollide(RibRect rect)
		{
			if (rect.rib.sprite == null ||
				rib.sprite.Equals(rect.rib.sprite)) {
				return false;
			}
			if ((rib.time1 < rect.rib.time0) ||
				(rect.rib.time1 < rib.time0)) {
				// missing time
				return false;
			}
			if (((min.X <= rect.max.X) && (rect.min.X <= max.X)) &&
				((min.Y <= rect.max.Y) && (rect.min.Y <= min.Y))) {
				// overlap exists
				return true;
			}
			return false;
		}

		private double collide(RibRect rect)
		{
			// FIXME
			return -1;
		}
		*/
	}
}
