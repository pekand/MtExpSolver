namespace MtExpSolver
{
    partial class ConsoleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            scintilla1 = new ScintillaNET.Scintilla();
            SuspendLayout();
            // 
            // scintilla1
            // 
            scintilla1.AutocompleteListSelectedBackColor = Color.FromArgb(0, 120, 215);
            scintilla1.Dock = DockStyle.Fill;
            scintilla1.Font = new Font("Consolas", 15.75F);
            scintilla1.LexerName = null;
            scintilla1.Location = new Point(0, 0);
            scintilla1.Name = "scintilla1";
            scintilla1.ScrollWidth = 120;
            scintilla1.Size = new Size(800, 450);
            scintilla1.TabIndex = 0;
            // 
            // ConsoleForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(scintilla1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ConsoleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Console";
            Load += Console_Load;
            ResumeLayout(false);
        }

        #endregion

        private ScintillaNET.Scintilla scintilla1;
    }
}