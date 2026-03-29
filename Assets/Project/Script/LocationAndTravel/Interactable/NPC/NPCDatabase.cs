using System.Collections.Generic;
using UnityEngine;

public class NPCDatabase : MonoBehaviour
{
    public List<NPCDataSO> npcs;

    Dictionary<string, NPCDataSO> map;

    void Awake()
    {
        map = new Dictionary<string, NPCDataSO>();

        foreach (var n in npcs)
            map.Add(n.id, n);
    }

    public NPCDataSO Get(string id)
    {
        if (map.TryGetValue(id, out var n))
            return n;

        return null;
    }
}