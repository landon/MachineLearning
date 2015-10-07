using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RamseyGame
{
    class Game
    {
        IBoard _board;
        IPlayer _player1, _player2;

        public Game(IBoard board, IPlayer player1, IPlayer player2)
        {
            _board = board;
            _player1 = player1;
            _player2 = player2;
        }

        public GameEnd Play()
        {
            while (true)
            {
                if (!_board.MovesExist())
                    break;

                _player1.Move(_board);
                if (_board.IsWon())
                    return GameEnd.Player1;

                if (!_board.MovesExist())
                    break;

                _player2.Move(_board);
                if (_board.IsWon())
                    return GameEnd.Player2;
            }

            return GameEnd.Draw;
        }
    }
}