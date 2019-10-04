using Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    public class State : IState
    {
        public int PlayerToMove => Round & 1;
        public int Round { get; private set; }
        public bool GameOver { get; private set; }

        private ulong[] _pieces = new ulong[]
        {
            0b0101010110101010010101010000000000000000000000000000000000000000,
            0b0000000000000000000000000000000000000000101010100101010110101010
        };
        private ulong[] _kings = new[] { 0UL, 0UL };
        private ulong[] _allPieces => new [] { _pieces[0] | _kings[0], _pieces[1] | _kings[1] };
        private int[] _pieceCounts = new [] { 12, 12 };
        private int[] _kingCounts = new[] { 0, 0 };
        private double _score;

        public void Play(IMove move)
        {
            if (!IsValid(move))
            {
                throw new ArgumentException("Invalid move " + move);
            }

            var m = (Move)move;
            var player = Round++ & 1;
            var opponent = player ^ 1;
            var dest = m.Last();
            var isKing = (_kings[player] & (1UL << dest)) != 0;

            _pieces[player] &= ~(1UL << m.First());
            _kings[player] &= ~(1UL << m.First());

            var from = m.First();
            foreach (var to in m.Skip(1))
            {
                var d = from - to; 
                if(d != -9 && d != 9 && d != 7 && d != -7)
                {
                    var bit = (1UL << (from - d / 2));
                    if ((_pieces[opponent] & bit) != 0)
                    {
                        _pieces[opponent] &= ~bit;
                        _pieceCounts[opponent]--;
                    }
                    else if ((_kings[opponent] & bit) != 0)
                    {
                        _kings[opponent] &= ~(1UL << (from - d / 2));
                        _kingCounts[opponent]--;
                    }
                }
                from = to;
            }
             
            if (isKing)
            {
                _kings[player] |= (1UL << dest);
            }
            else if (dest < 8 || dest > 55)
            {
                _kings[player] |= (1UL << dest);
                _pieceCounts[player]--;
                _kingCounts[player]++;
            }
            else
            {
                _pieces[player] |= (1UL << dest);
            }

            if (!GetValidMoves().Any())
            {
                GameOver = true;
                _score = player ^ 1;
            }
        }

        public bool IsValid(IMove move)
        {
            var player = Round & 1; 
            var captureMoves = GetCaptureMoves(player).ToList();
            return captureMoves.Count > 0
                ? captureMoves.Contains(move)
                : GetMoves(player).Contains(move);
        }

        public IEnumerable<IMove> GetValidMoves()
        {
            var player = Round & 1;
            var captureMoves = GetCaptureMoves(player).ToList();
            return captureMoves.Any() ? captureMoves
                : GetMoves(player).ToList();
        }

        private IEnumerable<IMove> GetMoves(int player)
        {
            for (int i = 0; i < 64; i++)
            {
                var isPiece = (_pieces[player] & (1UL << i)) != 0;
                var isKing = (_kings[player] & (1UL << i)) != 0;

                if (!isPiece && !isKing)
                {
                    continue;
                }
                 
                var moves = GetMoves(player, i, isKing);

                foreach (var move in moves)
                {
                    yield return move;
                }
            }
        }

        private IEnumerable<IMove> GetMoves(int player, int position, bool isKing)
        {
            var directions = GetDirections(player, position, isKing, 1);
            foreach (var d in directions)
            {
                if (position + d >= 0 && position + d < 64 && ((_allPieces[0] | _allPieces[1]) & (1UL << (position + d))) == 0)
                {
                    yield return new Move { position, position + d };
                }
            }
        }

        private IEnumerable<IMove> GetCaptureMoves(int player)
        {   
            for (int i = 0; i < 64; i++)
            {
                var isPiece = (_pieces[player] & (1UL << i)) != 0;
                var isKing = (_kings[player] & (1UL << i)) != 0;

                if (!isPiece && !isKing)
                {
                    continue;
                }

                var captureMoves = GetCaptureMoves(player, i, isKing);

                foreach (var captureMove in captureMoves)
                {
                    yield return captureMove;
                }
            } 
        }

        private IEnumerable<IMove> GetCaptureMoves(int player, int position, bool isKing)
        {
            var captureMoves = GetDirectCaptureMoves(player, position, isKing).Select(m => new Move { position, m });
            var queue = new Queue<Move>(captureMoves);

            while (queue.Any())
            {
                var captureMove = queue.Dequeue();
                var followupCaptures = GetDirectCaptureMoves(player, captureMove.Last(), isKing);
                var validFollowupCaptures = followupCaptures.Where(x => !captureMove.Contains(x)).ToList();
                if (validFollowupCaptures.Any())
                {
                    foreach (var followupCapture in validFollowupCaptures)
                    { 
                        var newCaptureMove = new Move();
                        newCaptureMove.AddRange(captureMove);
                        newCaptureMove.Add(followupCapture);
                        queue.Enqueue(newCaptureMove); 
                    }
                }
                else
                {
                    yield return captureMove;
                }
            }
        }

        private IEnumerable<int> GetDirectCaptureMoves(int player, int position, bool isKing)
        { 
            var directions = GetDirections(player, position, isKing, 2);
            foreach (var d in directions)
            {
                if (position + d * 2 >= 0 && position + d * 2 < 64
                    && (_allPieces[player ^ 1] & (1UL << (position + d))) != 0
                    && ((_allPieces[0] | _allPieces[1]) & (1UL << (position + d * 2))) == 0)
                {
                    yield return position + d * 2;
                }
            }
        }

        private static List<int> GetDirections(int player, int position, bool isKing, int distance)
        {
            var possibleDirections = new List<int>();

            if (position % 8 >= distance)
            {
                if (player == 0 || isKing)
                {
                    possibleDirections.Add(-9);
                }
                if (player == 1 || isKing)
                {
                    possibleDirections.Add(7);
                }
            }
            if (position % 8 < 8 - distance)
            {
                if (player == 0 || isKing)
                {
                    possibleDirections.Add(-7);
                }
                if (player == 1 || isKing)
                {
                    possibleDirections.Add(9);
                }
            }

            return possibleDirections;
        }

        public double GetScore(int playerNr)
        {
            var score = _score; 

            if (!GameOver)
            {
                var pieceRatio = 0.5 + (_pieceCounts[0] - _pieceCounts[1]) / 24.0;
                var kingRatio = 0.5 + (_kingCounts[0] - _kingCounts[1]) / 24.0;
                score = 0.4 * pieceRatio + 0.6 * kingRatio;
            }

            return playerNr == 0 ? score : 1 - score;
        }

        public void Set(IState state)
        {
            var s = (State)state;
            Round = s.Round;
            GameOver = s.GameOver;
            _pieces = s._pieces.ToArray();
            _kings = s._kings.ToArray();
            _pieceCounts = s._pieceCounts.ToArray();
            _kingCounts = s._kingCounts.ToArray();
            _score = s._score;
        }

        public void Set(int round, List<int>[] pieces, List<int>[] kings)
        {
            Round = round;
            GameOver = false;
            _score = 0.5;
            _pieces = pieces.Select(x => x.Select(y => (1UL << y)).Aggregate(0UL, (y1, y2) => y1 | y2)).ToArray();
            _kings = kings.Select(x => x.Select(y => (1UL << y)).Aggregate(0UL, (y1, y2) => y1 | y2)).ToArray();
            _pieceCounts = pieces.Select(x => x.Count).ToArray();
            _kingCounts = kings.Select(x => x.Count).ToArray();
        }

        public IState Clone()
        {
            var state = new State();
            state.Set(this);
            return state;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 64; i++)
            {
                var p0 = (_pieces[0] & (1UL << i)) != 0;
                var p1 = (_pieces[1] & (1UL << i)) != 0;
                var k0 = (_kings[0] & (1UL << i)) != 0;
                var k1 = (_kings[1] & (1UL << i)) != 0;

                builder.Append(p0 ? 'x' : p1 ? 'o' : k0 ? 'X' : k1 ? 'O' : '.');

                if ((i + 1) % 8 == 0)
                {
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }
    }
}