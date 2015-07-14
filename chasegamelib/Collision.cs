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
	}
}

