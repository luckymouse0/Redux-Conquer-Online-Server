using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;
namespace Redux.Space
{
    public static class Calculations
    {
        public static int GetDistance(int _x1, int _y1, int _x2, int _y2)
        {
            return Math.Max(Math.Abs(_x1 - _x2), Math.Abs(_y1 -_y2));
        }
        public static int GetDistance(Point _from, Point _to)
        {
            return Math.Max(Math.Abs(_from.X- _to.X), Math.Abs(_from.Y - _to.Y));
        }
        public static int GetDistance(Entity _from, Entity _to)
        {
            return Math.Max(Math.Abs(_from.Location.X - _to.Location.X), Math.Abs(_from.Location.Y - _to.Location.Y));
        }
        public static int GetLineLength(Point _from, Point _to)
        {
            var deltaX = Math.Abs(_to.X - _from.X);
            var deltaY = Math.Abs(_to.Y - _from.Y);
            return (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
        public static bool IsInRange(Entity _p1, Entity _p2, int _range)
        {
            return GetDistance(_p1.Location.X, _p1.Location.Y, _p2.Location.X, _p2.Location.Y) <= _range;
        }
        public static bool InScreen(ushort x1, ushort y1, ushort x2, ushort y2)
        {
            return Math.Abs(x1 - x2) <= 18 && Math.Abs(y1 - y2) <= 18; 
        }
        public static byte GetDirection(Point _from, Point _to)
        {
            int dir = 0;
            int[] tan = new int[]{-241, -41, 41, 241};
            int deltaX = _to.X - _from.X;
            int deltaY = _to.Y - _from.Y;

            if (deltaX == 0)
                if (deltaY > 0)
                    dir = 0;
                else
                    dir = 4;
            else if (deltaY == 0)
                if (deltaX > 0)
                    dir = 6;
                else
                    dir = 2;
            else
            {
                int flag = Math.Abs(deltaX) / deltaX;
                int tempY = deltaY * 100 * flag;
                int i;
                for (i = 0; i < 4; i++)
                    tan[i] *= Math.Abs(deltaX);
                for (i = 0; i < 3; i++)
                    if (tempY >= tan[i] && tempY < tan[i + 1])
                        break;
                if (deltaX > 0)
                {
                    if (i == 0) dir = 5;
                    else if (i == 1) dir = 6;
                    else if (i == 2) dir = 7;
                    else if (i == 3)
                        if (deltaY > 0)
                            dir = 0;
                        else
                            dir = 4;
                }
                else
                {
                    if (i == 0) dir = 1;
                    else if (i == 1) dir = 2;
                    else if (i == 2) dir = 3;
                    else if (i == 3)
                        if (deltaY > 0)
                            dir = 0;
                        else
                            dir = 4;
                }
                    
            }
            dir = (dir + 8) % 8;
            return (byte)dir;

        }
    }
}
