using UnityEngine;

public class LocationMapController : MonoBehaviour
{
    public LocationData locationData;
    public GameManager gameManager;
    public void OnClickNPC(string npcID)
    {
        Debug.Log("Clicked NPC: " + npcID);
    }

    public void OnClickGate()
    {
        gameManager.OpenWorldMap();
    }
}