using UnityEngine;

[CreateAssetMenu(menuName = "Game/EventChoice v2")]
public class EventChoiceSO : ScriptableObject
{
    [TextArea(2, 6)]
    public string description;

    [Header("Resource")]
    public int coinDelta;
    public int reputationDelta;
    public float timeDelta;

    [Header("Flags")]
    public string[] flagsSet;
    public string[] flagsCleared;

    [Header("Quest Progression (Story only)")]
    public bool advanceStep; // dùng nếu muốn explicit
}