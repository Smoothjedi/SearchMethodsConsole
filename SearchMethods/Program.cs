using System.Diagnostics;
using SearchMethods.Algorithms;
using SearchMethods.Data;
using SearchMethods.Factories;
using SearchMethods.Utility;

namespace SearchMethods
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Load cities and coordinates from a CSV file
            var coordinates = Path.Combine(Environment.CurrentDirectory,"coordinates.csv");
            var adjacencies = Path.Combine(Environment.CurrentDirectory,"Adjacencies.txt");
            var cities = NodeFactory.LoadCitiesFromCSV(coordinates, adjacencies);

            // Create an instance of A* pathfinding
            var aStarPathfinder = new AStar();
            var breadthFirst = new BreadthFirst();
            var depthFirst = new DepthFirst();
            var idDepthFirst = new ID_DepthFirst();
            var bestFirst = new BestFirst();

            DisplayCities(cities);
            Node startNode = InputCity(cities, "Please enter your start city: ");
            Node endNode = InputCity(cities, "Please enter your end city: ");
            Console.WriteLine();

            int searchType;
            do
            {
                do
                {
                    Console.WriteLine("Please select your search type (Searches have a 60s timeout): ");
                    Console.WriteLine("Brute Force options:");
                    Console.WriteLine(" 1: Breadth First");
                    Console.WriteLine(" 2: Depth First");
                    Console.WriteLine(" 3: Limited Depth First");
                    Console.WriteLine("Guided Options:");
                    Console.WriteLine(" 4: Best First");
                    Console.WriteLine(" 5: A*");
                    Console.WriteLine("6: Clear screen and select new cities");
                    Console.WriteLine("7: Quit ");
                    Console.WriteLine();
                    string searchTypeInput = Console.ReadLine();
                    if (!(int.TryParse(searchTypeInput, out searchType) && searchType > 0 && searchType < 8))
                    {
                        Console.WriteLine($"Sorry, {searchTypeInput} is not a valid input");
                        searchType = 0;
                    }
                }
                while (searchType == 0);

                var path = new List<Node>();
                Stopwatch sw = new Stopwatch();
                var timeout = 60000;
                var tokenSource = new CancellationTokenSource(millisecondsDelay: timeout);
                var token = tokenSource.Token;

                if (startNode != null && endNode != null)
                {
                    try
                    {
                        // Find the path
                        switch (searchType)
                        {
                            case 1:
                                sw.Start();
                                var breadthPathTask = Task.Run(() => breadthFirst.FindPath(startNode, endNode), token);
                                sw.Stop();
                                PrintOutputPathAndTime(await breadthPathTask, "Breadth first", sw.ElapsedTicks);
                                break;
                            case 2:
                                sw.Start();
                                var depthPathTask = Task.Run(() => path = depthFirst.FindPath(startNode, endNode), token);
                                sw.Stop();
                                PrintOutputPathAndTime(await depthPathTask, "Depth first", sw.ElapsedTicks);
                                break;
                            case 3:
                                int depth = 0;
                                do
                                {
                                    Console.WriteLine("Please input the depth you'd like to limit to (Integer > 0): ");
                                    string inputDepth = Console.ReadLine();
                                    if (int.TryParse(inputDepth, out depth) && depth > 0)
                                    {
                                        sw.Start();
                                        var limitedDepthPathTask = Task.Run(() => idDepthFirst.FindPath(startNode, endNode, depth), token);
                                        sw.Stop();
                                        PrintOutputPathAndTime(await limitedDepthPathTask, "Limited Depth first", sw.ElapsedTicks);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Sorry, not a valid depth. Please try again.");
                                        depth = 0;
                                    }
                                }
                                while (depth == 0);
                                break;
                            case 4:
                                sw.Start();
                                var bestFirstTask = Task.Run(() => bestFirst.FindPath(startNode, endNode), token);
                                sw.Stop();
                                PrintOutputPathAndTime(await bestFirstTask, "Best first", sw.ElapsedTicks);
                                break;
                            case 5:
                                sw.Start();
                                var aStarPathTask = Task.Run(() => aStarPathfinder.FindPath(startNode, endNode), token);
                                sw.Stop();
                                PrintOutputPathAndTime(await aStarPathTask, "A*", sw.ElapsedTicks);
                                break;
                            case 6:
                                Console.Clear();
                                DisplayCities(cities);
                                startNode = InputCity(cities, "Please enter your start city: ");
                                endNode = InputCity(cities, "Please enter your end city: ");
                                break;
                            default:
                                Console.WriteLine("Exiting...");
                                break;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Sorry, your search timed out as it exceeded {timeout} milliseconds");
                        Console.WriteLine("----------------------------------");
                    }

                }
            }
            while (searchType != 7);
        }

        private static Node InputCity(List<Node> cities, string inputPrompt)
        {
            string startCity = string.Empty;
            Node startNode;
            do
            {
                Console.WriteLine(inputPrompt);
                startCity = Console.ReadLine();
                startNode = cities.FirstOrDefault(city =>
                    string.Equals(city.CityName, startCity, StringComparison.OrdinalIgnoreCase));

                if (startNode == null)
                {
                    Console.WriteLine($"Sorry, {startCity} is not a valid city. ");
                    startCity = string.Empty;
                }
            }
            while (string.IsNullOrWhiteSpace(startCity));
            return startNode;
        }

        private static void DisplayCities(List<Node> cities)
        {
            Console.WriteLine("The following cities were loaded: ");
            int i = 0;
            foreach (var c in cities)
            {
                if (i > 6)
                {
                    Console.WriteLine();
                    i = 0;
                }

                Console.Write($"{c.CityName}");
                if (c != cities.Last())
                { Console.Write(", "); }
                i++;
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void PrintOutputPathAndTime(List<Node> path, string searchType, long ticksElapsed)
        {
            if (path != null && path.Any())
            {
                Console.WriteLine($"{searchType} path found over {ticksElapsed} ticks (10,000 ticks = 1ms):");
                foreach (Node node in path)
                {
                    Console.WriteLine($"City: {node.CityName}, X: {node.X}, Y: {node.Y}");
                }
                Console.WriteLine();
                Console.WriteLine($"Total distance covered was {PathCalculator.CalculateTotalPathDistance(path)}");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"No path could be found over {ticksElapsed} ticks (10,000 ticks = 1ms).");
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------");
        }
    }
}

