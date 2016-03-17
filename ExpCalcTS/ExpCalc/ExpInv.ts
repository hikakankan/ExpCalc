class ExpInv implements Expression
{
    private mtype: string;
    private exp1: Expression;
    private rect: WYRectangle;

    public constructor(op, x) {
        this.mtype = op;
        this.exp1 = x;
        this.rect = null;
    }

    public setRect = function (p: WYPoint, d: WYDimension): void {
		this.rect = new WYRectangle(p, d);
	}

    public isSelected = function (p: WYPoint): boolean {
		return this.rect != null && p.x >= this.rect.x && p.x < this.rect.x + this.rect.width && p.y >= this.rect.y && p.y < this.rect.y + this.rect.height;
	}

    public nodeIsSelected = function (p: WYPoint): boolean {
		return this.isSelected(p) && !this.exp1.isSelected(p);
	}

    public selectedPos = function (p: WYPoint, pos: string): string {
		if ( this.nodeIsSelected(p) ) 
		{
			return pos + "*";
		} 
		else if ( this.exp1.isSelected(p) ) 
		{
			return this.exp1.selectedPos(p, pos + "1");
		} 
		else 
		{
			return "";
		}
	}

    public IsNothing = function (): boolean {
		return false;
	}

    public is_const = function (): boolean {
		return false;
	}

    public count_op1 = function (): number {
        if (this.exp1.getmtype() == "+") {
            return this.exp1.count_op1() + 1;
        }
        else {
            return this.exp1.count_op1();
        }
    }

    public count_op2 = function (): number {
		return this.exp1.count_op2() + 1;
	}

    public count_all = function (): number {
		return this.exp1.count_all() + 1;
	}

    public equal = function (e: Expression): boolean {
        if (e.getmtype() == "-") {
            return this.exp1.equal(e.getexp1());
        }
        else {
            return false;
        }
    }

    public mprint = function (): string {
        var s1;
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--") {
            s1 = "(" + this.exp1.mprint() + ")";
        }
        else {
            s1 = this.exp1.mprint();
        }
        return "- " + s1;
    }

    public draw = function (g: WYGraphics, pnt: WYPoint, p: boolean, r: number, cf: WYColor, cb: WYColor, cc: WYColor, ps: WYPoint, cs: WYColor, sel: boolean, english: boolean): void {
        var opsize = g.getFontMetrics().stringWidth("- ");
        var size = this.drawsize(g, p, r, english);
        this.setRect(pnt, size);
        if (sel || this.nodeIsSelected(ps)) {
            g.setColor(cs);
            sel = true;
        }
        else {
            g.setColor(cf);
        }
        g.fillRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2);
        g.setColor(cb);
        g.drawRoundRect(pnt.x, pnt.y, size.width, size.height, r * 2, r * 2);
        var ch = (size.height - g.getFontMetrics().getHeight()) / 2 + g.getFontMetrics().getAscent();
        var psize1 = 0;
        var psize2 = 0;
        if (p) {
            psize1 = g.getFontMetrics().stringWidth("( ");
            psize2 = g.getFontMetrics().stringWidth(" )");
            g.setColor(cc);
            g.drawString("( ", pnt.x + r, pnt.y + ch);
            g.drawString(" )", pnt.x - r + size.width - psize2, pnt.y + ch);
        }
        g.setColor(cc);
        g.drawString("- ", pnt.x + r + psize1, pnt.y + ch);
        var ah = pnt.y + (size.height - this.exp1.drawsize(g, true, r, english).height) / 2;
        var pnt2 = new WYPoint(pnt.x + r + opsize + psize1, ah);
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--") {
            this.exp1.draw(g, pnt2, true, r, cf, cb, cc, ps, cs, sel, english);
        }
        else {
            this.exp1.draw(g, pnt2, false, r, cf, cb, cc, ps, cs, sel, english);
        }
    }

    public drawsize = function (g: WYGraphics, p: boolean, r: number, english: boolean): WYDimension {
        var opsize = g.getFontMetrics().stringWidth("- ");
        var psize1 = 0;
        var psize2 = 0;
        if (p) {
            psize1 = g.getFontMetrics().stringWidth("( ");
            psize2 = g.getFontMetrics().stringWidth(" )");
        }
        var d;
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--") {
            d = this.exp1.drawsize(g, true, r, english);
        }
        else {
            d = this.exp1.drawsize(g, false, r, english);
        }
        return new WYDimension(d.width + r * 2 + opsize + psize1 + psize2, d.height + r * 2);
    }

    public getmtype = function (): string {
        return this.mtype;
    }

    public getname = function (): string {
        return "";
    }

    public getvartype = function (): string {
        return "";
    }

    public getexp1 = function (): Expression {
        return this.exp1;
    }

    public getexp2 = function (): Expression {
		return new ExpEmpty();
	}

    public reduce = function (selpos: string, status: WYCanvasTextBox): Expression {
        if (selpos == "*") {
            var n = this.exp1.getnumber();
            if (n == 0) {
                return this.exp1;
            }
            n = this.exp1.getnegnumber();
            if (n == -1) {
                return this;
            }
            if (!no_overflow(n)) {
                status.setText("オーバーフローしました");
                return this;
            }
            return new ExpVar(n);
        }
        else if (selpos.length > 0 && selpos[0] == '1') {
            return new ExpInv(this.mtype, this.exp1.reduce(selpos.substr(1), status));
        }
        else {
            return this;
        }
    }

    public getnumber = function (): number {
		return -1;
	}

    public getnegnumber = function (): number {
		return this.exp1.getnumber();
	}
}
