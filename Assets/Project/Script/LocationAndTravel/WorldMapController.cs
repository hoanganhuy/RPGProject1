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
    float eventChanceMultiplier = 1f;
    public void Init(TravelManager tm, System.Action travelFinishedCallback)
    {
        travelManager = tm;
        onTravelFinished = travelFinishedCallback;
    }
    public void OpenMap(WorldMapGraph graph)
    {
        worldMapPanel.SetActive(true);
        DrawConnections(graph);

        // FIX
        var current = travelManager.GetCurrentLocation();
        var node = FindNode(current);

        if (node != null)
            playerUI.position = node.transform.position;
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

       
        var path = travelManager.GetCurrentPath();

        int steps = travelManager.GetTotalSteps();

        foreach (var seg in path)
        {
            var nodeA = FindNodeById(seg.fromId);
            var nodeB = FindNodeById(seg.toId);

            Vector3 startPos = nodeA.transform.position;
            Vector3 endPos = nodeB.transform.position;

            for (int i = 1; i <= seg.stepCount; i++)
            {
                if (travelManager.IsPaused())
                {
                    yield return null;
                    i--;
                    continue;
                }

                bool arrived = travelManager.StepTravel();

                // ===== MOVE TRƯỚC =====
                float t = (float)i / steps;
                Vector3 stepPos = Vector3.Lerp(startPos, endPos, t);

                yield return MovePlayer(stepPos);

                // ===== CHECK EVENT SAU =====
                var e = EventRuntimeSystem.Instance.TryGetTravelEvent(eventChanceMultiplier);

                if (e != null)
                {
                    eventChanceMultiplier *= 0.8f; // giảm 20%
                    travelManager.PauseByEvent();

                    GameManager.Instance.ChangeMode(GameMode.InEvent);
                    GameManager.Instance.dialogueSystem.StartEventDialogue(e);

                    // WAIT tới khi event xong
                    yield return new WaitUntil(() => !travelManager.IsPaused());

                    GameManager.Instance.ChangeMode(GameMode.Busy);
                }
                if (arrived)
                {
                    eventChanceMultiplier = 1f;
                    EnableNodes();
                    ClearDots();
                    CloseMap();
                    onTravelFinished?.Invoke();
                    yield break;
                }
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
            // ===== CASE 1: có waypoint (segments) =====
            if (conn.segments != null && conn.segments.Count > 0)
            {
                for (int s = 0; s < conn.segments.Count; s++)
                {
                    var seg = conn.segments[s];

                    var nodeA = FindNodeById(seg.fromId);
                    var nodeB = FindNodeById(seg.toId);

                    if (nodeA == null || nodeB == null)
                        continue;

                    Vector3 startPos = nodeA.transform.position;
                    Vector3 endPos = nodeB.transform.position;

                    for (int i = 1; i <= seg.stepCount; i++)
                    {
                        float t = (float)i / (seg.stepCount + 1);
                        Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                        var dot = Instantiate(stepDotPrefab, dotHolder);
                        dot.transform.position = pos;

                        spawnedDots.Add(dot);
                    }
                }
            }
            else
            {
                // ===== fallback cũ =====
                var nodeA = FindNode(conn.A);
                var nodeB = FindNode(conn.B);

                Vector3 startPos = nodeA.transform.position;
                Vector3 endPos = nodeB.transform.position;

                for (int i = 1; i <= conn.stepCount; i++)
                {
                    float t = (float)i / (conn.stepCount + 1);
                    Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                    var dot = Instantiate(stepDotPrefab, dotHolder);
                    dot.transform.position = pos;

                    spawnedDots.Add(dot);
                }
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
        {
            var btn = n.GetComponent<Button>();

            //  waypoint không bao giờ click được
            if (n.location == null)
            {
                btn.interactable = false;
                continue;
            }

            btn.interactable = false;
        }
    }

    void EnableNodes()
    {
        foreach (var n in nodes)
        {
            var btn = n.GetComponent<Button>();

            if (n.location == null)
            {
                btn.interactable = false;
                continue;
            }

            btn.interactable = true;
        }
    }
    WorldMapNodeUI FindNodeById(string id)
    {
        foreach (var n in nodes)
        {
            if (n.pointId == id)
                return n;
        }

        Debug.LogWarning("Node not found: " + id);
        return null;
    }
    WorldMapNodeUI FindNode(LocationData loc)
    {
        foreach (var n in nodes)
            if (n.location == loc)
                return n;

        return null;
    }
    public float GetEventChanceMultiplier()
    {
        return eventChanceMultiplier;
    }
}