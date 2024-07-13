using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public Vector2Int gridSize = new Vector2Int(10,10);

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 position = new Vector3(x, 0, y);                                        //store the position of the current tile
                GameObject newCube = Instantiate(cubePrefab, position, Quaternion.identity);    //spawn the tile
                newCube.transform.parent = this.transform;                                      //set the parent as this gameobject
                newCube.GetComponent<TileInfo>().SetPosition(x, y);                             //update the position in the tileinfo script
            }
        }
    }
}
