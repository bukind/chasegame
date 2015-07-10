using System;
using chasegame;

namespace test
{
	public class Test1
	{
		public Test1()
		{
		}

		public void Run()
		{
			Vec v = new Vec(1.0,1.0);
			Console.WriteLine("v={0}", v);
			v = v.Scale(5.0).Add(new Vec(3.0,-2.0));
			Console.WriteLine("v={0}", v);
		}
	}
}

