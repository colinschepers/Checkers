using System;

namespace Core.Games
{
    public interface IGame
    {
        event Action<IGame> StartOfGame;
        event Action<IState, IPlayer> StartOfTurn;
        event Action<IState, IPlayer, IMove> EndOfTurn;
        event Action<IGame> EndOfGame;

        IState State { get; }
        IPlayer[] Players { get; }

        void Start();
    }
}