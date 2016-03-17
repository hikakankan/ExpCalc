var minval = 1.0e-10; // 小数点以下10桁で切る
var r_minval = 1.0e+10; // 小数点以下10桁で切る
var flen = 10; // 小数点以下10桁で切る

function no_overflow(n)
{
	var name = round(n);
	return new cut_zero(name).return_value;
}

function round_p(n)
{
	var sval = String(n);
	if ( sval.indexOf("E") == -1 && sval.indexOf("e") == -1 )
	{
		// 浮動小数点表示にならなかったとき
		if ( sval.indexOf(".") != -1 && sval.length - sval.indexOf(".") - 1 > flen )
		{
			// 小数点以下の桁数が10より多い(11桁以上)のとき
			// 四捨五入する
			sval = String(n + minval / 2);
		} 
	} 
	if ( sval.indexOf("E") == -1 && sval.indexOf("e") == -1 )
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
		var sval1 = String(Math.floor((n + minval / 2) * r_minval));
		if ( sval1.length < flen )
		{
			sval1 = "0000000000".substr(flen - sval1.length) + sval1;
		}
		return "0." + sval1;
	}
}

function round(n)
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

function cut_zero(name)
{
	if ( name.indexOf("E") == -1 && name.indexOf("e") == -1 )
	{
		// 浮動小数点表示になっていないとき
		var p = name.indexOf(".");
		if ( p >= 0 )
		{
			// 小数点があるとき小数点以下10桁で切る
			// 小数点の位置が p
			// nameの長さが p + 1 のとき、小数点以下の桁数は 0
			// 小数点以下の桁数 = nameの長さ - (p + 1)
			if ( name.length - (p + 1) > flen )
			{
				name = name.substr(0, (p + 1) + flen);
			}
		}
		if ( name.indexOf(".") >= 0 )
		{
			// 後ろの0をとる
			while ( name.length > 0 && name[name.length - 1] == '0' )
			{
				name = name.substr(0, name.length - 1);
			}
			// 後ろの.をとる
			if ( name.length > 0 && name[name.length - 1] == '.' )
			{
				name = name.substr(0, name.length - 1);
			}
		}
		this.cut_zero_result = name; // JavaScriptではこれが返せないので大域変数に代入しておく
		this.return_value = true;
	}
	else
	{
		// オーバーフローにする
		this.cut_zero_result = ""; // JavaScriptではこれが返せないので大域変数に代入しておく
		this.return_value = false;
	}
}

function get_cut_zero(name)
{
	return new cut_zero(name).cut_zero_result;
}

class ExpVar implements Expression
{
    private mtype: string;
    private vartype: string;
    private name: string;

    public constructor(s: string);
    public constructor(s: number);
    public constructor(s: any) {
        this.mtype = "var";
        this.vartype = "const";
        if (typeof s == "number") {
            this.name = get_cut_zero(round(String(s)));
        } else {
            this.name = get_cut_zero(s);
        }
    }

    public isSelected(p: WYPoint): boolean {
        return false;
    }

    public nodeIsSelected(p: WYPoint): boolean {
        return false;
    }

    public selectedPos(p: WYPoint, pos: string): string {
        return "";
    }

    public IsNothing(): boolean {
        return false;
    }

    public is_const(): boolean {
        return this.name == "O";
    }

    public count_op1(): number {
        return 0;
    }

    public count_op2(): number {
        return 0;
    }

    public count_all(): number {
        return 1;
    }

    public equal(e: Expression): boolean {
        if (e.getmtype() == "var") {
            return this.name == e.getname();
        }
        else {
            return false;
        }
    }

    public mprint(): string {
        return this.name;
    }

    public draw(g, pnt, p, r, cf, cb, cc, ps, cs, sel, english): void {
        g.setColor(cc);
        g.drawString(this.name, pnt.x, pnt.y + g.getFontMetrics().getAscent());
    }

    public drawsize(g, p, r, english): WYDimension {
        var w = g.getFontMetrics().stringWidth(this.name);
        var h = g.getFontMetrics().getHeight();
        return new WYDimension(w, h);
    }

    public getmtype(): string {
        return this.mtype;
    }

    public getname(): string {
        return this.name;
    }

    public getvartype(): string {
        return this.vartype;
    }

    public getexp1(): Expression {
        return new ExpEmpty();
    }

    public getexp2(): Expression {
        return new ExpEmpty();
    }

    public reduce(selpos, status): Expression {
        return this;
    }

    public getnumber(): number {
        var pcnt = 0;
        for (var i = 0; i < this.name.length; i++) {
            if (this.name[i] == '.') {
                pcnt++;
                if (pcnt > 1) {
                    return -1;
                }
            }
            else if (!isdigit(this.name[i])) {
                return -1;
            }
        }
        return Number(this.name);
    }

    public getnegnumber(): number {
        return -1;
    }
}
