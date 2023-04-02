using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public static class Control {
        public static Board<char> board = new Board<char>();
        public static Color pColor;
        public static Color eColor;

        public static int depth = 4;
        public static int nodes = 0;
        public static Dictionary<string, DuplicateValue> cache = new Dictionary<string, DuplicateValue>();

        public static string nextBookMove = "";

        public static void SetupBoard() {
            int[] setup = new int[64];
            for (byte i = 0; i < 64; i++)
                setup[i] = Utils.startSetup[i];

            for (byte i = 0; i < 64; i++)
                board[i] = Utils.startSetup[i];
        }

        public static void GenerateSetup(string[] inp) {
            nextBookMove = "";
            SetupBoard();

            eColor = Color.White;
            pColor = Color.Black;

            List<string> args = inp.ToList();
            if (args.Count > 3) args.RemoveRange(0, 3);
            else {
                nextBookMove = OpeningsBook.CheckForBookMove(Array.Empty<string>());
                return;
            }

            string book = OpeningsBook.CheckForBookMove(args.ToArray());
            if (book != "") nextBookMove = book;

            int count = 0;
            foreach (string s in args) {
                count++;
                Move m = Utils.StrToMove(s);
                Core.PerformMove(m, board);
            }

            eColor = count % 2 == 0 ? Color.White : Color.Black;
            pColor = count % 2 == 0 ? Color.Black : Color.White;
        }

        public static string EngineTurn() {
            cache.Clear();
            Console.WriteLine($"new depth: {Utils.NewDepth()}");

            if (nextBookMove != "") {
                Console.WriteLine($"book move: {nextBookMove}");
                Move move = Utils.StrToMove(nextBookMove);
                Core.PerformMove(move, board);
                return $"bestmove {nextBookMove}";
            }

            List<Move> psb = Core.GetLegalMoves(board, eColor, true);
            List<Move> best = new List<Move>();
            Board<char> tempBoard = board.Clone();
            int max = int.MinValue;

            foreach (Move m in psb) {
                for (int i = 0; i < 64; i++)
                    tempBoard[i] = board[i];

                Core.PerformMove(m, tempBoard);
                int eval = Core.Minimax(tempBoard.Clone(), Utils.NewDepth() - 1, false, int.MinValue, int.MaxValue);
                Console.WriteLine($"{m.start} {m.end} {eval}");

                if (eval > max) {
                    best.Clear();
                    max = eval;
                    best.Add(m);
                } else if (eval == max) best.Add(m);
            }
            Move bestMove = best[new Random().Next(0, best.Count)];
            Core.PerformMove(bestMove, board);
            Console.WriteLine($"nodes: {nodes}");
            Console.WriteLine($"duplicates: {Utils.CountDuplicates()}");
            nodes = 0;
            return $"bestmove {Utils.MoveToStr(bestMove)}";
        }
    }
}