using System.Collections.Generic;

public static class EventUtility
{
    public static bool IsEventEligible(EventData eventData, GameState state)
    {
        // Check location
        if (eventData.triggerLocationIDs.Count > 0 &&
            !eventData.triggerLocationIDs.Contains(state.currentLocationID))
            return false;

        // Check one-time
        if (eventData.oneTimeOnly && state.completedEventIDs.Contains(eventData.id))
            return false;

        // Required flags
        foreach (var flag in eventData.requiredFlags)
        {
            if (!state.flags.Contains(flag))
                return false;
        }

        // Blocked flags
        foreach (var flag in eventData.blockedFlags)
        {
            if (state.flags.Contains(flag))
                return false;
        }

        return true;
    }

    public static void ApplyChoice(EventData eventData, EventChoiceData choice, GameState state)
    {
        foreach (var flag in choice.setFlags)
        {
            state.flags.Add(flag);
        }

        foreach (var flag in choice.clearFlags)
        {
            state.flags.Remove(flag);
        }

        if (eventData.oneTimeOnly)
        {
            state.completedEventIDs.Add(eventData.id);
        }
    }
}