using System;
using System.Collections.Generic;

namespace Core.Spatial
{
    public class Geometry
    {
        /// <summary>
        /// Checks if a coordinate lies inside a convex polygon in a 2D plane.
        /// Source: https://algorithmtutor.com/Computational-Geometry/Check-if-a-point-is-inside-a-polygon/
        /// </summary>
        /// <param name="coordinate">The coordinate to check</param>
        /// <param name="convex">A list of coordinates of the corners of the convex polygon ordered in a
        /// clockwise or counter clockwise fashion. </param>
        /// <returns>True when the coordinate is inside or on the border of the convex polygon, False otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown when the convex list is of size less than 3.</exception>
        public static bool IsInsideConvex(Coordinate coordinate, List<Coordinate> convex)
        {
            coordinate = coordinate ?? throw new ArgumentNullException("coordinate");
            convex = convex ?? throw new ArgumentNullException("convex");

            if (convex.Count < 3)
            {
                throw new ArgumentException("The list should be at least of size 3.", "convex");
            }

            bool? prev = null;
            
            for (int i = 0; i < convex.Count; i++)
            {
                var p1 = convex[i];
                var p2 = convex[(i + 1) % convex.Count];

                var a = -(p2.Y - p1.Y);
                var b = p2.X - p1.X;
                var c = -(a * p1.X + b * p1.Y);
                var d = a * coordinate.X + b * coordinate.Y + c;

                if (prev.HasValue && prev.Value != (d >= 0))
                {
                    return false;
                }

                prev = (d >= 0);
            }

            return true;
        }
    }
}
