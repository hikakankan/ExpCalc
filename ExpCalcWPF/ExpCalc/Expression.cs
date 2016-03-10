using System;
using System.Windows.Controls;

namespace ExpCalc
{
	/// <summary>
	/// Expression ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public abstract class Expression
	{
		//	abstract public bool isSelected(Point p);
		abstract public bool isSelected(MVPoint p);
		//	abstract public bool nodeIsSelected(Point p);
		abstract public bool nodeIsSelected(MVPoint p);
		//	abstract public String selectedPos(Point p, String pos);
		abstract public String selectedPos(MVPoint p, String pos);
		abstract public bool IsNothing();
		abstract public bool is_const();
		abstract public int count_op1();
		abstract public int count_op2();
		abstract public int count_all();
		abstract public bool equal(Expression e);
		abstract public String mprint();
		abstract public String getmtype();
		abstract public String getname();
		abstract public String getvartype();
		abstract public Expression getexp1();
		abstract public Expression getexp2();
		abstract public void draw(MVGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english);
		abstract public MVDimension drawsize(MVGraphics g, bool p, int r, bool english);
        abstract public void draw(XGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english);
        abstract public MVDimension drawsize(XGraphics g, bool p, int r, bool english);
        abstract public Expression reduce(String selpos, TextBox status);
		abstract public double getnumber();
		abstract public double getnegnumber();
	}
}
