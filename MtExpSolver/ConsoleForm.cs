using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace MtExpSolver
{
    public partial class ConsoleForm : Form
    {
        public FormMtExpSolver? MtExpSolver = null;

        public TextEditor? editor = null;
        public ElementHost? editorHost = null;

        public ConsoleForm(FormMtExpSolver MtExpSolver)
        {
            InitializeComponent();

            this.MtExpSolver = MtExpSolver;

            MtExpSolver.consoleWrapper.MessageEvent += Write;

            editor = new TextEditor();
            editorHost = new ElementHost();
            editorHost.Dock = DockStyle.Fill;
            editorHost.Child = editor;

            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
            editor.WordWrap = true;
            editor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            editor.FontSize = 20;
            editor.IsReadOnly = true;

            this.Controls.Add(editorHost);

        }

        private void Console_Load(object sender, EventArgs e)
        {

        }

        public void Write(string message)
        {
            if (!this.Visible) {
                return;
            }

            if (editorHost.InvokeRequired)
            {
                editorHost.Invoke(new Action(() => editor.Text += message + "\n"));
            }
            else
            {
                editor.Text += message + "\n";
            }

            
        }

        
    }
}
