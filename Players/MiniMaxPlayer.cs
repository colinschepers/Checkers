using Core.Algorithms;
using Core.Games;

namespace Players
{
    public class MiniMaxPlayer : IPlayer
    {
        private readonly MiniMax _minimax = new MiniMax();

        public IMove GetMove(IState state)
        {
            return _minimax.GetBestMove(state);
        }

        public void OpponentMoved(IMove move)
        {
        }

        public override string ToString()
        {
            return $"MiniMaxPlayer()";
        }
    }
}
