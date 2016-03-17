function isspace(c)
{
	return c == ' ';
}

function isalnum(c)
{
	return c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '_';
}

function isdigit(c)
{
	return c >= '0' && c <= '9';
}

class ExpInpStr
{
    private str: string;

    public constructor(s: string) {
        this.str = s;
    }

    public get_word(): string {
        var res;
        var i = 0;
        var ilen = this.str.length;
        if (i >= ilen) {
            this.str = "";
            return "";
        }
        var c = this.str[i];
        while (isspace(c)) {
            i = i + 1;
            if (i >= ilen) {
                this.str = "";
                return "";
            }
            c = this.str[i];
        }
        var start = i;
        if (isdigit(c)) {
            while (isdigit(c)) {
                i = i + 1;
                if (i >= ilen) {
                    res = this.str.substr(start);
                    this.str = "";
                    return res;
                }
                c = this.str[i];
            }
            if (c == '.') {
                i = i + 1;
                c = this.str[i];
                while (isdigit(c)) {
                    i = i + 1;
                    if (i >= ilen) {
                        res = this.str.substr(start);
                        this.str = "";
                        return res;
                    }
                    c = this.str[i];
                }
            }
            res = this.str.substr(start, i - start);
            this.str = this.str.substr(i);
            return res;
        }
        else if (isalnum(c)) {
            while (isalnum(c)) {
                i = i + 1;
                if (i >= ilen) {
                    res = this.str.substr(start);
                    this.str = "";
                    return res;
                }
                c = this.str[i];
            }
            res = this.str.substr(start, i - start);
            this.str = this.str.substr(i);
            return res;
        }
        else {
            i = i + 1;
            res = this.str.substr(start, i - start);
            this.str = this.str.substr(i);
            return res;
        }
    }

    public unget_word(s: string): void {
        this.str = s + " " + this.str;
    }

    public get_prim(): Expression {
        var op = this.get_word();
        if (op == "(") {
            var e = this.get_sum();
            op = this.get_word();
            if (op != ")") {
                // error;
            }
            return e;
        }
        if (op.length > 0 && isalnum(op[0])) {
            // ここはオーバーフローのチェックをしない
            return new ExpVar(op);
        }
        else {
            return new ExpEmpty();
        }
    }

    public get_prod(): Expression {
        var e1 = this.get_prim();
        if (e1.IsNothing()) {
            return e1;
        }
        var op = this.get_word();
        while (op == "*" || op == "/") {
            var e2 = this.get_prim();
            if (e2.IsNothing()) {
                return e2;
            }
            e1 = new ExpProd(op, e1, e2);
            op = this.get_word();
        }
        this.unget_word(op);
        return e1;
    }

    public get_inv(): Expression {
        var e;
        var op = this.get_word();
        if (op == "-") {
            e = this.get_prod();
            if (e.IsNothing()) {
                return e;
            }
            return new ExpInv("--", e);
        }
        this.unget_word(op);
        return this.get_prod();
    }

    public get_sum(): Expression {
        var e1 = this.get_inv();
        if (e1.IsNothing()) {
            return e1;
        }
        var op = this.get_word();
        while (op == "+" || op == "-") {
            var e2 = this.get_inv();
            if (e2.IsNothing()) {
                return e2;
            }
            e1 = new ExpSum(op, e1, e2);
            op = this.get_word();
        }
        this.unget_word(op);
        return e1;
    }
}

interface Expression {
    isSelected(p: WYPoint): boolean;
    nodeIsSelected(p: WYPoint): boolean;
    selectedPos(p: WYPoint, pos: string): string;
    IsNothing(): boolean;
    is_const(): boolean;
    count_op1(): number;
    count_op2(): number;
    count_all(): number;
    equal(e: Expression): boolean;
    mprint(): string;
    draw(g: WYGraphics, pnt: WYPoint, p: boolean, r: number, cf: WYColor, cb: WYColor, cc: WYColor, ps: WYPoint, cs: WYColor, sel: boolean, english: boolean): void;
    drawsize(g: WYGraphics, p: boolean, r: number, english: boolean): WYDimension;
    getmtype(): string;
    getname(): string;
    getvartype(): string;
    getexp1(): Expression;
    getexp2(): Expression;
    reduce(selpos: string, status: WYCanvasTextBox): Expression;
    getnumber(): number;
    getnegnumber(): number;
}

class ExpEmpty implements Expression
{
    public isSelected(p: WYPoint): boolean { return false }
    public nodeIsSelected(p: WYPoint): boolean { return false }
    public selectedPos(p: WYPoint, pos: string): string { return "" }
    public IsNothing(): boolean {
        return true;
    }
    public is_const(): boolean { return false; }
    public count_op1(): number { return 0 }
    public count_op2(): number { return 0 }
    public count_all(): number { return 0 }
    public equal(e: Expression): boolean { return false; }
    public mprint(): string {
        return "Empty";
    }
    public draw(g: WYGraphics, pnt: WYPoint, p: boolean, r: number, cf: WYColor, cb: WYColor, cc: WYColor, ps: WYPoint, cs: WYColor, sel: boolean, english: boolean): void { }
    public drawsize(g: WYGraphics, p: boolean, r: number, english: boolean): WYDimension { return null }
    public getmtype(): string { return "" }
    public getname(): string { return "" }
    public getvartype(): string { return "" }
    public getexp1(): Expression { return null }
    public getexp2(): Expression { return null }
    public reduce(selpos: string, status: WYCanvasTextBox): Expression { return null }
    public getnumber(): number { return 0 }
    public getnegnumber(): number { return 0 }
}
