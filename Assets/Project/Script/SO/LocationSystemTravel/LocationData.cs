using UnityEngine;

[CreateAssetMenu(menuName = "Game/Location")]
public class LocationData : ScriptableObject
{
    public string id;
    public string displayName;

    public Sprite locationBackground;   // cảnh khi ở trong location
    public Sprite mapIcon;              // icon trên world map

    public GameObject locationMapPrefab;
}