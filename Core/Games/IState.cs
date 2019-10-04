using System.Collections.Generic;

namespace Core.Games
{
    public interface IState
    {
        int PlayerToMove { get; }
        int Round { get; }
        bool GameOver { get; }
        void Play(IMove move);
        bool IsValid(IMove move);
        IEnumerable<IMove> GetValidMoves();
        double GetScore(int playerNr);
        void Set(IState state);
        IState Clone();
    }
}