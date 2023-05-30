using flashPriceFx.MiniMarket;
using flashPriceFx.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace flashPrice
{
    public partial class default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var minimarkets = new List<Node>();

                // Define minimarkets with coordinates
                BOMiniMarketList miniMarketList = BLLMiniMarket.getListAllMiniMarket();


                // Retrieve live coordinates (example: latitude = 12.3456, longitude = -98.7654)
                double liveLatitude = -6.177437;
                double liveLongitude = 106.621188;

                // Create startMinimarket using live coordinates
                var startMinimarket = new Node("Start Minimarket", liveLatitude, liveLongitude);
                minimarkets.Add(startMinimarket);

                // Connect minimarkets based on categories
                for (int i = 0; i < minimarkets.Count; i++)
                {
                    var currentMinimarket = minimarkets[i];

                    for (int j = i + 1; j < minimarkets.Count; j++)
                    {
                        var neighborMinimarket = minimarkets[j];

                        var distance = HaversineDistance(currentMinimarket.Latitude, currentMinimarket.Longitude, neighborMinimarket.Latitude, neighborMinimarket.Longitude);

                        if (distance <= 100) // Category 1: Distance <= 1 km
                        {
                            currentMinimarket.Neighbors.Add(neighborMinimarket);
                            neighborMinimarket.Neighbors.Add(currentMinimarket);
                        }
                        else if (distance <= 200) // Category 2: 1 km < Distance <= 2 km
                        {
                            currentMinimarket.Neighbors.Add(neighborMinimarket);
                        }
                        else if (distance <= 300) // Category 3: 2 km < Distance <= 3 km
                        {
                            currentMinimarket.Neighbors.Add(neighborMinimarket);
                        }
                        else // Category 4: 3 km < Distance
                        {
                            // No connection between minimarkets in this category
                        }
                    }
                }

                var goalMinimarket = minimarkets[3];

                var path = BestFirstSearch.Search(startMinimarket, goalMinimarket);

                // Display the result
                ResultLiteral.Text = "Best-First Search Path:<br/>";
                ResultLiteral.Text += "Start Minimarket -> ";

                foreach (var node in path)
                {
                    ResultLiteral.Text += node.Name + " -> ";
                }

                ResultLiteral.Text = ResultLiteral.Text.TrimEnd('-', '>');

                // Register JavaScript function to draw the graph
                ScriptManager.RegisterStartupScript(this, GetType(), "DrawGraph", "drawGraph();", true);
            }
        }

        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in km

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c;

            return distance;
        }

        private double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Node> Neighbors { get; set; }

        public Node(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Neighbors = new List<Node>();
        }
    }

    public static class BestFirstSearch
    {
        public static List<Node> Search(Node start, Node goal)
        {
            var visited = new HashSet<Node>();
            var path = new List<Node>();
            var priorityQueue = new SortedList<double, Node>();

            priorityQueue.Add(0, start);

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Values[0];
                priorityQueue.RemoveAt(0);

                visited.Add(current);

                if (current == goal)
                {
                    path = ReconstructPath(start, goal);
                    break;
                }

                foreach (var neighbor in current.Neighbors)
                {
                    if (!visited.Contains(neighbor) && !priorityQueue.ContainsValue(neighbor))
                    {
                        var priority = HaversineDistance(current.Latitude, current.Longitude, neighbor.Latitude, neighbor.Longitude);
                        priorityQueue.Add(priority, neighbor);
                    }
                }
            }

            return path;
        }

        private static List<Node> ReconstructPath(Node start, Node goal)
        {
            var path = new List<Node>();
            var current = goal;

            while (current != start)
            {
                path.Insert(0, current);
                current = GetLowestCostNeighbor(current);
            }

            path.Insert(0, start);

            return path;
        }

        private static Node GetLowestCostNeighbor(Node node)
        {
            var lowestCost = double.MaxValue;
            Node lowestCostNode = null;

            foreach (var neighbor in node.Neighbors)
            {
                var cost = HaversineDistance(node.Latitude, node.Longitude, neighbor.Latitude, neighbor.Longitude);

                if (cost < lowestCost)
                {
                    lowestCost = cost;
                    lowestCostNode = neighbor;
                }
            }

            return lowestCostNode;
        }

        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in km

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c;

            return distance;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}

        