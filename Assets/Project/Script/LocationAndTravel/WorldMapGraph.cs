using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/WorldMap")]
public class WorldMapGraph : ScriptableObject
{
    public List<LocationConnection> connections;

    Dictionary<string, LocationData> locationMap;

    void OnEnable()
    {
        locationMap = new Dictionary<string, LocationData>();

        foreach (var conn in connections)
        {
            if (conn.A != null && !locationMap.ContainsKey(conn.A.id))
                locationMap.Add(conn.A.id, conn.A);

            if (conn.B != null && !locationMap.ContainsKey(conn.B.id))
                locationMap.Add(conn.B.id, conn.B);

            //  thêm waypoint nếu có
            if (conn.segments != null)
            {
                foreach (var seg in conn.segments)
                {
                    // waypoint không có LocationData → bỏ qua
                    // (sau này nếu bạn tạo WaypointData thì add vào đây)
                }
            }
        }
    }

    public LocationData GetLocationById(string id)
    {
        if (locationMap.TryGetValue(id, out var loc))
            return loc;

        Debug.LogError("Location not found: " + id);
        return null;
    }
}