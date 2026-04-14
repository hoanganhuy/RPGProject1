using UnityEngine;
public enum GameMode
{
    Exploration,
    WorldMap,
    Busy,
    InGuild,
    InEvent,
    InDialogue,
    InShop
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Start Data")]
    public LocationData startLocation;
    public WorldMapGraph worldGraph;

    [Header("Systems")]
    public WorldMapController worldMapController;
    public LocationSystem locationSystem;
    public GameMode currentMode;
    public GuildSystem guildSystem;
    public DialogueSystem dialogueSystem;

    public TravelManager travelManager;
    LocationData currentLocation;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentMode = GameMode.Exploration;
        currentLocation = startLocation;

        travelManager = new TravelManager(worldGraph, currentLocation);

        worldMapController.Init(travelManager, OnTravelFinished);

        locationSystem.EnterLocation(currentLocation,this);

        worldMapController.CloseMap();
    }

    // ===== MAP FLOW =====
    public void OpenWorldMap()
    {
        ChangeMode(GameMode.WorldMap);
        worldMapController.OpenMap(worldGraph);
    }

    // ===== GAME MODE ====
    public void ChangeMode(GameMode mode)
    {
        currentMode = mode;
    }
    // ===== TRAVEL =====
    public void StartTravel(LocationData target)
    {
        if (travelManager.IsTravelling())
            return;
        ChangeMode(GameMode.Busy);
        //if (!travelManager.CanTravelTo(target))
        //{
        //    Debug.Log("Location not connected!");
        //    return;
        //}

        travelManager.StartTravel(target);

        worldMapController.StartTravelUI();
    }

    void OnTravelFinished()
    {
        ChangeMode(GameMode.Exploration);
        currentLocation = travelManager.GetCurrentLocation();
        locationSystem.EnterLocation(currentLocation,this);

        //Trigger event enter locaition 
        var e = EventRuntimeSystem.Instance.TryGetEventLocation();
        if (e != null)
        {
            ChangeMode(GameMode.InEvent);
            dialogueSystem.StartEventDialogue(e);
        }
    }
    public LocationData GetCurrentLocation()
    {
        return currentLocation;
    }

    public void SetCurrentLocation(LocationData loc)
    {
        currentLocation = loc;
        locationSystem.EnterLocation(currentLocation, this);
    }

    public TravelManager GetTravelManager()
    {
        return travelManager;
    }
    public void OnClickSaveSlot0()
    {
        SaveSystem.Instance.SaveGame(0);
    }
    public void OnClickLoadSlot0()
    {
        SaveSystem.Instance.LoadGame(0);
    }
}