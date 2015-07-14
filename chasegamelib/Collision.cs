using System;
using System.Collections.Generic;

namespace chasegame
{
	public class Collision
	{
		private Rib self;
		private RibRect rect;
		private Dictionary<Rib, double> times;

		public Collision(Rib rib)
		{
			self = rib;
			rect = new RibRect(rib);
			times = new Dictionary<Rib, double>();
		}

		public void Fill(List<Rib> ribs)
		{
			foreach (Rib rib in ribs) {
				if (self.Equals(rib)) {
					continue;
				}
				if (self.EndTime < rib.StartTime ||
				    rib.EndTime < self.StartTime) {
					continue;
				}
				if (!rect.Overlaps(rib.Collision.rect)) {
					continue;
				}
				if (times.ContainsKey(rib)) {
					continue;
				}
				double hittime;
				if (self.CollidesWith(rib, out hittime)) {
					times.Add(rib, hittime);
					rib.Collision.times.Add(self, hittime);
				}
			}
			if (times.Count > 0) {
				U.show(string.Format("rib has {0} collisions", times.Count));
			}
		}
	}
}

