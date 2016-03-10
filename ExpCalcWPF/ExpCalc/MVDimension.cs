using System;
using System.Windows;

namespace ExpCalc
{
	/// <summary>
	/// MVDimension ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MVDimension
	{
		private Size size;

		public MVDimension()
		{
			size = new Size(0, 0);
		}

		public MVDimension(int width, int height)
		{
			size = new Size(width, height);
		}

		public Size get_size()
		{
			return size;
		}

		public int width
		{
			get
			{
				return (int)size.Width;
			}
		}

		public int height
		{
			get
			{
				return (int)size.Height;
			}
		}
	}
}
