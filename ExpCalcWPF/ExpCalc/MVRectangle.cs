using System;

namespace ExpCalc
{
    /// <summary>
    /// MVRectangle ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    class MVRectangle
    {
        private MVPoint point;
        private MVDimension size;

        public MVRectangle(MVPoint p, MVDimension d)
        {
            point = p;
            size = d;
        }

        public int x
        {
            get
            {
                return point.x;
            }
        }

        public int y
        {
            get
            {
                return point.y;
            }
        }

        public int width
        {
            get
            {
                return size.width;
            }
        }

        public int height
        {
            get
            {
                return size.height;
            }
        }
    }
}
