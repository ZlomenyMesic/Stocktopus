using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class Utils {
        public static string files = "abcdefgh";
        public static char[] startSetup = new char[] { 'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r',
                                      'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p',
                                      '0', '0', '0', '0', '0', '0', '0', '0',
                                      '0', '0', '0', '0', '0', '0', '0', '0',
                                      '0', '0', '0', '0', '0', '0', '0', '0',
                                      '0', '0', '0', '0', '0', '0', '0', '0',
                                      'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P',
                                      'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R',};

        public static int XYToIndex(int x, int y) {
            return ((y - 1) * 8) + x - 1;
        }

        public static (int, int) IndexToXY(int i) {
            return ((i % 8) + 1, ((i - (i % 8)) / 8) + 1);
        }

        public static Color OppCol(Color color) {
            return color == Color.White ? Color.Black : Color.White;
        }

        public static string MoveToStr(Move move) {
            int sFile = IndexToXY(move.start).Item1;
            int sRank = 9 - IndexToXY(move.start).Item2;
            int eFile = IndexToXY(move.end).Item1;
            int eRank = 9 - IndexToXY(move.end).Item2;
            string prom = move.promotion == '0' ? "" : move.promotion.ToString().ToLower();

            return $"{files[sFile - 1]}{sRank}{files[eFile - 1]}{eRank}{prom}";
        }

        public static Move StrToMove(string str) {
            string startStr = str.Substring(0, 2);
            string endStr = str.Substring(2, 2);

            int start = XYToIndex(files.IndexOf(startStr[0]) + 1, 9 - int.Parse(startStr[1].ToString()));
            int end = XYToIndex(files.IndexOf(endStr[0]) + 1, 9 - int.Parse(endStr[1].ToString()));
            char prom = str.Length == 5 ? str[4] : '0';

            return new Move(start, end, prom);
        }

        public static string BoardToString(Board<char> board, int depth) {
            string output = "";
            for (int i = 0; i < 64; i++) {
                output += board[i];
            }
            return output += depth.ToString();
        }

        public static int NewDepth() {
            int numberOfPieces = 0;
            int defaultDepth = Control.depth;
            for (int i = 0; i < 64; i++)
                if (Control.board[i] != '0') numberOfPieces++;

            if (numberOfPieces <= 13 && numberOfPieces > 7) return defaultDepth + 1;
            else if (numberOfPieces == 8 || numberOfPieces == 7) return defaultDepth + 2;
            else if (numberOfPieces == 6) return defaultDepth + 3;
            else if (numberOfPieces == 5 || numberOfPieces == 4) return defaultDepth + 4;
            else if (numberOfPieces == 3) return defaultDepth + 6;
            else return defaultDepth;
        }

        public static int CountDuplicates() {
            int result = 0;
            foreach (DuplicateValue dp in Control.cache.Values) result += dp.numberOfDuplicates;
            return result;
        }

        public static Color Col(char c) {
            return new char[] { 'P', 'N', 'B', 'R', 'Q', 'K' }.Contains(c) ? Color.White : Color.Black;
        }
    }
}
