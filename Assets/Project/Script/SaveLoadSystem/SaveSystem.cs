using System.IO;
using UnityEngine;
public enum SaveLoadMode
{
    Save,
    Load
}
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    void Awake()
    {
        Instance = this;
    }

    string GetPath(int slot)
    {
        return Application.persistentDataPath + "/save_" + slot + ".json";
    }
    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();

        var gm = GameManager.Instance;

        // ===== LOCATION =====
        data.currentLocationId = gm.GetCurrentLocation().id;

        // ===== PLAYER =====
        var gameData = GameDataSystem.Instance;
        data.coin = gameData.coin;
        data.reputation = gameData.reputation;
        data.day = gameData.day;
        data.hour = gameData.hour;

        // ===== FLAGS =====
        data.flags = FlagSystem.Instance.GetAllFlags();

        // ===== QUEST =====
        var quest = QuestRuntimeSystem.Instance.GetActiveQuest();
        if (quest != null)
        {
            data.activeQuestId = quest.questId;
            data.questStep = quest.currentStep;
            data.questStartHour = quest.startHour;
        }

        // ===== QUEST HISTORY =====
        data.completedQuests = QuestHistorySystem.Instance.GetCompleted();
        data.expiredQuests = QuestHistorySystem.Instance.GetExpired();

        // ===== TRAVEL =====
        var travel = gm.GetTravelManager();
        var tState = travel.CaptureState();

        data.isTravelling = tState.isTravelling;
        data.travelTargetId = tState.targetLocationId;
        data.travelStep = tState.currentStep;
        data.travelTotalStep = tState.totalStep;
        data.jobBoardQuestIds = GameManager.Instance.guildSystem.jobBoardSystem.GetCurrentBoardIds();
        // ===== WRITE FILE =====
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);

        Debug.Log("Saved slot " + slot);
    }
    public void LoadGame(int slot)
    {
        string path = GetPath(slot);

        if (!File.Exists(path))
        {
            Debug.Log("No save file!");
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        var gm = GameManager.Instance;

        // ===== LOCATION =====
        var loc = gm.worldGraph.GetLocationById(data.currentLocationId);
        gm.SetCurrentLocation(loc);

        // ===== PLAYER =====
        var gameData = GameDataSystem.Instance;
        gameData.coin = data.coin;
        gameData.reputation = data.reputation;
        gameData.day = data.day;
        gameData.hour = data.hour;

        // ===== FLAGS =====
        FlagSystem.Instance.LoadFlags(data.flags);

        // ===== QUEST =====
        QuestRuntimeSystem.Instance.ClearQuest();

        if (!string.IsNullOrEmpty(data.activeQuestId))
        {
            var questSO = GameDatabase.Instance.GetQuest(data.activeQuestId);
            if (questSO != null)
            {
                QuestRuntimeSystem.Instance.LoadQuest(
                    questSO,
                    data.questStep,
                    data.questStartHour
                );
            }
            else
            {
                Debug.LogError("Failed to load quest: " + data.activeQuestId);
            }
            QuestRuntimeSystem.Instance.LoadQuest(
                questSO,
                data.questStep,
                data.questStartHour
            );
        }
        var jobSystem = GameManager.Instance.guildSystem.jobBoardSystem;

        jobSystem.isLoadedFromSave = true;
        jobSystem.LoadBoard(data.jobBoardQuestIds);
        // ===== QUEST HISTORY =====
        QuestHistorySystem.Instance.LoadData(
            data.completedQuests,
            data.expiredQuests
        );

        // ===== TRAVEL =====
        var travel = gm.GetTravelManager();
        travel.RestoreState(new TravelState
        {
            currentLocationId = data.currentLocationId,
            targetLocationId = data.travelTargetId,
            currentStep = data.travelStep,
            totalStep = data.travelTotalStep,
            isTravelling = data.isTravelling
        }, gm.worldGraph);

        // ===== REFRESH UI =====
        UIDataSystem.Instance.Refresh();

        Debug.Log("Loaded slot " + slot);
    }
    
}