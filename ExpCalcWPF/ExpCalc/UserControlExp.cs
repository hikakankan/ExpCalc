using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace ExpCalc
{
    public class UserControlExp : Canvas
    {
        public UserControlExp()
        {
            init();
        }

        //public FormExpCalc.FormExpCalcData Data { get; set; }

        private String source_str;
        private MVPoint mouse_point;
        private String selpos;
        private Expression exp;
        private bool english;
        private TextBox result_text;

        private ViewSettings settings;

        /// <summary>
        /// 設定オブジェクトをセット
        /// </summary>
        /// <param name="view"></param>
        public void set_settings(ViewSettings view)
        {
            settings = view;
        }

        private void init()
        {
            mouse_point = new MVPoint(0, 0);
            selpos = "";
            exp = null;
            english = false;
            //result_text = null;
            source_str = "";
        }

        private void create_exp()
        {
            if ( exp == null )
            {
                if ( !source_str.Equals("") )
                {
                    ExpInpStr inpstr = new ExpInpStr(source_str);
                    exp = inpstr.get_sum();
                    if ( exp.IsNothing() )
                    {
                        exp = null;
                    }
                }
            }
        }

        public void setEnglish(bool e)
        {
            english = e;
        }

        /// <summary>
        /// 描画しないバージョン
        /// </summary>
        public void paint()
        {
            XGraphics g = new XGraphics(this, Font, settings.UseImage, settings.ImageSettings);
            int width = (int)Width;
            int height = (int)Height;
            MVColor cf = settings.CalcAreaFrameBackColor;         // 選択されていないときの式の背景色
            MVColor cs = settings.CalcAreaFrameSelectedBackColor; // 選択されたときの式の背景色
            MVColor cb = settings.CalcAreaFrameColor;             // 式の枠の色
            MVColor cc = settings.CalcAreaTextColor;              // 式の文字の色
            create_exp();
            set_result();
            if ( exp != null )
            {
                Children.Clear();
                exp.draw(g, new MVPoint(20, 20), false, g.getFontMetrics().getHeight() / 2, cf, cb, cc, mouse_point, cs, false, settings.AsciiMode);
            }
        }

        public void setResultTextBox(TextBox text)
        {
            result_text = text;
        }

        public void setSource(String s)
        {
            source_str = s;
            exp = null;
            paint();
        }

        private void set_result()
        {
            if ( result_text != null )
            {
                if ( exp == null )
                {
                    if ( source_str.Equals("") )
                    {
                        result_text.Text = "";
                    }
                    else
                    {
                        result_text.Text = "式に誤りがあります";
                    }
                }
                else
                {
                    // result_textに文字列が設定されているときは、式の変形でエラーになったか、
                    // 式の変形をしていないか、最初に表示したとき
                    // このときは表示しない
                    if ( result_text.Text == "" )
                    {
                        result_text.Text = exp.mprint();
                    }
                }
            }
        }

        public void mouseClicked(MouseEventArgs e)
        {
            int x = (int)e.GetPosition(this).X;
            int y = (int)e.GetPosition(this).Y;
            mouse_point.move(x, y);
            create_exp();
            if ( exp != null )
            {
                String pos = exp.selectedPos(mouse_point, "");
                result_text.Text = "";
                exp = exp.reduce(pos, result_text);
                selpos = pos;
                paint();
            }
        }

        public void mouseMoved(MouseEventArgs e)
        {
            int x = (int)e.GetPosition(this).X;
            int y = (int)e.GetPosition(this).Y;
            mouse_point.move(x, y);
            create_exp();
            if ( exp != null )
            {
                String pos = exp.selectedPos(mouse_point, "");
                if ( !pos.Equals(selpos) )
                {
                    selpos = pos;
                    paint();
                }
            }
        }

        public XFont Font { get; set; }
    }
}
