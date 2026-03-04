using UnityEngine;

public class TravelManager
{
    private GameState state;
    private EventManager eventManager;

    public TravelManager(GameState state, EventManager eventManager)
    {
        this.state = state;
        this.eventManager = eventManager;
    }

    public EventData StepTravel()
    {
        if (state.currentLocationID == "guild")
            state.currentLocationID = "village_gate";

        else if (state.currentLocationID == "village_gate")
            state.currentLocationID = "old_mill";

        else if (state.currentLocationID == "old_mill")
            state.currentLocationID = "garden";

        Debug.Log("Travelled to: " + state.currentLocationID);

        return eventManager.TryTriggerEvent();
    }
}