using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace wwfSolver
{
    public class GameSolver
    {
        public const int BOARD_SIZE = 15;

        private String[,] mBoardLetters = new String[BOARD_SIZE, BOARD_SIZE];
        public GameSolver(String[,] boardLetters, String availableLetters)
        {
            mBoardLetters = boardLetters;
        }
    }
}
