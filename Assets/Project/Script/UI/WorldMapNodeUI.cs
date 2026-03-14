using UnityEngine;

public class WorldMapNodeUI : MonoBehaviour
{
    public LocationData location;
    public GameManager gameManager;

    public void OnClickNode()
    {
        gameManager.StartTravel(location);
    }
}