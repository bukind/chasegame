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

		public void Run()
		{
			foreach (Rib rib in ribs) {
				rib.Collision.Fill(ribs);
			}


			// finally, cleanup ribs
			ribs.RemoveAll(matchRibNull);
			ribs.Sort(compareRibsByEndTime);

		}

		private static bool matchRibNull(Rib rib) {
			return rib == null || rib.Sprite == null;
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

