using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RamseyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = 0;
            var p2 = 0;

            while (true)
            {
                var board = new CompleteRamseyBoard(17, 4, 4);
                var player1 = new RandomPlayer(-1);
                var player2 = new RandomPlayer(1);

                var game = new Game(board, player1, player2);
                var result = game.Play();

                if (result == GameEnd.Draw)
                {
                    var renderer = new DotRenderer(@"C:\Program Files (x86)\Graphviz2.38\bin\neato.exe");
                    renderer.DrawCompleteGraph = true;
                    renderer.Render(board.State, "test.png", DotRenderType.png);
                    break;
                }
                else
                {
                    if (result == GameEnd.Player1)
                        p1++;
                    else
                        p2++;

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("p1: " + 100.0 * p1 / (p1 + p2) + "%         ");
                    Console.WriteLine("p2: " + 100.0 * p2 / (p1 + p2) + "%         ");
                }
            }
        }
    }
}
