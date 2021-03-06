using System;
using System.Windows;

namespace ExpCalc
{
    /// <summary>
    /// MVPoint の概要の説明です。
    /// </summary>
    public class MVPoint
    {
        private Point point;

        public Point get_point()
        {
            return point;
        }

        public MVPoint(int x, int y)
        {
            point = new Point(x, y);
        }

        public void move(int x, int y)
        {
            point.X = x;
            point.Y = y;
        }

        public int x
        {
            get
            {
                return (int)point.X;
            }
        }

        public int y
        {
            get
            {
                return (int)point.Y;
            }
        }
    }
}
