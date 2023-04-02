using Stocktopus;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

class UCI {
    static void Main(string[] args) {
        Control.SetupBoard();
        while (true) {
            string[] cmd = Console.ReadLine().Split(" ");
            switch (cmd[0]) {
                case "uci": Console.WriteLine("uciok"); break;
                case "isready": Console.WriteLine("readyok"); break;
                case "ucinewgame": Control.SetupBoard(); break;
                case "go": Console.WriteLine(Control.EngineTurn()); break;
                case "position": Control.GenerateSetup(cmd); break;
                case "quit": break; // TODO
            }
        }
    }
}