using UnityEngine;

[System.Serializable]
public class QuestStep
{
    public string npcID;
    public LocationData location;

    [TextArea(2, 5)]
    public string stepDescription;
}