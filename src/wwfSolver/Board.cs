using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace wwfSolver
{
    [Serializable()]
    public class Board 
    {
        private char[,] mBoardLetters;
        private char[] mAvailableLetters;

        public static Board Load(string filename)
        {
            Stream stream = File.OpenRead(filename);
            BinaryFormatter deserializer = new BinaryFormatter();
            Board b = (Board)deserializer.Deserialize(stream);
            stream.Close();

            return b;
        }

        public Board(char[,] boardLetters, char[] availableLetters)
        {
            mBoardLetters = boardLetters;
            mAvailableLetters = availableLetters;
        }

        public void SaveToFile(string filename)
        {
            Stream stream = File.Create(filename);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, this);
            stream.Close();
        }

        public char[,] BoardLetters
        {
            get { return mBoardLetters; }
        }

        public char[] AvailableLetters
        {
            get { return mAvailableLetters; }
        }
    }
}
