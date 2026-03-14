using UnityEngine;

public class TravelManager
{
    LocationData currentLocation;
    LocationData targetLocation;

    int currentStep;
    int totalStep;

    bool isTravelling;

    WorldMapGraph graph;

    public TravelManager(WorldMapGraph graph, LocationData start)
    {
        this.graph = graph;
        currentLocation = start;
    }

    public void StartTravel(LocationData target)
    {
        foreach (var conn in graph.connections)
        {
            if ((conn.A == currentLocation && conn.B == target) ||
                (conn.B == currentLocation && conn.A == target))
            {
                targetLocation = target;
                totalStep = conn.stepCount;
                currentStep = 0;
                isTravelling = true;

                Debug.Log("Start Travel to " + target.displayName);
                return;
            }
        }

        Debug.Log("No path!");
    }

    public bool StepTravel()
    {
        if (!isTravelling) return false;

        currentStep++;

        Debug.Log("Step " + currentStep + "/" + totalStep);

        if (currentStep >= totalStep)
        {
            currentLocation = targetLocation;
            isTravelling = false;

            Debug.Log("Arrived at " + currentLocation.displayName);
            return true;
        }

        return false;
    }

    public LocationData GetCurrentLocation()
    {
        return currentLocation;
    }
    public bool CanTravelTo(LocationData target)
    {
        foreach (var conn in graph.connections)
        {
            if ((conn.A == currentLocation && conn.B == target) ||
                (conn.B == currentLocation && conn.A == target))
                return true;
        }

        return false;
    }
    public LocationData GetTargetLocation()
    {
        return targetLocation;
    }

    public bool IsTravelling()
    {
        return isTravelling;
    }
    public int GetTotalSteps()
    {
        return totalStep;
    }
}