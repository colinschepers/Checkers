using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Arithmetic
{
    public static class PrimeGenerator
    {
        public static IEnumerable<int> Generate(int lowerLimit, int upperLimit)
        {
            if (lowerLimit < 0)
            {
                throw new ArgumentException("lowerLimit be must greater than or equal to 0.");
            }
            if (upperLimit < 2)
            {
                throw new ArgumentException("upperLimit be must greater than or equal to 2.");
            }
            if (upperLimit < lowerLimit)
            {
                throw new ArgumentException("upperLimit be must greater than lowerLimit.");
            }

            var primes = Enumerable.Repeat(true, upperLimit).ToArray();
            for (int i = 2; i < upperLimit; i++)
            {
                if (primes[i])
                {
                    if (i >= lowerLimit)
                    {
                        yield return i;
                    }

                    if (i < Math.Sqrt(upperLimit))
                    {
                        for (int j = i * i; j < upperLimit; j += i)
                        {
                            primes[j] = false;
                        }
                    }
                }
            }
        }
    }
}
