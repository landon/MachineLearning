using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe();
        }

        static void TicTacToe()
        {
            var agent1Wins = 0;
            var agent2Wins = 0;
            var draws = 0;

            var agent1 = new RandomAgent<TicTacToe.State, TicTacToe.Action>();
            //var agent2 = new RandomAgent<TicTacToe.State, TicTacToe.Action>();

            var agent2 = new LearningAgent<TicTacToe.State, TicTacToe.Action, TableActionValueFunction<TicTacToe.State, TicTacToe.Action>, EpsilonGreedyActionSelector<TicTacToe.State, TicTacToe.Action>, BasicQUpdateRule<TicTacToe.State, TicTacToe.Action>>();

            while (true)
            {
                var world = new TicTacToe.World();
                while (true)
                {
                    var state = agent1.Act(world);
                    if (state.IsWon)
                    {
                        agent1Wins++;
                        break;
                    }
                    if (state.IsDraw)
                    {
                        draws++;
                        break;
                    }

                    state = agent2.Act(world);
                    if (state.IsWon)
                    {
                        agent2Wins++;
                        break;
                    }
                    if (state.IsDraw)
                    {
                        draws++;
                        break;
                    }
                }

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Agent 1 wins: " + agent1Wins);
                Console.WriteLine("Agent 2 wins: " + agent2Wins);
                Console.WriteLine("       Draws: " + draws);
            }
        }
    }
}
