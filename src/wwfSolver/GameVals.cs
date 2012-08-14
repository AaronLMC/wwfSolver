using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wwfSolver
{
    public class GameVals
    {
        public const int BOARD_SIZE = 15;
        public const int AVAILABLE_LETTER_MAX = 7;
        public const int BOARD_CENTER_LOC = 7;
        public const int BONUS_USED_ALL_TILES = 35;

        public static readonly Dictionary<char, int> LETTER_SCORE = new Dictionary<char, int>()
        {
            {'A', 1},
            {'B', 4},
            {'C', 4},
            {'D', 2},
            {'E', 1},
            {'F', 4},
            {'G', 3},
            {'H', 3},
            {'I', 1},
            {'J', 10},
            {'K', 5},
            {'L', 2},
            {'M', 4},
            {'N', 2},
            {'O', 1},
            {'P', 4},
            {'Q', 10},
            {'R', 1},
            {'S', 1},
            {'T', 1},
            {'U', 2},
            {'V', 5},
            {'W', 4},
            {'X', 8},
            {'Y', 3},
            {'Z', 10}
        };

        public enum Bonus
        {
            NONE,
            D_LT,
            T_LT,
            D_WD,
            T_WD
        }

        public static readonly Bonus[,] BONUS_TILES = new Bonus[,]
        {
            {Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE},
            {Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE},
            {Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE},
            {Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.T_WD},
            {Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE},
            {Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE},
            {Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT},
            {Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE},
            {Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT},
            {Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE},
            {Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE},
            {Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.T_WD},
            {Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE},
            {Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.D_WD, Bonus.NONE, Bonus.NONE, Bonus.D_LT, Bonus.NONE, Bonus.NONE},
            {Bonus.NONE, Bonus.NONE, Bonus.NONE, Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.T_LT, Bonus.NONE, Bonus.NONE, Bonus.T_WD, Bonus.NONE, Bonus.NONE, Bonus.NONE},
        };
    }
}
