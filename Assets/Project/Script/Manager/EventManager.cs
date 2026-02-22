using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public List<EventData> allEvents;  // drag all Event assets here in Inspector

    void Awake() => Instance = this;

    public void CheckForEvent(LocationData location)
    {
        List<EventData> possible = allEvents.FindAll(e => e.triggerLocation == location || e.triggerLocation == null);
        if (possible.Count == 0) return;

        EventData selected = possible[Random.Range(0, possible.Count)];
        if (Random.value <= selected.probability)
        {
            TriggerEvent(selected);
        }
    }

    void TriggerEvent(EventData evt)
    {
        UIManager.Instance.ShowEventPanel(evt);
    }

    public void ExecuteChoice(EventChoice choice)
    {
        GameManager.Instance.goods += choice.goodsDelta;
        // apply time, flags, etc.
        foreach (string flag in choice.flagsToSet)
        {
            GameManager.Instance.flags[flag] = true;
        }
        UIManager.Instance.ShowEventResult(choice.resultText);
    }
}