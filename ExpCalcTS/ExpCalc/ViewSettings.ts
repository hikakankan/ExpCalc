class ViewSettings
{
    public BodyBackColor: WYColor;
    public BodyTextColor: WYColor;
    public ButtonBackColor: WYColor;
    public ButtonTextColor: WYColor;
    public TextBackColor: WYColor;
    public TextTextColor: WYColor;
    public CalcAreaBackColor: WYColor;
    public CalcAreaFrameBackColor: WYColor;
    public CalcAreaFrameSelectedBackColor: WYColor;
    public CalcAreaFrameColor: WYColor;
    public CalcAreaTextColor: WYColor;
    public MainFont: GCDefaultFont;
    public CalcAreaFont: GCDefaultFont;
    public AsciiMode: boolean;
    public ImageSettings: ImageSettings;
    public UseImage: boolean;

    public constructor() {
        this.BodyBackColor = new WYColor(220, 240, 255);  // 背景色
        this.BodyTextColor = new WYColor(0, 0, 0);      // テキストの色
        this.ButtonBackColor = new WYColor(200, 220, 255);   // ボタンの色
        this.ButtonTextColor = new WYColor(0, 0, 0);    // ボタンのテキストの色
        this.TextBackColor = new WYColor(255, 240, 240);  // テキストボックスの色
        this.TextTextColor = new WYColor(0, 0, 0);      // テキストボックスのテキストの色

        this.CalcAreaBackColor = new WYColor(255, 240, 240);  // 式の領域の背景色
        this.CalcAreaFrameBackColor = new WYColor(220, 255, 255);   // 選択されていないときの式の背景色
        this.CalcAreaFrameSelectedBackColor = new WYColor(200, 250, 250);      // 選択されたときの式の背景色
        this.CalcAreaFrameColor = new WYColor(150, 200, 200); // 式の枠の色
        this.CalcAreaTextColor = new WYColor(0, 0, 0);           // 式の文字の色

        this.MainFont = new GCDefaultFont();
        this.CalcAreaFont = this.MainFont; // 計算領域のフォントは仮にメインウィンドウのフォントを設定しておく

        this.AsciiMode = false; // 掛け算の記号を「＊」、割り算の記号を「／」で表示するモード

        this.ImageSettings = new ImageSettings();			// イメージの設定
        this.UseImage = false;						// イメージを使うかどうか
    }
}

class ImageSettings
{
    public GetWidth(str: string): number {
        return 0; // 実装していません
    }
    public GetHeight(): number {
        return 0; // 実装していません
    }
    public DrawString(str: string, x: number, y: number, graph: CanvasRenderingContext2D): void {
        // 実装していません
    }
}
