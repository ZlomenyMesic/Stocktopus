using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public class Piece {
        public int boardNum;
        public int value;
        public Color color;

        public Piece(Color color = Color.White) {
            this.color = color;
        }

        public void MoveTo(Move move, Board<Piece?> board) {
            board[move.end] = null;
            if (move.promotion == "") {
                board[move.end] = this;
                if (boardNum == 6) {
                    if (color == Color.Black) {
                        board.canBlackShortCastle = false;
                        board.canBlackLongCastle = false;
                    }
                    else {
                        board.canWhiteShortCastle = false;
                        board.canWhiteLongCastle = false;
                    }
                }
                else if (boardNum == 6) {
                    if (move.start == 0) board.canBlackLongCastle = false;
                    else if (move.start == 7) board.canBlackShortCastle = false;
                    else if (move.start == 56) board.canWhiteLongCastle = false;
                    else if (move.start == 63) board.canWhiteShortCastle = false;
                }
            }
            else {
                Piece? promoted = null;
                switch (move.promotion) {
                    case "q": promoted = new Queen(color); break;
                    case "r": promoted = new Rook(color); break;
                    case "n": promoted = new Knight(color); break;
                    case "b": promoted = new Bishop(color); break;
                }

                board[move.end] = promoted;
            }
            if (move.end == 0) board.canBlackLongCastle = false;
            else if (move.end == 7) board.canBlackShortCastle = false;
            else if (move.end == 56) board.canWhiteLongCastle = false;
            else if (move.end == 63) board.canWhiteShortCastle = false;

            board[move.start] = null;
        }
    }

    internal class Pawn : Piece {
        public Pawn(Color color = Color.White) : base(color) {
            boardNum = 1;
            value = 1;
        }
    }

    internal class Knight : Piece {
        public Knight(Color color = Color.White) : base(color) {
            boardNum = 2;
            value = 3;
        }
    }

    internal class Bishop : Piece {
        public Bishop(Color color = Color.White) : base(color) {
            boardNum = 3;
            value = 3;
        }
    }

    internal class Rook : Piece  {
        public Rook(Color color = Color.White) : base(color) {
            boardNum = 4;
            value = 5;
        }
    }

    internal class Queen : Piece {
        public Queen(Color color = Color.White) : base(color) {
            boardNum = 5;
            value = 9;
        }
    }

    internal class King : Piece {
        public King(Color color = Color.White) : base(color) {
            boardNum = 6;
            value = 1000;
        }
    }
}
