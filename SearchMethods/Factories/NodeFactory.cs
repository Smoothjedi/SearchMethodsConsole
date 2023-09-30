using SearchMethods.Data;

namespace SearchMethods.Factories
{
    public class NodeFactory
    {
        public static List<Node> LoadCitiesFromCSV(string nodeFilePath, string adjacencies)
        {
            List<Node> cities = new List<Node>();

            try
            {
                using (StreamReader nodeReader = new StreamReader(nodeFilePath))
                {
                    string line;
                    while ((line = nodeReader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3 && float.TryParse(parts[1], out float x) && float.TryParse(parts[2], out float y))
                        {
                            Node city = new Node(parts[0].Trim(), x, y);
                            cities.Add(city);
                        }
                    }
                }

                using (StreamReader adjReader = new StreamReader(adjacencies))
                {
                    string line;
                    while ((line = adjReader.ReadLine()) != null)
                    {
                        string[] parts = line.Trim().Split(' ');
                        if (parts.Length == 2)
                        {
                            var foundNode = cities.Where(x => x.CityName.Equals(parts[0].Trim())).First();
                            var foundNeighbor = cities.Where(x => x.CityName.Equals(parts[1].Trim())).First();
                            //Console.WriteLine($"foundNode: {foundNode.CityName} foundNeighbor: {foundNeighbor.CityName}");
                            foundNode.Neighbors.Add(foundNeighbor);
                            foundNeighbor.Neighbors.Add(foundNode);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading cities from CSV: {e.Message}");
            }

            return cities;
        }
    }
}
