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
            var list = new List<FreeMove>();
            const int len = 10;
            list.Capacity = len;
            for (int i = 0; i < len; ++i) {
                var s = new FreeMove();
                s.x = GetF(100.0);
                s.y = GetF(50.0);
                s.x0 = GetF(100.0);
                s.y0 = GetF(50.0);
                list.Add(s);
            }

            Console.WriteLine("The list is [{0}]", string.Join(", ", list));
            list.Sort();
            Console.WriteLine("The list is [{0}]", string.Join(", ", list));

            Assert.That(false);
        }
        /*
        public static void Main(string[] args)
        {
            var t = new Test1();
            t.TestSort();
        }
        */
    }
}

