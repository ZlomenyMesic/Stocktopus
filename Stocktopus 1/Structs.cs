using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    public struct Move {
        public int start;
        public int end;
        public int eval = 0;
        public char promotion;

        public Move(int start, int end, char promotion = '0', int eval = 0) {
            this.start = start;
            this.end = end;
            this.promotion = promotion;
            this.eval = eval;
        }
    }

    public class DuplicateValue {
        public int numberOfDuplicates;
        public int eval;

        public DuplicateValue() {
            eval = 0;
            numberOfDuplicates = 0;
        }
    }
}