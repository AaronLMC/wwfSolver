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
            this.mLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.boardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mGameBoardLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.mAvailableLettersTxt = new System.Windows.Forms.TextBox();
            this.mGoBtn = new System.Windows.Forms.Button();
            this.mSolutionsListView = new System.Windows.Forms.ListView();
            this.mScoreCol = new System.Windows.Forms.ColumnHeader();
            this.mNumTilesCol = new System.Windows.Forms.ColumnHeader();
            this.mWordCol = new System.Windows.Forms.ColumnHeader();
            this.mHighestScoreLbl = new System.Windows.Forms.Label();
            this.mSolutionCountLbl = new System.Windows.Forms.Label();
            this.mWordsEvaluatedLbl = new System.Windows.Forms.Label();
            this.mMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMainMenu
            // 
            this.mMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileMenu,
            this.boardToolStripMenuItem});
            this.mMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mMainMenu.Name = "mMainMenu";
            this.mMainMenu.Size = new System.Drawing.Size(1064, 24);
            this.mMainMenu.TabIndex = 0;
            this.mMainMenu.Text = "menuStrip1";
            // 
            // mFileMenu
            // 
            this.mFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mLoadMenuItem,
            this.mSaveMenuItem,
            this.mExitMenu});
            this.mFileMenu.Name = "mFileMenu";
            this.mFileMenu.Size = new System.Drawing.Size(37, 20);
            this.mFileMenu.Text = "&File";
            // 
            // mLoadMenuItem
            // 
            this.mLoadMenuItem.Name = "mLoadMenuItem";
            this.mLoadMenuItem.Size = new System.Drawing.Size(100, 22);
            this.mLoadMenuItem.Text = "&Load";
            this.mLoadMenuItem.Click += new System.EventHandler(this.LoadMenuItemOnClick);
            // 
            // mSaveMenuItem
            // 
            this.mSaveMenuItem.Name = "mSaveMenuItem";
            this.mSaveMenuItem.Size = new System.Drawing.Size(100, 22);
            this.mSaveMenuItem.Text = "&Save";
            this.mSaveMenuItem.Click += new System.EventHandler(this.SaveMenuItemOnClick);
            // 
            // mExitMenu
            // 
            this.mExitMenu.Name = "mExitMenu";
            this.mExitMenu.Size = new System.Drawing.Size(100, 22);
            this.mExitMenu.Text = "&Exit";
            this.mExitMenu.Click += new System.EventHandler(this.ExitMenuOnClick);
            // 
            // boardToolStripMenuItem
            // 
            this.boardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mClearMenuItem});
            this.boardToolStripMenuItem.Name = "boardToolStripMenuItem";
            this.boardToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.boardToolStripMenuItem.Text = "&Board";
            // 
            // mClearMenuItem
            // 
            this.mClearMenuItem.Name = "mClearMenuItem";
            this.mClearMenuItem.Size = new System.Drawing.Size(101, 22);
            this.mClearMenuItem.Text = "&Clear";
            this.mClearMenuItem.Click += new System.EventHandler(this.ClearMenuItemOnClick);
            // 
            // mGameBoardLayout
            // 
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
            // mSolutionsListView
            // 
            this.mSolutionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mScoreCol,
            this.mNumTilesCol,
            this.mWordCol});
            this.mSolutionsListView.FullRowSelect = true;
            this.mSolutionsListView.HideSelection = false;
            this.mSolutionsListView.Location = new System.Drawing.Point(606, 28);
            this.mSolutionsListView.MultiSelect = false;
            this.mSolutionsListView.Name = "mSolutionsListView";
            this.mSolutionsListView.Size = new System.Drawing.Size(446, 533);
            this.mSolutionsListView.TabIndex = 4;
            this.mSolutionsListView.UseCompatibleStateImageBehavior = false;
            this.mSolutionsListView.View = System.Windows.Forms.View.Details;
            // 
            // mScoreCol
            // 
            this.mScoreCol.Text = "Score";
            this.mScoreCol.Width = 52;
            // 
            // mNumTilesCol
            // 
            this.mNumTilesCol.Text = "# Tiles";
            this.mNumTilesCol.Width = 50;
            // 
            // mWordCol
            // 
            this.mWordCol.Text = "Word";
            this.mWordCol.Width = 302;
            // 
            // mHighestScoreLbl
            // 
            this.mHighestScoreLbl.AutoSize = true;
            this.mHighestScoreLbl.Location = new System.Drawing.Point(605, 574);
            this.mHighestScoreLbl.Name = "mHighestScoreLbl";
            this.mHighestScoreLbl.Size = new System.Drawing.Size(80, 13);
            this.mHighestScoreLbl.TabIndex = 5;
            this.mHighestScoreLbl.Text = "Highest Score: ";
            // 
            // mSolutionCountLbl
            // 
            this.mSolutionCountLbl.AutoSize = true;
            this.mSolutionCountLbl.Location = new System.Drawing.Point(725, 574);
            this.mSolutionCountLbl.Name = "mSolutionCountLbl";
            this.mSolutionCountLbl.Size = new System.Drawing.Size(105, 13);
            this.mSolutionCountLbl.TabIndex = 6;
            this.mSolutionCountLbl.Text = "Number of Solutions:";
            // 
            // mWordsEvaluatedLbl
            // 
            this.mWordsEvaluatedLbl.AutoSize = true;
            this.mWordsEvaluatedLbl.Location = new System.Drawing.Point(886, 574);
            this.mWordsEvaluatedLbl.Name = "mWordsEvaluatedLbl";
            this.mWordsEvaluatedLbl.Size = new System.Drawing.Size(95, 13);
            this.mWordsEvaluatedLbl.TabIndex = 7;
            this.mWordsEvaluatedLbl.Text = "Words Evaluated: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 620);
            this.Controls.Add(this.mWordsEvaluatedLbl);
            this.Controls.Add(this.mSolutionCountLbl);
            this.Controls.Add(this.mHighestScoreLbl);
            this.Controls.Add(this.mSolutionsListView);
            this.Controls.Add(this.mGoBtn);
            this.Controls.Add(this.mAvailableLettersTxt);
            this.Controls.Add(this.mGameBoardLayout);
            this.Controls.Add(this.mMainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mMainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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
        private System.Windows.Forms.ToolStripMenuItem mSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mClearMenuItem;
        private System.Windows.Forms.ListView mSolutionsListView;
        private System.Windows.Forms.ColumnHeader mScoreCol;
        private System.Windows.Forms.ColumnHeader mNumTilesCol;
        private System.Windows.Forms.ColumnHeader mWordCol;
        private System.Windows.Forms.Label mHighestScoreLbl;
        private System.Windows.Forms.Label mSolutionCountLbl;
        private System.Windows.Forms.Label mWordsEvaluatedLbl;
    }
}

