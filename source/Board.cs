using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public class Board<T> : IEnumerable, IEnumerator where T: Piece? {
        private Piece?[] board;
        private int position;
        public bool canWhiteShortCastle = true;
        public bool canWhiteLongCastle = true;
        public bool canBlackShortCastle = true;
        public bool canBlackLongCastle = true;

        public int Length => board.Length;
        public object? Current => board[position];

        public Piece? this[int index] {
            get => board[index];
            set => board[index] = value;
        }

        public Board(params Piece?[] setup) {
            board = new Piece?[64];
            position = -1;

            for (int i = 0; i < setup.Length; i++) {
                board[i] = setup[i];
            }
        }

        public static List<Board<Piece?>> GetChildren(Color color) {

            // TODO

            return new List<Board<Piece?>>();
        }

        public Board<Piece?> Clone() {
            Board<Piece?> nBoard = new Board<Piece?>();
            for (int i = 0; i < board.Length; i++)
                nBoard[i] = board[i];

            nBoard.canWhiteLongCastle = canWhiteLongCastle;
            nBoard.canWhiteShortCastle = canWhiteShortCastle;
            nBoard.canBlackLongCastle = canBlackLongCastle;
            nBoard.canBlackShortCastle = canBlackShortCastle;

            return nBoard;
        }

        public IEnumerator GetEnumerator() {
            return this;
        }

        public bool MoveNext() {
            if (position < board.Length - 1) {
                position++;
                return true;
            }
            return false;
        }

        public void Reset() {
            position = -1;
        }

        public void Print() {
            foreach (Piece? p in board) {
                if (p != null) Console.Write($"{p.boardNum} ");
                else Console.Write("0 ");
            }
        }

        public int KingPos(Color color) {
            for (int i = 0; i < board.Length; i++)
                if (board[i] != null && board[i].boardNum == 6 && board[i].color == color)
                    return i;
            return 1;
        }
    }

    public enum Color {
        Black,
        White
    }
}
