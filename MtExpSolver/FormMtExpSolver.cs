using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

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
            elementHostIn.ContextMenuStrip = contextMenuStrip1;

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
            elementHostOut.ContextMenuStrip = contextMenuStrip1;

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
        }

        private void FormMtExpSolver_Load(object sender, EventArgs e)
        {
            this.RestoreState();

            elementHostIn.Height = (this.ClientSize.Height / 4) * 3;
            elementHostOut.Height = (this.ClientSize.Height / 4) * 1;

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

        }

        public void insertText(string text)
        {

            int caretPosition = editorIn.CaretOffset;
            editorIn.Document.Insert(caretPosition, text);
        }



        private async Task Timer_Tick(object sender, EventArgs e)
        {
            if (locked) { 
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
            if (editorOut.Text != message) {
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

    }
}
