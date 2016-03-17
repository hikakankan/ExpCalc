class ExpProd implements Expression
{
    private mtype: string;
    private dtype: string;
    private exp1: Expression;
    private exp2: Expression;
    private rect: WYRectangle;

    public constructor(op: string, x: Expression, y: Expression) {
        this.mtype = op;
        if (op == "*") {
            this.dtype = "×";
        }
        else if (op == "/") {
            this.dtype = "÷";
        }
        this.exp1 = x;
        this.exp2 = y;
        this.rect = null;
    }

    public setRect = function (p: WYPoint, d: WYDimension): void {
        this.rect = new WYRectangle(p, d);
    }

    public isSelected = function (p: WYPoint): boolean {
        return this.rect != null && p.x >= this.rect.x && p.x < this.rect.x + this.rect.width && p.y >= this.rect.y && p.y < this.rect.y + this.rect.height;
    }

    public nodeIsSelected = function (p: WYPoint): boolean {
        return this.isSelected(p) && !this.exp1.isSelected(p) && !this.exp2.isSelected(p);
    }

    public selectedPos = function (p: WYPoint, pos: string): string {
        if (this.nodeIsSelected(p)) {
            return pos + "*";
        }
        else if (this.exp1.isSelected(p)) {
            return this.exp1.selectedPos(p, pos + "1");
        }
        else if (this.exp2.isSelected(p)) {
            return this.exp2.selectedPos(p, pos + "2");
        }
        else {
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
            return this.exp1.count_op1() + this.exp2.count_op1() + 1;
        }
        else {
            return this.exp1.count_op1() + this.exp2.count_op1();
        }
    }

    public count_op2 = function (): number {
        return this.exp1.count_op2() + this.exp2.count_op2() + 1;
    }

    public count_all = function (): number {
        return this.exp1.count_all() + this.exp2.count_all() + 1;
    }

    public equal = function (e: Expression): boolean {
        if (e.getmtype() == "+") {
            if (this.exp1.equal(e.getexp1())) {
                return this.exp2.equal(e.getexp2());
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    public mprint = function (): string {
        var s1;
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--" || t == "*" || t == "/") {
            s1 = "(" + this.exp1.mprint() + ")";
        }
        else {
            s1 = this.exp1.mprint();
        }
        var s2;
        t = this.exp2.getmtype();
        if (t == "+" || t == "-" || t == "--" || t == "*" || t == "/") {
            s2 = "(" + this.exp2.mprint() + ")";
        }
        else {
            s2 = this.exp2.mprint();
        }
        return s1 + " " + this.mtype + " " + s2;
    }

    public draw = function (g: WYGraphics, pnt: WYPoint, p: boolean, r: number, cf: WYColor, cb: WYColor, cc: WYColor, ps: WYPoint, cs: WYColor, sel: boolean, english:boolean ): void {
        var opsize;
        if (english) {
            opsize = g.getFontMetrics().stringWidth(" " + this.mtype + " ");
        }
        else {
            opsize = g.getFontMetrics().stringWidth(" " + this.dtype + " ");
        }
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
            g.drawString(" )", pnt.x + size.width - r - psize2, pnt.y + ch);
        }
        var aw = pnt.x + r + psize1;
        var ah;
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--") {
            ah = pnt.y + (size.height - this.exp1.drawsize(g, true, r, english).height) / 2;
            this.exp1.draw(g, new WYPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
            g.setColor(cc);
            if (english) {
                g.drawString(" " + this.mtype + " ", pnt.x + r + psize1 + this.exp1.drawsize(g, true, r, english).width, pnt.y + ch);
            }
            else {
                g.drawString(" " + this.dtype + " ", pnt.x + r + psize1 + this.exp1.drawsize(g, true, r, english).width, pnt.y + ch);
            }
            aw = pnt.x + r + psize1 + this.exp1.drawsize(g, true, r, english).width + opsize;
        }
        else {
            ah = pnt.y + (size.height - this.exp1.drawsize(g, false, r, english).height) / 2;
            this.exp1.draw(g, new WYPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
            g.setColor(cc);
            if (english) {
                g.drawString(" " + this.mtype + " ", pnt.x + r + psize1 + this.exp1.drawsize(g, false, r, english).width, pnt.y + ch);
            }
            else {
                g.drawString(" " + this.dtype + " ", pnt.x + r + psize1 + this.exp1.drawsize(g, false, r, english).width, pnt.y + ch);
            }
            aw = pnt.x + r + psize1 + this.exp1.drawsize(g, false, r, english).width + opsize;
        }
        t = this.exp2.getmtype();
        if (t == "+" || t == "-" || t == "--" || t == "*" || t == "/") {
            ah = pnt.y + (size.height - this.exp2.drawsize(g, true, r, english).height) / 2;
            this.exp2.draw(g, new WYPoint(aw, ah), true, r, cf, cb, cc, ps, cs, sel, english);
        }
        else {
            ah = pnt.y + (size.height - this.exp2.drawsize(g, false, r, english).height) / 2;
            this.exp2.draw(g, new WYPoint(aw, ah), false, r, cf, cb, cc, ps, cs, sel, english);
        }
    }

    public drawsize = function (g: WYGraphics, p: boolean, r: number, english: boolean): WYDimension {
        var opsize;
        if (english) {
            opsize = g.getFontMetrics().stringWidth(" " + this.mtype + " ");
        }
        else {
            opsize = g.getFontMetrics().stringWidth(" " + this.dtype + " ");
        }
        var psize1 = 0;
        var psize2 = 0;
        if (p) {
            psize1 = g.getFontMetrics().stringWidth("( ");
            psize2 = g.getFontMetrics().stringWidth(" )");
        }
        var d1;
        var d2;
        var t = this.exp1.getmtype();
        if (t == "+" || t == "-" || t == "--") {
            d1 = this.exp1.drawsize(g, true, r, english);
        }
        else {
            d1 = this.exp1.drawsize(g, false, r, english);
        }
        t = this.exp2.getmtype();
        if (t == "+" || t == "-" || t == "--" || t == "*" || t == "/") {
            d2 = this.exp2.drawsize(g, true, r, english);
        }
        else {
            d2 = this.exp2.drawsize(g, false, r, english);
        }
        var h = Math.max(d1.height, d2.height);
        return new WYDimension(d1.width + d2.width + r * 2 + opsize + psize1 + psize2, h + r * 2);
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
        return this.exp2;
    }

    public reduce = function (selpos: string, status: WYCanvasTextBox): Expression {
        if (selpos == "*") {
            var n1 = this.exp1.getnumber();
            if (n1 == -1) {
                var m1 = this.exp1.getnegnumber();
                if (m1 == -1) {
                    return this;
                }
                n1 = -m1;
            }
            var n2 = this.exp2.getnumber();
            if (n2 == -1) {
                var m2 = this.exp2.getnegnumber();
                if (m2 == -1) {
                    return this;
                }
                n2 = -m2;
            }
            var n;
            if (this.mtype == "*") {
                n = n1 * n2;
            }
            else {
                if (n2 == 0) {
                    status.setText("0で割ることはできません");
                    return this;
                }
                n = n1 / n2;
            }
            if (n >= 0) {
                if (!no_overflow(n)) {
                    status.setText("オーバーフローしました");
                    return this;
                }
                return new ExpVar(n);
            }
            else {
                if (!no_overflow(-n)) {
                    status.setText("オーバーフローしました");
                    return this;
                }
                return new ExpInv("--", new ExpVar(-n));
            }
        }
        else if (selpos.length > 0 && selpos[0] == '1') {
            return new ExpProd(this.mtype, this.exp1.reduce(selpos.substr(1), status), this.exp2);
        }
        else if (selpos.length > 0 && selpos[0] == '2') {
            return new ExpProd(this.mtype, this.exp1, this.exp2.reduce(selpos.substr(1), status));
        }
        else {
            return this;
        }
    }

    public getnumber = function (): number {
        return -1;
    }

    public getnegnumber = function (): number {
        return -1;
    }
}
