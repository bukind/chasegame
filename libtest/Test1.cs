using NUnit.Framework;
using System;
using System.Collections.Generic;
using chasegame;

namespace chasegame
{
    [TestFixture()]
    public class Test1
    {
        private Random random;

        public Test1()
        {
            random = new Random();
        }


        private float GetF(double range)
        {
            return (float)(random.NextDouble() * range);
        }


        [Test()]
        public void TestSort()
        {
            var list = new List<Vec>();
            const int len = 10;
            list.Capacity = len;
            for (int i = 0; i < len; ++i) {
				var s = new Vec(GetF(100.0), GetF(50.0));
                list.Add(s);
            }

            Console.WriteLine("The list is [{0}]", string.Join(", ", list));
			list.Sort(this.compareVecByX);
            Console.WriteLine("The list is [{0}]", string.Join(", ", list));

            Assert.That(false);
        }

		private int compareVecByX(Vec a, Vec b) {
			return (a.X < b.X) ? -1 : ((a.X > b.X) ? 1 : 0);
		}

		[Test()]
		public void TestEmptySprite()
		{
			var s = new Sprite();
			Assert.That(s.image == null);
			s.image = null;
			Assert.That(s.Id == 1);
			s = new Sprite();
			Assert.That(!s.IsValid());
			Assert.That(s.Id == 2);
		}

		private class Temp
		{
			public Temp() {}
		}

		[Test()]
		public void TestSortNull()
		{
			var list = new List<Temp>();
			list.Add(new Temp());
			list.Add(new Temp());
			list.Add(new Temp());
			Console.WriteLine(string.Join(",", list));
			list[1] = null;
			Console.WriteLine("item is ", list[1]);
		}
    }
}

