using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Start Data")]
    public LocationData startLocation;
    public WorldMapGraph worldMapGraph;

    [Header("UI")]
    public Transform locationMapHolder;
    public Image backgroundImage;
    public GameObject worldMapPanel;
    public UIDataController uiData;
    public WorldMapNodeUI[] worldMapNodes;
    public RectTransform playerUI;
    public GameObject stepDotPrefab;
    public Transform worldMapPanelTransform;

    [Header("Systems")]
    public TravelManager travelManager;

    GameObject currentLocationMapObj;
    LocationData currentLocation;

    void Start()
    {
        currentLocation = startLocation;

        travelManager = new TravelManager(worldMapGraph, currentLocation);

        LoadLocation(currentLocation);

        worldMapPanel.SetActive(false);
    }

    // ===== LOCATION LOAD =====
    public void LoadLocation(LocationData loc)
    {
        currentLocation = loc;

        backgroundImage.sprite = loc.locationBackground;

        uiData.UpdateLocation(loc.displayName);

        if (currentLocationMapObj != null)
            Destroy(currentLocationMapObj);

        currentLocationMapObj =
            Instantiate(loc.locationMapPrefab, locationMapHolder);

        LocationMapController controller =
            currentLocationMapObj.GetComponent<LocationMapController>();

        controller.gameManager = this;

        locationMapHolder.gameObject.SetActive(false);
    }

    // ===== LOCATION MAP =====
    public void ToggleLocationMap()
    {
        bool active = locationMapHolder.gameObject.activeSelf;
        locationMapHolder.gameObject.SetActive(!active);
    }

    // ===== WORLD MAP =====
    public void OpenWorldMap()
    {
        worldMapPanel.SetActive(true);
        locationMapHolder.gameObject.SetActive(false);

        DrawAllConnections();
    }

    public void CloseWorldMap()
    {
        worldMapPanel.SetActive(false);
    }

    // ===== TRAVEL =====
    public void StartTravel(LocationData target)
    {
        // Không cho travel nếu đang đi
        if (travelManager.IsTravelling())
            return;

        // Check graph connection
        if (!travelManager.CanTravelTo(target))
        {
            Debug.Log("Location not connected!");
            return;
        }

        // Bắt đầu logic travel
        travelManager.StartTravel(target);

        // Lấy node start và end
        WorldMapNodeUI nodeA = FindNode(travelManager.GetCurrentLocation());
        WorldMapNodeUI nodeB = FindNode(target);

        if (nodeA == null || nodeB == null)
        {
            Debug.LogError("WorldMapNode missing!");
            return;
        }

        Vector3 startPos =
            nodeA.GetComponent<RectTransform>().position;

        Vector3 endPos =
            nodeB.GetComponent<RectTransform>().position;

        
        // Disable click node khi đang travel
        DisableWorldMapNodes();

        // Bắt đầu di chuyển player
        StartCoroutine(TravelMovementRoutine());

        Debug.Log("Travel UI started");
    }

    public void StepTravel()
    {
        bool arrived = travelManager.StepTravel();

        if (arrived)
        {
            EnableWorldMapNodes();
            ClearStepDots();
            CloseWorldMap();

            LoadLocation(travelManager.GetCurrentLocation());
        }
    }
    WorldMapNodeUI FindNode(LocationData loc)
    {
        foreach (var n in worldMapNodes)
            if (n.location == loc)
                return n;

        return null;
    }
    System.Collections.IEnumerator TravelMovementRoutine()
    {
        LocationData from = currentLocation;
        LocationData to = travelManager.GetTargetLocation();

        WorldMapNodeUI nodeA = FindNode(from);
        WorldMapNodeUI nodeB = FindNode(to);

        Vector3 startPos = nodeA.GetComponent<RectTransform>().position;
        Vector3 endPos = nodeB.GetComponent<RectTransform>().position;

        int steps = travelManager.GetTotalSteps();

        for (int i = 1; i <= steps; i++)
        {
            float t = (float)i / steps;

            Vector3 stepPos = Vector3.Lerp(startPos, endPos, t);

            yield return MovePlayer(stepPos);

            bool arrived = travelManager.StepTravel();

            if (arrived)
            {
                EnableWorldMapNodes();
                ClearStepDots();
                CloseWorldMap();
                LoadLocation(travelManager.GetCurrentLocation());
            }
        }
    }
    System.Collections.IEnumerator MovePlayer(Vector3 target)
    {
        float speed = 150f;

        while (Vector3.Distance(playerUI.position, target) > 1f)
        {
            playerUI.position =
                Vector3.MoveTowards(playerUI.position, target, speed * Time.deltaTime);

            yield return null;
        }
    }
    
    List<GameObject> spawnedDots = new List<GameObject>();

    void ClearStepDots()
    {
        foreach (var d in spawnedDots)
            Destroy(d);

        spawnedDots.Clear();
    }
    void DisableWorldMapNodes()
    {
        foreach (var n in worldMapNodes)
            n.GetComponent<Button>().interactable = false;
    }

    void EnableWorldMapNodes()
    {
        foreach (var n in worldMapNodes)
            n.GetComponent<Button>().interactable = true;
    }
    void DrawAllConnections()
    {
        ClearStepDots();

        foreach (var conn in worldMapGraph.connections)
        {
            WorldMapNodeUI nodeA = FindNode(conn.A);
            WorldMapNodeUI nodeB = FindNode(conn.B);

            Vector3 startPos = nodeA.GetComponent<RectTransform>().position;
            Vector3 endPos = nodeB.GetComponent<RectTransform>().position;

            for (int i = 1; i <= conn.stepCount; i++)
            {
                float t = (float)i / (conn.stepCount + 1);

                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                GameObject dot =
                    Instantiate(stepDotPrefab, worldMapPanelTransform);

                dot.transform.position = pos;

                spawnedDots.Add(dot);
            }
        }
    }
}