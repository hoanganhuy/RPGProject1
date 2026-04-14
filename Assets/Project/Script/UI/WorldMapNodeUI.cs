using UnityEngine;

public class WorldMapNodeUI : MonoBehaviour
{
    public string pointId;
    public LocationData location;
    public GameManager gameManager;

    public void OnClickNode()
    {
        if (location == null)
        {
            Debug.Log("Waypoint - cannot travel directly");
            return;
        }

        gameManager.StartTravel(location);
    }

}