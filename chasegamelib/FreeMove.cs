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
    }
}

