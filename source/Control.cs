using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public static class Control {
        public static string files = "abcdefgh";
        public static Board<Piece?> board = new Board<Piece?>();
        public static Color pColor;
        public static Color eColor;

        public static void SetupBoard() {
            int[] setup = new int[64];
            for (int i = 0; i < board.Length; i++)
                setup[i] = Utils.startSetup[i];

            for (int i = 0; i < board.Length; i++) {
                int piece = setup[i];
                if (piece == 0) board[i] = null;
                else {
                    Color color = int.Parse(piece.ToString()[1].ToString()) == 1 ? Color.White : Color.Black;
                    piece = int.Parse(piece.ToString()[0].ToString());
                    switch (piece) {
                        case 1: board[i] = new Pawn(color); break;
                        case 2: board[i] = new Knight(color); break;
                        case 3: board[i] = new Bishop(color); break;
                        case 4: board[i] = new Rook(color); break;
                        case 5: board[i] = new Queen(color); break;
                        case 6: board[i] = new King(color); Utils.kingPos = i; break;
                        default: board[i] = null; break;
                    }
                }
            }

            
        }

        public static void GenerateSetup(string[] inp) {
            SetupBoard();

            eColor = Color.White;
            pColor = Color.Black;

            List<string> args = inp.ToList();
            if (args.Count > 3) args.RemoveRange(0, 3);
            else return;

            int count = 0;
            foreach (string s in args) {
                count++;
                Move m = StrToMove(s);
                board[m.start].MoveTo(m, board);
            }

            eColor = count % 2 == 0 ? Color.White : Color.Black;
            pColor = count % 2 == 0 ? Color.Black : Color.White;
        }

        public static string EngineTurn() {
            //board.Print();
            List<Move> psb = Core.GetLegalMoves(board, eColor, true);
            List<Move> best = new List<Move>();
            int max = -10000;
            foreach (Move m in psb) {
                if (m.eval > max) {
                    best.Clear();
                    best.Add(m);
                    max = m.eval;
                } else if (m.eval == max) best.Add(m);
            }
            Move bestMove = best[new Random().Next(0, best.Count)];
            board[bestMove.start].MoveTo(bestMove, board);
            Console.WriteLine($"WS: {board.canWhiteShortCastle}");
            Console.WriteLine($"WL: {board.canWhiteLongCastle}");
            Console.WriteLine($"BS: {board.canBlackShortCastle}");
            Console.WriteLine($"BL: {board.canBlackLongCastle}");
            return $"bestmove {MoveToStr(bestMove)}";
        }

        public static string MoveToStr(Move move) {
            int sFile = Utils.IndexToXY(move.start).Item1;
            int sRank = 9 - Utils.IndexToXY(move.start).Item2;
            int eFile = Utils.IndexToXY(move.end).Item1;
            int eRank = 9 - Utils.IndexToXY(move.end).Item2;

            return $"{files[sFile - 1]}{sRank}{files[eFile - 1]}{eRank}{move.promotion}";
        }

        public static Move StrToMove(string str) {
            string startStr = str.Substring(0, 2);
            string endStr = str.Substring(2, 2);

            int start = Utils.XYToIndex(files.IndexOf(startStr[0]) + 1, 9 - int.Parse(startStr[1].ToString()));
            int end = Utils.XYToIndex(files.IndexOf(endStr[0]) + 1, 9 - int.Parse(endStr[1].ToString()));

            bool isCapture = board[end] != null;
            string prom = str.Length == 5 && str[4] != '+' ? str[4].ToString() : "";

            return new Move(start, end, isCapture, prom);
        }
    }
}