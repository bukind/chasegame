using System;
using System.Collections.Generic;

namespace chasegame
{
	public class Collision
	{
		private Rib self;
		private RibRect rect;
		private Dictionary<Rib, double> times;

		public Rib SelfRib { get { return self; } }
		public bool IsEmpty { get { return times.Count == 0; } }

		public Collision(Rib rib)
		{
			self = rib;
			rect = new RibRect(rib);
			times = new Dictionary<Rib, double>();
		}

		// return true if collisions has at least one hit
		public bool Fill(List<Rib> ribs)
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
				U.show("rib #{0} has {1} times",self.Sprite.Id,times.Count);
			}
			return times.Count > 0;
		}

		public double GetTime(Rib rib) {
			double res;
			if (!times.TryGetValue(rib, out res)) {
				throw new KeyNotFoundException(string.Format("the rib {0} is not found",rib.Sprite.Id));
			}
			return res;
		}

		public Rib GetFirstHit(out double time)
		{
			Rib rib = null;
			time = 0.0;
			foreach (KeyValuePair<Rib,double> kv in times) {
				if (rib == null || kv.Value < time) {
					rib = kv.Key;
					time = kv.Value;
				}
			}
			return rib;
		}

		public void Remove(Rib rib)
		{
			if (times.ContainsKey(rib)) {
				rib.Collision.times.Remove(self);
				times.Remove(rib);
			}
		}

		public void RemoveAll()
		{
			foreach (Rib rib in times.Keys) {
				rib.Collision.times.Remove(self);
			}
			times.Clear();
		}
	}
}

