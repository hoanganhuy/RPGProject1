using UnityEngine;

public class TravelManager
{
    LocationData currentLocation;
    LocationData targetLocation;

    int currentStep;
    int totalStep;

    bool isTravelling;

    WorldMapGraph graph;

    bool isPausedByEvent;
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
        if (!isTravelling || isPausedByEvent)
            return false;

        currentStep++;

        Debug.Log("Step " + currentStep + "/" + totalStep);

        // ===== EVENT =====
        var e = EventRuntimeSystem.Instance.TryGetTravelEvent();

        if (e != null)
        {
            Debug.Log("Travel Event Triggered: " + e.id);

            isPausedByEvent = true; // 🔥 PAUSE

            GameManager.Instance.ChangeMode(GameMode.InEvent);
            GameManager.Instance.dialogueSystem.StartEventDialogue(e);

            return false;
        }

        // ===== ARRIVAL =====
        if (currentStep >= totalStep)
        {
            currentLocation = targetLocation;
            isTravelling = false;

            Debug.Log("Arrived at " + currentLocation.displayName);
            return true;
        }

        return false;
    }
    public void ResumeTravel()
    {
        isPausedByEvent = false;
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
    public bool IsPaused()
    {
        return isPausedByEvent;
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