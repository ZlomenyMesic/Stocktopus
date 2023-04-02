using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public class Board<T> {
        private char[] board;
        public bool canWhiteShortCastle = true;
        public bool canWhiteLongCastle = true;
        public bool canBlackShortCastle = true;
        public bool canBlackLongCastle = true;

        public char this[int index] {
            get => board[index];
            set => board[index] = value;
        }

        public Board(params char[] setup) {
            board = new char[64];

            for (int i = 0; i < setup.Length; i++) {
                board[i] = setup[i];
            }
        }

        public List<Board<char>> GetChildren(Board<char> inpBoard, Color color) {
            List<Board<char>> children = new List<Board<char>>();
            Board<char> tempBoard = new Board<char>();
            Move[] moves = Core.GetLegalMoves(inpBoard, color, true, false).ToArray();
            for (byte i = 0; i < moves.Length; i++) {
                for (byte j = 0; j < 64; j++)
                    tempBoard[j] = inpBoard[j];

                Core.PerformMove(moves[i], tempBoard);
                children.Add(tempBoard.Clone());
            }
            return children;
        }

        public Board<char> Clone() {
            Board<char> nBoard = new Board<char>();
            for (byte i = 0; i < board.Length; i++)
                nBoard[i] = board[i];

            nBoard.canWhiteLongCastle = canWhiteLongCastle;
            nBoard.canWhiteShortCastle = canWhiteShortCastle;
            nBoard.canBlackLongCastle = canBlackLongCastle;
            nBoard.canBlackShortCastle = canBlackShortCastle;

            return nBoard;
        }

        public void Print() {
            for (int i = 0; i < 64; i++)
                Console.Write($"{board[i]} ");
        }

        public int KingPos(Color color) {
            for (byte i = 0; i < 64; i++)
                if (board[i] != '0' && board[i] == (color == Color.White ? 'K' : 'k'))
                    return i;
            return -1;
        }

        public GameState CurrentGameState() {
            byte pieces = 0;
            for (byte i = 0; i < 64; i++)
                if (board[i] != '0') pieces++;
            return pieces > 12 ? GameState.Midgame : GameState.Endgame;
        }
    }

    public enum Color {
        Black,
        White
    }

    public enum GameState {
        Midgame,
        Endgame
    }
}