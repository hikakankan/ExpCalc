using System;
using System.Windows.Controls;

namespace ExpCalc
{
	/// <summary>
	/// ExpEmpty ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class ExpEmpty : Expression
	{
		public override bool isSelected(MVPoint p)
		{
			return false;
		}

		public override bool nodeIsSelected(MVPoint p)
		{
			return false;
		}

		public override String selectedPos(MVPoint p, String pos)
		{
			return "";
		}

		public override bool IsNothing()
		{
			return true;
		}

		public override bool is_const()
		{
			return false;
		}

		public override int count_op1()
		{
			return 0;
		}

		public override int count_op2()
		{
			return 0;
		}

		public override int count_all()
		{
			return 1;
		}

		public override bool equal(Expression e)
		{
			return false;
		}

		public override String mprint()
		{
			return "";
		}

		public override void draw(MVGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
		{
		}

		public override MVDimension drawsize(MVGraphics g, bool p, int r, bool english)
		{
			return new MVDimension();
		}

        public override void draw(XGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
        }

        public override MVDimension drawsize(XGraphics g, bool p, int r, bool english)
        {
            return new MVDimension();
        }

        public override String getmtype()
		{
			return "";
		}

		public override String getname()
		{
			return "";
		}

		public override String getvartype()
		{
			return "";
		}

		public override Expression getexp1()
		{
			return new ExpEmpty();
		}

		public override Expression getexp2()
		{
			return new ExpEmpty();
		}

		public override Expression reduce(String selpos, TextBox status)
		{
			return this;
		}

		public override double getnumber()
		{
			return -1;
		}

		public override double getnegnumber()
		{
			return -1;
		}
	}
}
