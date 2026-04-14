using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaypointNode
{
    public string id;
    public string displayName;

    public bool isWaypoint = true;
}
[Serializable]
public class PathSegment
{
    public string fromId;
    public string toId;
    public int stepCount;
}
[Serializable]
public class LocationConnection
{
    public LocationData A;
    public LocationData B;

    public int stepCount; // fallback

    public List<PathSegment> segments; //  NEW
}