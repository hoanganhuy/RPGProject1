using System.Collections.Generic;
using UnityEngine;

public class QuestHistorySystem : MonoBehaviour
{
    public static QuestHistorySystem Instance;

    HashSet<string> completed = new HashSet<string>();
    HashSet<string> expired = new HashSet<string>();

    void Awake()
    {
        Instance = this;
    }

    public void MarkCompleted(string id)
    {
        completed.Add(id);
    }

    public void MarkExpired(string id)
    {
        expired.Add(id);
    }

    public bool IsCompleted(string id)
    {
        return completed.Contains(id);
    }

    public bool IsExpired(string id)
    {
        return expired.Contains(id);
    }
    public List<string> GetCompleted()
    {
        return new List<string>(completed);
    }

    public List<string> GetExpired()
    {
        return new List<string>(expired);
    }

    public void LoadData(List<string> completedList, List<string> expiredList)
    {
        completed = new HashSet<string>(completedList);
        expired = new HashSet<string>(expiredList);
    }
}