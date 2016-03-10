using System;
using System.Windows.Controls;

namespace ExpCalc
{
    /// <summary>
    /// ExpProd の概要の説明です。
    /// </summary>
    class ExpProd : Expression
    {
        private String mtype;
        private String dtype;
        private Expression exp1;
        private Expression exp2;
        private MVRectangle rect;

        public ExpProd(String op, Expression x, Expression y)
        {
            mtype = op;
            if ( op.Equals("*") )
            {
                dtype = "×";
            }
            else if ( op.Equals("/") )
            {
                dtype = "÷";
            }
            exp1 = x;
            exp2 = y;
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
            return isSelected(p) && !exp1.isSelected(p) && !exp2.isSelected(p);
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
            else if ( exp2.isSelected(p) )
            {
                return exp2.selectedPos(p, pos + "2");
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
            if ( exp1.getmtype().Equals("+") )
            {
                return exp1.count_op1() + exp2.count_op1() + 1;
            }
            else
            {
                return exp1.count_op1() + exp2.count_op1();
            }
        }

        public override int count_op2()
        {
            return exp1.count_op2() + exp2.count_op2() + 1;
        }

        public override int count_all()
        {
            return exp1.count_all() + exp2.count_all() + 1;
        }

        public override bool equal(Expression e)
        {
            if ( e.getmtype().Equals("+") )
            {
                if ( exp1.equal(e.getexp1()) )
                {
                    return exp2.equal(e.getexp2());
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override String mprint()
        {
            String t;
            String s1;
            t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                s1 = "(" + exp1.mprint() + ")";
            }
            else
            {
                s1 = exp1.mprint();
            }
            String s2;
            t = exp2.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                s2 = "(" + exp2.mprint() + ")";
            }
            else
            {
                s2 = exp2.mprint();
            }
            return s1 + " " + mtype + " " + s2;
        }

        public override void draw(MVGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            int opsize;
            if ( english )
            {
                opsize = g.getFontMetrics().stringWidth(" " + mtype + " ");
            }
            else
            {
                opsize = g.getFontMetrics().stringWidth(" " + dtype + " ");
            }
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
                g.drawString(" )", pnt.x + size.width - r - psize2, pnt.y + ch);
            }
            int aw = pnt.x + r + psize1;
            int ah;
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                ah = pnt.y + (size.height - exp1.drawsize(g, true, r, english).height) / 2;
                exp1.draw(g, new MVPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
                g.setColor(cc);
                if ( english )
                {
                    g.drawString(" " + mtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width, pnt.y + ch);
                }
                else
                {
                    g.drawString(" " + dtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width, pnt.y + ch);
                }
                aw = pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width + opsize;
            }
            else
            {
                ah = pnt.y + (size.height - exp1.drawsize(g, false, r, english).height) / 2;
                exp1.draw(g, new MVPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
                g.setColor(cc);
                if ( english )
                {
                    g.drawString(" " + mtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width, pnt.y + ch);
                }
                else
                {
                    g.drawString(" " + dtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width, pnt.y + ch);
                }
                aw = pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width + opsize;
            }
            t = exp2.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                ah = pnt.y + (size.height - exp2.drawsize(g, true, r, english).height) / 2;
                exp2.draw(g, new MVPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
            }
            else
            {
                ah = pnt.y + (size.height - exp2.drawsize(g, false, r, english).height) / 2;
                exp2.draw(g, new MVPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
            }
        }

        public override MVDimension drawsize(MVGraphics g, bool p, int r, bool english)
        {
            int opsize;
            if ( english )
            {
                opsize = g.getFontMetrics().stringWidth(" " + mtype + " ");
            }
            else
            {
                opsize = g.getFontMetrics().stringWidth(" " + dtype + " ");
            }
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
            }
            MVDimension d1;
            MVDimension d2;
            String t;
            t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                d1 = exp1.drawsize(g, true, r, english);
            }
            else
            {
                d1 = exp1.drawsize(g, false, r, english);
            }
            t = exp2.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                d2 = exp2.drawsize(g, true, r, english);
            }
            else
            {
                d2 = exp2.drawsize(g, false, r, english);
            }
            int h = Math.Max(d1.height, d2.height);
            return new MVDimension(d1.width + d2.width + r * 2 + opsize + psize1 + psize2, h + r * 2);
        }

        public override void draw(XGraphics g, MVPoint pnt, bool p, int r, MVColor cf, MVColor cb, MVColor cc, MVPoint ps, MVColor cs, bool sel, bool english)
        {
            int opsize;
            if ( english )
            {
                opsize = g.getFontMetrics().stringWidth(" " + mtype + " ");
            }
            else
            {
                opsize = g.getFontMetrics().stringWidth(" " + dtype + " ");
            }
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
                g.drawString(" )", pnt.x + size.width - r - psize2, pnt.y + ch);
            }
            int aw = pnt.x + r + psize1;
            int ah;
            String t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                ah = pnt.y + (size.height - exp1.drawsize(g, true, r, english).height) / 2;
                exp1.draw(g, new MVPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
                g.setColor(cc);
                if ( english )
                {
                    g.drawString(" " + mtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width, pnt.y + ch);
                }
                else
                {
                    g.drawString(" " + dtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width, pnt.y + ch);
                }
                aw = pnt.x + r + psize1 + exp1.drawsize(g, true, r, english).width + opsize;
            }
            else
            {
                ah = pnt.y + (size.height - exp1.drawsize(g, false, r, english).height) / 2;
                exp1.draw(g, new MVPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
                g.setColor(cc);
                if ( english )
                {
                    g.drawString(" " + mtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width, pnt.y + ch);
                }
                else
                {
                    g.drawString(" " + dtype + " ", pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width, pnt.y + ch);
                }
                aw = pnt.x + r + psize1 + exp1.drawsize(g, false, r, english).width + opsize;
            }
            t = exp2.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                ah = pnt.y + (size.height - exp2.drawsize(g, true, r, english).height) / 2;
                exp2.draw(g, new MVPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
            }
            else
            {
                ah = pnt.y + (size.height - exp2.drawsize(g, false, r, english).height) / 2;
                exp2.draw(g, new MVPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
            }
        }

        public override MVDimension drawsize(XGraphics g, bool p, int r, bool english)
        {
            int opsize;
            if ( english )
            {
                opsize = g.getFontMetrics().stringWidth(" " + mtype + " ");
            }
            else
            {
                opsize = g.getFontMetrics().stringWidth(" " + dtype + " ");
            }
            int psize1 = 0;
            int psize2 = 0;
            if ( p )
            {
                psize1 = g.getFontMetrics().stringWidth("( ");
                psize2 = g.getFontMetrics().stringWidth(" )");
            }
            MVDimension d1;
            MVDimension d2;
            String t;
            t = exp1.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") )
            {
                d1 = exp1.drawsize(g, true, r, english);
            }
            else
            {
                d1 = exp1.drawsize(g, false, r, english);
            }
            t = exp2.getmtype();
            if ( t.Equals("+") || t.Equals("-") || t.Equals("--") || t.Equals("*") || t.Equals("/") )
            {
                d2 = exp2.drawsize(g, true, r, english);
            }
            else
            {
                d2 = exp2.drawsize(g, false, r, english);
            }
            int h = Math.Max(d1.height, d2.height);
            return new MVDimension(d1.width + d2.width + r * 2 + opsize + psize1 + psize2, h + r * 2);
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
            return exp2;
        }

        public override Expression reduce(String selpos, TextBox status)
        {
            try
            {
                if ( selpos.Equals("*") )
                {
                    double n1 = exp1.getnumber();
                    if ( n1 == -1 )
                    {
                        double m1 = exp1.getnegnumber();
                        if ( m1 == -1 )
                        {
                            return this;
                        }
                        n1 = -m1;
                    }
                    double n2 = exp2.getnumber();
                    if ( n2 == -1 )
                    {
                        double m2 = exp2.getnegnumber();
                        if ( m2 == -1 )
                        {
                            return this;
                        }
                        n2 = -m2;
                    }
                    double n;
                    if ( mtype.Equals("*") )
                    {
                        n = n1 * n2;
                    }
                    else
                    {
                        //if ( n2 == 0 || n1 % n2 != 0 ) {
                        if ( n2 == 0 )
                        {
                            status.Text = "0で割ることはできません";
                            return this;
                        }
                        n = n1 / n2;
                    }
                    if ( n >= 0 )
                    {
                        if ( !ExpVar.no_overflow(n) )
                        {
                            status.Text = "オーバーフローしました";
                            return this;
                        }
                        return new ExpVar(n);
                    }
                    else
                    {
                        if ( !ExpVar.no_overflow(-n) )
                        {
                            status.Text = "オーバーフローしました";
                            return this;
                        }
                        return new ExpInv("--", new ExpVar(-n));
                    }
                }
                else if ( selpos.Length > 0 && selpos[0] == '1' )
                {
                    return new ExpProd(mtype, exp1.reduce(selpos.Substring(1), status), exp2);
                }
                else if ( selpos.Length > 0 && selpos[0] == '2' )
                {
                    return new ExpProd(mtype, exp1, exp2.reduce(selpos.Substring(1), status));
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
            return -1;
        }
    }
}
