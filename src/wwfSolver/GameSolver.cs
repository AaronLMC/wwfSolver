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

                    //evaluate legality of placing an initial letter here
                    if (!IsLegalMove(i, j))
                    {
                        continue;
                    }

                    List<WordSolution> words = SolutionSearch(i, j);
                    solutions.AddRange(words);

                    foreach (WordSolution w in words)
                    {
                        Console.Out.WriteLine("Found: " + w);
                    }
                }
            }

            return solutions;
        }

        private List<WordSolution> SolutionSearch(int x, int y)
        {
            Trace.Assert(IsLegalMove(x, y), "Given board coordinates are not a legal move");

            List<WordSolution> solutions = new List<WordSolution>();
            
            //cycle through all starting idx so that we exercise all letter combinations
            for (int firstIdx = 0; firstIdx < mAvailableLetters.Length; firstIdx++)
            {
                int letterIdx = firstIdx - 1;
                for (int evaluatedLetters = 0; evaluatedLetters < mAvailableLetters.Length; evaluatedLetters++)
                {
                    letterIdx++;
                    if (letterIdx == mAvailableLetters.Length)
                    {
                        letterIdx = 0;
                    }

                    if (mUsedLetterIdxs.Contains(letterIdx))
                    {
                        continue;
                    }

                    //we have a letter and a location to try out
                    char letter = mAvailableLetters.ElementAt(letterIdx);
                    mBoardLetters[x, y] = letter;

                    LetterLoc curLetterLoc = new LetterLoc(letter, x, y);
                    mCurrentWord.Add(curLetterLoc);
                    mUsedLetterIdxs.Add(letterIdx);

                    //get score of current turn
                    WordSet wordSet = GetWordList();
                    List<WordLocation> wordList = wordSet.GetFullList();
                    List<WordLocation> illegalWords;
                    int score = GetWordScore(wordList, out illegalWords);
                    if (score > 0)
                    {
                        //store this move as one that has scores
                        LetterLoc[] letterArr = new LetterLoc[mCurrentWord.Count];
                        mCurrentWord.CopyTo(letterArr);
                        WordSolution solution = new WordSolution(letterArr, wordList, score);
                        solutions.Add(solution);
                    }

                    //see if we should continue down this path
                    if (score < 0
                        && wordSet.PrimaryWord != null
                        && !illegalWords.Contains(wordSet.PrimaryWord))
                    {
                        //an incidental word is illegal.  Adding new tiles won't help with this so we are done
                        continue;
                    }
                    else if (wordSet.PrimaryWord != null
                        && mWordDict.GetWordsThatContain(wordSet.PrimaryWord.WordText).Count == 0)
                    {
                        //there are no words that contain our primary word, so stop
                        continue;
                    }
                    //else if (false)
                    //{
                    //    //TODO: evaluate possible words for available tiles and board pieces
                    //}


                    //Look for next word
                    if (wordSet.Orientation == WordOrientation.SINGLE_TILE)
                    {
                        //go in all 4 directions

                        int y_next = NextTileUp(x, y);
                        if (y_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x, y_next);
                            solutions.AddRange(words);
                        }

                        y_next = NextTileDown(x, y);
                        if (y_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x, y_next);
                            solutions.AddRange(words);
                        }

                        int x_next = NextTileLeft(x, y);
                        if (x_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x_next, y);
                            solutions.AddRange(words);
                        }

                        x_next = NextTileRight(x, y);
                        if (x_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x_next, y);
                            solutions.AddRange(words);
                        }
                    }
                    else if (wordSet.Orientation == WordOrientation.HORIZONTAL)
                    {
                        //go top and bottom
                        int y_next = NextTileUp(x, y);
                        if (y_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x, y_next);
                            solutions.AddRange(words);
                        }

                        y_next = NextTileDown(x, y);
                        if (y_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x, y_next);
                            solutions.AddRange(words);
                        }
                    }
                    else
                    {
                        //go left and right
                        int x_next = NextTileLeft(x, y);
                        if (x_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x_next, y);
                            solutions.AddRange(words);
                        }

                        x_next = NextTileRight(x, y);
                        if (x_next >= 0)
                        {
                            List<WordSolution> words = SolutionSearch(x_next, y);
                            solutions.AddRange(words);
                        }
                    }

                    //clear the location we are looking at
                    mBoardLetters[x, y] = ' ';
                    mUsedLetterIdxs.RemoveAt(mUsedLetterIdxs.Count - 1);
                    mCurrentWord.Remove(curLetterLoc);
                }

            }

            

            return solutions;
        }

        private int NextTileUp(int x, int y)
        {
            int y_next = -1;
            for (int j = y; j >= 0; j--)
            {
                if (IsLegalMove(x, j))
                {
                    y_next = j;
                    break;
                }
            }
            return y_next;
        }

        private int NextTileDown(int x, int y)
        {
            int y_next = -1;
            for (int j = y; j < GameVals.BOARD_SIZE; j++)
            {
                if (IsLegalMove(x, j))
                {
                    y_next = j;
                    break;
                }
            }
            return y_next;
        }

        private int NextTileLeft(int x, int y)
        {
            int x_next = -1;
            for (int i = x; i >= 0; i--)
            {
                if (IsLegalMove(i, y))
                {
                    x_next = i;
                    break;
                }
            }
            return x_next;
        }

        private int NextTileRight(int x, int y)
        {
            int x_next = -1;
            for (int i = x; i < GameVals.BOARD_SIZE; i++)
            {
                if (IsLegalMove(i, y))
                {
                    x_next = i;
                    break;
                }
            }
            return x_next;
        }

        private int GetWordScore(List<WordLocation> wordList, out List<WordLocation> illegalWords)
        {
            illegalWords = new List<WordLocation>();
            int score = 0;

            //first check for illegal words
            foreach (WordLocation word in wordList)
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

        private WordSet GetWordList()
        { 
            //determine orientation of played tiles
            WordOrientation orientation;
            int lastWordIdx = mCurrentWord.Count - 1;
            int startX = mCurrentWord[0].X;
            int startY = mCurrentWord[0].Y;
            int endX = mCurrentWord[lastWordIdx].X;
            int endY = mCurrentWord[lastWordIdx].Y;

            if (mCurrentWord.Count == 1)
                orientation = WordOrientation.SINGLE_TILE;
            else if (startX != endX)
                orientation = WordOrientation.VERTICAL;
            else
                orientation = WordOrientation.HORIZONTAL;
            
            //Initialize word set
            WordSet wordSet = new WordSet(orientation);

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

                    if (orientation == WordOrientation.VERTICAL)
                        wordSet.PrimaryWord = new WordLocation(letters);
                    else
                        wordSet.AddIncidentalWord(new WordLocation(letters));
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

                    if (orientation == WordOrientation.HORIZONTAL)
                        wordSet.PrimaryWord = new WordLocation(letters);
                    else
                        wordSet.AddIncidentalWord(new WordLocation(letters));
                }
            }

            return wordSet;
        }

        private bool IsLegalMove(int x, int y)
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
                if (x != initX && y != initY)
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

    public enum WordOrientation
    {
        SINGLE_TILE,
        VERTICAL,
        HORIZONTAL
    }

    public class WordSet
    {
        private WordLocation mPrimaryWord = null;
        private List<WordLocation> mIncidentalWords;
        private WordOrientation mOrientation;

        public WordSet(WordOrientation orientation)
        {
            mIncidentalWords = new List<WordLocation>();
        }

        public WordLocation PrimaryWord
        {
            get { return mPrimaryWord; }
            set { mPrimaryWord = value; }
        }

        public WordOrientation Orientation
        {
            get { return mOrientation; }
        }

        public List<WordLocation> IncidentalWords
        {
            get { return mIncidentalWords; }
        }

        public void AddIncidentalWord(WordLocation word)
        {
            mIncidentalWords.Add(word);
        }

        public List<WordLocation> GetFullList()
        {
            List<WordLocation> fullSet = new List<WordLocation>();

            if (mPrimaryWord != null)
            {
                fullSet.Add(mPrimaryWord);
            }
            fullSet.AddRange(mIncidentalWords);
            return fullSet;
        }
    }

    [DebuggerDisplay("{ToString()}")]
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

        public override string ToString()
        {
            return mWordText;
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
