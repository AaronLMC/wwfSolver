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

        private const bool _useDemoInput = true;

        public MainForm()
        {
            InitializeComponent();
            mAvailableLettersTxt.MaxLength = GameVals.AVAILABLE_LETTER_MAX;
            mAvailableLettersTxt.TextChanged += new EventHandler(OnGameBoardTextChanged);

            mWordDict = new WordDict("res/wwfDict.txt");

            SetupGameBoard();
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
            if (_useDemoInput)
            {
                mAvailableLettersTxt.Text = _demoAvailableLetters;
            }

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
                    textBox.Tag = new int[] { i, j };

                    if (i == GameVals.BOARD_CENTER_LOC
                        && j == GameVals.BOARD_CENTER_LOC)
                    {
                        textBox.BackColor = Color.LightBlue;
                    }

                    mGameTextBoxes[i,j] = textBox;
                    rowPanel.Controls.Add(textBox);

                    if (_useDemoInput)
                    {
                        textBox.Text = _demoGameBoard[i, j];
                    }

                    rowPanel.Height = textBox.Height + 3;
                }

                mGameBoardLayout.Controls.Add(rowPanel);
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            
                if (sender is TextBox
                    && ((TextBox)sender).Tag is int[])
                {
                    if (e.KeyCode == Keys.Up
                    || e.KeyCode == Keys.Down
                    || e.KeyCode == Keys.Left
                    || e.KeyCode == Keys.Right)
                    {
                        TextBox txtBox = sender as TextBox;
                        int[] location = txtBox.Tag as int[];
                        if (location.Length != 2)
                        {
                            Trace.Fail("Textbox did not have tag coordinates");
                            return;
                        }
                        int x = location[0];
                        int y = location[1];

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

            foreach (WordSolution solution in solutions)
            {
                Console.Out.WriteLine("Solution: " + solution);
            }

            SetGoBtnEnabled(true);
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

        private String _demoAvailableLetters = "";
        private String[,] _demoGameBoard = {
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        };

        //private String _demoAvailableLetters = "DGYODDT";
        //private String[,] _demoGameBoard = {
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "T"},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "S", "I"},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", "N", " ", " ", " ", "U", "P"},
        //    {" ", " ", " ", " ", " ", " ", " ", "B", "R", "U", "I", "T", " ", "R", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", "O", " ", "M", " ", " ", " ", "E", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", "N", " ", "B", "R", "A", "W", "L", "S"},
        //    {" ", " ", " ", " ", "L", " ", "H", "E", "H", "S", " ", " ", " ", "Y", "O"},
        //    {" ", " ", " ", "Z", "O", "O", "I", "D", " ", " ", " ", " ", "N", " ", "C"},
        //    {" ", " ", " ", " ", "P", " ", "D", " ", " ", "R", " ", "L", "I", "N", "K"},
        //    {" ", " ", " ", " ", " ", "V", "E", "G", " ", "E", " ", "E", "X", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", "I", "F", "S", " ", "V", " ", " ", " "},
        //};

        //private String _demoAvailableLetters = "II";
        //private String[,] _demoGameBoard = {
        //    {" ", " ", " ", " ", " ", " ", " ", "H", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", "O", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", "E", " ", " ", " ", " ", " ", " ", " "},
        //    {"S", " ", " ", "C", " ", "S", "A", "D", " ", " ", " ", "N", " ", " ", "V"},
        //    {"H", " ", "W", "H", "E", "T", " ", " ", " ", " ", " ", "O", " ", "H", "E"},
        //    {"Y", "O", " ", "A", " ", "U", " ", " ", " ", "C", "L", "O", "V", "E", "R"},
        //    {" ", "U", " ", "R", " ", "B", "O", "B", " ", "U", " ", "D", " ", "A", "M"},
        //    {" ", "T", " ", "K", " ", " ", "F", "E", "E", "D", " ", "L", " ", "T", "I"},
        //    {"J", "A", "R", "S", " ", " ", " ", "N", " ", " ", " ", "E", " ", "I", "N"},
        //    {" ", "G", " ", " ", " ", " ", "G", "E", "E", "Z", " ", " ", " ", "N", " "},
        //    {"Y", "E", "T", "I", " ", " ", " ", "F", "L", "A", "W", " ", "A", "G", "O"},
        //    {"E", "S", " ", "D", "O", " ", "L", "I", "D", " ", " ", " ", "N", " ", "P"},
        //    {"T", " ", " ", "I", " ", " ", " ", "T", " ", " ", "Q", "A", "T", " ", "E"},
        //    {" ", " ", " ", "O", " ", " ", " ", "S", "A", "X", " ", " ", " ", " ", "R"},
        //    {" ", " ", " ", "M", "I", "R", "E", " ", " ", "U", "P", "O", "N", " ", "A"},
        //};

        //private String _demoAvailableLetters = "THDACQR";
        //private String[,] _demoGameBoard = {
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", "E", "S", "T", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "C", " "},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Z", "O", "O", "N"},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "I", " ", "O", " "},
        //    {" ", " ", " ", " ", " ", " ", "D", "A", "U", "B", " ", "P", "A", "L", "L"},
        //    {" ", " ", " ", " ", " ", " ", " ", " ", " ", "E", "M", " ", " ", " ", "A"},
        //    {" ", " ", " ", " ", " ", " ", " ", "S", "N", "E", "E", "R", "S", " ", "R"},
        //    {" ", " ", " ", " ", " ", " ", " ", "W", "E", "R", "E", " ", "I", "N", "K"},
        //    {" ", " ", " ", " ", " ", " ", " ", "E", "T", " ", "D", " ", "T", "O", " "},
        //    {" ", " ", " ", " ", " ", " ", "V", "A", "S", "E", "S", " ", "H", "E", "R"},
        //    {" ", "C", " ", " ", " ", "B", " ", "T", " ", "V", " ", " ", " ", "L", "I"},
        //    {" ", "H", " ", "T", "W", "I", "G", "S", " ", "E", "Y", "E", " ", " ", "D"},
        //    {" ", "I", " ", "O", " ", "G", " ", " ", " ", "N", " ", " ", "F", "I", "E"},
        //    {" ", "P", "R", "Y", " ", " ", " ", " ", " ", "T", "U", "X", " ", " ", " "}
        //};

        #region FrontEndInterface Members

        public void SetSearchStartLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.Red);
            
        }

        public void SetSearchRecurLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.Yellow);
        }

        public void ClearSearchLocation(int x, int y)
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
            throw new NotImplementedException();
        }

        public void ClearWordSolution()
        {
            throw new NotImplementedException();
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
        }
    }   
}
