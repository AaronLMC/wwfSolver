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
            this.mGameBoardLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.mAvailableLettersTxt = new System.Windows.Forms.TextBox();
            this.mGoBtn = new System.Windows.Forms.Button();
            this.mMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMainMenu
            // 
            this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileMenu});
            this.mMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mMainMenu.Name = "mMainMenu";
            this.mMainMenu.Size = new System.Drawing.Size(611, 24);
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
            this.mExitMenu.Size = new System.Drawing.Size(92, 22);
            this.mExitMenu.Text = "&Exit";
            this.mExitMenu.Click += new System.EventHandler(this.ExitMenuOnClick);
            // 
            // mGameBoardLayout
            // 
            this.mGameBoardLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mGameBoardLayout.Location = new System.Drawing.Point(12, 27);
            this.mGameBoardLayout.Name = "mGameBoardLayout";
            this.mGameBoardLayout.Size = new System.Drawing.Size(587, 534);
            this.mGameBoardLayout.TabIndex = 1;
            // 
            // mAvailableLettersTxt
            // 
            this.mAvailableLettersTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mAvailableLettersTxt.Location = new System.Drawing.Point(12, 574);
            this.mAvailableLettersTxt.MaxLength = 7;
            this.mAvailableLettersTxt.Name = "mAvailableLettersTxt";
            this.mAvailableLettersTxt.Size = new System.Drawing.Size(392, 32);
            this.mAvailableLettersTxt.TabIndex = 2;
            // 
            // mGoBtn
            // 
            this.mGoBtn.Location = new System.Drawing.Point(426, 574);
            this.mGoBtn.Name = "mGoBtn";
            this.mGoBtn.Size = new System.Drawing.Size(173, 32);
            this.mGoBtn.TabIndex = 3;
            this.mGoBtn.Text = "Find Best Words";
            this.mGoBtn.UseVisualStyleBackColor = true;
            this.mGoBtn.Click += new System.EventHandler(this.GoBtnOnClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 620);
            this.Controls.Add(this.mGoBtn);
            this.Controls.Add(this.mAvailableLettersTxt);
            this.Controls.Add(this.mGameBoardLayout);
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
        private System.Windows.Forms.FlowLayoutPanel mGameBoardLayout;
        private System.Windows.Forms.TextBox mAvailableLettersTxt;
        private System.Windows.Forms.Button mGoBtn;
    }
}

