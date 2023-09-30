namespace SearchMethods.Data
{
    public class Node
    {
        public string CityName { get; }
        public float X { get; }
        public float Y { get; }
        public bool IsObstacle { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost => GCost + HCost;
        public Node Parent { get; set; }
        public List<Node> Neighbors { get; set; }

        public Node(string cityName, float x, float y)
        {
            CityName = cityName;
            X = x;
            Y = y;
            Neighbors = new List<Node>();
        }
    }
}