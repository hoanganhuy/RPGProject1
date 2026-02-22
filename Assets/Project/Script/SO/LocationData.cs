using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Location", menuName = "Game/Location")]
public class LocationData : ScriptableObject
{
    public string locationName;
    [TextArea] public string description;
    public List<LocationData> neighbors;  // drag in Inspector
}