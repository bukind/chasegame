using System;

namespace chasegame
{
    public enum WrapType
    {
        NoWrap = 0, WrapXLow, WrapXHigh, WrapYLow, WrapYHigh
    };

    public struct FreeMove
    {
        public float x0;
        public float y0;
        public float x;
        public float y;
        public WrapType wrap;
        public Sprite sprite;

        public override string ToString()
        {
            return string.Format("{0}/{1}",x0,x);
        }

        public static int SortByX(FreeMove a, FreeMove b)
        {
            return (a.x < b.x) ? -1 : ((a.x > b.x) ? 1 : 0);
        }

    }
}

