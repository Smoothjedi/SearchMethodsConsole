using SearchMethods.Data;

namespace SearchMethods.Algorithms
{
    public class AStar
    {
        public List<Node> FindPath(Node startNode, Node endNode)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbor in currentNode.Neighbors)
                {
                    if (closedSet.Contains(neighbor) || neighbor.IsObstacle)
                    {
                        continue;
                    }

                    float newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, endNode);
                        neighbor.Parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
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

        private float GetDistance(Node nodeA, Node nodeB)
        {
            float distX = Math.Abs(nodeA.X - nodeB.X);
            float distY = Math.Abs(nodeA.Y - nodeB.Y);
            return distX + distY;
        }
    }
}
