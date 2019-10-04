using Core.Games;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    public class Move : List<int>, IMove
    {
        public override bool Equals(object obj)
        {
            return obj is Move move && Count == Count 
                && this.Select((x, i) => x == move[i]).All(x => x);
        }

        public override int GetHashCode()
        {
            return this.Sum(x => 107 * x).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0})", string.Join(",", this));
        }
    }
}