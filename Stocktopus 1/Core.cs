using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class Core {
        public static List<Move> GetLegalMoves(Board<char> board, Color color, bool careChecks = true, bool attacks = false) {
            List<Move> moves = new List<Move>();
            for (byte i = 0; i < 64; i++) {
                if (board[i] != '0' && Utils.Col(board[i]) == color) {
                    switch (board[i]) {
                        case 'p': case 'P': moves.AddRange(MovePatterns.GetPawnMoves(board, i, attacks)); break;
                        case 'n': case 'N': moves.AddRange(MovePatterns.GetKnightMoves(board, i)); break;
                        case 'b': case 'B': moves.AddRange(MovePatterns.GetBishopMoves(board, i)); break;
                        case 'r': case 'R': moves.AddRange(MovePatterns.GetRookMoves(board, i)); break;
                        case 'q': case 'Q': moves.AddRange(MovePatterns.GetQueenMoves(board, i)); break;
                        case 'k': case 'K': moves.AddRange(MovePatterns.GetKingMoves(board, i)); break;
                    }
                }
            }

            if (careChecks) {
                Board<char> tempBoard = board.Clone();
                foreach (Move m in moves.ToList()) {
                    for (byte i = 0; i < 64; i++)
                        tempBoard[i] = board[i];

                    PerformMove(m, tempBoard);
                    if (IsCheck(tempBoard, color, tempBoard.KingPos(color)))
                        moves.Remove(m);
                }
            }

            return moves;
        }

        public static int Minimax(Board<char> board, int depth, bool maximizingPlayer, int alpha, int beta) {
            string strValue = Utils.BoardToString(board, depth);
            if (Control.cache.ContainsKey(strValue)) {
                Control.cache[strValue].numberOfDuplicates++;
                return Control.cache[strValue].eval;
            } else Control.cache.Add(strValue, new DuplicateValue());

            if (depth == 0) {
                Control.nodes++;
                int eval = Evaluate(board, Control.eColor);
                Control.cache[strValue].eval = eval;
                return eval;
            }
            if (maximizingPlayer) {
                int value = depth * -5000;
                List<Board<char>> children = board.GetChildren(board, Control.eColor);
                if (children.Count == 0 && !IsCheck(board, Control.eColor, board.KingPos(Control.eColor))) value = 420;

                for (byte i = 0; i < children.Count; i++) {
                    int nextMinimax = Minimax(children[i].Clone(), depth - 1, false, alpha, beta);
                    value = value > nextMinimax ? value : nextMinimax;
                    alpha = alpha > value ? alpha : value;
                    if (beta <= alpha) break;
                } 
                Control.cache[strValue].eval = value;
                return value;
            } else {
                int value = depth * 5000;
                List<Board<char>> children = board.GetChildren(board, Control.pColor);
                if (children.Count == 0 && !IsCheck(board, Control.pColor, board.KingPos(Control.pColor))) value = -420;

                for (byte i = 0; i < children.Count; i++) {
                    int nextMinimax = Minimax(children[i].Clone(), depth - 1, true, alpha, beta);
                    value = value < nextMinimax ? value : nextMinimax;
                    beta = beta < value ? beta : value;
                    if (beta <= alpha) break;
                }
                Control.cache[strValue].eval = value;
                return value;
            }
        }

        public static int Evaluate(Board<char> board, Color color) {
            int reverse = color == Color.White ? -1 : 1;
            int eval = 0;

            for (int i = 0; i < 64; i++) {
                if (board[i] != '0') {
                    if (Utils.Col(board[i]) == color) eval += Tables.GetValue(board[i], i, board.CurrentGameState());
                    else eval -= Tables.GetValue(board[i], i, board.CurrentGameState());
                }
            }
            //if (board.canBlackLongCastle || board.canBlackShortCastle) eval += 5 * reverse;
            //if (board.canWhiteLongCastle || board.canWhiteShortCastle) eval -= 5 * reverse;

            return eval;
        }

        public static bool IsCheck(Board<char> board, Color kingColor, params int[] kingPos) {
            foreach (Move m in GetLegalMoves(board, Utils.OppCol(kingColor), false, true)) {
                //Console.WriteLine($"{m.start} {m.end} {kingPos} {kingColor}");
                if (kingPos.Contains(m.end)) return true;
            }
            return false;
        }

        public static void PerformMove(Move move, Board<char> board) {
            board[move.end] = '0';

            if (board[move.start] == 'k' || board[move.start] == 'K') {
                // Black queenside
                if (move.start == 4 && move.end == 2) {
                    board[0] = '0';
                    board[3] = 'r';
                }
                // Black kingside
                if (move.start == 4 && move.end == 6) {
                    board[7] = '0';
                    board[5] = 'r';
                }
                // White queenside
                if (move.start == 60 && move.end == 58) {
                    board[56] = '0';
                    board[59] = 'R';
                }
                // White kingside
                if (move.start == 60 && move.end == 62) {
                    board[63] = '0';
                    board[61] = 'R';
                }
            }

            if (move.promotion == '0') {
                board[move.end] = board[move.start];
                //if (board[move.start] == 'k') {
                //    board.canBlackShortCastle = false;
                //    board.canBlackLongCastle = false;
                //} else if (board[move.start] == 'K') {
                //    board.canWhiteShortCastle = false;
                //    board.canWhiteLongCastle = false;
                //} else if (board[move.start] == 'R' || board[move.start] == 'r') {
                //    if (move.start == 0) board.canBlackLongCastle = false;
                //    else if (move.start == 7) board.canBlackShortCastle = false;
                //    else if (move.start == 56) board.canWhiteLongCastle = false;
                //    else if (move.start == 63) board.canWhiteShortCastle = false;
                //}
            } else {
                if (Utils.Col(board[move.start]) == Color.Black) board[move.end] = move.promotion;
                else board[move.end] = move.promotion.ToString().ToUpper()[0];
            }
            
            //if (move.end == 0) board.canBlackLongCastle = false;
            //else if (move.end == 7) board.canBlackShortCastle = false;
            //else if (move.end == 56) board.canWhiteLongCastle = false;
            //else if (move.end == 63) board.canWhiteShortCastle = false;

            board[move.start] = '0';
        }
    }
}
