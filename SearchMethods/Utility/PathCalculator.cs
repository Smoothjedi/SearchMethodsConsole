using SearchMethods.Data;

namespace SearchMethods.Utility
{
    public class PathCalculator
    {
        public static float CalculateTotalPathDistance(List<Node> path)
        {
            float totalDistance = 0;
            for (int i = 0; i < path.Count; i++)
            {
                Node node = path[i];
                if (i + 1 < path.Count)
                {
                    totalDistance += CalculateDistance(path[i], path[i + 1]);
                }
            }
            return totalDistance;
        }

        public static float CalculateDistance(Node startNode, Node endNode)
        {
            // Calculate the heuristic cost (e.g., Euclidean distance) from node to endNode.
            float dx = startNode.X - endNode.X;
            float dy = startNode.Y - endNode.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

    }
}
