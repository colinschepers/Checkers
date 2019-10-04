using System.Collections.Generic;

namespace Core.DataStructures
{
    public class Node
    {
        public int Depth { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }

        public Node() : this(null)
        {
        }

        public Node(Node parent)
        {
            Depth = parent == null ? 0 : parent.Depth + 1;
            Parent = parent;
            Children = new List<Node>();
        }

        public void ResetDepth()
        {
            Depth = Parent == null ? 0 : Parent.Depth + 1;
            Children.ForEach(c => c.ResetDepth());
        }
    }
}
