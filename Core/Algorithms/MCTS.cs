using Core.Arithmetic;
using Core.DataStructures;
using Core.Games;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.Algorithms
{
    public class MCTS
    {
        class MCTSNode : Node
        {
            public IMove Move { get; private set; }
            public double Score { get; set; }
            public int Visits { get; set; }
            public int PlayerToMove { get; private set; }
            public bool IsTerminal { get; private set; }
            public Queue<IMove> UntriedMoves { get; private set; }

            public MCTSNode(IState state) : this(null, null, state)
            {
            }

            public MCTSNode(Node parent, IMove move, IState state) : base(parent)
            {
                Move = move;
                PlayerToMove = state.PlayerToMove;
                IsTerminal = state.GameOver;
                UntriedMoves = IsTerminal ? new Queue<IMove>(0) : new Queue<IMove>(state.GetValidMoves().Shuffle());
            }

            public override string ToString()
            {
                return string.Format("<{0}> [{1} | {2}]", Move, Math.Round(Score, 2), Visits);
            }
        }

        private readonly double _uctC = Math.Sqrt(2);
        private readonly Random _random = RandomGenerator.Instance;
        private MCTSNode _root;

        public MCTS() : this(Math.Sqrt(2))
        {
        } 

        public MCTS(double uctC)
        {
            _uctC = uctC;
        }

        public IMove GetBestMove(IState initialState, long responseTimeInMilliseconds)
        {
            var state = initialState.Clone();

            _root = _root ?? new MCTSNode(initialState);

            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < responseTimeInMilliseconds)
            {
                state.Set(initialState);
                var node = Selection(_root, state);
                node = Expand(node, state);
                Rollout(state);
                Backtrack(node, state);
            }
            
            var move = _root.Children.Any()
                ? _root.Children.Cast<MCTSNode>().Aggregate((x, y) => x.Score > y.Score ? x : y).Move
                : GetRandomMove(initialState);

            _root = _root.Children.Cast<MCTSNode>().FirstOrDefault(c => c.Move.Equals(move));
            
            if(_root != null)
            {
                _root.Parent = null;
            }

            return move;
        }

        private MCTSNode Selection(MCTSNode node, IState state)
        {
            while (node.UntriedMoves.Count == 0 && !node.IsTerminal)
            {
                MCTSNode bestChild = null;
                var bestScore = double.MinValue;
                foreach (var child in node.Children.Cast<MCTSNode>())
                {
                    var score = child.Score + _uctC * Math.Sqrt(2.0 * Math.Log(node.Visits) / child.Visits);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestChild = child;
                    }
                }
                
                node = bestChild;
                state.Play(node.Move);
            }

            return node;
        }

        private MCTSNode Expand(MCTSNode node, IState state)
        {
            if (node.UntriedMoves.Count > 0 && !state.GameOver)
            {
                var move = node.UntriedMoves.Dequeue();
                state.Play(move);
                var newChild = new MCTSNode(node, move, state);
                node.Children.Add(newChild);
                return newChild;
            }
            return node;
        }

        private void Rollout(IState state)
        {
            while (!state.GameOver)
            {
                state.Play(GetRandomMove(state));
            }
        }

        private void Backtrack(MCTSNode node, IState state)
        {
            while (node.Parent != null)
            {
                var score = state.GetScore(((MCTSNode)node.Parent).PlayerToMove);
                node.Score += (score - node.Score) / ++node.Visits;
                node = (MCTSNode)node.Parent;
            }

            node.Visits++;
        }

        private IMove GetRandomMove(IState initialState)
        {
            var moves = initialState.GetValidMoves().ToArray();
            return moves[_random.Next(moves.Count())];
        }

        public void OpponentMoved(IMove move)
        {
            if (_root != null)
            {
                _root = _root.Children.Cast<MCTSNode>().FirstOrDefault(c => c.Move.Equals(move));

                if (_root != null)
                {
                    _root.Parent = null;
                }
            }
        }
        
        public void Print(int depth = 0, int maxDepth = int.MaxValue, int maxBreadth = int.MaxValue)
        {
            Print(_root, depth, maxDepth, maxBreadth);
        }

        private void Print(Node node, int depth = 0, int maxDepth = int.MaxValue, int maxBreadth = int.MaxValue)
        {
            Console.WriteLine(new string('-', depth) + "O " + node);

            if (depth >= maxDepth)
            {
                return;
            }

            var children = node.Children
                .Cast<MCTSNode>()
                .OrderByDescending(c => c.Score)
                .Take(maxBreadth);

            foreach (var child in children)
            {
                Print(child, depth + 1, maxDepth, maxBreadth);
            }
        }

        public override string ToString()
        {
            return $"MCTS(UCT_C={Math.Round(_uctC, 2)})";
        }
    }
}