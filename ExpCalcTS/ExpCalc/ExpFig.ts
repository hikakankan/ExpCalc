class ExpFig implements TouchEventSource {
    private canvas: HTMLCanvasElement;
    private org_width: number;
    private org_height: number;
    private gc: CanvasRenderingContext2D;
    private mouse_point: WYPoint;
    private selpos: string;
    private exp: Expression;
    private english: boolean;
    private result_text: WYCanvasTextBox;
    private status_text: WYCanvasTextBox;
    private source_str: string;
    private settings: ViewSettings;
    private can_size_up: boolean;
    private can_size_down: boolean;
    private margin: WYPoint;

    public constructor(canvas: HTMLCanvasElement, result_text: WYCanvasTextBox, status_text: WYCanvasTextBox, settings: ViewSettings) {
        this.canvas = canvas;
        this.org_width = canvas.width;
        this.org_height = canvas.height;
        this.gc = canvas.getContext("2d");
        this.mouse_point = new WYPoint(0, 0);
        this.selpos = "";
        this.exp = null;
        this.english = false;
        this.result_text = result_text;
        this.status_text = status_text;
        this.source_str = "";
        this.settings = settings;
        this.can_size_up = true;
        this.can_size_down = true;
        this.margin = new WYPoint(20, 20);
    }

    public create_exp(): void {
        if (this.exp == null) {
            if (this.source_str != "") {
                var inpstr = new ExpInpStr(this.source_str);
                this.exp = inpstr.get_sum();
                if (this.exp.IsNothing()) {
                    this.exp = null;
                }
            }
        }
    }

    public setEnglish(e: boolean): void {
        this.english = e;
    }

    public change_size(g: WYGraphics): void {
        var size = this.exp.drawsize(g, false, g.getFontMetrics().getHeight() / 2, this.settings.AsciiMode);
        var e_width = size.width + this.margin.x * 2;
        var e_height = size.height + this.margin.y * 2;
        var new_width = this.canvas.width;
        var new_height = this.canvas.height;
        if (this.can_size_up && e_width > this.canvas.width) {
            new_width = e_width;
        } else if (this.can_size_down && e_width < this.canvas.width) {
            new_width = Math.max(e_width, this.org_width);
        }
        if (this.can_size_up && e_height > this.canvas.height) {
            new_height = e_height;
        } else if (this.can_size_down && e_height < this.canvas.height) {
            new_height = Math.max(e_height, this.org_height);
        }
        if (new_width != this.canvas.width || new_height != this.canvas.height) {
            this.canvas.width = new_width;
            this.canvas.height = new_height;
            g.fillRect(0, 0, this.canvas.width, this.canvas.height);
        }
    }

    public paint(g_org: CanvasRenderingContext2D): void {
        var g = new WYGraphics(g_org, this.settings.CalcAreaFont, this.settings.UseImage, this.settings.ImageSettings);
        g.setColor(this.settings.CalcAreaBackColor);                            // 式の領域の背景色
        g.fillRect(0, 0, this.canvas.width, this.canvas.height);
        var cf = this.settings.CalcAreaFrameBackColor;         // 選択されていないときの式の背景色
        var cs = this.settings.CalcAreaFrameSelectedBackColor; // 選択されたときの式の背景色
        var cb = this.settings.CalcAreaFrameColor;             // 式の枠の色
        var cc = this.settings.CalcAreaTextColor;              // 式の文字の色
        this.create_exp();
        this.set_result();
        if (this.exp != null) {
            this.change_size(g);
            this.exp.draw(g, this.margin, false, g.getFontMetrics().getHeight() / 2, cf, cb, cc, this.mouse_point, cs, false, this.settings.AsciiMode);
        }
    }

    public setSource(s: string): void {
        this.source_str = s;
        this.exp = null;
        this.Refresh();
    }

    public set_result(): void {
        if (this.result_text != null) {
            if (this.exp == null) {
                if (this.source_str == "") {
                    this.result_text.setText("");
                }
                else {
                    this.result_text.setText("式に誤りがあります");
                }
            }
            else {
                // result_textに文字列が設定されているときは、式の変形でエラーになったか、
                // 式の変形をしていないか、最初に表示したとき
                // このときは表示しない
                if (this.result_text.getText() == "") {
                    this.result_text.setText(this.exp.mprint());
                }
            }
        }
    }

    public mousePressed(x: number, y: number): void {
        this.mouse_point.move(x, y);
        this.create_exp();
        if (this.exp != null) {
            var pos = this.exp.selectedPos(this.mouse_point, "");
            this.result_text.setText("");
            this.exp = this.exp.reduce(pos, this.status_text);
            this.selpos = pos;
            this.Refresh();
        }
    }

    public mouseMoved(x: number, y: number): void {
        this.mouse_point.move(x, y);
        this.create_exp();
        if (this.exp != null) {
            var pos = this.exp.selectedPos(this.mouse_point, "");
            if (pos != this.selpos) {
                this.selpos = pos;
                this.Refresh();
            }
        }
    }

    public mouseReleased(x: number, y: number): void {
    }

    //public touchStart(x: number, y: number, id: number): void {
    public touchStart(x: number, y: number): void {
        this.mousePressed(x, y);
    }

    public touchEnd(ids: number[]): void {
    }

    public touchMove(x: number, y: number): void {
    }

    public Refresh() {
        this.paint(this.gc);
    }
}
