using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase Instance;

    public List<QuestSO> quests;

    Dictionary<string, QuestSO> questMap;

    
    void Awake()
    {
        Instance = this;

        questMap = new Dictionary<string, QuestSO>();

        foreach (var q in quests)
        {
            questMap[q.id] = q;
        }
    }

    public QuestSO GetQuest(string id)
    {
        if (questMap.TryGetValue(id, out var q))
            return q;

        Debug.LogError("Quest not found: " + id);
        return null;
    }
}