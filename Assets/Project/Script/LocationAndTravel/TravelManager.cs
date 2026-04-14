using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TravelState
{
    public string currentLocationId;
    public string targetLocationId;
    public int currentStep;
    public int totalStep;
    public bool isTravelling;
}
public class TravelManager
{
    LocationData currentLocation;
    LocationData targetLocation;

    int currentStep;
    int totalStep;
    List<PathSegment> currentPath;
    int currentSegmentIndex;
    int stepInSegment;
    

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
        var pathConnections = FindPath(currentLocation, target);

        if (pathConnections == null)
        {
            Debug.Log("No path found!");
            return;
        }

        var fullPath = BuildFullPath(pathConnections, currentLocation);

        SetupPath(fullPath, target);
    }
    
    public List<PathSegment> GetCurrentPath()
    {
        return currentPath;
    }
    void SetupPath(List<PathSegment> segments, LocationData target)
    {
        currentPath = segments;
        currentSegmentIndex = 0;
        stepInSegment = 0;

        totalStep = 0;
        foreach (var s in segments)
            totalStep += s.stepCount;

        currentStep = 0;
        isTravelling = true;
        targetLocation = target;

        Debug.Log("Start Travel with " + segments.Count + " segments");
    }
    List<PathSegment> ReverseSegments(List<PathSegment> segments)
    {
        var reversed = new List<PathSegment>();

        for (int i = segments.Count - 1; i >= 0; i--)
        {
            var s = segments[i];

            reversed.Add(new PathSegment
            {
                fromId = s.toId,   // swap
                toId = s.fromId,   // swap
                stepCount = s.stepCount
            });
        }

        return reversed;
    }
    List<LocationConnection> FindPath(LocationData start, LocationData target)
    {

        var visited = new HashSet<string>();
        var queue = new Queue<(LocationData node, List<LocationConnection> path)>();

        queue.Enqueue((start, new List<LocationConnection>()));

        while (queue.Count > 0)
        {
            var (current, path) = queue.Dequeue();

            if (current.id == target.id)
                return path;

            if (visited.Contains(current.id))
                continue;

            visited.Add(current.id);

            foreach (var conn in graph.connections)
            {
                LocationData next = null;

                if (conn.A != null && conn.A.id == current.id)
                    next = conn.B;
                else if (conn.B != null && conn.B.id == current.id)
                    next = conn.A;
                
                if (next != null && !visited.Contains(next.id))
                {
                    var newPath = new List<LocationConnection>(path) { conn };
                    queue.Enqueue((next, newPath));
                }
            }
        }

        return null;
    }
    List<PathSegment> BuildFullPath(List<LocationConnection> connections, LocationData start)
    {
        var result = new List<PathSegment>();
        var current = start;

        foreach (var conn in connections)
        {
            List<PathSegment> segs;

            // ===== CASE 1: có segments =====
            if (conn.segments != null && conn.segments.Count > 0)
            {
                segs = new List<PathSegment>(conn.segments);

                //  FIX hướng
                if (segs[0].fromId != current.id)
                    segs = ReverseSegments(segs);
            }
            else
            {
                // ===== CASE 2: fallback =====
                var next = conn.A == current ? conn.B : conn.A;

                segs = new List<PathSegment>()
            {
                new PathSegment
                {
                    fromId = current.id,
                    toId = next.id,
                    stepCount = conn.stepCount
                }
            };
            }

            result.AddRange(segs);

            // update current
            current = graph.GetLocationById(segs[^1].toId);
        }

        return result;
    }
    public bool StepTravel()
    {
        if (!isTravelling || isPausedByEvent)
            return false;

        currentStep++;
        stepInSegment++;
        GameDataSystem.Instance.AddHour(0.2f);

        var currentSegment = currentPath[currentSegmentIndex];
      
        // ===== SEGMENT COMPLETE =====
        if (stepInSegment >= currentSegment.stepCount)
        {
            stepInSegment = 0;
            currentSegmentIndex++;

            //  ĐÃ TỚI WAYPOINT
            Debug.Log("Reached node: " + currentSegment.toId);

            // Hook tương lai (event waypoint)
            // TryTriggerWaypointEvent(currentSegment.toId);
        }

        // ===== ARRIVAL =====
        if (currentSegmentIndex >= currentPath.Count)
        {
            currentLocation = graph.GetLocationById(targetLocation.id);
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
    public TravelState CaptureState()
    {
        return new TravelState
        {
            currentLocationId = currentLocation.id,
            targetLocationId = targetLocation != null ? targetLocation.id : null,
            currentStep = currentStep,
            totalStep = totalStep,
            isTravelling = isTravelling
        };
    }

    public void RestoreState(TravelState state, WorldMapGraph graph)
    {
        this.graph = graph;

        currentLocation = graph.GetLocationById(state.currentLocationId);
        targetLocation = string.IsNullOrEmpty(state.targetLocationId)
            ? null
            : graph.GetLocationById(state.targetLocationId);

        currentStep = state.currentStep;
        totalStep = state.totalStep;
        isTravelling = state.isTravelling;

        isPausedByEvent = false; //  reset tránh kẹt game
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
    public void PauseByEvent()
    {
        isPausedByEvent = true;
    }
    public int GetTotalSteps()
    {
        return totalStep;
    }
}