using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public QuestData activeQuest;
    public LocationData currentLocation;
    public int goods = 9;
    public int coins = 0;
    public Dictionary<string, bool> flags = new Dictionary<string, bool>();

    void Awake() => Instance = this;
}