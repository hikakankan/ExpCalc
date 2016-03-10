using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Windows.Controls;

namespace ExpCalc
{
    /// <summary>
    /// FormExpCalc.xaml の相互作用ロジック
    /// </summary>
    public partial class FormExpCalc : Window
    {
        private Settings settings;

        /// <summary>
        /// ヘルプファイル
        /// </summary>
        private const string help_file = "ExpCalc.chm";
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        private const string settings_file_name = "ExpCalc.xci";

        private const int _fontHeight = 16;

        private FormExpCalcData data;

        public FormExpCalc()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            Typeface tf = new Typeface(new FontFamily("Times New Roman"), new FontStyle(), new FontWeight(), new FontStretch());
            settings = new Settings(settings_file_name, new XFont(tf, _fontHeight));
            load_settings();

            data = new FormExpCalcData(settings.get_view_settings());
            DataContext = data;
        }

        private void update_view()
        {
            data.UpdateWindowView();
            Font = settings.get_view_settings().MainFont;
            userControlExp1.Font = settings.get_view_settings().CalcAreaFont;
        }

        private void after_load_proc()
        {
            update_view();
            userControlExp1.set_settings(settings.get_view_settings());
            data.userControlExp1 = userControlExp1;
            data.textBoxOutput = textBoxOutput;
        }

        public class FormExpCalcData : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public FormExpCalcData(ViewSettings view_settings)
            {
                ViewSettings = view_settings;
            }

            public UserControlExp userControlExp1 { get; set; }

            public TextBox textBoxOutput { get; set; }

            public ViewSettings ViewSettings { get; private set; }

            private void callPropertyChanged(string name)
            {
                if ( PropertyChanged != null )
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }

            public void UpdateWindowView()
            {
                callPropertyChanged(nameof(ViewSettings));
            }

            private string _textBoxInput_Text = "";
            private string _textBoxOutput_Text = "";

            public string textBoxInput_Text
            {
                get
                {
                    return _textBoxInput_Text;
                }
                set
                {
                    _textBoxInput_Text = value;
                    callPropertyChanged(nameof(textBoxInput_Text));
                }
            }

            public string textBoxOutput_Text
            {
                get
                {
                    return _textBoxOutput_Text;
                }
                set
                {
                    _textBoxOutput_Text = value;
                    callPropertyChanged(nameof(textBoxOutput_Text));
                }
            }
        }

        private bool is_num(char c)
        {
            return c >= '0' && c <= '9' || c == '.';
        }

        private bool is_op(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }

        private bool last_is_point_num(String text)
        {
            for ( int i = text.Length - 1; i >= 0; i-- )
            {
                if ( !is_num(text[i]) )
                {
                    return false;
                }
                else if ( text[i] == '.' )
                {
                    return true;
                }
            }
            return false;
        }

        private void add_string(String str)
        {
            String text = data.textBoxInput_Text;
            if ( !str.Equals("") )
            {
                char top_char = str[0];
                if ( text.Equals("") )
                {
                    // 先頭に「-」以外の演算子や「)」は禁止
                    if ( top_char != '-' && is_op(top_char) || top_char == ')' )
                    {
                        // 何もしない
                    }
                    else
                    {
                        text = String.Concat(text, str);
                    }
                }
                else
                {
                    char last_char = text[text.Length - 1];
                    // 数の後に「(」は禁止
                    // 演算子の後に演算子や「)」は禁止
                    // 「(」の後に「-」以外の演算子や「)」は禁止
                    // 「)」の後に数は禁止
                    // 「.」を含む数の後に「.」は禁止
                    // 数の後に数以外はスペースを入れる
                    if ( is_num(last_char) && top_char == '(' )
                    {
                        // 何もしない
                    }
                    else if ( is_op(last_char) && (is_op(top_char) || top_char == ')') )
                    {
                        // 何もしない
                    }
                    else if ( last_char == '(' && (top_char != '-' && is_op(top_char) || top_char == ')') )
                    {
                        // 何もしない
                    }
                    else if ( last_char == ')' && is_num(top_char) )
                    {
                        // 何もしない
                    }
                    else if ( is_num(last_char) && is_num(top_char) )
                    {
                        if ( last_is_point_num(text) && top_char == '.' )
                        {
                            // 何もしない
                        }
                        else
                        {
                            text = String.Concat(text, str);
                        }
                    }
                    else
                    {
                        text = String.Concat(text, " ");
                        text = String.Concat(text, str);
                    }
                }
            }
            data.textBoxInput_Text = text;
        }

        private void buttonN0_Click(object sender, System.EventArgs e)
        {
            add_string("0");
        }

        private void buttonN1_Click(object sender, System.EventArgs e)
        {
            add_string("1");
        }

        private void buttonN2_Click(object sender, System.EventArgs e)
        {
            add_string("2");
        }

        private void buttonN3_Click(object sender, System.EventArgs e)
        {
            add_string("3");
        }

        private void buttonN4_Click(object sender, System.EventArgs e)
        {
            add_string("4");
        }

        private void buttonN5_Click(object sender, System.EventArgs e)
        {
            add_string("5");
        }

        private void buttonN6_Click(object sender, System.EventArgs e)
        {
            add_string("6");
        }

        private void buttonN7_Click(object sender, System.EventArgs e)
        {
            add_string("7");
        }

        private void buttonN8_Click(object sender, System.EventArgs e)
        {
            add_string("8");
        }

        private void buttonN9_Click(object sender, System.EventArgs e)
        {
            add_string("9");
        }

        private void buttonDot_Click(object sender, System.EventArgs e)
        {
            add_string(".");
        }

        private void buttonPlus_Click(object sender, System.EventArgs e)
        {
            add_string("+");
        }

        private void buttonMinus_Click(object sender, System.EventArgs e)
        {
            add_string("-");
        }

        private void buttonMult_Click(object sender, System.EventArgs e)
        {
            add_string("*");
        }

        private void buttonDiv_Click(object sender, System.EventArgs e)
        {
            add_string("/");
        }

        private void buttonParOpen_Click(object sender, System.EventArgs e)
        {
            add_string("(");
        }

        private void buttonParClose_Click(object sender, System.EventArgs e)
        {
            add_string(")");
        }

        private void buttonEqual_Click(object sender, System.EventArgs e)
        {
            userControlExp1.setResultTextBox(textBoxOutput);
            userControlExp1.setSource(data.textBoxInput_Text);
        }

        private void buttonBS_Click(object sender, System.EventArgs e)
        {
            String str = data.textBoxInput_Text;
            str = str.Trim();
            if ( str.Length > 0 )
            {
                str = str.Substring(0, str.Length - 1);
                str = str.Trim();
            }
            data.textBoxInput_Text = str;
        }

        private void buttonClear_Click(object sender, System.EventArgs e)
        {
            data.textBoxInput_Text = "";
        }

        /// <summary>
        /// ヘルプを表示
        /// </summary>
        private void help()
        {
            try
            {
                System.Diagnostics.Process.Start(@"http://www2.biglobe.ne.jp/~optimist/software/expcalc21/help/main.html");
                //System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, help_file));
            }
            catch ( Exception )
            {
            }
        }

        private void menuItemView_Click(object sender, System.EventArgs e)
        {
            FormViewSettings dlg = new FormViewSettings();
            dlg.ViewSettings = settings.get_view_settings();
            if ( dlg.ShowDialog() == true )
            {
                update_view();
            }
        }

        private void menuItemLoadSettings_Click(object sender, System.EventArgs e)
        {
            load_settings_file();
        }

        private void menuItemSaveSettings_Click(object sender, System.EventArgs e)
        {
            save_settings_file();
        }

        private void menuItemHelp_Click(object sender, System.EventArgs e)
        {
            help();
        }

        private void menuItemVersion_Click(object sender, System.EventArgs e)
        {
            FormAboutDialog dlg = new FormAboutDialog(settings.get_view_settings());
            if ( dlg.ShowDialog() == true )
            {
            }
        }

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 設定ファイルから設定を読み込む
        /// </summary>
        private void load_settings()
        {
            settings.load_settings();
        }

        /// <summary>
        /// ファイルから読み込んだ後の処理
        /// </summary>
        private void load_settings_post_proc()
        {
            Left = settings.Left;
            Top = settings.Top;
            Width = settings.Width;
            Height = settings.Height;
        }

        /// <summary>
        /// ファイルに書き込む前に行う処理
        /// </summary>
        private void save_settings_pre_proc()
        {
            settings.Left = (int)Left;
            settings.Top = (int)Top;
            settings.Width = (int)Width;
            settings.Height = (int)Height;
        }

        /// <summary>
        /// 設定ファイルに設定を保存
        /// </summary>
        private void save_settings()
        {
            save_settings_pre_proc();
            settings.save_settings();
        }

        /// <summary>
        /// ファイルダイアログでファイルを指定して設定ファイルから設定を読み込む
        /// </summary>
        private void load_settings_file()
        {
            save_settings_pre_proc(); // 設定を更新しておく
            if ( settings.load_settings_file() )
            {
                update_view();
                load_settings_post_proc();
            }
        }

        /// <summary>
        /// ファイルダイアログでファイルを指定して設定ファイルに設定を保存
        /// </summary>
        private void save_settings_file()
        {
            save_settings_pre_proc();
            settings.save_settings_file();
        }

        private void Main_Closing(object sender, EventArgs e)
        {
            save_settings();
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            load_settings_post_proc();
            after_load_proc();
        }

        private int fontHeight;

        public XFont Font
        {
            get
            {
                return new XFont(new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), fontHeight);
            }
            set
            {
                if ( value != null )
                {
                    FontFamily = value.Typeface.FontFamily;
                    FontStyle = value.Typeface.Style;
                    FontWeight = value.Typeface.Weight;
                    FontStretch = value.Typeface.Stretch;
                    fontHeight = value.Height;
                }
            }
        }

        private void userControlExp1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            userControlExp1.mouseClicked(e);
        }

        private void userControlExp1_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            userControlExp1.mouseMoved(e);
        }
    }
}
