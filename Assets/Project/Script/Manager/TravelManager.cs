using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TravelManager : MonoBehaviour
{
    public static TravelManager Instance;

    private List<LocationData> currentPath;
    private int pathIndex;

    void Awake() => Instance = this;

    public void SetCurrentLocation(LocationData loc)
    {
        GameManager.Instance.currentLocation = loc;
        currentPath = null;
    }

    public void StartTravel(LocationData destination)
    {
        // Simple BFS to find path (you can precompute or use simple neighbor walk)
        currentPath = FindPath(GameManager.Instance.currentLocation, destination);
        pathIndex = 0;
        NextStep();
    }

    public void NextStep()
    {
        if (currentPath == null) return;

        pathIndex++;
        if (pathIndex >= currentPath.Count)
        {
            // Arrived
            GameManager.Instance.currentLocation = currentPath.Last();
            UIManager.Instance.OnArrival();
            currentPath = null;
            return;
        }

        GameManager.Instance.currentLocation = currentPath[pathIndex];
        UIManager.Instance.UpdateUI();

        // Trigger events?
        EventManager.Instance.CheckForEvent(GameManager.Instance.currentLocation);
    }

    private List<LocationData> FindPath(LocationData start, LocationData goal)
    {
        // Quick BFS – you can copy a simple implementation online.
        // For now, just assume direct neighbor or return null.
        // Since our nodes are few, you can even manually specify routes.
        // Keep it simple: if they are neighbors, go directly.
        if (start.neighbors.Contains(goal))
            return new List<LocationData> { start, goal };
        else
            return new List<LocationData> { start, start.neighbors[0], goal }; // naive
    }
}