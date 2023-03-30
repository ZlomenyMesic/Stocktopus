using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public class Move {
        public int start;
        public int end;
        public int eval = 0;
        public bool isCapture;
        public string promotion;

        public Move(int start, int end, bool isCapture, string promotion = "", int eval = 0) {
            this.start = start;
            this.end = end;
            this.isCapture = isCapture;
            this.promotion = promotion;
            this.eval = eval;
        }
    }
}