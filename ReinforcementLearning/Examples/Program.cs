using ReinforcementLearning;
using ReinforcementLearning.GameSpecific;
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
            int chunk = 100000;

            var lastAgent1Wins = 0;
            var lastAgent2Wins = 0;
            var lastDraws = 0;
            var agent1Wins = 0;
            var agent2Wins = 0;
            var draws = 0;

            //var agent1 = new RandomAgent<TicTacToe.State, TicTacToe.Action>();
            //var agent2 = new RandomAgent<TicTacToe.State, TicTacToe.Action>();

            //var agent1 = new LearningAgent<TicTacToe.State, TicTacToe.Action, TableActionValueFunction<TicTacToe.State, TicTacToe.Action>, EpsilonGreedyActionSelector<TicTacToe.State, TicTacToe.Action>, BasicQUpdateRule<TicTacToe.State, TicTacToe.Action>>();
            //var agent2 = new LearningAgent<TicTacToe.State, TicTacToe.Action, TableActionValueFunction<TicTacToe.State, TicTacToe.Action>, EpsilonGreedyActionSelector<TicTacToe.State, TicTacToe.Action>, BasicQUpdateRule<TicTacToe.State, TicTacToe.Action>>();

            var agent1 = new GameLearningAgent<TicTacToe.State, TicTacToe.Action, TableGameValueFunction<TicTacToe.State, TicTacToe.Action>, GameGreedyActionSelector<TicTacToe.State, TicTacToe.Action>, GameUpdateRule<TicTacToe.State, TicTacToe.Action>>();
            var agent2 = new GameLearningAgent<TicTacToe.State, TicTacToe.Action, TableGameValueFunction<TicTacToe.State, TicTacToe.Action>, GameGreedyActionSelector<TicTacToe.State, TicTacToe.Action>, GameUpdateRule<TicTacToe.State, TicTacToe.Action>>();
            
            int game = 1;
            while (true)
            {
                var world = new TicTacToe.World();
                agent1.BeginEpisode();
                agent2.BeginEpisode();
                int move = 0;
                while (true)
                {
                    if ((game + move) % 2 == 0)
                    {
                        var state = agent1.Act(world);
                        if (state.IsTerminal)
                        {
                            if (state.IsWon)
                            {
                                agent1.EndEpisode(1);
                                agent2.EndEpisode(-1);
                                agent1Wins++;
                                break;
                            }
                            if (state.IsDraw)
                            {
                                agent1.EndEpisode(0.0);
                                agent2.EndEpisode(0.0);
                                draws++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        var state = agent2.Act(world);
                        if (state.IsTerminal)
                        {
                            if (state.IsWon)
                            {
                                agent1.EndEpisode(-1);
                                agent2.EndEpisode(1);
                                agent2Wins++;
                                break;
                            }
                            if (state.IsDraw)
                            {
                                agent1.EndEpisode(0.0);
                                agent2.EndEpisode(0.0);
                                draws++;
                                break;
                            }
                        }
                    }

                    move++;
                }

                game++;

                if ((game - 2) % chunk == 0)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Agent 1 wins: " + agent1Wins + " : " + 100 * (agent1Wins - lastAgent1Wins) / chunk + "%");
                    Console.WriteLine("Agent 2 wins: " + agent2Wins + " : " + 100 * (agent2Wins - lastAgent2Wins) / chunk + "%");
                    Console.WriteLine("       Draws: " + draws + " : " + 100 * (draws - lastDraws) / chunk + "%");

                    lastAgent1Wins = agent1Wins;
                    lastAgent2Wins = agent2Wins;
                    lastDraws = draws;

                    agent1.Save("agent1.txt");
                    agent2.Save("agent2.txt");
                }

                if (game % 500000 == 0)
                {
                    GameUpdateRule<TicTacToe.State, TicTacToe.Action>.LearningRate /= 2.0;
                }
            }
        }
    }
}
