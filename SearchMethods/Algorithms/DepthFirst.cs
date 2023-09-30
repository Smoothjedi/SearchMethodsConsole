using SearchMethods.Data;

namespace SearchMethods.Algorithms
{
    public class DepthFirst
    {

        public List<Node> FindPath(Node startNode, Node endNode)
        {
            var stack = new Stack<Node>();
            var visited = new HashSet<Node>();

            startNode.Parent = null;
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                Node currentNode = stack.Pop();

                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                visited.Add(currentNode);

                foreach (Node neighbor in currentNode.Neighbors)
                {
                    if (!visited.Contains(neighbor) && !neighbor.IsObstacle)
                    {
                        neighbor.Parent = currentNode;
                        stack.Push(neighbor);
                    }
                }
            }

            return null; // No path found
        }
        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}
