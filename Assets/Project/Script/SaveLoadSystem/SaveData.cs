using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // ===== PLAYER =====
    public int coin;
    public int reputation;
    public int day;
    public float hour;

    // ===== LOCATION =====
    public string currentLocationId;

    // ===== FLAGS =====
    public List<string> flags;

    // ===== QUEST =====
    public string activeQuestId;
    public int questStep;
    public int questStartHour;

    public List<string> completedQuests;
    public List<string> expiredQuests;
    public List<string> jobBoardQuestIds;
    // ===== TRAVEL =====
    public bool isTravelling;
    public string travelTargetId;
    public int travelStep;
    public int travelTotalStep;
}