using UnityEngine;

public class WorldMapUI : MonoBehaviour
{
    public TravelManager travelManager;

    public void OnClickLocation(LocationData target)
    {
        travelManager.StartTravel(target);
    }
}