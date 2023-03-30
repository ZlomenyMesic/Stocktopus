using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class Core {
        public static List<Move> GetLegalMoves(Board<Piece?> board, Color color, bool careChecks = true, bool attacks = false) {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < board.Length; i++) {
                if (board[i] != null && board[i].color == color) {
                    switch (board[i].boardNum) {
                        case 1: moves.AddRange(MovePatterns.GetPawnMoves(board, i, attacks)); break;
                        case 2: moves.AddRange(MovePatterns.GetKnightMoves(board, i)); break;
                        case 3: moves.AddRange(MovePatterns.GetBishopMoves(board, i)); break;
                        case 4: moves.AddRange(MovePatterns.GetRookMoves(board, i)); break;
                        case 5: moves.AddRange(MovePatterns.GetQueenMoves(board, i)); break;
                        case 6: moves.AddRange(MovePatterns.GetKingMoves(board, i)); break;
                    }
                }
            }

            if (careChecks) {
                Board<Piece?> tempBoard = board.Clone();
                foreach (Move m in moves.ToList()) {
                    for (int i = 0; i < board.Length; i++)
                        tempBoard[i] = board[i];

                    tempBoard[m.start].MoveTo(m, tempBoard);
                    //tempBoard.Print();
                    //Console.WriteLine($"{m.start} {m.end} {IsCheck(tempBoard, color, tempBoard.KingPos(color))}");
                    if (IsCheck(tempBoard, color, tempBoard.KingPos(color)))
                        moves.Remove(m);
                    else m.eval = Evaluate(tempBoard, color);
                    //Console.WriteLine($"{m.start} {m.end} {m.eval}");
                }
            }

            return moves;
        }

        public static int Minimax(Board<Piece?> board, int depth, int alpha, int beta) {
            return 1;
        }

        public static int Evaluate(Board<Piece?> board, Color color) {
            int eval = 0;
            foreach (Piece? piece in board) {
                if (piece != null && piece.color == color) eval += piece.value;
                else if (piece != null && piece.color != color) eval -= piece.value;
            }
            return eval;
        }

        public static bool IsCheck(Board<Piece?> board, Color kingColor, int kingPos) {
            foreach (Move m in GetLegalMoves(board, Utils.OppCol(kingColor), false, true)) {
                //Console.WriteLine($"{m.start} {m.end} {kingPos} {kingColor}");
                if (m.end == kingPos) return true;
            }
            return false;
        }
    }
}
