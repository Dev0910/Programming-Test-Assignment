using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public ObstacleData obstacleData;
    [SerializeField] Vector3 playerStartPos;
    [SerializeField] Vector3 enemyStartPos;

    private void Start()
    {
        //to spawn the player and the enemy
        GameObject player = Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
        GameObject enemy = Instantiate(enemyPrefab, enemyStartPos, Quaternion.identity);

        // Assign obstacle data to player and enemy
        player.GetComponent<PlayerController>().obstacleData = obstacleData;
        enemy.GetComponent<EnemyAI>().obstacleData = obstacleData;
    }
}
