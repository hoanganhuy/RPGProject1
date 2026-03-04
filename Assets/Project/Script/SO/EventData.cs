using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event")]
public class EventData : ScriptableObject
{
    [Header("Identity")]
    public string id;

    [Header("Trigger")]
    public List<string> triggerLocationIDs = new List<string>();

    [Header("Conditions")]
    public List<string> requiredFlags = new List<string>();
    public List<string> blockedFlags = new List<string>();

    [Header("Rules")]
    public bool oneTimeOnly = false;

    [Header("Content")]
    [TextArea(4, 8)]
    public string description;

    public List<EventChoiceData> choices = new List<EventChoiceData>();
}