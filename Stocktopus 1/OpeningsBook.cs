using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus {
    internal static class OpeningsBook {
        private static List<string[]> openings = new List<string[]> {
            // Alekhines's defense
            new string[]{ "e2e4", "g8f6", "e4e5", "f6d5", "d2d4", "d7d6", "c2c4", "d5b6", "f2f4", "d6e5", "f4e5", "b8c6", "c1e3", "c8f5", "b1c3", "e7e6", "g1f3", "f8e7" }, // Four pawns opening
            new string[]{ "e2e4", "g8f6", "e4e5", "f6d5", "d2d4", "d7d6", "c2c4", "d5b6", "e5d6", "c7d6", "b1c3", "g7g6", "c1e3", "f8g7", "a1c1" }, // Exchange variation
            new string[]{ "e2e4", "g8f6", "e4e5", "f6d5", "d2d4", "d7d6", "g1f3", "c8g4", "f1e2" }, // Modern variation
            // Caro-kann defense
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "e4e5", "c8f5", "b1c3", "e7e6", "g2g4" }, // Advance variation
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "b1c3", "d5e4", "c3e4", "c8f5", "e4g3", "f5g6", "h2h4", "h7h6", "g1f3", "b8d7", "h4h5", "g6h7" }, // Classical variation Nc3
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "b1d2", "d5e4", "d2e4", "c8f5", "e4g3", "f5g6", "h2h4", "h7h6", "g1f3", "b8d7", "h4h5", "g6h7" }, // Classical variation Nd2
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "e4d5", "c6d5", "c2c4", "g8f6", "b1c3" }, // Panov-Botvinnik attack
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "e4d5", "c6d5", "c2c3", "b7b5" }, // Exchange variation
            new string[]{ "e2e4", "c7c6", "d2d4", "d7d5", "f2f3", "d5e4", "f3e4", "e7e5" }, // Fantasy variation
            // French defense
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "e4e5", "c4c5", "c2c3", "b8c6", "g1f3", "d8b6" }, // Advance variation
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "b1c3", "d5e4", "c3e4", "g8f6" }, // Main line - Rubinstein variation
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "b1c3", "g8f6", "e4e5", "f6d7", "f2f4", "c7c5", "g1f3", "b8c6" }, // Main line - Classical variation
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "b1c3", "g8f6", "c1g5" }, // Main line - Classical variation, pinning the knight
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "b1c3", "f8b4", "e4e5", "c7c5", "a2a3", "b4c3", "b2c3" }, // Main line - Wineawer variation
            new string[]{ "e2e4", "e7e6", "d2d4", "d7d5", "b1d2", "g8f6", "e4e5", "f6d7", "b1d3", "c7c5", "c2c3" }, // Tarrasch variation
            // Italian game
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1c4", "g8f6", "f3g5" }, // 3. Nf6
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1c4", "f8c5", "d2d3", "g8f6" }, // Giuoco Piano, 4. d3
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1c4", "f8c5", "c2c3", "g8f6", "d2d4", "e5d4", "c3d4", "c5b4", "c1d2", "b4d2", "b1d2", "d7d5" }, // Giuoco, 4. c3
            // Pirc defense
            new string[]{ "e2e4", "d7d6", "d2d4", "g8f6", "b1c3", "g7g6", "f2f4", "f8g7" }, // Austrian attack
            new string[]{ "e2e4", "d7d6", "d2d4", "g8f6", "b1c3", "g7g6", "c1e3", "f8g7", "d1d2" }, // Pawn storm
            new string[]{ "e2e4", "d7d6", "d2d4", "g8f6", "b1c3", "g7g6", "g1f3", "f8g7", "h2h3" }, // Classical variation
            // Ruy Lopez
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "a7a6", "b5c6", "d7c6" }, // Main line, Exchange variation
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "a7a6", "b5a4", "g8f6" }, // Main line
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "g8f6" }, // Berlin defense
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "f8c5" }, // Classical defense
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "g8e7" }, // Cozio defense
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "f1b5", "c6d4" }, // Bird defense
            // Scandinavian defense
            new string[]{ "e2e4", "d7d5", "e4d5", "d8d5", "b1c3", "d5a5", "d2d4", "g8f6", "g1f3", "c7c6", "f1c4", "c8f5" }, // Main line
            new string[]{ "e2e4", "d7d5", "e4d5", "g8f6", "c2c4", "c7c6", "d5c6", "b8c6", "g1f3", "e7e5", "d2d3", "c8f5" }, // Modern variation
            // Scotch game
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "d2d4", "e5d4", "f3d4", "f8c5", "d4c6", "d8f6", "d1d2", "d7c6", "b1c3", "b8c6" }, // Classical variation
            new string[]{ "e2e4", "e7e5", "g1f3", "b8c6", "d2d4", "e5d4", "f3d4", "g8f6" }, // Mieses variation
            // Slav defense
            new string[]{ "d2d4", "d7d5", "c2c4", "c7c6", "c4d5", "c6d5", "b1c3", "g8f6", "g1f3", "b8c6", "c1f4", "c8f5", "e2e3", "e7e6", "f1d3", "f5d3", "d1d3", "f8d6", "f4d6", "d8d6" }, // Exchange variation
            new string[]{ "d2d4", "d7d5", "c2c4", "c7c6", "b1c3", "g8f6", "g1f3", "d5c4", "a2a4", "c8f5", "e2e3", "e7e6", "f1c4", "f8b4" }, // Main line
            // King's Indian Attack
            new string[]{ "g1f3", "d7d5", "g2g3", "g8f6", "f1g2", "c7c5", "d2d3", "b8c6", "b1d2", "e7e6" }, // Main line
        };

        public static string CheckForBookMove(string[] args) {
            List<string> possibleNextMoves = new List<string>();
            if (args.Length == 0) {
                for (int i = 0; i < openings.Count; i++)
                    possibleNextMoves.Add(openings[i][0]);
                return possibleNextMoves[new Random().Next(0, possibleNextMoves.Count)];
            }
            for (int i = 0; i < openings.Count; i++) {
                for (int j = 0; j < args.Count() + 1; j++) {
                    if (args[j] == openings[i][j]) {
                        if (j == args.Count() - 1 && j < openings[i].Length - 1) {
                            possibleNextMoves.Add(openings[i][j + 1]);
                            break;
                        } else if (j == openings[i].Length - 1) break;
                        else continue;
                    } else break;
                }
            }
            if (possibleNextMoves.Count != 0) return possibleNextMoves[new Random().Next(0, possibleNextMoves.Count)];
            else return "";
        }
    }
}
