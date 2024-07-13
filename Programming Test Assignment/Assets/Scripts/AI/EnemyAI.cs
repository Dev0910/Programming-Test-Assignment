using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IAI
{
    public ObstacleData obstacleData;// Reference to the obstacle data
    public float moveSpeed = 2.0f;
    private bool isMoving;
    private AStarPathfinding pathfinding; // Reference to the A* pathfinding algorithm

    private void Start()
    {
        // Initialize the A* pathfinding with the obstacle data grid
        pathfinding = new AStarPathfinding(obstacleData.gridData);
        isMoving = false; // Initially, the enemy is not moving
    }

    // Method to move the enemy towards a specified target position
    public void MoveTowards(Vector3 targetPosition)
    {
        // If the enemy is already moving, exit the method
        if (isMoving) return;

        // Convert the current position and target position from world space to grid space
        Vector2Int start = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        Vector2Int target = new Vector2Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.z));

        // Find the best path from the start to the target position
        List<Vector2Int> bestPath = pathfinding.FindPath(start, target);

        // Check horizontally adjacent tiles for potentially shorter paths
        for (int x = -1; x <= 1; x += 2)
        {
            Vector2Int newTarget = target + new Vector2Int(x, 0);
            if (IsWithinBounds(newTarget))
            {
                List<Vector2Int> path = pathfinding.FindPath(start, newTarget);
                if (path.Count > 0 && path.Count < bestPath.Count)
                {
                    bestPath = path;
                }
            }
        }

        // Check vertically adjacent tiles for potentially shorter paths
        for (int y = -1; y <= 1; y += 2)
        {
            Vector2Int newTarget = target + new Vector2Int(0, y);
            if (IsWithinBounds(newTarget))
            {
                List<Vector2Int> path = pathfinding.FindPath(start, newTarget);
                if (path.Count > 0 && path.Count < bestPath.Count)
                {
                    bestPath = path;
                }
            }
        }

        // If a valid path is found, start moving along the path
        if (bestPath != null && bestPath.Count > 0)
        {
            StartCoroutine(MoveAlongPath(bestPath));
        }
    }

    // Coroutine to move the enemy along the calculated path
    private IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isMoving = true; // Set the moving flag to true

        // Iterate through each point in the path
        foreach (Vector2Int point in path)
        {
            // Convert the grid position back to world position
            Vector3 targetPosition = new Vector3(point.x, 1.33f, point.y);

            // Move towards the target position smoothly
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            // Ensure the position is set exactly to the target position
            transform.position = targetPosition;
        }

        isMoving = false; // Set the moving flag to false after reaching the destination
    }

    // Helper method to check if a position is within the grid bounds
    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < obstacleData.gridData.GetLength(0) &&
               position.y >= 0 && position.y < obstacleData.gridData.GetLength(1);
    }
}
