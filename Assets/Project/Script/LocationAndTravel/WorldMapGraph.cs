using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/WorldMap")]
public class WorldMapGraph : ScriptableObject
{
    public List<LocationConnection> connections;
}