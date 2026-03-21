using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapController : MonoBehaviour
{
    public GameObject worldMapPanel;
    public RectTransform playerUI;

    public WorldMapNodeUI[] nodes;

    public GameObject stepDotPrefab;
    public Transform dotHolder;

    List<GameObject> spawnedDots = new List<GameObject>();

    TravelManager travelManager;
    System.Action onTravelFinished;

    public void Init(TravelManager tm, System.Action travelFinishedCallback)
    {
        travelManager = tm;
        onTravelFinished = travelFinishedCallback;
    }

    public void OpenMap(WorldMapGraph graph)
    {
        worldMapPanel.SetActive(true);
        DrawConnections(graph);
    }

    public void CloseMap()
    {
        worldMapPanel.SetActive(false);
    }

    public void StartTravelUI()
    {
        DisableNodes();
        StartCoroutine(TravelRoutine());
    }

    IEnumerator TravelRoutine()
    {
        LocationData from = travelManager.GetCurrentLocation();
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
                EnableNodes();
                ClearDots();
                CloseMap();
                onTravelFinished?.Invoke();
            }
        }
    }

    IEnumerator MovePlayer(Vector3 target)
    {
        float speed = 150f;

        while (Vector3.Distance(playerUI.position, target) > 1f)
        {
            playerUI.position =
                Vector3.MoveTowards(playerUI.position, target, speed * Time.deltaTime);

            yield return null;
        }
    }

    void DrawConnections(WorldMapGraph graph)
    {
        ClearDots();

        foreach (var conn in graph.connections)
        {
            WorldMapNodeUI nodeA = FindNode(conn.A);
            WorldMapNodeUI nodeB = FindNode(conn.B);

            Vector3 startPos = nodeA.GetComponent<RectTransform>().position;
            Vector3 endPos = nodeB.GetComponent<RectTransform>().position;

            for (int i = 1; i <= conn.stepCount; i++)
            {
                float t = (float)i / (conn.stepCount + 1);

                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                GameObject dot = Instantiate(stepDotPrefab, dotHolder);
                dot.transform.position = pos;

                spawnedDots.Add(dot);
            }
        }
    }

    void ClearDots()
    {
        foreach (var d in spawnedDots)
            Destroy(d);

        spawnedDots.Clear();
    }

    void DisableNodes()
    {
        foreach (var n in nodes)
            n.GetComponent<Button>().interactable = false;
    }

    void EnableNodes()
    {
        foreach (var n in nodes)
            n.GetComponent<Button>().interactable = true;
    }

    WorldMapNodeUI FindNode(LocationData loc)
    {
        foreach (var n in nodes)
            if (n.location == loc)
                return n;

        return null;
    }
}