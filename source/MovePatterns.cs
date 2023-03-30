using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class MovePatterns {
        public static IEnumerable<Move> GetPawnMoves(Board<Piece?> board, int i, bool attacks) {
            List<Move> moves = new List<Move>();

            int rev = board[i].color == Color.White ? 1 : -1;
            int file = Utils.IndexToXY(i).Item1;
            int startRank = board[i].color == Color.White ? 7 : 2;
            int lastRank = board[i].color == Color.White ? 1 : 8;

            // Moves
            if (board[i - (8 * rev)] == null) moves.Add(new Move(i, i - (8 * rev), false));
            if (Utils.IndexToXY(i).Item2 == startRank && board[i - (16 * rev)] == null && board[i - (8 * rev)] == null)
                moves.Add(new Move(i, i - (16 * rev), false));

            // Attacks
            if (i - (9 * rev) < 64 && i - (9 * rev) > -1) {
                if (board[i - (9 * rev)] != null && board[i - (9 * rev)].color != board[i].color) moves.Add(new Move(i, i - (9 * rev), true));
                if (attacks && board[i - (9 * rev)] == null) moves.Add(new Move(i, i - (9 * rev), false));
            }
            if (i - (7 * rev) < 64 && i - (7 * rev) > -1) {
                if (board[i - (7 * rev)] != null && board[i - (7 * rev)].color != board[i].color) moves.Add(new Move(i, i - (7 * rev), true));
                if (attacks && board[i - (7 * rev)] == null) moves.Add(new Move(i, i - (7 * rev), false));
            }

            // TODO: EN PASSANT

            foreach (Move m in moves.ToList()) {
                //Console.WriteLine($"{m.start} {m.end} {Utils.Max(file, Utils.IndexToXY(m.end).Item1) - Utils.Min(file, Utils.IndexToXY(m.end).Item1)}");
                if (Utils.Max(file, Utils.IndexToXY(m.end).Item1) - Utils.Min(file, Utils.IndexToXY(m.end).Item1) <= 1) {
                    if (Utils.IndexToXY(m.end).Item2 == lastRank) {
                        yield return new Move(m.start, m.end, m.isCapture, "q");
                        yield return new Move(m.start, m.end, m.isCapture, "r");
                        yield return new Move(m.start, m.end, m.isCapture, "b");
                        yield return new Move(m.start, m.end, m.isCapture, "n");
                    } else yield return m;
                }
            }
        }

        public static List<Move> GetKnightMoves(in Board<Piece?> board, int i) {
            List<Move> moves = new List<Move>();

            int[] patterns = new int[] { -10, -6, 10, 6, -15, -17, 15, 17 };

            int file = Utils.IndexToXY(i).Item1;

            foreach (int p in patterns) {
                int newFile = Utils.IndexToXY(i + p).Item1;
                if (i + p >= 0 && i + p < 64 && Utils.Max(file, newFile) - Utils.Min(file, newFile) <= 2) {
                    if ((board[i + p] == null) || (board[i + p] != null && board[i + p].color != board[i].color))
                        moves.Add(new Move(i, i + p, board[i + p] != null));
                }
            }

            return moves;
        }

        public static List<Move> GetMovesByPattern(in Board<Piece?> board, int i, int[] pattern) {
            List<Move> moves = new List<Move>();

            int file = Utils.IndexToXY(i).Item1;
            int lFile;
            int nFile;

            foreach (int p in pattern) {
                lFile = file;
                for (int c = 1; c <= 8; c++) {
                    int j = i + (c * p);
                    nFile = Utils.IndexToXY(j).Item1;

                    if (j >= 0 && j < 64 && Utils.Max(lFile, nFile) - Utils.Min(lFile, nFile) < 2) {
                        if (board[j] == null) {
                            moves.Add(new Move(i, j, false));
                            lFile = nFile;
                        } else if (board[j].color != board[i].color) {
                            moves.Add(new Move(i, j, true));
                            break;
                        } else break;
                    } else break;

                }
            }

            return moves;
        }

        public static List<Move> GetBishopMoves(Board<Piece?> board, int i) {
            return GetMovesByPattern(board, i, new int[] { 7, 9, -7, -9 });
        }

        public static List<Move> GetRookMoves(Board<Piece?> board, int i) {
            return GetMovesByPattern(board, i, new int[] { 1, 8, -1, -8 });
        }

        public static List<Move> GetQueenMoves(in Board<Piece?> board, int i) {
            List<Move> moves = new List<Move>();

            moves.AddRange(GetBishopMoves(board, i));
            moves.AddRange(GetRookMoves(board, i));

            return moves;
        }

        public static List<Move> GetKingMoves(in Board<Piece?> board, int i)
            {
            List<Move> moves = new List<Move>();

            int[] pattern = new int[] { -1, -7, -8, -9, 1, 7, 8, 9 };

            int file = Utils.IndexToXY(i).Item1;

            foreach (int p in pattern) {
                if (i + p >= 0 && i + p < 64) {
                    if (((board[i + p] == null) || (board[i + p] != null && board[i + p].color != board[i].color)))
                        moves.Add(new Move(i, i + p, board[i + p] != null));
                }
            }

            foreach (Move m in moves.ToList())
                if (Utils.Max(file, Utils.IndexToXY(m.end).Item1) - Utils.Min(file, Utils.IndexToXY(m.end).Item1) > 2)
                    moves.Remove(m);

            return moves;
        }
    }
}