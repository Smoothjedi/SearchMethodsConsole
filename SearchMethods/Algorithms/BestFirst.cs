using SearchMethods.Data;
using SearchMethods.Utility;

namespace SearchMethods.Algorithms
{
    public class BestFirst
    {

        public List<Node> FindPath(Node startNode, Node endNode)
        {
            if (BestFirstTraversal(startNode, endNode))
            {
                return BuildPath(startNode, endNode); // Build and return the complete path
            }
            return null; // Path not found
        }

        private bool BestFirstTraversal(Node startNode, Node endNode)
        {
            HashSet<Node> visited = new HashSet<Node>();
            PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();

            priorityQueue.Enqueue(startNode, PathCalculator.CalculateDistance(startNode, endNode));

            while (priorityQueue.Count > 0)
            {
                Node currentNode = priorityQueue.Dequeue();

                if (currentNode == endNode)
                {
                    return true; // Path found
                }

                visited.Add(currentNode);

                foreach (Node neighbor in currentNode.Neighbors)
                {
                    if (!visited.Contains(neighbor) && !neighbor.IsObstacle)
                    {
                        float heuristic = PathCalculator.CalculateDistance(neighbor, endNode);
                        priorityQueue.Enqueue(neighbor, heuristic);
                        neighbor.Parent = currentNode; // Store the parent node to reconstruct the path
                    }
                }
            }

            return false; // Path not found
        }

        private List<Node> BuildPath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.Parent;
            }

            return path;
        }
    }

    // Priority Queue implementation (unchanged)
    public class PriorityQueue<T>
    {
        private List<(T item, float priority)> elements = new List<(T item, float priority)>();

        public int Count => elements.Count;

        public void Enqueue(T item, float priority)
        {
            elements.Add((item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].priority < elements[bestIndex].priority)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

}

