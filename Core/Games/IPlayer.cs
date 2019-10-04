namespace Core.Games
{
    public interface IPlayer
    {
        IMove GetMove(IState state);
        void OpponentMoved(IMove move);
    }
}
