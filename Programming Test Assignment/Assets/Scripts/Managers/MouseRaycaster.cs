using UnityEngine;
using TMPro;

public class MouseRaycaster : MonoBehaviour
{
    public TextMeshProUGUI positionText;

    void Update()
    {
        //create a ray from the camera to the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; // to store the objects it hit

        //when the ray hits a object
        if (Physics.Raycast(ray, out hit))
        {
            //try to get the TileInfo component on the object hit and store it in var
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            //update the text to the posistion of the tile if the tile is not empty
            if (tileInfo != null)
            {
                positionText.text = "Position: (" + tileInfo.position.x + ", " + tileInfo.position.y + ")";
            }
        }
        //else set the text value to default
        else
        {
            positionText.text = "Position: N/A";
        }
    }
}
