using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameFlowState
{
    Idle,
    Event
}

public class GameManager : MonoBehaviour
{
    public List<EventData> allEvents;

    public EventUIController eventUI;
    public LocationUI locationUI;
    public Button continueButton;

    private GameState state;
    private EventManager eventManager;
    private TravelManager travelManager;

    private EventData currentEvent;
    private GameFlowState currentState = GameFlowState.Idle;

    public List<LocationData> allLocations;
    void Awake()
    {
        state = new GameState();
        state.currentLocationID = "guild";   // start location

        eventManager = new EventManager(allEvents, state);
        travelManager = new TravelManager(state, eventManager);
    }

    void Start()
    {
        var location = GetLocationByID(state.currentLocationID);

        
        locationUI.UpdateLocation(location.displayName);
        
    }
    public void OnContinueButton()
    {
        if (currentState == GameFlowState.Event)
            return;

        var e = travelManager.StepTravel();

        locationUI.UpdateLocation(state.currentLocationID);

        if (e != null)
        {
            currentEvent = e;

            currentState = GameFlowState.Event;

            continueButton.interactable = false;

            eventUI.ShowEvent(e, OnChoiceSelected);
        }
    }
    private void OnChoiceSelected(EventChoiceData choice)
    {
        eventManager.ApplyChoice(currentEvent, choice);

        currentEvent = null;

        currentState = GameFlowState.Idle;

        continueButton.interactable = true;
    }
    LocationData GetLocationByID(string id)
    {
        foreach (var loc in allLocations)
        {
            if (loc.id == id)
                return loc;
        }

        return null;
    }
}