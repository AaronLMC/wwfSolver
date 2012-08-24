using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace wwfSolver
{
    public partial class MainForm : Form, FrontEndInterface
    {
        private TextBox[,] mGameTextBoxes = new TextBox[GameVals.BOARD_SIZE, GameVals.BOARD_SIZE];
        private WordDict mWordDict;

        public MainForm()
        {
            InitializeComponent();
            mAvailableLettersTxt.MaxLength = GameVals.AVAILABLE_LETTER_MAX;
            mAvailableLettersTxt.TextChanged += new EventHandler(OnGameBoardTextChanged);

            mSolutionsListView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(SolutionsListViewOnItemSelectionChanged);

            mWordDict = new WordDict("res/wwfDict.txt");

            SetupGameBoard();
        }

        private void SolutionsListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ClearWordSolution();

            if (e.Item != null)
            {
                WordSolution solution = e.Item.Tag as WordSolution;
                Trace.Assert(solution != null, "Selected solution did not have a WordSolution object tag");
                if (solution == null) return;

                SetWordSolution(solution);
            }
        }
                
        private Board CurrentBoardConfig
        {
            get
            {
                char[,] boardLetters = new char[GameVals.BOARD_SIZE, GameVals.BOARD_SIZE];

                for (int i = 0; i < GameVals.BOARD_SIZE; i++)
                {
                    for (int j = 0; j < GameVals.BOARD_SIZE; j++)
                    {
                        Trace.Assert(mGameTextBoxes[i, j].Text != null, "Text box in game is null");
                        String letter = mGameTextBoxes[i, j].Text;
                        if (letter.Length == 0)
                        {
                            letter = " ";
                        }
                        boardLetters[i, j] = letter[0];
                    }
                }

                char[] availableLetters = mAvailableLettersTxt.Text.ToCharArray();

                return new Board(boardLetters, availableLetters);
            }

            set
            {
                SetBoardConfig(value);
                
            }
        }

        private void SetBoardConfig(Board board)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetBoardConfig(board);
                };
                Invoke(del);
            }
            else
            {
                mSolutionsListView.Items.Clear();

                mAvailableLettersTxt.Text = new String(board.AvailableLetters);

                for (int i = 0; i < GameVals.BOARD_SIZE; i++)
                {
                    for (int j = 0; j < GameVals.BOARD_SIZE; j++)
                    {
                        mGameTextBoxes[i, j].Text = board.BoardLetters[i, j].ToString();
                    }
                }
            }
        }

        private void SetupGameBoard()
        {
            for (int i = 0; i < GameVals.BOARD_SIZE; i++)
            {
                FlowLayoutPanel rowPanel = new FlowLayoutPanel();
                rowPanel.Width = mGameBoardLayout.Width;

                for (int j = 0; j < GameVals.BOARD_SIZE; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Font = new Font(FontFamily.GenericSansSerif, 12);
                    textBox.Size = new Size(33, textBox.Size.Height);
                    textBox.MaxLength = 1;
                    textBox.TextAlign = HorizontalAlignment.Center;
                    textBox.TextChanged += new EventHandler(OnGameBoardTextChanged);
                    textBox.KeyUp += new KeyEventHandler(textBox_KeyUp);
                    textBox.Tag = new SpaceInfo(i, j);

                    if (i == GameVals.BOARD_CENTER_LOC
                        && j == GameVals.BOARD_CENTER_LOC)
                    {
                        textBox.BackColor = Color.LightBlue;
                    }

                    mGameTextBoxes[i,j] = textBox;
                    rowPanel.Controls.Add(textBox);

                    rowPanel.Height = textBox.Height + 3;
                }

                mGameBoardLayout.Controls.Add(rowPanel);
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            
                if (sender is TextBox
                    && ((TextBox)sender).Tag is SpaceInfo)
                {
                    if (e.KeyCode == Keys.Up
                    || e.KeyCode == Keys.Down
                    || e.KeyCode == Keys.Left
                    || e.KeyCode == Keys.Right)
                    {
                        TextBox txtBox = sender as TextBox;
                        SpaceInfo info = txtBox.Tag as SpaceInfo;
                        int x = info.Location.X;
                        int y = info.Location.Y;

                        TextBox selectedBox = null;

                        if (e.KeyCode == Keys.Up)
                        {
                            if (x > 0)
                            {
                                selectedBox = mGameTextBoxes[x - 1, y];
                            }
                        }
                        else if (e.KeyCode == Keys.Down)
                        {
                            if (x < GameVals.BOARD_SIZE - 1)
                            {
                                selectedBox = mGameTextBoxes[x + 1, y];
                            }
                        }
                        else if (e.KeyCode == Keys.Left)
                        {
                            if (y > 0)
                            {
                                selectedBox = mGameTextBoxes[x, y - 1];
                            }
                        }
                        else if (e.KeyCode == Keys.Right)
                        {
                            if (y < GameVals.BOARD_SIZE - 1)
                            {
                                selectedBox = mGameTextBoxes[x, y + 1];
                            }
                        }

                        if (selectedBox != null)
                        {
                            selectedBox.Focus();
                            selectedBox.SelectAll();
                        }
                    }
                }
        }

        private void OnGameBoardTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;
                textBox.Text = (textBox.Text != null ? textBox.Text.ToUpper() : null);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void ExitMenuOnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GoBtnOnClick(object sender, EventArgs e)
        {
            ClearWordSolution();
            mSolutionsListView.Items.Clear();

            SetGoBtnEnabled(false);

            Thread goThread = new Thread(ProcessGoClick);
            goThread.Start();            
        }

        private void ProcessGoClick()
        {
            Board board = CurrentBoardConfig;
            GameSolver solver = new GameSolver(this, mWordDict, board);
            List<WordSolution> solutions = solver.GetSolutions();
            solutions.Sort();

            SetSolutions(solutions);

            SetGoBtnEnabled(true);
        }

        private void SetSolutions(List<WordSolution> solutions)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetSolutions(solutions);
                };
                Invoke(del);
            }
            else
            {
                mSolutionsListView.Items.Clear();

                foreach (WordSolution solution in solutions)
                {
                    ListViewItem item = new ListViewItem( new string[] {
                        solution.Score.ToString(),
                        solution.Letters.Length.ToString(),
                        solution.getWordList()});
                    item.Tag = solution;
                    mSolutionsListView.Items.Add(item);
                }
            }
        }

        private void SetGoBtnEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetGoBtnEnabled(enabled);
                };
                Invoke(del);
            }
            else
            {
                mGoBtn.Enabled = enabled;
            }
        }

        #region FrontEndInterface Members

        public void SetSearchStartLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.Red);
            
        }

        public void SetSearchRecurLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.Yellow);
        }

        public void SetLocationColorToDefault(int x, int y)
        {
            SetLocationColor(x, y, Color.White);
        }

        private void SetLocationColor(int x, int y, Color color)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetLocationColor(x, y, color);
                };
                Invoke(del);
            }
            else
            {
                mGameTextBoxes[x, y].BackColor = color;
                Update();
            }
        }

        public void SetWordSolution(WordSolution solution)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetWordSolution(solution);
                };
                Invoke(del);
            }
            else
            {
                foreach (LetterLoc letter in solution.Letters)
                {
                    TextBox txtBox = mGameTextBoxes[letter.X, letter.Y];

                    txtBox.Text = letter.Letter.ToString() + (letter.IsBlankLetter ? "*" : "");
                    (txtBox.Tag as SpaceInfo).IsSolutionTile = true;

                    SetLocationColor(letter.X, letter.Y, Color.LightCoral);
                }
            }
        }

        public void ClearWordSolution()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    ClearWordSolution();
                };
                Invoke(del);
            }
            else
            {
                for (int i = 0; i < GameVals.BOARD_SIZE; i++)
                {
                    for (int j = 0; j < GameVals.BOARD_SIZE; j++)
                    {
                        SpaceInfo info = mGameTextBoxes[i, j].Tag as SpaceInfo;
                        if (info.IsSolutionTile)
                        {
                            mGameTextBoxes[i, j].Text = " ";                            
                            SetLocationColorToDefault(i, j);
                            info.IsSolutionTile = false;
                        }
                    }
                }
            }
        }

        #endregion

        private void SaveMenuItemOnClick(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Game Board|*.wwf";
                DialogResult result = sfd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string targetFilename = sfd.FileName;
                    if (!targetFilename.EndsWith(".wwf")) targetFilename += ".wwf";
                    Board board = CurrentBoardConfig;
                    board.SaveToFile(targetFilename);                    
                }
            }
        }

        private void LoadMenuItemOnClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Game Board (*.wwf)|*.wwf";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string filename = ofd.FileName;
                    if (!File.Exists(filename))
                    {
                        MessageBox.Show("File does not exist");
                        return;
                    }

                    Board board = Board.Load(filename);
                    CurrentBoardConfig = board;
                }
            }
        }

        private void ClearMenuItemOnClick(object sender, EventArgs e)
        {
            Board clearBoard = new Board(new char[GameVals.BOARD_SIZE, GameVals.BOARD_SIZE], new char[0]);
            CurrentBoardConfig = clearBoard;

            mSolutionsListView.Items.Clear();
        }

        #region FrontEndInterface Members


        public void SetNumWordsEvaluated(int numWords)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetNumWordsEvaluated(numWords);
                };
                Invoke(del);
            }
            else
            {
                mWordsEvaluatedLbl.Text = "Words Evaluated: " + numWords.ToString("#,##0");
            }
        }

        public void SetNumSolutionsFound(int numSolutions)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetNumSolutionsFound(numSolutions);
                };
                Invoke(del);
            }
            else
            {
                mSolutionCountLbl.Text = "Number of Solutions: " + numSolutions.ToString("#,##0");
            }
        }

        public void SetHighestScoreFound(int maxScore)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetHighestScoreFound(maxScore);
                };
                Invoke(del);
            }
            else
            {
                mHighestScoreLbl.Text = "Highest Score: " + maxScore.ToString("#,##0");
            }
        }

        #endregion
    }

    class SpaceInfo
    {
        private Point mLocation;
        private bool mIsSolutionTile;

        public SpaceInfo(int x, int y)
        {
            mLocation = new Point(x, y);
            mIsSolutionTile = false;
        }

        public Point Location
        {
            get { return mLocation; }
        }

        public bool IsSolutionTile
        {
            get { return mIsSolutionTile; }
            set { mIsSolutionTile = value; }
        }
    }
}
