using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Quest v2")]
public class QuestSO : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public string title;
    [TextArea(3, 8)]
    public string description;

    [Header("Step")]
    public int startStep = 0;
    public int endStep = 3;

    [Header("Events")]
    public List<EventSO> events;

    [Header("Reward")]
    public int coinReward;
    public int reputationReward;

    [Header("Rules")]
    public bool repeatable;
    public int durationHours;

    [Header("Conditions")]
    public string[] requiredFlags;
    public string[] forbiddenFlags;
}