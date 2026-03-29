using UnityEngine;
public enum EventTriggerType
{
    OnInteract,
    OnTravelStep,
    OnLocationEnter,
    OnTimePass
}

public enum EventLayer
{
    Story,        // thuộc quest → điều khiển step
    Contextual,   // phụ thuộc context (flag, trạng thái)
    Generic       // random flavor
}

[System.Serializable]
public class DialogueLine
{
    public string text;
    public string npcId; // null = narrator
}

[CreateAssetMenu(menuName = "Game/Event v2")]
public class EventSO : ScriptableObject
{
    [Header("Identity")]
    public string id;

    [Header("Classification")]
    public EventTriggerType triggerType;
    public EventLayer layer;

    [Header("Target")]
    public string targetId; // dùng cho OnInteract / OnLocationEnter

    [Header("Step Condition (Story only)")]
    public int requiredStep = -1;  // -1 = không dùng
    public int nextStep = -1;      // -1 = không đổi step

    [Header("Conditions")]
    public string[] requiredFlags;
    public string[] forbiddenFlags;

    [Header("Random")]
    [Range(0, 1)]
    public float triggerChance = 1f;

    public int priority = 0;
    public bool repeatable = true;

    [Header("Dialogue")]
    public DialogueLine[] dialogues;

    [Header("Choices")]
    public EventChoiceSO[] choices;
}