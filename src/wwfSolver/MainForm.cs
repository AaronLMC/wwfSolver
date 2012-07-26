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

            GameSolver solver = new GameSolver(mWordDict, boardLetters, availableLetters);
            List<WordSolution> solutions = solver.GetSolutions();

            foreach (WordSolution solution in solutions)
            {
                Console.Out.WriteLine("Solution: " + solution);
            }

            int das = 5;
            das++;
        }

        private String _demoAvailableLetters4 = "T";
        private String[,] _demoGameBoard4 = {
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", "E", "S", "T", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
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

        private String _demoAvailableLetters = "LHDAAHR";
        private String[,] _demoGameBoard = {
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", "E", "S", "T", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "C", " "},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "Z", "O", "O", "N"},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", "I", " ", "O", " "},
            {" ", " ", " ", " ", " ", " ", "D", "A", "U", "B", " ", "P", "A", "L", "L"},
            {" ", " ", " ", " ", " ", " ", " ", " ", " ", "E", "M", " ", " ", " ", "A"},
            {" ", " ", " ", " ", " ", " ", " ", "S", "N", "E", "E", "R", "S", " ", "R"},
            {" ", " ", " ", " ", " ", " ", " ", "W", "E", "R", "E", " ", "I", "N", "K"},
            {" ", " ", " ", " ", " ", " ", " ", "E", "T", " ", "D", " ", "T", "O", " "},
            {" ", " ", " ", " ", " ", " ", "V", "A", "S", "E", "S", " ", "H", "E", "R"},
            {" ", "C", " ", " ", " ", "B", " ", "T", " ", "V", " ", " ", " ", "L", "I"},
            {" ", "H", " ", "T", "W", "I", "G", "S", " ", "E", "Y", "E", " ", " ", "D"},
            {" ", "I", " ", "O", " ", "G", " ", " ", " ", "N", " ", " ", "F", "I", "E"},
            {" ", "P", "R", "Y", " ", " ", " ", " ", " ", "T", "U", "X", " ", " ", " "}
        };
    }   
}
