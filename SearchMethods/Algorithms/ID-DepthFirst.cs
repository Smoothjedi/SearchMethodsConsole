using SearchMethods.Data;

namespace SearchMethods.Algorithms
{
    public class ID_DepthFirst
    {
        private List<Node> path;

        public List<Node> FindPath(Node startNode, Node endNode, int maxDepth)
        {
            path = new List<Node>();
            if (DepthLimitedSearch(startNode, endNode, maxDepth))
            {
                path.Add(startNode);
                path.Reverse();
                return path;
            }
            return null; // Path not found
        }

        private bool DepthLimitedSearch(Node currentNode, Node endNode, int depth)
        {
            if (depth < 0)
            {
                return false;
            }
            if (currentNode == endNode)
            {
                path.Add(currentNode);
                return true;
            }

            foreach (Node neighbor in currentNode.Neighbors)
            {
                if (!neighbor.IsObstacle)
                {
                    if (DepthLimitedSearch(neighbor, endNode, depth - 1))
                    {
                        path.Add(currentNode);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
