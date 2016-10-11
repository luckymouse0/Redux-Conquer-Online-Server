using System;

namespace Redux.Space
{
    public struct Rectangle
    {
        public Point LowerBound, UpperBound;

        public Rectangle(int _centerX, int _centerY, int _width, int _height)
        {
            LowerBound = new Point(_centerX - _width / 2, _centerY - _height / 2);
            UpperBound = new Point(_centerX + _width / 2, _centerY + _height / 2);
        }
        public Rectangle(Point p, int w, int h)
        {
            LowerBound = p;
            UpperBound = new Point(p.X + w, p.Y + h);
        }
        public Rectangle(Point a, Point b)
        {
            LowerBound = a;
            UpperBound = b;
        }

        public bool AreaContains(Point p)
        {
            return (p.X >= LowerBound.X && p.X < UpperBound.X)
                && (p.Y >= LowerBound.Y && p.Y < UpperBound.Y);
        }
    }
}