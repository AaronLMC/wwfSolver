namespace wwfSolver
{
    partial class MainForm
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
            this.mMainMenu = new System.Windows.Forms.MenuStrip();
            this.mFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMainMenu
            // 
            this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileMenu});
            this.mMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mMainMenu.Name = "mMainMenu";
            this.mMainMenu.Size = new System.Drawing.Size(692, 24);
            this.mMainMenu.TabIndex = 0;
            this.mMainMenu.Text = "menuStrip1";
            // 
            // mFileMenu
            // 
            this.mFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mExitMenu});
            this.mFileMenu.Name = "mFileMenu";
            this.mFileMenu.Size = new System.Drawing.Size(35, 20);
            this.mFileMenu.Text = "&File";
            // 
            // mExitMenu
            // 
            this.mExitMenu.Name = "mExitMenu";
            this.mExitMenu.Size = new System.Drawing.Size(152, 22);
            this.mExitMenu.Text = "&Exit";
            this.mExitMenu.Click += new System.EventHandler(this.mExitMenu_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 473);
            this.Controls.Add(this.mMainMenu);
            this.MainMenuStrip = this.mMainMenu;
            this.Name = "MainForm";
            this.Text = "WWF Solver";
            this.mMainMenu.ResumeLayout(false);
            this.mMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mMainMenu;
        private System.Windows.Forms.ToolStripMenuItem mFileMenu;
        private System.Windows.Forms.ToolStripMenuItem mExitMenu;
    }
}

