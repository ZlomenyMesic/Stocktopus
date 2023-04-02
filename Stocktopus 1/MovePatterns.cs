using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class MovePatterns {
        public static IEnumerable<Move> GetPawnMoves(Board<char> board, int i, bool attacks) {
            List<Move> moves = new List<Move>();

            int rev = Utils.Col(board[i]) == Color.White ? 1 : -1;
            int file = Utils.IndexToXY(i).Item1;
            int startRank = Utils.Col(board[i]) == Color.White ? 7 : 2;
            int lastRank = Utils.Col(board[i]) == Color.White ? 1 : 8;

            // Moves
            if (board[i - (8 * rev)] == '0') moves.Add(new Move(i, i - (8 * rev)));
            if (Utils.IndexToXY(i).Item2 == startRank && board[i - (16 * rev)] == '0' && board[i - (8 * rev)] == '0')
                moves.Add(new Move(i, i - (16 * rev)));

            // Attacks
            if (i - (9 * rev) < 64 && i - (9 * rev) > -1) {
                if (board[i - (9 * rev)] != '0' && Utils.Col(board[i - (9 * rev)]) != Utils.Col(board[i])) moves.Add(new Move(i, i - (9 * rev)));
                if (attacks && board[i - (9 * rev)] == '0') moves.Add(new Move(i, i - (9 * rev)));
            }
            if (i - (7 * rev) < 64 && i - (7 * rev) > -1) {
                if (board[i - (7 * rev)] != '0' && Utils.Col(board[i - (7 * rev)]) != Utils.Col(board[i])) moves.Add(new Move(i, i - (7 * rev)));
                if (attacks && board[i - (7 * rev)] == '0') moves.Add(new Move(i, i - (7 * rev)));
            }

            // TODO: EN PASSANT

            foreach (Move m in moves.ToList()) {
                //Console.WriteLine($"{m.start} {m.end} {Utils.Max(file, Utils.IndexToXY(m.end).Item1) - Utils.Min(file, Utils.IndexToXY(m.end).Item1)}");
                int x = Utils.IndexToXY(m.end).Item1;
                if ((file > x ? file : x) - (file < x ? file : x) <= 1) {
                    if (Utils.IndexToXY(m.end).Item2 == lastRank) {
                        yield return new Move(m.start, m.end, 'q');
                        yield return new Move(m.start, m.end, 'r');
                        yield return new Move(m.start, m.end, 'n');
                        yield return new Move(m.start, m.end, 'b');
                    } else yield return m;
                }
            }
        }

        public static List<Move> GetKnightMoves(in Board<char> board, int i) {
            List<Move> moves = new List<Move>();

            int[] patterns = new int[] { -10, -6, 10, 6, -15, -17, 15, 17 };

            int file = Utils.IndexToXY(i).Item1;

            foreach (int p in patterns) {
                int newFile = Utils.IndexToXY(i + p).Item1;
                if (i + p >= 0 && i + p < 64 && (file > newFile ? file : newFile) - (file < newFile ? file : newFile) <= 2) {
                    if ((board[i + p] == '0') || (board[i + p] != '0' && Utils.Col(board[i + p]) != Utils.Col(board[i])))
                        moves.Add(new Move(i, i + p));
                }
            }

            return moves;
        }

        public static List<Move> GetMovesByPattern(in Board<char> board, int i, int[] pattern) {
            List<Move> moves = new List<Move>();

            int file = Utils.IndexToXY(i).Item1;
            int lFile;
            int nFile;

            foreach (int p in pattern) {
                lFile = file;
                for (int c = 1; c <= 8; c++) {
                    int j = i + (c * p);
                    nFile = Utils.IndexToXY(j).Item1;

                    if (j >= 0 && j < 64 && (lFile > nFile ? lFile : nFile) - (lFile < nFile ? lFile : nFile) < 2) {
                        if (board[j] == '0') {
                            moves.Add(new Move(i, j));
                            lFile = nFile;
                        } else if (Utils.Col(board[j]) != Utils.Col(board[i])) {
                            moves.Add(new Move(i, j));
                            break;
                        } else break;
                    } else break;

                }
            }

            return moves;
        }

        public static List<Move> GetBishopMoves(Board<char> board, int i) {
            return GetMovesByPattern(board, i, new int[] { 7, 9, -7, -9 });
        }

        public static List<Move> GetRookMoves(Board<char> board, int i) {
            return GetMovesByPattern(board, i, new int[] { 1, 8, -1, -8 });
        }

        public static List<Move> GetQueenMoves(in Board<char> board, int i) {
            List<Move> moves = new List<Move>();

            moves.AddRange(GetBishopMoves(board, i));
            moves.AddRange(GetRookMoves(board, i));

            return moves;
        }

        public static List<Move> GetKingMoves(in Board<char> board, int i)
            {
            List<Move> moves = new List<Move>();

            int[] pattern = new int[] { -1, -7, -8, -9, 1, 7, 8, 9 };

            int file = Utils.IndexToXY(i).Item1;

            foreach (int p in pattern) {
                if (i + p >= 0 && i + p < 64) {
                    if ((board[i + p] == '0') || (board[i + p] != '0' && Utils.Col(board[i + p]) != Utils.Col(board[i])))
                        moves.Add(new Move(i, i + p));
                }
            }

            //if (i == 4 && board[i] == 'k') {
            //    if (board.canBlackLongCastle && board[1] == '0' && board[2] == '0' && board[3] == '0') {
            //        // Black queenside
            //        Board<char> tempBoard = board.Clone();
            //        tempBoard[2] = 'k';
            //        tempBoard[3] = 'k';
            //        if (!Core.IsCheck(tempBoard.Clone(), Color.Black, new int[] { 4, 3 })) moves.Add(new Move(4, 2));
            //    }
            //    if (board.canBlackShortCastle && board[5] == '0' && board[6] == '0') {
            //        // Black kingside
            //        Board<char> tempBoard = board.Clone();
            //        tempBoard[5] = 'k';
            //        if (!Core.IsCheck(tempBoard.Clone(), Color.Black, new int[] { 4, 5 })) moves.Add(new Move(4, 6));
            //    }
            //} else if (i == 60 && board[i] == 'K') {
            //    if (board.canWhiteLongCastle && board[57] == '0' && board[58] == '0' && board[59] == '0') {
            //        // White queenside
            //        Board<char> tempBoard = board.Clone();
            //        tempBoard[58] = 'K';
            //        tempBoard[59] = 'K';
            //        if (!Core.IsCheck(tempBoard.Clone(), Color.White, new int[] { 60, 59 })) moves.Add(new Move(60, 58));
            //    }
            //    if (board.canWhiteShortCastle && board[61] == '0' && board[62] == '0') {
            //        // White kingside
            //        Board<char> tempBoard = board.Clone();
            //        tempBoard[61] = 'K';
            //        if (!Core.IsCheck(tempBoard.Clone(), Color.White, new int[] { 60, 61 })) moves.Add(new Move(60, 62));
            //    }
            //}

            foreach (Move m in moves.ToList()) {
                int x = Utils.IndexToXY(m.end).Item1;
                if ((file > x ? file : x) - (file < x ? file : x) > 5)
                    moves.Remove(m);
            }
            return moves;
        }
    }
}