namespace MtExpSolver
{
    partial class FormMtExpSolver
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMtExpSolver));
            scintillaIn = new ScintillaNET.Scintilla();
            contextMenuStrip1 = new ContextMenuStrip(components);
            functionsToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            consoleToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            mostTopToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            scintillaOut = new ScintillaNET.Scintilla();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // scintillaIn
            // 
            scintillaIn.AutocompleteListSelectedBackColor = Color.FromArgb(0, 120, 215);
            scintillaIn.BorderStyle = ScintillaNET.BorderStyle.None;
            scintillaIn.ContextMenuStrip = contextMenuStrip1;
            scintillaIn.Dock = DockStyle.Top;
            scintillaIn.Font = new Font("Consolas", 15.75F);
            scintillaIn.LexerName = null;
            scintillaIn.Location = new Point(0, 0);
            scintillaIn.Name = "scintillaIn";
            scintillaIn.ScrollWidth = 93;
            scintillaIn.Size = new Size(911, 470);
            scintillaIn.TabIndex = 8;
            scintillaIn.TextChanged += scintillaIn_TextChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { functionsToolStripMenuItem, fileToolStripMenuItem, clearToolStripMenuItem, consoleToolStripMenuItem, optionsToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(138, 148);
            // 
            // functionsToolStripMenuItem
            // 
            functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            functionsToolStripMenuItem.Size = new Size(137, 24);
            functionsToolStripMenuItem.Text = "Functions";
            functionsToolStripMenuItem.Click += functionsToolStripMenuItem_Click;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, saveAsToolStripMenuItem, openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(137, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(123, 24);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(123, 24);
            saveAsToolStripMenuItem.Text = "Save as";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(123, 24);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(137, 24);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // consoleToolStripMenuItem
            // 
            consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            consoleToolStripMenuItem.Size = new Size(137, 24);
            consoleToolStripMenuItem.Text = "Console";
            consoleToolStripMenuItem.Click += consoleToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mostTopToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(137, 24);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // mostTopToolStripMenuItem
            // 
            mostTopToolStripMenuItem.Name = "mostTopToolStripMenuItem";
            mostTopToolStripMenuItem.Size = new Size(135, 24);
            mostTopToolStripMenuItem.Text = "Most top";
            mostTopToolStripMenuItem.Click += mostTopToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(137, 24);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // scintillaOut
            // 
            scintillaOut.AutocompleteListSelectedBackColor = Color.FromArgb(0, 120, 215);
            scintillaOut.BorderStyle = ScintillaNET.BorderStyle.None;
            scintillaOut.Dock = DockStyle.Bottom;
            scintillaOut.Font = new Font("Consolas", 15.75F);
            scintillaOut.LexerName = null;
            scintillaOut.Location = new Point(0, 468);
            scintillaOut.Name = "scintillaOut";
            scintillaOut.ScrollWidth = 49;
            scintillaOut.Size = new Size(911, 145);
            scintillaOut.TabIndex = 9;
            // 
            // FormMtExpSolver
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(911, 613);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(scintillaOut);
            Controls.Add(scintillaIn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMtExpSolver";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MtExpSolver";
            FormClosing += FormMtExpSolver_FormClosing;
            Load += FormMtExpSolver_Load;
            Resize += FormMtExpSolver_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label labelError;
        private ScintillaNET.Scintilla scintillaIn;
        private ContextMenuStrip contextMenuStrip1;
        private ScintillaNET.Scintilla scintillaOut;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem functionsToolStripMenuItem;
        private ToolStripMenuItem consoleToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem mostTopToolStripMenuItem;
    }
}
