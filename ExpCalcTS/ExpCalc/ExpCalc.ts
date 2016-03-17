function is_num(c: string): boolean {
    return c >= '0' && c <= '9' || c == '.';
}

function is_op(c: string): boolean {
    return c == '+' || c == '-' || c == '*' || c == '/';
}

function last_is_point_num(text: string): boolean {
    for (var i = text.length - 1; i >= 0; i--) {
        if (!is_num(text[i])) {
            return false;
        }
        else if (text[i] == '.') {
            return true;
        }
    }
    return false;
}

//function isspace(c) {
//    return c == " " || c == "\t";
//}

function trim(str: string): string {
    var start = -1;
    for (var i = 0; i < str.length; i++) {
        start = i;
        var c = str.charAt(i);
        if (!isspace(c)) {
            break;
        }
    }
    if (start == -1) {
        return "";
    }
    var end = -1;
    for (var i = str.length - 1; i >= 0; i--) {
        end = i;
        var c = str.charAt(i);
        if (!isspace(c)) {
            break;
        }
    }
    return str.substring(start, end - start + 1);
}

function add_string(str: string): void {
    var text = inputText.getText();
    if (str != "") {
        var top_char = str[0];
        if (text == "") {
            // 先頭に「-」以外の演算子や「)」は禁止
            if (top_char != '-' && is_op(top_char) || top_char == ')') {
                // 何もしない
            }
            else {
                text = text + str;
            }
        }
        else {
            var last_char = text[text.length - 1];
            // 数の後に「(」は禁止
            // 演算子の後に演算子や「)」は禁止
            // 「(」の後に「-」以外の演算子や「)」は禁止
            // 「)」の後に数は禁止
            // 「.」を含む数の後に「.」は禁止
            // 数の後に数以外はスペースを入れる
            if (is_num(last_char) && top_char == '(') {
                // 何もしない
            }
            else if (is_op(last_char) && (is_op(top_char) || top_char == ')')) {
                // 何もしない
            }
            else if (last_char == '(' && (top_char != '-' && is_op(top_char) || top_char == ')')) {
                // 何もしない
            }
            else if (last_char == ')' && is_num(top_char)) {
                // 何もしない
            }
            else if (is_num(last_char) && is_num(top_char)) {
                if (last_is_point_num(text) && top_char == '.') {
                    // 何もしない
                }
                else {
                    text = text + str;
                }
            }
            else {
                text = text + " " + str;
            }
        }
    }
    inputText.setText(text);
}

var inputText: WYCanvasTextBox;
var outputText: WYCanvasTextBox;
var statusText: WYCanvasTextBox;
var exp_fig: ExpFig;

function make_button(id: string, settings: ViewSettings, left: number, top: number, width: number, height: number): WYRoundButton {
    //var canvas = document.getElementById(id);
    var canvas: HTMLCanvasElement = <HTMLCanvasElement>document.getElementById(id);
    var gc = canvas.getContext("2d");
    var button = new WYRoundButton(gc, settings, 12);
    button.setRect(left, top, width, height);
    return button;
}

class buttonBlock {
    public width: number;
    private height: number;

    public constructor(id: string, settings: ViewSettings, rows: number, cols: number, start_left: number, start_top: number, width: number, height: number, space_width: number, space_height: number, buttons: Buttons) {
        var max_left = 0;
        var top = start_top;
        for (var r = 0; r < rows; r++) {
            var left = start_left;
            for (var c = 0; c < cols; c++) {
                var button = make_button(id, settings, left, top, width, height);
                buttons.add(button);
                left += width + space_width;
                max_left = Math.max(max_left, left);
            }
            top += height + space_height;
        }
        this.width = max_left - start_left - space_width;
        this.height = top - start_top - space_height;
    }
}

class Buttons {
    private buttons: Array<WYRoundButton>;

    public constructor() {
        this.buttons = new Array<WYRoundButton>();
    }

    public add = function (button: WYRoundButton): void {
        this.buttons.push(button);
    }

    public setText = function (text_list: string[]): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].setText(text_list[i]);
        }
    }

    public setOnClick = function (text: string, func: () => void): void {
        for (var i = 0; i < this.buttons.length; i++) {
            if (this.buttons[i].text == text) {
                this.buttons[i].onclick = func;
            }
        }
    }

    public draw = function (): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].draw();
        }
    }

    public mousePressed = function (x: number, y: number): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].mousedown(x, y);
        }
    }

    public mouseReleased = function (x: number, y: number): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].mouseup(x, y);
        }
    }

    public mouseMoved = function (x: number, y: number): void {
    }

    public touchStart = function (x: number, y: number): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].touchstart(x, y);
        }
    }

    public touchEnd = function (ids): void {
        for (var i = 0; i < this.buttons.length; i++) {
            this.buttons[i].touchend();
        }
    }

    public touchMove = function (x: number, y: number): void {
    }
}

function make_buttons(id: string, settings: ViewSettings): Buttons {
    var w = 40;
    var h = 30;
    var ws = 6;
    var hs = 6;
    var x = 8;
    var y = 8;
    var wss = 12;
    var buttons = new Buttons();
    var block1 = new buttonBlock(id, settings, 4, 3, x, y, w, h, ws, hs, buttons);
    x += block1.width + wss;
    var block2 = new buttonBlock(id, settings, 3, 2, x, y, w, h, ws, hs, buttons);
    x += block2.width + wss;
    var block3 = new buttonBlock(id, settings, 2, 1, x, y, 45, h, ws, hs, buttons);
    buttons.setText(new Array("7", "8", "9", "4", "5", "6", "1", "2", "3", "0", ".", "=", "*", "/", "+", "-", "(", ")", "BS", "CLR"));
    return buttons;
}

function init_input(settings: ViewSettings): void {
    inputText = createTextBox("input-canvas", settings, "");
    outputText = createTextBox("output-canvas", settings, "");
    statusText = outputText;

    var buttons = make_buttons("buttons", settings);
    var buttons_canvas = <HTMLCanvasElement>document.getElementById("buttons");
    buttons.draw();
    buttons.setOnClick("0", function () { add_string("0"); });
    buttons.setOnClick("1", function () { add_string("1"); });
    buttons.setOnClick("2", function () { add_string("2"); });
    buttons.setOnClick("3", function () { add_string("3"); });
    buttons.setOnClick("4", function () { add_string("4"); });
    buttons.setOnClick("5", function () { add_string("5"); });
    buttons.setOnClick("6", function () { add_string("6"); });
    buttons.setOnClick("7", function () { add_string("7"); });
    buttons.setOnClick("8", function () { add_string("8"); });
    buttons.setOnClick("9", function () { add_string("9"); });
    buttons.setOnClick(".", function () { add_string("."); });
    buttons.setOnClick("+", function () { add_string("+"); });
    buttons.setOnClick("-", function () { add_string("-"); });
    buttons.setOnClick("*", function () { add_string("*"); });
    buttons.setOnClick("/", function () { add_string("/"); });
    buttons.setOnClick("(", function () { add_string("("); });
    buttons.setOnClick(")", function () { add_string(")"); });
    buttons.setOnClick("=", function () { buttonEqual_Click(); });
    buttons.setOnClick("BS", function () { buttonBS_Click(); });
    buttons.setOnClick("CLR", function () { buttonClear_Click(); });

    def_mouse_event(buttons_canvas, buttons);
}

//function init_input2(settings) {
//    inputText = new WYTextElement(document.getElementById("input"));
//    outputText = new WYTextElement(document.getElementById("output"));
//    statusText = new WYTextElement(document.getElementById("status"));
//}

function init_fig(settings: ViewSettings): void {
    var fig = <HTMLCanvasElement>document.getElementById("fig");
    exp_fig = new ExpFig(fig, outputText, statusText, settings);
    exp_fig.Refresh();

    def_mouse_event(fig, exp_fig);
}

function init_all(): void {
    var settings = new ViewSettings();
    document.body.style.background = settings.BodyBackColor.getColor();
    document.body.style.color = settings.BodyTextColor.getColor();
    init_input(settings);
    init_fig(settings);
}

//function init_all2() {
//    var settings = new ViewSettings();
//    init_input2(settings);
//    init_fig(settings);
//}

function buttonEqual_Click(): void {
    outputText.setText("");
    statusText.setText("");
    exp_fig.setSource(inputText.getText());
}

function buttonBS_Click(): void {
    var str = trim(inputText.getText());
    if (str.length > 0) {
        str = str.substr(0, str.length - 1);
        str = trim(str);
    }
    inputText.setText(str);
}

function buttonClear_Click(): void {
    inputText.setText("");
    outputText.setText("");
    statusText.setText("");
    exp_fig.setSource("");
}
