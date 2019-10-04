using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class CollectionsExtensions
    {
        private static Random _random = new Random();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(item => _random.Next());
        }
    }
}
