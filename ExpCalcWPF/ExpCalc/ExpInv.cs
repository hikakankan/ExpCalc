using System;
using System.Windows.Controls;

namespace ExpCalc
{
    /// <summary>
    /// ExpInv の概要の説明です。
    /// </summary>
    class ExpInv : Expression
    {
        private String mtype;
        private Expression exp1;
        private MVRectangle rect;

        public ExpInv(String op, Expression x)
        {
            mtype = op;
            exp1 = x;
            rect = null;
        }

        private void setRect(MVPoint p, MVDimension d)
        {
            rect = new MVRectangle(p, d);
        }

        public override bool isSelected(MVPoint p)
        {
            return rect != null && p.x >= rect.x && p.x < rect.x + rect.width && p.y >= rect.y && p.y < rect.y + rect.height;
        }

        public override bool nodeIsSelected(MVPoint p)
        {
            return isSelected(p) && !exp1.isSelected(p);
        }

        public override String selectedPos(MVPoint p, String pos)
        {
            if ( nodeIsSelected(p) )
            {
                return pos + "*";
            }
            else if ( exp1.isSelected(p) )
            {
                return exp1.selectedPos(p, pos + "1");
            }
            else
            {
                return "";
            }
        }

        public override bool IsNothing()
        {
            return false;
        }

        public override bool is_const()
        {
            return false;
        }

        public override int count_op1()
        {
            if ( exp1.getmtype() == "+" )
            {
                return exp1.count_op1() + 1;
            }
            else
            {
                return exp1.count_op1();
            }
        }

        public override int count_op2()
        {
            return exp1.count_op2() + 1;
        }

        public override int count_all()
        {
            return exp1.count_all() + 1;
        }

        public override bool equal(Expression e)
        {
            if ( e.getmtype() == "-" )
            {
                return exp1.equal(e.getexp1());
            }
            else
            {
                return false;
            }
        }

        public override String mprint()
        {
            String s1;
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                s1 = "(" + exp1.mprint() + ")";
            }
            else
            {
                s1 = exp1.mprint();
            }
            return "- " + s1;
        }

        public override void draw(MVGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            int opsize = g.getFontMetrics().stringWidth("- ");
            MVDimension size = drawsize(g, p, r, english);
            setRect(pnt, size);
            if ( sel || nodeIsSelected(ps) )
            {
                g.setColor(cs);
                sel = true;
            }
            else
            {
                g.setColor(cf);
            }
            g.fillRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2);
            g.setColor(cb);
            g.drawRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2);
            int ch = (size.height - g.getFontMetrics().getHeight()) / 2 + g.getFontMetrics().getAscent();
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
                g.setColor(cc);
                g.drawString("( ", pnt.x + r, pnt.y + ch);
                g.drawString(" )", pnt.x - r + size.width - psize2, pnt.y + ch);
            }
            g.setColor(cc);
            g.drawString("- ", pnt.x + r + psize1, pnt.y + ch);
            int ah = pnt.y + (size.height - exp1.drawsize(g, true, r, english).height) / 2;
            MVPoint pnt2 = new MVPoint(pnt.x + r + opsize + psize1, ah);
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                exp1.draw(g, pnt2, true, r, cf, cb, cc, ps, cs, sel, english);
            }
            else
            {
                exp1.draw(g, pnt2, false, r, cf, cb, cc, ps, cs, sel, english);
            }
        }

        public override MVDimension drawsize(MVGraphics g, bool p, int r, bool english)
        {
            int opsize = g.getFontMetrics().stringWidth("- ");
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
            }
            MVDimension d;
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                d = exp1.drawsize(g, true, r, english);
            }
            else
            {
                d = exp1.drawsize(g, false, r, english);
            }
            return new MVDimension(d.width + r * 2 + opsize + psize1 + psize2, d.height + r * 2);
        }

        public override void draw(XGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            int opsize = g.getFontMetrics().stringWidth("- ");
            MVDimension size = drawsize(g, p, r, english);
            setRect(pnt, size);
            MVColor backColor;
            if ( sel || nodeIsSelected(ps) )
            {
                //g.setColor(cs);
                sel = true;
                backColor = cs;
            }
            else
            {
                //g.setColor(cf);
                backColor = cf;
            }
            //g.fillRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2);
            //g.setColor(cb);
            g.drawRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2, cb, backColor);
            int ch = (size.height - g.getFontMetrics().getHeight()) / 2 + g.getFontMetrics().getAscent();
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
                g.setColor(cc);
                g.drawString("( ", pnt.x + r, pnt.y + ch);
                g.drawString(" )", pnt.x - r + size.width - psize2, pnt.y + ch);
            }
            g.setColor(cc);
            g.drawString("- ", pnt.x + r + psize1, pnt.y + ch);
            int ah = pnt.y + (size.height - exp1.drawsize(g, true, r, english).height) / 2;
            MVPoint pnt2 = new MVPoint(pnt.x + r + opsize + psize1, ah);
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                exp1.draw(g, pnt2, true, r, cf, cb, cc, ps, cs, sel, english);
            }
            else
            {
                exp1.draw(g, pnt2, false, r, cf, cb, cc, ps, cs, sel, english);
            }
        }

        public override MVDimension drawsize(XGraphics g, bool p, int r, bool english)
        {
            int opsize = g.getFontMetrics().stringWidth("- ");
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
            }
            MVDimension d;
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                d = exp1.drawsize(g, true, r, english);
            }
            else
            {
                d = exp1.drawsize(g, false, r, english);
            }
            return new MVDimension(d.width + r * 2 + opsize + psize1 + psize2, d.height + r * 2);
        }

        public override String getmtype()
        {
            return mtype;
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
            return exp1;
        }

        public override Expression getexp2()
        {
            return new ExpEmpty();
        }

        public override Expression reduce(String selpos, TextBox status)
        {
            try
            {
                if ( selpos.Equals("*") )
                {
                    double n = exp1.getnumber();
                    if ( n == 0 )
                    {
                        return exp1;
                    }
                    n = exp1.getnegnumber();
                    if ( n == -1 )
                    {
                        return this;
                    }
                    if ( !ExpVar.no_overflow(n) )
                    {
                        status.Text = "オーバーフローしました";
                        return this;
                    }
                    return new ExpVar(n);
                }
                else if ( selpos.Length > 0 && selpos[0] == '1' )
                {
                    return new ExpInv(mtype, exp1.reduce(selpos.Substring(1), status));
                }
                else
                {
                    return this;
                }
            }
            catch ( System.OverflowException )
            {
                status.Text = "オーバーフローしました";
                return this;
            }
            catch ( System.Exception e )
            {
                status.Text = e.Message;
                return this;
            }
        }

        public override double getnumber()
        {
            return -1;
        }

        public override double getnegnumber()
        {
            return exp1.getnumber();
        }
    }
}
