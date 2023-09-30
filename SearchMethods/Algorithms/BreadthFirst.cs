using SearchMethods.Data;

namespace SearchMethods.Algorithms
{
    public class BreadthFirst
    {
        public List<Node> FindPath(Node startNode, Node endNode)
        {
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            startNode.Parent = null;
            queue.Enqueue(startNode);
            visited.Add(startNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbor in currentNode.Neighbors)
                {
                    if (!visited.Contains(neighbor) && !neighbor.IsObstacle)
                    {
                        neighbor.Parent = currentNode;
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
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