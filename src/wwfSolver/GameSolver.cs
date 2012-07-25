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
        

        private WordDict mWordDict;
        private char[,] mBoardLetters;
        private char[] mAvailableLetters;
        private List<int> mUsedLetterIdxs = new List<int>();
        private List<LetterLoc> mCurrentWord = new List<LetterLoc>();

        public GameSolver(WordDict wordDict, char[,] boardLetters, char[] availableLetters)
        {
            mWordDict = wordDict;
            mBoardLetters = boardLetters;
            mAvailableLetters = availableLetters;
        }

        public List<WordSolution> GetSolutions()
        {
            List<WordSolution> solutions = new List<WordSolution>();

            for (int i = 0; i < GameVals.BOARD_SIZE; i++)
            {
                for (int j = 0; j < GameVals.BOARD_SIZE; j++)
                {
                    if (LocationContainsLetter(i, j))
                    {
                        continue;
                    }

                    //evaluate legality of placing a letter here
                    mBoardLetters[i, j] = 'X';
                    bool legalMove = IsLegalMove(i, j);
                    mBoardLetters[i, j] = ' ';
                    if (!legalMove)
                    {
                        continue;
                    }

                    for (int letterIdx = 0; letterIdx < mAvailableLetters.Length; letterIdx++)
                    {
                        if (mUsedLetterIdxs.Contains(letterIdx))
                        {
                            continue;
                        }

                        //we have a letter and a location to try out
                        char letter = mAvailableLetters.ElementAt(letterIdx);
                        mBoardLetters[i, j] = letter;

                        LetterLoc curLetterLoc = new LetterLoc(letter, i, j);
                        mCurrentWord.Add(curLetterLoc);
                        mUsedLetterIdxs.Add(letterIdx);

                        //get score of current turn
                        List<WordLocation> wordList = GetWordList();
                        List<WordLocation> illegalWords;
                        int score = GetWordScore(wordList, out illegalWords);
                        if (score > 0)
                        {
                            //store this move as one that has scores
                            LetterLoc[] letterArr = new LetterLoc[mCurrentWord.Count];
                            mCurrentWord.CopyTo(letterArr);
                            WordSolution solution = new WordSolution(letterArr, legalWords, score);
                            solutions.Add(solution);
                        }

                        //see if we should continue down this path
                        bool shouldContinue;
                        if (score < 0)
                        {
                                
                        }
                    }

                }
            }

            return solutions;
        }

        //public List<WordSolution> GetSolutionsSimple()
        //{
        //    List<WordSolution> solutions = new List<WordSolution>();
        //    for (int i = 0; i < GameVals.BOARD_SIZE; i++)
        //    {
        //        for (int j = 0; j < GameVals.BOARD_SIZE; j++)
        //        {
        //            if (LocationContainsLetter(i, j))
        //            {
        //                continue;
        //            }

        //            for (int letterIdx = 0; letterIdx < GameVals.AVAILABLE_LETTER_MAX; letterIdx++)
        //            {
        //                if (mUsedLetterIdxs.Contains(letterIdx))
        //                {
        //                    continue;
        //                }

        //                //we have a letter and a location to try out
        //                char letter = mAvailableLetters.ElementAt(letterIdx);
        //                mBoardLetters[i, j] = letter;
                        
        //                //evaluate legality of move
        //                if (!IsLegalMove(i, j))
        //                {
        //                    mBoardLetters[i, j] = ' ';
        //                    continue;
        //                }

        //                LetterLoc curLetterLoc = new LetterLoc(letter, i, j);
                        
        //                mCurrentWord.Add(curLetterLoc);                        
        //                mUsedLetterIdxs.Add(letterIdx);

        //                string illegalWord;
        //                List<WordLocation> wordList = GetWordList();
        //                int score = GetWordScore(wordList, out illegalWords);
        //                if (score < 0)
        //                {
        //                    Console.Out.WriteLine("Illegal word: " + illegalWord);

        //                    //remove letter from board
        //                    mBoardLetters[i, j] = ' ';
        //                    mUsedLetterIdxs.Remove(letterIdx);
        //                    mCurrentWord.Remove(curLetterLoc);
        //                }
        //                else
        //                {
        //                    LetterLoc[] letterArr = new LetterLoc[mCurrentWord.Count];
        //                    mCurrentWord.CopyTo(letterArr);

        //                    WordSolution solution = new WordSolution(letterArr, legalWords, score);
        //                    solutions.Add(solution);
        //                }
        //             }
        //        }
        //    }

        //    return solutions;
        //}

        private int GetWordScore(List<WordLocation> wordList, out List<WordLocation> illegalWords)
        {
            illegalWord = new List<WordLocation>();
            int score = 0;

            //first check for illegal words
            foreach (WordLocation word in legalWords)
            {                
                if (!mWordDict.IsWordInList(word.WordText))
                {
                    illegalWords.Add(word);
                }
            }
            if (illegalWords.Count > 0)
            {
                return -1;
            }

            foreach (WordLocation word in wordList)
            {
                //score the word
                int wordScore = 0;
                int multipliers = 1;
                foreach (LetterLoc letter in word.Letters)
                {
                    int letterScore = GameVals.LETTER_SCORE[letter.Letter];
                    GameVals.Bonus letterBonus = GameVals.BONUS_TILES[letter.X, letter.Y];
                    switch (letterBonus)
                    {
                        case GameVals.Bonus.D_LT:
                            letterScore *= 2;
                            break;
                        case GameVals.Bonus.T_LT:
                            letterScore *= 3;
                            break;
                        case GameVals.Bonus.D_WD:
                            multipliers *= 2;
                            break;
                        case GameVals.Bonus.T_WD:
                            multipliers *= 3;
                            break;
                    }

                    wordScore += letterScore;
                }
                wordScore *= multipliers;

                score += wordScore;
            }

            return score;
        }

        enum WordOrientation
        {
            NONE,
            VERTICAL,
            HORIZONTAL
        }

        private WordSet GetWordList()
        {
            WordSet wordSet = new WordSet();

            //determine orientation of played tiles
            bool vertical;
            int lastWordIdx = mCurrentWord.Count - 1;
            int startX = mCurrentWord[0].X;
            int startY = mCurrentWord[0].Y;
            int endX = mCurrentWord[lastWordIdx].X;
            int endY = mCurrentWord[lastWordIdx].Y;

            //for each letter in the proposed word, add the score of any new words generated by that letter
            foreach (LetterLoc letter in mCurrentWord)
            {
                //evaluate in X direction
                int initX = letter.X;
                int minX = initX;
                for (int i = initX; i >= 0; i--)
                {
                    if (mBoardLetters[i, letter.Y] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        minX = i;
                    }
                }

                int maxX = initX;
                for (int i = initX; i < GameVals.BOARD_SIZE; i++)
                {
                    if (mBoardLetters[i, letter.Y] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        maxX = i;
                    }
                }

                int wordLenX = maxX - minX + 1;
                if (wordLenX > 1)
                {
                    //get word
                    string word = "";
                    for (int i = minX; i <= maxX; i++)
                    {
                        word += mBoardLetters[i, letter.Y];
                    }

                    List<LetterLoc> letters = new List<LetterLoc>();
                    for (int i = minX; i <= maxX; i++)
                    {
                        letters.Add(new LetterLoc(mBoardLetters[i, letter.Y], i, letter.Y));
                    }
                    wordList.Add(new WordLocation(letters));
                }

                //evaluate in Y direction
                int initY = letter.Y;
                int minY = initY;
                for (int j = initY; j >= 0; j--)
                {
                    if (mBoardLetters[letter.X, j] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        minY = j;
                    }
                }

                int maxY = initY;
                for (int j = initY; j < GameVals.BOARD_SIZE; j++)
                {
                    if (mBoardLetters[letter.X, j] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        maxY = j;
                    }
                }

                int wordLenY = maxY - minY + 1;
                if (wordLenY > 1)
                {
                    //get word
                    List<LetterLoc> letters = new List<LetterLoc>();
                    for (int j = minY; j <= maxY; j++)
                    {
                        letters.Add(new LetterLoc(mBoardLetters[letter.X, j], letter.X, j));
                    }
                    wordList.Add(new WordLocation(letters));
                }
            }

            return wordSet;
        }

        private bool IsLegalMove(int x, int y)
        {
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
            else
            {
                //this is the first move; make sure it is adjacent to a cell
                if (x > 0 && mBoardLetters[x - 1, y] != ' ')
                    return true;
                if (x < GameVals.BOARD_SIZE - 1 && mBoardLetters[x + 1, y] != ' ')
                    return true;

                if (y > 0 && mBoardLetters[x, y - 1] != ' ')
                    return true;
                if (y < GameVals.BOARD_SIZE - 1 && mBoardLetters[x, y + 1] != ' ')
                    return true;

                return false; //not adjacent to any tiles already on board
            }

            return true;
        }


        private bool LocationContainsLetter(int x, int y)
        {
            //look at board location
            if (mBoardLetters[x, y] != ' ')
            {
                return true;
            }

            return false;
        }
    }

    [DebuggerDisplay("{ToString()}")]
    public class WordSolution : IComparable
    {
        private LetterLoc[] mLetters;
        private List<WordLocation> mLegalWords;
        private int mScore;

        public WordSolution(LetterLoc[] letterList, List<WordLocation> legalWords, int score)
        {
            mLetters = letterList;
            mLegalWords = legalWords;
            mScore = score;
        }

        public LetterLoc[] Letters
        {
            get { return mLetters; }
        }

        public List<WordLocation> LegalWords
        {
            get { return mLegalWords; }
        }

        public int Score
        {
            get { return mScore; }
        }

        public override string ToString()
        {
            string wordList = "";
            for (int i = 0; i < mLegalWords.Count; i++)
            {
                wordList += mLegalWords[i];

                if (i < mLegalWords.Count - 1)
                {
                    wordList += ", ";
                }
            }

            return string.Format("Score = {0}, Words = {1}", mScore, wordList);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is WordSolution)
            {
                WordSolution objWordSolution = obj as WordSolution;

                if (objWordSolution.mScore < mScore)
                    return -1;
                else if (objWordSolution.mScore == mScore)
                    return 0;
                else
                    return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }

    public class WordSet
    {
        private WordLocation mPrimaryWord;
        private List<WordLocation> mIncidentalWords;

        public WordSet()
        {
            mIncidentalWords = new List<WordLocation>();
        }

        public WordLocation PrimaryWord
        {
            get { return mPrimaryWord; }
            set { mPrimaryWord = value; }
        }

        public List<WordLocation> IncidentalWords
        {
            get { return mIncidentalWords; }
        }

        public void AddIncidentalWord(WordLocation word)
        {
            mIncidentalWords.Add(word);
        }
    }

    public class WordLocation
    {
        private LetterLoc[] mLetters;
        private string mWordText;

        public WordLocation(List<LetterLoc> letters)
        {
            mLetters = new LetterLoc[letters.Count];
            letters.CopyTo(mLetters);

            mWordText = "";
            foreach (LetterLoc letter in mLetters)
            {
                 mWordText += letter.Letter;
            }
        }

        public string WordText
        {
            get { return mWordText;}
        }

        public LetterLoc[] Letters
        {
            get {return mLetters; }
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
