using System;
using System.Windows.Media;

namespace ExpCalc
{
	/// <summary>
	/// MVColor ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MVColor
	{
		private Color col;

        public MVColor(int r, int g, int b)
        {
            col = Color.FromRgb((byte)r, (byte)g, (byte)b);
        }

        public static MVColor FromArgb(int r, int g, int b)
        {
            return new MVColor(r, g, b);
        }

        public Color WColor
        {
            get
            {
                return col;
            }
        }

        public Brush XColor
        {
            get
            {
                return new SolidColorBrush(col);
            }
        }
    }
}
