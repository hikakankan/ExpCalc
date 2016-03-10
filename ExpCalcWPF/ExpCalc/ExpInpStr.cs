using System;

namespace ExpCalc
{
	/// <summary>
	/// ExpInpStr の概要の説明です。
	/// </summary>
	public class ExpInpStr
	{
		private String str;

		public ExpInpStr(String s)
		{
			str = s;
		}

		public bool isspace(int c)
		{
			return c == ' ';
		}

		public bool isalnum(int c)
		{
			return c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '_';
		}

		public bool isdigit(int c)
		{
			return c >= '0' && c <= '9';
		}

		public String get_word()
		{
			String res;
			int i;
			i = 0;
			long ilen;
			ilen = str.Length;
			if ( i >= ilen ) 
			{
				str = "";
				return "";
			}
			char c = str[i];
			while ( isspace(c) ) 
			{
				i = i + 1;
				if ( i >= ilen ) 
				{
					str = "";
					return "";
				}
				c = str[i];
			}
			int start;
			start = i;
			if ( isdigit(c) ) 
			{
				while ( isdigit(c) ) 
				{
					i = i + 1;
					if ( i >= ilen ) 
					{
						res = str.Substring(start);
						str = "";
						return res;
					}
					c = str[i];
				}
				if ( c == '.' )
				{
					i = i + 1;
					c = str[i];
					while ( isdigit(c) ) 
					{
						i = i + 1;
						if ( i >= ilen ) 
						{
							res = str.Substring(start);
							str = "";
							return res;
						}
						c = str[i];
					}
				}
				res = str.Substring(start, i - start);
				str = str.Substring(i);
				return res;
			}
			else if ( isalnum(c) ) 
			{
				while ( isalnum(c) ) 
				{
					i = i + 1;
					if ( i >= ilen ) 
					{
						res = str.Substring(start);
						str = "";
						return res;
					}
					c = str[i];
				}
				res = str.Substring(start, i - start);
				str = str.Substring(i);
				return res;
			} 
			else 
			{
				i = i + 1;
				//c = str.substr(-1 + i + 1, 1);
				res = str.Substring(start, i - start);
				str = str.Substring(i);
				return res;
			}
		}

		public void unget_word(String s)
		{
			str = s + " " + str;
		}

		public Expression get_prim()
		{
			String op = get_word();
			if ( op == "(" ) 
			{
				Expression e = get_sum();
				op = get_word();
				if ( op != ")" ) 
				{
					// error;
				}
				return e;
			}
			if ( op.Length > 0 && Char.IsLetterOrDigit(op[0]) ) 
			{
				// ここはオーバーフローのチェックをしない
				return new ExpVar(op);
			} 
			else 
			{
				return new ExpEmpty();
			}
		}

		public Expression get_prod()
		{
			Expression e1 = get_prim();
			if ( e1.IsNothing() ) 
			{
				return e1;
			}
			String op = get_word();
			while ( op == "*" || op == "/" ) 
			{
				Expression e2 = get_prim();
				if ( e2.IsNothing() ) 
				{
					return e2;
				}
				e1 = new ExpProd(op, e1, e2);
				op = get_word();
			}
			unget_word(op);
			return e1;
		}

		public Expression get_inv()
		{
			String op;
			Expression e;
			op = get_word();
			if ( op == "-" ) 
			{
				e = get_prod();
				if ( e.IsNothing() ) 
				{
					return e;
				}
				return new ExpInv("--", e);
			}
			unget_word(op);
			return get_prod();
		}

		public Expression get_sum()
		{
			Expression e1 = get_inv();
			if ( e1.IsNothing() ) 
			{
				return e1;
			}
			String op = get_word();
			while ( op == "+" || op == "-" ) 
			{
				Expression e2 = get_inv();
				if ( e2.IsNothing() ) 
				{
					return e2;
				}
				e1 = new ExpSum(op, e1, e2);
				op = get_word();
			}
			unget_word(op);
			return e1;
		}
	}
}
