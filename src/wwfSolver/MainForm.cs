using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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
                    boardLetters[i, j] = mGameTextBoxes[i, j].Text[0];
                }
            }

            char[] availableLetters = mAvailableLettersTxt.Text.ToCharArray();

            GameSolver solver = new GameSolver(this, mWordDict, boardLetters, availableLetters);
            List<WordSolution> solutions = solver.GetSolutions();
            solutions.Sort();

            foreach (WordSolution solution in solutions)
            {
                Console.Out.WriteLine("Solution: " + solution);
            }

            int das = 5;
            das++;
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
            Update();
        }

        public void SetSearchRecurLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.Yellow);
            Update();
        }

        public void ClearSearchLocation(int x, int y)
        {
            SetLocationColor(x, y, Color.White);
            Update();
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
    }   
}
