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
    public partial class MainForm : Form
    {
        private TextBox[,] mGameTextBoxes = new TextBox[GameSolver.BOARD_SIZE, GameSolver.BOARD_SIZE];
        private WordDict mWordDict;

        private const bool _useDemoInput = true;

        public MainForm()
        {
            InitializeComponent();
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

            for (int i = 0; i < GameSolver.BOARD_SIZE; i++)
            {
                FlowLayoutPanel rowPanel = new FlowLayoutPanel();
                rowPanel.Width = mGameBoardLayout.Width;
                
                for (int j = 0; j < GameSolver.BOARD_SIZE; j++)
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
            char[,] boardLetters = new char[GameSolver.BOARD_SIZE, GameSolver.BOARD_SIZE];
            
            for (int i = 0; i < GameSolver.BOARD_SIZE; i++)
            {
                for (int j = 0; j < GameSolver.BOARD_SIZE; j++)
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

            GameSolver solver = new GameSolver(mWordDict, boardLetters, availableLetters);
        }

        private String _demoAvailableLetters = "ILAUAEI";
        private String[,] _demoGameBoard = {
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "C", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Z", "O", "O", "N"},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "I", " ", "O", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", "B", " ", "P", "A", "L", "L"},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", "E", "M", " ", " ", " ", "A"},
            {" ", " ", " ", " ", " ", " ", " ", "S", "N", "E", "E", "R", "S", " ", "R"},
            {" ", " ", " ", " ", " ", " ", " ", "W", "E", "R", "E", " ", "I", "N", "K"},
            {" ", " ", " ", " ", " ", " ", " ", "E", "T", " ", "D", " ", "T", "O", " "},
            {" ", " ", " ", " ", " ", " ", "V", "A", "S", "E", " ", " ", "H", "E", "R"},
            {" ", " ", " ", " ", " ", "B", " ", "T", " ", "V", " ", " ", " ", "L", "I"},
            {" ", " ", " ", "T", "W", "I", "G", "S", " ", "E", "Y", "E", " ", " ", "D"},
            {" ", " ", " ", "O", " ", "G", " ", " ", " ", "N", " ", " ", "F", "I", "E"},
            {" ", " ", " ", "Y", " ", " ", " ", " ", " ", "T", "U", "X", " ", " ", " "}
        };
    }   
}
