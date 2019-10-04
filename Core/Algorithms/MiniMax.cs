using Core.Games;
using System;
using System.Diagnostics;

namespace Core.Algorithms
{
    public class MiniMax
    {
        public int RunningTimeInMilliseconds { get; set; } = 100;
        public int DepthLimit { get; set; } = 20;

        private Stopwatch _stopwatch = new Stopwatch();

        public IMove GetBestMove(IState state)
        {
            _stopwatch.Restart();

            IMove bestMove = null;
            for (int d = 1; d < DepthLimit; d++)
            {
                var move = GetBestMove(state, d);
                if (_stopwatch.ElapsedMilliseconds < RunningTimeInMilliseconds)
                {
                    bestMove = move;
                }
            }

            return bestMove;
        }

        public IMove GetBestMove(IState state, int maxDepth)
        {
            var bestScore = double.MinValue;
            IMove bestMove = null;
            foreach (var move in state.GetValidMoves())
            {
                var nextState = state.Clone();
                nextState.Play(move);
                var score = CalculateScore(nextState, 1, maxDepth, state.PlayerToMove, double.MinValue, double.MaxValue);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }
            return bestMove;
        }

        private double CalculateScore(IState state, int depth, int maxDepth, int playerNr, double alpha, double beta)
        { 
            if (state.GameOver || depth == maxDepth || _stopwatch.ElapsedMilliseconds >= RunningTimeInMilliseconds)
            {
                return state.GetScore(playerNr);
            }

            var bestScore = double.MinValue;

            if (state.PlayerToMove == playerNr)
            {
                bestScore = double.MinValue;
                foreach (var move in state.GetValidMoves())
                {
                    var nextState = state.Clone();
                    nextState.Play(move);
                    var score = CalculateScore(nextState, depth + 1, maxDepth, playerNr, alpha, beta);
                    bestScore = Math.Max(bestScore, score);
                    alpha = Math.Max(alpha, bestScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            else
            {
                bestScore = double.MaxValue;
                foreach (var move in state.GetValidMoves())
                {
                    var nextState = state.Clone();
                    nextState.Play(move);
                    var score = CalculateScore(nextState, depth + 1, maxDepth, playerNr, alpha, beta);
                    bestScore = Math.Min(bestScore, score);
                    alpha = Math.Min(beta, bestScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }

            return bestScore;
        }
    }
}
