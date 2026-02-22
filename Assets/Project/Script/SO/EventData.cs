using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Event", menuName = "Game/Event")]
public class EventData : ScriptableObject
{
    public string eventName;
    [TextArea] public string description;
    public LocationData triggerLocation;   // null = anywhere
    [Range(0, 1)] public float probability = 0.5f;

    public List<EventChoice> choices;
}

[System.Serializable]
public class EventChoice
{
    public string buttonText;
    [TextArea] public string resultText;
    public int goodsDelta;           // how much bread changes
    public int timeDelta;           // hours added
    public List<string> flagsToSet;
    // reputation changes, item gains, etc. – add later
}