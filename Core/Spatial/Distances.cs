using System;

namespace Core.Spatial
{
    public class Distances
    {
        public static double ManhattanDistance(Coordinate first, Coordinate second)
        {
            return Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);
        }

        public static double EuclideanDistance(Coordinate first, Coordinate second)
        {
            var dX = first.X - second.X;
            var dY = first.Y - second.Y;
            return Math.Sqrt(dX*dX + dY*dY);
        }
    }
}
