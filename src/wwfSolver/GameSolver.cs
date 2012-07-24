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

        private WordDict mWordDict;
        private char[,] mBoardLetters;
        private char[] mAvailableLetters;
        private List<int> mUsedLetterIdxs = new List<int>();
        private List<LetterLoc> mCurrentWord = new List<LetterLoc>();

        private readonly Dictionary<char, int> LETTER_VALS = new Dictionary<char, int>()
        {
            {'A', 1}
        };

        public GameSolver(WordDict wordDict, char[,] boardLetters, char[] availableLetters)
        {
            mWordDict = wordDict;
            mBoardLetters = boardLetters;
            mAvailableLetters = availableLetters;
        }

        public List<WordSolution> GetSolutions()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if (!IsLegalLocation(i, j))
                    {
                        continue;
                    }

                    for (int letterIdx = 0; letterIdx < 7; letterIdx++)
                    {
                        if (mUsedLetterIdxs.Contains(letterIdx))
                        {
                            continue;
                        }

                        //we have a letter and a location to try out
                        char letter = mAvailableLetters.ElementAt(letterIdx);
                        mUsedLetterIdxs.Add(letterIdx);

                        mCurrentWord.Add(new LetterLoc(letter, i, j));
                    }
                }
            }

            return null;
        }

        private int GetWordScore()
        {
            return 0;
        }

        private bool IsLegalLocation(int x, int y)
        {
            if (LocationContainsLetter(x, y))
            {
                return false;
            }

            //look at whether location is in-line with current word
            if (mCurrentWord.Count > 0)
            {
                int initX = mCurrentWord[0].X;
                int initY = mCurrentWord[0].Y;

                //location is not in line with existing current word
                if (x != initX || y != initY)
                {
                    return false;
                }

                if (x == initX)
                {
                    //make sure there is a letter between InitX and X
                    int smallerX = Math.Min(x, initX);
                    int largerX = Math.Max(x, initX);
                    for (int i = smallerX + 1; i < largerX; i++)
                    {
                        if (!LocationContainsLetter(i, y))
                        {
                            return false;
                        }
                    }
                }
                else if (y == initY)
                {
                    //make sure there is a letter between InitY and Y
                    int smallerY = Math.Min(y, initY);
                    int largerY = Math.Max(y, initY);
                    for (int j = smallerY + 1; j < largerY; j++)
                    {
                        if (!LocationContainsLetter(x, j))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        private bool LocationContainsLetter(int x, int y)
        {
            //look at board location
            if (mBoardLetters[x, y] == ' ')
            {
                return true;
            }

            //look in current word
            foreach (LetterLoc letter in mCurrentWord)
            {
                if (letter.X == x && letter.Y == y)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class WordSolution
    {
        private List<LetterLoc> mLetters = new List<LetterLoc>();

        public WordSolution()
        {
        }

        public void AddLetter(LetterLoc letter)
        {
            mLetters.Add(letter);
        }

        public List<LetterLoc> Word
        {
            get { return mLetters; }
        }
    }

    public struct LetterLoc
    {
        public readonly char Letter;
        public readonly int X;
        public readonly int Y;

        public LetterLoc(char letter, int x, int y)
        {
            Letter = letter;
            X = x;
            Y = y;
        }
    }
}
