using System;
using System.Collections.Generic;

namespace Core.Algorithms
{
    /// <summary>
    /// A* algorithm to find the optimal path from a start to goal state.
    /// </summary>
    /// <typeparam name="T">Generic type describing the nodes in the graph. Must properly implement Equals() method.</typeparam>
    public class AStar<T>
    {
        private Func<T, List<T>> GetNeighbors;
        private Func<T, T, double> GetCost;
        private Func<T, T, double> GetHeuristicCost;

        public AStar(Func<T, List<T>> neighborFunction, Func<T, T, double> costFunction, Func<T, T, double> heuristicFunction)
        {
            GetNeighbors = neighborFunction;
            GetCost = costFunction;
            GetHeuristicCost = heuristicFunction;
        }

        public List<T> GetShortestPath(T start, T goal, out double cost)
        {
            var openSet = new HashSet<T>();
            var cameFrom = new Dictionary<T, T>();
            var costs = new Dictionary<T, double>();
            var expectedCosts = new Dictionary<T, double>();
            
            cameFrom[start] = start;
            costs[start] = 0;
            expectedCosts[start] = GetHeuristicCost(start, goal);

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                T current = default(T);
                var minExpectedCost = double.MaxValue;
                
                foreach (var item in openSet)
                {
                    var expectedCost = expectedCosts[item];
                    if (expectedCost < minExpectedCost)
                    {
                        current = item;
                        minExpectedCost = expectedCost;
                    }
                }

                openSet.Remove(current);

                if (current.Equals(goal))
                {
                    break;
                }
                
                foreach (var neighbor in GetNeighbors(current))
                {
                    var newCost = costs[current] + GetCost(current, neighbor);
                    if (!costs.ContainsKey(neighbor) || newCost < costs[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        costs[neighbor] = newCost;
                        expectedCosts[neighbor] = newCost + GetHeuristicCost(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            cost = costs.ContainsKey(goal) ? costs[goal] : double.MaxValue;

            var path = new List<T>();
            while (cameFrom.ContainsKey(goal) && !goal.Equals(start))
            {
                path.Insert(0, goal);
                goal = cameFrom[goal];
            }
            
            return path;
        }
    }
}
