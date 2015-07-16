using System;
using System.Collections.Generic;

namespace chasegame
{
	public class Collider
	{
		private List<Rib> ribs; // all final ribs

		public Collider(List<Rib> theribs)
		{
			this.ribs = theribs;
		}

		public void Run(double dt)
		{
			List<Collision> collisions = new List<Collision>();
			foreach (Rib rib in ribs) {
				if (rib.Collision.Fill(ribs)) {
					collisions.Add(rib.Collision);
				}
			}

			while (collisions.Count > 0) {
				U.show(string.Format("Total {0} collisions", collisions.Count));

				// run through all collisions
				Collision first = null;
				double firsttime = 0.0;
				Rib firstrib = null;
				foreach (var coll in collisions) {
					if (coll == null || coll.IsEmpty || coll.SelfRib == null) {
						continue;
					}
					double time;
					Rib rib = coll.GetFirstHit(out time);
					if (rib == null) {
						throw new Exception("the collection must have a hit");
					}
					if (first == null || time < firsttime) {
						first = coll;
						firsttime = time;
						firstrib = rib;
					}
				}
				if (first == null) {
					break;
				}
				U.show(string.Format("first collision time found: {0}", firsttime));
				List<Rib> moreribs = applyCollision(first, firstrib, dt);
				if (moreribs != null) {
					// we have more ribs
					foreach (Rib rib in moreribs) {
						if (rib.Collision.Fill(ribs)) {
							collisions.Add(rib.Collision);
						}
					}
					ribs.AddRange(moreribs);
				}
			}

			// finally, cleanup ribs
			ribs.RemoveAll(rib => rib == null || rib.Sprite == null);
			ribs.Sort(compareRibsByEndTime);
		}

		private static bool matchRibNull(Rib rib) {
			return rib == null || rib.Sprite == null;
		}

		private List<Rib> applyCollision(Collision first, Rib rib, double dt)
		{
			double time = first.GetTime(rib);
			Collision second = rib.Collision;
			Rib self = first.SelfRib;
			// remove ribs from both collisions
			first.Remove(rib);
			if (self.Sprite.image == rib.Sprite.image) {
				// the collision occurs b/w the same type of objects
				// remove all ribs corresponding to this sprite
				ribs.RemoveAll(r => r.Sprite == rib.Sprite);
				ribs.RemoveAll(r => r.Sprite == self.Sprite);
				// scan through all other ribs in the collections and remove
				first.RemoveAll();
				second.RemoveAll();
				rib.SetEndTime(time);
				self.SetEndTime(time);
				// move sprites
				rib.Sprite.Update(rib);
				self.Sprite.Update(self);
				// create a new rib
				Sprite sprite = new Sprite(self.Sprite.image, self.Sprite, rib.Sprite);
				List<Rib> moreribs = new List<Rib>();
				sprite.MakeMove(time, dt, moreribs);
				return moreribs;
			} else {
				return null;
			}
		}

		/*
		private static int compareRibRectsByX(RibRect a, RibRect b) {
			return a.min.X.CompareTo(b.min.X);
		}
		*/

		private static int compareRibsByEndTime(Rib a, Rib b)
		{
			return a.EndTime.CompareTo(b.EndTime);
		}
	}
}

