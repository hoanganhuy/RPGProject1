using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private List<EventData> allEvents;
    private GameState state;

    public EventManager(List<EventData> events, GameState state)
    {
        this.allEvents = events;
        this.state = state;
    }

    public EventData TryTriggerEvent()
    {
        List<EventData> eligible = new List<EventData>();

        foreach (var e in allEvents)
        {
            if (EventUtility.IsEventEligible(e, state))
            {
                eligible.Add(e);
            }
        }

        if (eligible.Count == 0)
            return null;

        return eligible[0]; // tạm lấy cái đầu
    }

    public void ApplyChoice(EventData eventData, EventChoiceData choice)
    {
        EventUtility.ApplyChoice(eventData, choice, state);
    }
}