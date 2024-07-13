using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public ObstacleData obstacleData;
    public float moveSpeed = 2.0f;
    private bool isMoving;
    private AStarPathfinding pathfinding;// Reference to the A* pathfinding algorithm
    private EnemyAI enemyAI;

    private void Start()
    {
        // Initialize the A* pathfinding with the obstacle data grid
        pathfinding = new AStarPathfinding(obstacleData.gridData);
        isMoving = false;
        //Find the enemy ai
        enemyAI = FindObjectOfType<EnemyAI>();
    }

    private void Update()
    {
        //if left mouse pressed and player is not moving
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            //cast a ray to cheak if player click on a tile
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //store the x,y cordinates of the tile
                int x = Mathf.RoundToInt(hit.point.x);
                int y = Mathf.RoundToInt(hit.point.z);
                //cheak if tile does not have a obstical
                if (!obstacleData.gridData[x, y])
                {
                    //store the start and target position
                    Vector2Int start = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));//find the current posistion by rounding up the to int
                    Vector2Int target = new Vector2Int(x, y);

                    List<Vector2Int> path = pathfinding.FindPath(start, target);//try to find a path using the A* algorithm

                    if (path != null)
                    {
                        //start moving to that point if path found
                        StartCoroutine(MoveAlongPath(path));
                    }
                }
            }
        }
    }

    private IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isMoving = true;

        foreach (Vector2Int point in path)
        {
            Vector3 targetPosition = new Vector3(point.x, 1.33f, point.y); // convert grid position to world position

            // Move towards the target position until the unit is close enough
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            transform.position = targetPosition; // to make the unit is the target position
        }

        isMoving = false; // when finished moving

        enemyAI?.MoveTowards(transform.position); // If enemy AI exists, move it towards the unit's current position
    }

}
