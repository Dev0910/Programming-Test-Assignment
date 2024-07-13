using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;//prefab of the obstical

    void Start()
    {
        if (obstacleData == null)
        {
            Debug.LogError("ObstacleData is not assigned!");
            return;
        }
        // Generate Obstacles if obstavle data not empty
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        Debug.Log("Generating obstacles...");
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                //spawn obsyacle if bool at x,y true
                if (obstacleData.gridData[x, y])
                {
                    Vector3 position = new Vector3(x, 1.33f, y); // store the position of the obstical
                    GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);//spawn the obstical
                    obstacle.name = $"Obstacle_{x}_{y}";//rename the obstical
                    Debug.Log($"Obstacle instantiated at ({x}, {y})");
                }
            }
        }
    }
}
