using System.Collections.Generic;
using UnityEngine;

public class FlagSystem : MonoBehaviour
{
    public static FlagSystem Instance;

    HashSet<string> flags = new HashSet<string>();

    void Awake()
    {
        Instance = this;
    }

    // ===== SET =====
    public void SetFlag(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (flags.Add(id))
            Debug.Log("[Flag] Set: " + id);
    }

    // ===== CLEAR =====
    public void ClearFlag(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (flags.Remove(id))
            Debug.Log("[Flag] Cleared: " + id);
    }

    // ===== CHECK =====
    public bool HasFlag(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;

        return flags.Contains(id);
    }

    // ===== DEBUG =====
    public void PrintAllFlags()
    {
        foreach (var f in flags)
            Debug.Log("Flag: " + f);
    }
}