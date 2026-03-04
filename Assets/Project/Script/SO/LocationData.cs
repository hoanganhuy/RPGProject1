using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Location")]
public class LocationData : ScriptableObject
{
    [Header("Identity")]
    public string id;           // unique key
    public string displayName;  // name shown in UI
    public List<LocationData> neighbors = new List<LocationData>();
}