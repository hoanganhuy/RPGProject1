using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event Choice")]
public class EventChoiceSO : ScriptableObject
{
    [TextArea(2, 6)]
    public string text;

    public EventOutcome outcome;

    public EventSO nextEvent;
}