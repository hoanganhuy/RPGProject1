using UnityEngine;
public enum EventRarity
{
    Story,
    Quest,
    Common,
    Rare
}
public enum EventTriggerType
{
    TravelStep,
    LocationArrival,
    NPCInteract
}
[CreateAssetMenu(menuName = "Game/Event")]

public class EventSO : ScriptableObject
{
    public string id;

    public string title;

    [TextArea(5, 12)]
    public string description;

    public EventTriggerType triggerType;

    public LocationData location;
    public string npcID;

    public EventRarity rarity;

    public Vector2Int stepSegmentRange;

    public bool repeatable;

    public EventCondition[] conditions;

    public EventChoiceSO[] choices;
}