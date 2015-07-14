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
		}
	}
}

