using System;
using System.Windows.Controls;

namespace ExpCalc
{
    /// <summary>
    /// ExpVar の概要の説明です。
    /// </summary>
    class ExpVar : Expression
    {
        private String mtype;
        private String vartype;
        private String name;
        private const double minval = 1.0e-10; // 小数点以下10桁で切る
        private const double r_minval = 1.0e+10; // 小数点以下10桁で切る
        private const int flen = 10; // 小数点以下10桁で切る

        public ExpVar(String s)
        {
            mtype = "var";
            vartype = "const";
            name = s;
            cut_zero(ref name);
        }

        public ExpVar(double n)
        {
            mtype = "var";
            vartype = "const";
            name = round(n);
            cut_zero(ref name);
        }

        public static bool no_overflow(double n)
        {
            string name = round(n);
            return cut_zero(ref name);
        }

        private static string round_p(double n)
        {
            string sval = System.Convert.ToString(n);
            if ( sval.IndexOf("E") == -1 && sval.IndexOf("e") == -1 )
            {
                // 浮動小数点表示にならなかったとき
                if ( sval.IndexOf(".") != -1 && sval.Length - sval.IndexOf(".") - 1 > flen )
                {
                    // 小数点以下の桁数が10より多い(11桁以上)のとき
                    // 四捨五入する
                    sval = System.Convert.ToString(n + minval / 2);
                }
            }
            //System.Diagnostics.Debug.WriteLine("round_p " + sval);
            if ( sval.IndexOf("E") == -1 && sval.IndexOf("e") == -1 )
            {
                // 浮動小数点表示にならなかったとき
                return sval;
            }
            else if ( n >= 1 )
            {
                return sval;
            }
            else
            {
                string sval1 = System.Convert.ToString(Math.Floor((n + minval / 2) * r_minval));
                if ( sval1.Length < flen )
                {
                    sval1 = new string('0', flen - sval1.Length) + sval1;
                }
                return "0." + sval1;
            }
        }

        private static string round(double n)
        {
            if ( n >= minval )
            {
                return round_p(n);
            }
            else if ( n <= -minval )
            {
                return round_p(-n);
            }
            else
            {
                // 0にする
                return "0";
            }
        }

        private static bool cut_zero(ref string name)
        {
            if ( name.IndexOf("E") == -1 && name.IndexOf("e") == -1 )
            {
                // 浮動小数点表示になっていないとき
                int p = name.IndexOf(".");
                if ( p >= 0 )
                {
                    // 小数点があるとき小数点以下10桁で切る
                    // 小数点の位置が p
                    // nameの長さが p + 1 のとき、小数点以下の桁数は 0
                    // 小数点以下の桁数 = nameの長さ - (p + 1)
                    if ( name.Length - (p + 1) > flen )
                    {
                        name = name.Substring(0, (p + 1) + flen);
                        //System.Diagnostics.Debug.WriteLine("name=" + name);
                    }
                }
                if ( name.IndexOf(".") >= 0 )
                {
                    // 後ろの0をとる
                    while ( name.Length > 0 && name[name.Length - 1] == '0' )
                    {
                        name = name.Substring(0, name.Length - 1);
                    }
                    // 後ろの.をとる
                    if ( name.Length > 0 && name[name.Length - 1] == '.' )
                    {
                        name = name.Substring(0, name.Length - 1);
                    }
                }
                return true;
            }
            else
            {
                // オーバーフローにする
                return false;
            }
        }

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
            return false;
        }

        public override bool is_const()
        {
            return name.Equals("O");
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
            if ( e.getmtype().Equals("var") )
            {
                return name.Equals(e.getname());
            }
            else
            {
                return false;
            }
        }

        public override String mprint()
        {
            return name;
        }

        public override void draw(MVGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            g.setColor(cc);
            g.drawString(name, pnt.x, pnt.y + g.getFontMetrics().getAscent());
        }

        public override MVDimension drawsize(MVGraphics g, bool p, int r, bool english)
        {
            int w = g.getFontMetrics().stringWidth(name);
            int h = g.getFontMetrics().getHeight();
            return new MVDimension(w, h);
        }

        public override void draw(XGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            g.setColor(cc);
            g.drawString(name, pnt.x, pnt.y + g.getFontMetrics().getAscent());
        }

        public override MVDimension drawsize(XGraphics g, bool p, int r, bool english)
        {
            int w = g.getFontMetrics().stringWidth(name);
            int h = g.getFontMetrics().getHeight();
            return new MVDimension(w, h);
        }

        public override String getmtype()
        {
            return mtype;
        }

        public override String getname()
        {
            return name;
        }

        public override String getvartype()
        {
            return vartype;
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
            int pcnt = 0;
            for ( int i = 0; i < name.Length; i++ )
            {
                if ( name[i] == '.' )
                {
                    pcnt++;
                    if ( pcnt > 1 )
                    {
                        return -1;
                    }
                }
                else if ( !Char.IsDigit(name[i]) )
                {
                    return -1;
                }
            }
            return System.Convert.ToDouble(name);
        }

        public override double getnegnumber()
        {
            return -1;
        }
    }
}
