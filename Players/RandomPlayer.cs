using Core.Games;
using System;
using System.Linq;

namespace Players
{
    public class RandomPlayer : IPlayer
    {
        private readonly Random _random = new Random();

        public IMove GetMove(IState state)
        {
            var moves = state.GetValidMoves().ToArray();
            return moves[_random.Next(moves.Count())];
        }

        public void OpponentMoved(IMove move)
        {
        }

        public override string ToString()
        {
            return $"RandomPlayer()";
        }
    }
}
