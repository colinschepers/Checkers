using Core.Games;
using System;
using System.Diagnostics;

namespace Checkers
{
    public class GameEngine
    {
        public IGame Game { get; }
        
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public GameEngine(IGame game)
        {
            Game = game;
        }

        public void Run()
        {
            Game.StartOfGame += StartOfGame;
            Game.StartOfTurn += StartOfTurn;
            Game.EndOfTurn += EndOfTurn;
            Game.EndOfGame += EndOfGame;
            Game.Start();
        }

        public void StartOfGame(IGame game)
        {
            Console.Clear();
            Console.WriteLine("Game started...");
            Console.WriteLine();
            Console.WriteLine(game.State);
        }

        public void StartOfTurn(IState state, IPlayer player)
        {
            //Console.ReadLine();
            _stopwatch.Restart();
        }

        public void EndOfTurn(IState state, IPlayer player, IMove move)
        {
            Console.WriteLine("{0} played a move in {1}ms => {2}", player, _stopwatch.ElapsedMilliseconds, move);
            Console.WriteLine(state);
            //Console.ReadLine();
        }

        public void EndOfGame(IGame game)
        {
            Console.WriteLine("Game over!");
            Console.WriteLine();
            for (int i = 0; i < game.Players.Length; i++)
            {
                Console.WriteLine("Score {0}: {1}", game.Players[i], game.State.GetScore(i));
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}