using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class Utils {
        public static int kingPos;
        public static int[] startSetup = new int[] { 42, 22, 32, 52, 62, 32, 22, 42,
                                      12, 12, 12, 12, 12, 12, 12, 12,
                                      0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0,
                                      11, 11, 11, 11, 11, 11, 11, 11,
                                      41, 21, 31, 51, 61, 31, 21, 41 };

        public static int XYToIndex(int x, int y) {
            return ((y - 1) * 8) + x - 1;
        }

        public static (int, int) IndexToXY(int i) {
            return ((i % 8) + 1, ((i - (i % 8)) / 8) + 1);
        }

        public static int Max(int a, int b) {
            return a > b ? a : b;
        }

        public static int Min(int a, int b) {
            return a < b ? a : b;
        }

        public static Color OppCol(Color color) {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
