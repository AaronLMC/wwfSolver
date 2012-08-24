using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace wwfSolver
{
    public class WordDict
    {
        private HashSet<string> mLiveWords = new HashSet<string>();
        private HashSet<string> mDeadWords = new HashSet<string>();
     
        //{word length, word list}
        private Dictionary<int,HashSet<string>> mWordList = new Dictionary<int,HashSet<string>>();
        private int mMaxWordLength = 0;


        public WordDict(string dictFile)
        {
            try
            {
                StreamReader sr = new StreamReader(dictFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string word = line.Trim().ToUpper();
                    int length = word.Length;
                    if (length == 0)
                    {
                        //ignore empty words
                        continue;
                    }

                    HashSet<string> set;
                    if (!mWordList.ContainsKey(length))
                    {
                        set = new HashSet<string>();
                        mWordList.Add(length, set);
                    }
                    else
                    {
                        set = mWordList[length];
                    }

                    if (set.Contains(word))
                    {
                        Trace.Fail("Duplicate word in dictionary: " + word);
                    }

                    set.Add(word);

                    if (length > mMaxWordLength)
                    {
                        mMaxWordLength = length;
                    }
                }
            }
            catch (Exception e)
            {
                Trace.Fail("Error opening dictionary file", e.Message + "\n" + e.StackTrace);
            }
        }

        public bool IsWordInList(string word)
        {
            if (!mWordList.ContainsKey(word.Length))
            {
                return false;
            }

            return mWordList[word.Length].Contains(word);
        }

        /// <summary>
        /// Returns true if given word cannot be built into a longer word in the dictionary
        /// </summary>
        public bool IsDeadWord(string testWord)
        {
            HashSet<string> candidates = new HashSet<string>();

            if (mDeadWords.Contains(testWord))
            {
                return true;
            }
            else if (mLiveWords.Contains(testWord))
            {
                return false;
            }

            for (int i = testWord.Length + 1; i <= mMaxWordLength; i++)
            {
                if (mWordList.ContainsKey(i))
                {
                    HashSet<string> list = mWordList[i];
                    foreach (string word in list)
                    {
                        if (word.Contains(testWord))
                        {
                            mLiveWords.Add(testWord);
                            return false;
                        }
                    }
                }
            }

            //Keep record of words that have dead ends
            mDeadWords.Add(testWord);
            return true;
        }

        public int GetNumWordsEvaluated()
        {
            return mLiveWords.Count + mDeadWords.Count;
        }
    }
}
