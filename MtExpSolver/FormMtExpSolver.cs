using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Brush = System.Windows.Media.Brush;

namespace MtExpSolver
{
    public partial class FormMtExpSolver : Form
    {

        private System.Windows.Forms.Timer timer;

        public bool TextChanged = false;
        public bool Calculating = false;

        public ConsoleForm? consoleForm = null;
        public ConsoleWrapper consoleWrapper = new ConsoleWrapper();

        public string path = "";

        public bool locked = false;

        public TextEditor? editorIn = null;
        public ElementHost? elementHostIn = null;
        public TextEditor? editorOut = null;
        public ElementHost? elementHostOut = null;

        public bool darkMode = false;

        private Brush editorDefaultBackground;
        private Brush editorDefaultForeground;
        private Brush editorDefaultSelectionBrush;
        private Brush editorDefaultCaretBrush;
        private Brush editorDefaultCurrentLineBackground;

        public FormMtExpSolver(string path)
        {
            locked = true;

            this.path = path;

            InitializeComponent();

            this.consoleForm = new ConsoleForm(this);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += async (sender, e) => await Timer_Tick(sender, e);
            timer.Start();
            CreateContextmenuItems();

            editorIn = new TextEditor();
            elementHostIn = new ElementHost();
            elementHostIn.Dock = DockStyle.Top;
            elementHostIn.Child = editorIn;
            elementHostIn.ContextMenuStrip = contextMenuStrip;

            editorIn.ShowLineNumbers = true;
            editorIn.WordWrap = true;
            editorIn.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editorIn.FontSize = 20;
            editorIn.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");            
            editorIn.TextChanged += this.scintillaIn_TextChanged;
            editorIn.KeyDown += KeyDown;
            editorIn.PreviewMouseWheel += this.EditorIn_MouseWheel;
            this.Controls.Add(elementHostIn);

            ///////////////////////////

            editorOut = new TextEditor();

            editorOut.ShowLineNumbers = true;
            editorOut.WordWrap = true;
            editorOut.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editorOut.FontSize = 20;
            editorOut.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");

            elementHostOut = new ElementHost();
            elementHostOut.Dock = DockStyle.Bottom;
            elementHostOut.Child = editorOut;
            elementHostOut.ContextMenuStrip = contextMenuStrip;

            editorOut.ShowLineNumbers = true;
            editorOut.WordWrap = true;
            editorOut.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editorOut.FontSize = 20;
            editorOut.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
            editorOut.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(238, 238, 238));
            editorOut.PreviewMouseWheel += this.EditorOut_MouseWheel;
            editorOut.KeyDown += KeyDown;
            editorOut.IsReadOnly = true;
            this.Controls.Add(elementHostOut);

            editorDefaultBackground = editorIn.Background;
            editorDefaultForeground = editorIn.Foreground;
            editorDefaultSelectionBrush = editorIn.TextArea.SelectionBrush;
            editorDefaultCaretBrush = editorIn.TextArea.Caret.CaretBrush;
            editorDefaultCurrentLineBackground = editorIn.TextArea.TextView.CurrentLineBackground;
        }

        private void FormMtExpSolver_Load(object sender, EventArgs e)
        {
            this.RestoreState();

            elementHostIn.Height = (this.ClientSize.Height / 4) * 3;
            elementHostOut.Height = (this.ClientSize.Height / 4) * 1;

            this.switchDarkMode();

            locked = false;            
        }

        public void CreateContextmenuItems()
        {
            var MathItem = new ToolStripMenuItem("Math", null);
            functionsToolStripMenuItem.DropDownItems.Add(MathItem);

            var ConstantsItem = new ToolStripMenuItem("Constants", null);
            MathItem.DropDownItems.Add(ConstantsItem);
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("Pi", null, (s, e) => insertText("Math.PI")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("E", null, (s, e) => insertText("Math.E")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("SQRT2", null, (s, e) => insertText("Math.SQRT2")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("SQRT1_2", null, (s, e) => insertText("Math.SQRT1_2")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("LN2", null, (s, e) => insertText("Math.LN2")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("LN10", null, (s, e) => insertText("Math.LN10")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("LOG2E", null, (s, e) => insertText("Math.LOG2E")));
            ConstantsItem.DropDownItems.Add(new ToolStripMenuItem("LOG10E", null, (s, e) => insertText("Math.LOG10E")));


            var FunctionsItem = new ToolStripMenuItem("Functions", null);
            MathItem.DropDownItems.Add(FunctionsItem);
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("abs(x)", null, (s, e) => insertText("Math.abs()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("cbrt(x)", null, (s, e) => insertText("Math.cbrt()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("ceil(x)", null, (s, e) => insertText("Math.ceil()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("clz32(x)", null, (s, e) => insertText("Math.clz32()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("exp(x)", null, (s, e) => insertText("Math.exp()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("expm1(x)", null, (s, e) => insertText("Math.expm1()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("floor(x)", null, (s, e) => insertText("Math.floor()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("fround(x)", null, (s, e) => insertText("Math.fround()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("log(x)", null, (s, e) => insertText("Math.log()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("log10(x)", null, (s, e) => insertText("Math.log10()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("log1p(x)", null, (s, e) => insertText("Math.log1p()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("log2(x)", null, (s, e) => insertText("Math.log2()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("max(x1,x2,..)", null, (s, e) => insertText("Math.max()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("min(x1,x2,..)", null, (s, e) => insertText("Math.min()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("pow(x, y)", null, (s, e) => insertText("Math.pow()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("random()", null, (s, e) => insertText("Math.random()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("round(x)", null, (s, e) => insertText("Math.round()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("sign(x)", null, (s, e) => insertText("Math.sign()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("sqrt(x)", null, (s, e) => insertText("Math.sqrt()")));
            FunctionsItem.DropDownItems.Add(new ToolStripMenuItem("trunc(x)", null, (s, e) => insertText("Math.trunc()")));

            var TrigonometryItem = new ToolStripMenuItem("Trigonometry", null);
            MathItem.DropDownItems.Add(TrigonometryItem);
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("acos(x)", null, (s, e) => insertText("Math.acos()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("acosh(x)", null, (s, e) => insertText("Math.acosh()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("asin(x)", null, (s, e) => insertText("Math.asin()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("asinh(x)", null, (s, e) => insertText("Math.asinh()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("atan(x)", null, (s, e) => insertText("Math.atan()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("atan2(y,x)", null, (s, e) => insertText("Math.atan2()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("atanh(x)", null, (s, e) => insertText("Math.atanh()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("cos(x)", null, (s, e) => insertText("Math.cos()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("cosh(x)", null, (s, e) => insertText("Math.cosh()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("sin(x)", null, (s, e) => insertText("Math.sin()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("sinh(x)", null, (s, e) => insertText("Math.sinh()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("tan(x)", null, (s, e) => insertText("Math.tan()")));
            TrigonometryItem.DropDownItems.Add(new ToolStripMenuItem("tanh(x)", null, (s, e) => insertText("Math.tanh()")));

            var JavaScriptItem = new ToolStripMenuItem("JavaScript", null);
            functionsToolStripMenuItem.DropDownItems.Add(JavaScriptItem);

            var JSOperatorsItem = new ToolStripMenuItem("Operators", null);
            JavaScriptItem.DropDownItems.Add(JSOperatorsItem);


            var AssignmentOperatorsItem = new ToolStripMenuItem("Assignment operators", null);
            JSOperatorsItem.DropDownItems.Add(AssignmentOperatorsItem);

            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Assignment x = y", null, (s, e) => insertText("x = y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Addition assignment x += y", null, (s, e) => insertText("x += y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Subtraction assignment x -= y", null, (s, e) => insertText("x -= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Multiplication assignment x *= y", null, (s, e) => insertText("x *= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Division assignment x /= y", null, (s, e) => insertText("x /= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Remainder assignment x %= y", null, (s, e) => insertText("x %= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Exponentiation assignment x **= y", null, (s, e) => insertText("x **= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Left shift assignment x <<= y", null, (s, e) => insertText("x <<= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Right shift assignment x >>= y", null, (s, e) => insertText("x >>= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Unsigned right shift assignment x >>>= y", null, (s, e) => insertText("x >>>= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise AND assignment x &= y", null, (s, e) => insertText("x &= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise XOR assignment x ^= y", null, (s, e) => insertText("x ^= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise OR assignment x |= y", null, (s, e) => insertText("x |= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Logical AND assignment x &&= y", null, (s, e) => insertText("x &&= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Logical OR assignment x ||= y", null, (s, e) => insertText("x ||= y")));
            AssignmentOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Nullish coalescing assignment x ??= y", null, (s, e) => insertText("x ??= y")));

            var ComparisonOperatorsItem = new ToolStripMenuItem("Comparison operators", null);
            JSOperatorsItem.DropDownItems.Add(ComparisonOperatorsItem);

            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Equal (a==b)", null, (s, e) => insertText("a == b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Not equal (a!=b)", null, (s, e) => insertText("a != b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Strict equal (a===b)", null, (s, e) => insertText("a === b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Strict not equal (a!==b)", null, (s, e) => insertText("a !== b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Greater than (a>b)", null, (s, e) => insertText("a > b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Greater than or equal (a>=b)", null, (s, e) => insertText("a >= b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Less than (a<)", null, (s, e) => insertText("a < b")));
            ComparisonOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Less than or equal (a<=b)", null, (s, e) => insertText("a <= b")));

            var ArithmeticOperatorsItem = new ToolStripMenuItem("Arithmetic operators", null);
            JSOperatorsItem.DropDownItems.Add(ArithmeticOperatorsItem);

            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Remainder (a % b)", null, (s, e) => insertText("a % b")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Increment (a++)", null, (s, e) => insertText("a++")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Increment (++a)", null, (s, e) => insertText("++a")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Decrement (a--)", null, (s, e) => insertText("a--")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Decrement (--a)", null, (s, e) => insertText("--a")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Unary negation (-a)", null, (s, e) => insertText("-a")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Unary plus (+a)", null, (s, e) => insertText("+a")));
            ArithmeticOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Exponentiation operator (a**b)", null, (s, e) => insertText("a**b")));

            var BitwiseOperatorsItem = new ToolStripMenuItem("Bitwise operators", null);
            JSOperatorsItem.DropDownItems.Add(BitwiseOperatorsItem);

            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise AND (a&b)", null, (s, e) => insertText("a&b")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise OR (a|b)", null, (s, e) => insertText("a|b")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise XOR (a^b)", null, (s, e) => insertText("a^b")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Bitwise NOT (~a)", null, (s, e) => insertText("~a")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Left shift (a<<n)", null, (s, e) => insertText("a<<n")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Sign-propagating right shift (a>>n)", null, (s, e) => insertText("a>>n")));
            BitwiseOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Zero-fill right shift (a>>>n)", null, (s, e) => insertText("a>>>n")));

            var LogicalOperatorsItem = new ToolStripMenuItem("Logical operators", null);
            JSOperatorsItem.DropDownItems.Add(LogicalOperatorsItem);

            LogicalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Logical AND (a && b)", null, (s, e) => insertText("a && b")));
            LogicalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Logical OR (a || b)", null, (s, e) => insertText("a || b")));
            LogicalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Nullish coalescing operator (a ?? b)", null, (s, e) => insertText("a ?? b")));
            LogicalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("Logical NOT (!a)", null, (s, e) => insertText("!a")));

            var BigIntOperatorsItem = new ToolStripMenuItem("BigInt operators", null);
            JSOperatorsItem.DropDownItems.Add(BigIntOperatorsItem);

            BigIntOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("BigInt operators", null, (s, e) => insertText("\r\n1000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000n")));


            var StringOperatorsItem = new ToolStripMenuItem("String operators", null);
            JSOperatorsItem.DropDownItems.Add(StringOperatorsItem);

            StringOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("String operators", null, (s, e) => insertText("\"string\" + \"string\"")));

            var ConditionalOperatorItem = new ToolStripMenuItem("Conditional (ternary) operator", null);
            JSOperatorsItem.DropDownItems.Add(ConditionalOperatorItem);

            ConditionalOperatorItem.DropDownItems.Add(new ToolStripMenuItem("Conditional (ternary) operator ( true ? a : b )", null, (s, e) => insertText(" true ? a : b ")));

            var CommaOperatorItem = new ToolStripMenuItem("Comma operator", null);
            JSOperatorsItem.DropDownItems.Add(CommaOperatorItem);

            CommaOperatorItem.DropDownItems.Add(new ToolStripMenuItem("Comma operator", null, (s, e) => insertText("var x = [0, 1, 2]")));

            var UnaryOperatorsItem = new ToolStripMenuItem("Unary operators", null);
            JSOperatorsItem.DropDownItems.Add(UnaryOperatorsItem);

            UnaryOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("delete (delete object.property)", null, (s, e) => insertText("delete object.property")));
            UnaryOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("delete (delete object[propertyKey])", null, (s, e) => insertText("delete object[propertyKey]")));
            UnaryOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("typeof (typeof a)", null, (s, e) => insertText("typeof a")));
            UnaryOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("void", null, (s, e) => insertText("void")));

            var RelationalOperatorsItem = new ToolStripMenuItem("Relational operators", null);
            JSOperatorsItem.DropDownItems.Add(RelationalOperatorsItem);

            RelationalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("in (propNameOrNumber in objectName)", null, (s, e) => insertText("\"PI\" in Math")));
            RelationalOperatorsItem.DropDownItems.Add(new ToolStripMenuItem("instanceof (object instanceof objectType)", null, (s, e) => insertText("const obj = new Map();\r\nif (obj instanceof Map) {\r\n  // statements to execute\r\n}")));
        }

        public void insertText(string text)
        {

            int caretPosition = editorIn.CaretOffset;
            editorIn.Document.Insert(caretPosition, text);
        }

        private async Task Timer_Tick(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            if (TextChanged && !Calculating)
            {
                try
                {
                    Calculating = true;
                    string script = editorIn.Text;

                    if (script.Trim() != "")
                    {
                        this.Write("");
                        var result = await RunScriptAsync(script, timeout: 5000);
                        this.Write(result.ToString());
                    }
                    else
                    {
                        this.Write("");
                    }
                }
                catch (Exception ex)
                {
                    this.Write(ex.Message);
                }
                Calculating = false;
                TextChanged = false;
            }
        }

        private void scintillaIn_TextChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            TextChanged = true;
        }

        private void FormMtExpSolver_Resize(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }

            elementHostIn.Height = (this.ClientSize.Height / 4) * 3;
            elementHostOut.Height = (this.ClientSize.Height / 4) * 1;
        }

        private async Task<object?> RunScriptAsync(string jsCode, int timeout)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var engine = new V8ScriptEngine())
                    {
                        using (var cts = new CancellationTokenSource())
                        {
                            System.Threading.Timer? timer = null;

                            try
                            {
                                timer = new System.Threading.Timer(_ =>
                                {
                                    cts.Cancel();
                                    engine.Interrupt();
                                }, null, timeout, Timeout.Infinite);

                                string wrappedExpression = jsCode;

                                engine.AddHostObject("console", this.consoleWrapper);

                                return engine.Evaluate(wrappedExpression);
                            }
                            catch (ScriptInterruptedException)
                            {
                                return "Script execution timed out.";
                            }
                            finally
                            {
                                timer?.Dispose();
                            }
                        }
                    }
                }
                catch (ScriptEngineException ex)
                {
                    string message = "";

                    message += ex.ErrorDetails + "\n";

                    /*if (ex.ErrorDetails.Contains("at line"))
                    {
                        string[] details = ex.ErrorDetails.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in details)
                        {
                            if (line.Contains("at line"))
                            {                                
                                message += "line: " + line + "\n";
                            }
                        }
                    }*/

                    return message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

            });
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Write(string message)
        {
            if (editorOut.Text != message)
            {
                editorOut.Text = message;
            }

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editorOut.Text = "";
        }

        private void functionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.consoleForm.Show();
        }

        public void SaveState()
        {
            if (path == "")
            {
                return;
            }

            try
            {
                var xml = new XElement("Root",
                    new XElement("in", editorIn.Text),
                    new XElement("out", editorOut.Text),
                    new XElement("Left", this.Left),
                    new XElement("Top", this.Top),
                    new XElement("Width", this.Width),
                    new XElement("Height", this.Height),
                    new XElement("TopMost", this.TopMost),
                    new XElement("DarkMode", this.darkMode),
                    new XElement("FontInSize", editorIn.FontSize),
                    new XElement("FontOutSize", editorOut.FontSize)
                );

                xml.Save(this.path);
            }
            catch (Exception ex)
            {
                consoleForm.Write(ex.Message);
            }
        }

        public void RestoreState()
        {
            if (path == "" || !File.Exists(this.path))
            {
                return;
            }

            try
            {
                var xml = XElement.Load(this.path);
                editorIn.Text = xml.Element("in")?.Value ?? "";
                editorOut.Text = xml.Element("out")?.Value ?? "";

                this.StartPosition = FormStartPosition.Manual;
                this.Left = int.Parse(xml.Element("Left")?.Value);
                this.Top = int.Parse(xml.Element("Top")?.Value);
                this.Width = int.Parse(xml.Element("Width")?.Value);
                this.Height = int.Parse(xml.Element("Height")?.Value);
                this.TopMost = bool.Parse(xml.Element("TopMost")?.Value);
                this.darkMode = bool.Parse(xml.Element("DarkMode")?.Value);
                this.editorIn.FontSize = int.Parse(xml.Element("FontInSize")?.Value);
                this.editorOut.FontSize = int.Parse(xml.Element("FontOutSize")?.Value);

            }
            catch (Exception ex)
            {
                consoleForm.Write(ex.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == "")
            {
                this.saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveState();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "MtExpSolver Files (*.MtExpSolver)|*.MtExpSolver|All Files (*.*)|*.*";
                saveFileDialog.DefaultExt = "MtExpSolver";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.path = saveFileDialog.FileName;
                    SaveState();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.MtExpSolver)|*.MtExpSolver|All Files (*.*)|*.*";
                openFileDialog.DefaultExt = "MtExpSolver";
                openFileDialog.AddExtension = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.path = openFileDialog.FileName;
                    RestoreState();
                }
            }
        }

        private void mostTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            mostTopToolStripMenuItem.Checked = this.TopMost;
        }

        private void FormMtExpSolver_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveState();
        }

        public void EditorIn_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                    editorIn.FontSize += 1;
                else if (editorIn.FontSize > 1)
                    editorIn.FontSize -= 1;

                e.Handled = true;
            }
        }

        public void EditorOut_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                    editorOut.FontSize += 1;
                else if (editorOut.FontSize > 1)
                    editorOut.FontSize -= 1;

                e.Handled = true;
            }
        }

        private void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool isCtrlPressed = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (isCtrlPressed && e.Key == Key.S)
            {
                SaveState();
            }
        }

        // CONTEXT MENU OPENING
        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            darkModeToolStripMenuItem.Checked = this.darkMode;
        }


        // CONTEXT MENU OPTIONS DARK MODE SQITCH
        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.darkMode = !this.darkMode;
            switchDarkMode();
        }


        public void switchDarkMode() {
            if (editorIn == null || editorOut== null)
            {
                return;
            }


            /*var def = editorIn.SyntaxHighlighting;
            var names = def.NamedHighlightingColors
               .Select(c => c.Name)
               .ToList();

            foreach (var n in names)
                this.consoleForm.Write("Highlight name: " + n);*/

            if (this.darkMode)
            {

                editorIn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 56, 65));
                editorIn.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(216, 222, 233));
                editorIn.TextArea.SelectionBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(120, 100, 120, 170));
                editorIn.TextArea.Caret.CaretBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 220, 220));
                editorIn.TextArea.TextView.CurrentLineBackground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(20, 255, 255, 255));

                var highlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
                highlighting.GetNamedColor("Character").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(153, 199, 148));
                highlighting.GetNamedColor("Comment").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(166, 172, 185));
                highlighting.GetNamedColor("Digits").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(249, 171, 84));
                highlighting.GetNamedColor("JavaScriptGlobalFunctions").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(216, 222, 233));
                highlighting.GetNamedColor("JavaScriptIntrinsics").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(198, 149, 198));
                highlighting.GetNamedColor("JavaScriptKeyWords").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(190, 146, 198));
                highlighting.GetNamedColor("JavaScriptLiterals").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(200, 75, 68));  
                highlighting.GetNamedColor("Regex").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(198, 149, 198));
                highlighting.GetNamedColor("String").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(153, 199, 148));
                editorIn.SyntaxHighlighting = highlighting;

                editorOut.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 38, 42));
                editorOut.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(216, 222, 233));
            }
            else
            {

                editorIn.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                editorIn.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                editorIn.TextArea.SelectionBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(120, 100, 120, 170));
                editorIn.TextArea.Caret.CaretBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 220, 220));
                editorIn.TextArea.TextView.CurrentLineBackground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(20, 255, 255, 255));

                var highlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
                highlighting.GetNamedColor("Character").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(163, 21, 21));
                highlighting.GetNamedColor("Comment").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(0, 128, 0));
                highlighting.GetNamedColor("Digits").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(9, 134, 88)); 
                highlighting.GetNamedColor("JavaScriptGlobalFunctions").Foreground = new SimpleHighlightingBrush(Colors.Black);
                highlighting.GetNamedColor("JavaScriptIntrinsics").Foreground = new SimpleHighlightingBrush(Colors.Black);
                highlighting.GetNamedColor("JavaScriptKeyWords").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(0, 0, 255));
                highlighting.GetNamedColor("JavaScriptLiterals").Foreground = new SimpleHighlightingBrush(Colors.Black);
                highlighting.GetNamedColor("Regex").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(129, 31, 63));
                highlighting.GetNamedColor("String").Foreground = new SimpleHighlightingBrush(System.Windows.Media.Color.FromRgb(163, 21, 21)); 
                editorIn.SyntaxHighlighting = highlighting;

                editorOut.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(238, 238, 238)); 
                editorOut.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
            }
        }
    }
}
