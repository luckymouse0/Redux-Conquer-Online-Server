using System;

namespace Redux.Space
{
    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int Distance(Point to)
        {
            return Math.Max(Math.Abs(X - to.X), Math.Abs(Y - to.Y));
        }
    }
}
