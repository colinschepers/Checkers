using System;
using System.Threading;

namespace Core.Arithmetic
{
    public static class RandomGenerator
    {
        private static int seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random _random;
        public static Random Instance => _random ?? (_random = new Random(Interlocked.Increment(ref seedCounter)));
    }
}
