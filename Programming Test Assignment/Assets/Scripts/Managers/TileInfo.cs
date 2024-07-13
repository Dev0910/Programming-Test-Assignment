using UnityEngine;

public class TileInfo : MonoBehaviour
{
    //to store the position of the tile
    public Vector2Int position;

    //to update the position value
    public void SetPosition(int x, int y)
    {
        position = new Vector2Int(x, y);
    }
}
