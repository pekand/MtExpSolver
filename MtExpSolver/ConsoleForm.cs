using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MtExpSolver
{
    public partial class ConsoleForm : Form
    {

        FormMtExpSolver? MtExpSolver = null;

        public ConsoleForm(FormMtExpSolver MtExpSolver)
        {
            InitializeComponent();

            this.MtExpSolver = MtExpSolver;

            MtExpSolver.consoleWrapper.MessageEvent += Write;
        }

        public void Write(string message)
        {
            if (!this.Visible) {
                return;
            }

            if (scintilla1.InvokeRequired)
            {
                scintilla1.Invoke(new Action(() => scintilla1.Text += message + "\n"));
            }
            else
            {
                scintilla1.Text += message + "\n";
            }

            
        }

        private void Console_Load(object sender, EventArgs e)
        {
            scintilla1.Margins[0].Width = 40;
            scintilla1.Margins[0].BackColor = Color.Black;
        }
    }
}
