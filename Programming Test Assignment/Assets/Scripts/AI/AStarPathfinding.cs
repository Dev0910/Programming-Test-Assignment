using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    // Class to represent each node in the grid
    public class Node
    {
        public Vector2Int Position; // Position in the grid
        public Node Parent; // Parent node for path reconstruction
        public int GCost; // Cost from the start node to this node
        public int HCost; // Heuristic cost from this node to the target node
        public int FCost => GCost + HCost; // Total cost (G + H)

        public Node(Vector2Int position, Node parent)
        {
            Position = position;
            Parent = parent;
        }
    }

    private bool[,] obstacleGrid;
    private int gridWidth;
    private int gridHeight;

    // Constructor initializes the pathfinding with the given obstacle grid
    public AStarPathfinding(bool[,] obstacleGrid)
    {
        this.obstacleGrid = obstacleGrid;
        gridWidth = obstacleGrid.GetLength(0);
        gridHeight = obstacleGrid.GetLength(1);
    }

    // Helper function to calculate the Manhattan distance between two points
    private int GetDistance(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return dx + dy;
    }

    // Function to find a path from start to target positions
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Node> openList = new List<Node>(); // Nodes to be evaluated
        HashSet<Node> closedList = new HashSet<Node>(); // Nodes already evaluated
        Node startNode = new Node(start, null);
        Node targetNode = new Node(target, null);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Find the node with the lowest F cost in the open list
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If the current node is the target node and reconstruct the path
            if (currentNode.Position == target)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                Node current = currentNode;
                while (current != null)
                {
                    path.Add(current.Position);
                    current = current.Parent;
                }
                path.Reverse(); // Reverse the path to start from the beginning
                return path;
            }

            // Explore the neighbours of the current node
            foreach (Vector2Int neighbourPos in GetNeighbours(currentNode.Position))
            {
                // Skip if it's an obstacle or already evaluated
                if (obstacleGrid[neighbourPos.x, neighbourPos.y] || closedList.Contains(new Node(neighbourPos, null)))
                {
                    continue;
                }

                Node neighbourNode = new Node(neighbourPos, currentNode);
                neighbourNode.GCost = currentNode.GCost + 1;
                neighbourNode.HCost = GetDistance(neighbourPos, target);

                if (!openList.Contains(neighbourNode))
                {
                    openList.Add(neighbourNode);
                }
            }
        }

        // If we exhaust the open list without finding a path, return null
        return null;
    }

    // Helper function to get the valid neighbouring positions for a given position
    private List<Vector2Int> GetNeighbours(Vector2Int position)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // Check the bounds and add valid neighbours
        if (position.x - 1 >= 0) neighbours.Add(new Vector2Int(position.x - 1, position.y));
        if (position.x + 1 < gridWidth) neighbours.Add(new Vector2Int(position.x + 1, position.y));
        if (position.y - 1 >= 0) neighbours.Add(new Vector2Int(position.x, position.y - 1));
        if (position.y + 1 < gridHeight) neighbours.Add(new Vector2Int(position.x, position.y + 1));

        return neighbours;
    }
}
